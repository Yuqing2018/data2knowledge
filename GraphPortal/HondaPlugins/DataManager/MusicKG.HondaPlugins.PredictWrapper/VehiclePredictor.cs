using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.PredictWrapper.Settings;
using MusicKG.HondaPlugins.PredictWrapper.ServiceModels;
using MusicKG.Tensorflow.Wrapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MusicKG.HondaPlugins.PredictWrapper
{
    public class VehiclePredictor : IVehiclePredictor
    {
        private readonly ILogger<VehiclePredictor> logger;
        private readonly VehicleFaultPredictSettings settings;
        private TensorflowServingModel syndromeModel;
        private TensorflowServingModel partModel;

        public VehiclePredictor(VehicleFaultPredictSettings settings, ILogger<VehiclePredictor> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        public void Initialize()
        {
            syndromeModel = new TensorflowServingModel(settings.Host, settings.Port, settings.SyndromeModelName, settings.ModelSignature, settings.ModelInputs, settings.SyndromeModelOutputs, settings.MillisecondsTimeout);
            partModel = new TensorflowServingModel(settings.Host, settings.Port, settings.PartModelName, settings.ModelSignature, settings.ModelInputs, settings.PartModelOutputs, settings.MillisecondsTimeout);
        }

        public void Close()
        {
            syndromeModel?.Dispose();
            partModel?.Dispose();
        }

        public PredictResultServiceModel Predict(
            AnnotationDocumentItem item,
            bool predictPart = true,
            bool predictSyndrome = true)
        {
            var text = string.Join("|", item.Features.Values.Where(f => f.KeyFeature).Select(v => v.Value));

            var inputDict = GetInputDictionary(new List<string> { text });

            logger.LogInformation($"[{nameof(VehiclePredictor)}.{nameof(Predict)}] - Start predict syndrome.");

            Dictionary<string, Array> syndrome_output, part_output;

            syndrome_output = predictSyndrome ? syndromeModel.GetModelOutputsAsync(inputDict).GetAwaiter().GetResult() : null;

            if (predictSyndrome)
            {
                logger.LogInformation($"[{nameof(VehiclePredictor)}.{nameof(Predict)}] - Predict syndrome successed!");
            }

            logger.LogInformation($"[{nameof(VehiclePredictor)}.{nameof(Predict)}] - Start predict syndrome.");

            part_output = predictPart ? partModel.GetModelOutputsAsync(inputDict).GetAwaiter().GetResult() : null;

            if (predictPart)
            {
                logger.LogInformation($"[{nameof(VehiclePredictor)}.{nameof(Predict)}] - Predict syndrome successed!");
            }

            return new PredictResultServiceModel()
            {
                ItemId = item.Id,
                PartName = predictPart ? new PredictResult()
                {
                    Probability = double.Parse(part_output["max_prob"].GetValue(0).ToString() ?? "0"),
                    Value = part_output["pred_label"].GetValue(0).ToString()
                } : null,
                Syndrome = predictSyndrome ? new PredictResult()
                {
                    Id = syndrome_output["pred_label"].GetValue(0).ToString(),
                    Probability = double.Parse(syndrome_output["max_prob"].GetValue(0).ToString() ?? "0"),
                    Value = syndrome_output["syndr_id"].GetValue(0).ToString()
                } : null
            };
        }

        public async Task<List<PredictResultServiceModel>> PredictBatchAsync(
            List<AnnotationDocumentItem> items,
            bool predictPart = true,
            bool predictSyndrome = true)
        {
            var texts = items.Select(x => string.Join('|', x.Features.Values.Where(f => f.KeyFeature).Select(v => v.Value))).ToList();

            var batchsize = texts.Count();

            logger.LogInformation($"[{nameof(VehiclePredictor)}.{nameof(PredictBatchAsync)}] - Request batchsize: {items.Count}");

            var inputDict = GetInputDictionary(texts);

            Console.WriteLine("Get input dictionaries from request texts.");

            Dictionary<string, Array> syndrome_output, part_output;

            syndrome_output = predictSyndrome ? await syndromeModel.GetModelOutputsAsync(inputDict, batchsize) : null;

            if (predictSyndrome)
            {
                logger.LogInformation($"[{nameof(VehiclePredictor)}.{nameof(PredictBatchAsync)}] - predict syndrome successed!");
            }

            part_output = predictPart ? await partModel.GetModelOutputsAsync(inputDict, batchsize) : null;

            if (predictPart)
            {
                logger.LogInformation($"[{nameof(VehiclePredictor)}.{nameof(PredictBatchAsync)}] - predict part successed!");
            }

            var predictResult = Enumerable.Range(0, batchsize).Select(x =>
            {
                var item = new PredictResultServiceModel()
                {
                    ItemId = items[x].Id,
                    PartName = predictPart ? new PredictResult()
                    {
                        Probability = double.Parse(part_output["max_prob"].GetValue(x).ToString() ?? "0"),
                        Value = part_output["pred_label"].GetValue(x).ToString()
                    } : null,
                    Syndrome = predictSyndrome ? new PredictResult()
                    {
                        Id = syndrome_output["pred_label"].GetValue(x).ToString(),
                        Probability = double.Parse(syndrome_output["max_prob"].GetValue(x).ToString() ?? "0"),
                        Value = syndrome_output["syndr_id"].GetValue(x).ToString()
                    } : null
                };
                return item;
            }).ToList();

            return predictResult;
        }

        private Dictionary<string, Array> GetInputDictionary(List<string> texts)
        {
            var custom_dict = CustomizeTokenizer.GetVocabs();
            var dictKeys = custom_dict.Keys.ToList();
            var MAX_LENGTH = 256;
            var TOKEN_UNK = "[unk]";  // Token for unknown words
            var TOKEN_CLS = "[cls]"; // Token for classification
            var TOKEN_SEP = "[sep]";  // Token for separation
            var indexes = texts.Select(x =>
            {
                var tokens = CustomizeTokenizer.Tokenizer(x, ref dictKeys);
                if (tokens.Count > MAX_LENGTH - 2)
                {
                    var tokensCountRemove = tokens.Count - MAX_LENGTH + 2;

                    tokens.RemoveRange(tokens.Count - tokensCountRemove, tokensCountRemove);
                }
                tokens.Insert(0, TOKEN_CLS);
                tokens.Add(TOKEN_SEP);
                var index_list = tokens.Select(k => custom_dict.ContainsKey(k) ? custom_dict[k] : custom_dict[TOKEN_UNK]).ToList();
                var padLength = MAX_LENGTH - index_list.Count;
                if (padLength > 0)
                    index_list.AddRange(Enumerable.Repeat(0L, padLength));
                return index_list.ToArray();
            }).ToArray();

            var seges = Enumerable.Repeat(
                Enumerable.Repeat(0L, MAX_LENGTH).ToArray(),
                texts.Count).ToArray();

            var inputDict = new Dictionary<string, Array>()
            {
                { "text_indice", indexes },
                { "text_seg", seges}
            };
            return inputDict;
        }
    }
}
