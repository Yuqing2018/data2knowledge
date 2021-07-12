//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------MusicKG.DataManager.Predictor.

using Google.Protobuf;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tensorflow;
using Tensorflow.Serving;

namespace MusicKG.Tensorflow.Wrapper
{
    public class TensorflowServingModel: IDisposable
    {
        private readonly Channel _channel;
        private readonly PredictionService.PredictionServiceClient _client;
        private readonly string _modelName;
        private readonly string _signature;
        private readonly string[] _inputKeys;
        private readonly string[] _outputKeys;
        private readonly int _timeout;

        public TensorflowServingModel(string host,
                                      string port,
                                      string modelName,
                                      string signature,
                                      string[] inputKeys,
                                      string[] outputKeys,
                                      int timeout = 5000
                                      )
        {
            if (string.IsNullOrEmpty(host)
                || string.IsNullOrEmpty(port)
                || string.IsNullOrEmpty(modelName)
                || string.IsNullOrEmpty(signature)
                || inputKeys == null
                || !inputKeys.Any()
                || outputKeys == null
                || !outputKeys.Any()
                || timeout <= 0)
            {
                throw new ArgumentException("TensorflowServingModel invalid construction parameter!");
            }

            _channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
            _client = new PredictionService.PredictionServiceClient(_channel);
            _modelName = modelName;
            _signature = signature;
            _inputKeys = inputKeys;
            _outputKeys = outputKeys;
            _timeout = timeout;
        }

        public void Dispose()
        {
            Console.WriteLine("Shutting down GPRC channel.");
            _channel.ShutdownAsync().GetAwaiter().GetResult();
            Console.WriteLine("GRPC channel shutted down.");
        }

        public async Task<bool> IsModelAvailableAsync(long version = -1)
        {
            try
            {
                var currentVersion = await GetModelVersionAsync();
                if (currentVersion == null || (version > 0 && currentVersion != version))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<long?> GetModelVersionAsync()
        {
            var request = new GetModelMetadataRequest()
            {
                ModelSpec = new ModelSpec() { Name = _modelName },
                MetadataField = { "signature_def" }
            };
            var response = await _client.GetModelMetadataAsync(request);
            return response.ModelSpec.Version;
        }

        public async Task<Array> GetModelOutputAsync(string inputData)
        {
            if (_outputKeys.Length != 1)
            {
                throw new ArgumentException("More than 1 output keys are defined which may lead to confusion when you call this function!");
            }

            var outputs = await GetModelOutputsAsync(inputData);
            return outputs[_outputKeys.First()];
        }

        public async Task<Array> GetModelOutputAsync(List<string> inputData, int batchSize = 1)
        {
            if (_outputKeys.Length != 1)
            {
                throw new ArgumentException("More than 1 output keys are defined which may lead to confusion when you call this function!");
            }

            var outputs = await GetModelOutputsAsync(inputData, batchSize);
            return outputs[_outputKeys.First()];
        }

        public async Task<Array> GetModelOutputAsync(Array inputData, int batchSize = 1)
        {
            if (_outputKeys.Length != 1)
            {
                throw new ArgumentException("More than 1 output keys are defined which may lead to confusion when you call this function!");
            }

            var outputs = await GetModelOutputsAsync(inputData, batchSize);
            return outputs[_outputKeys.First()];
        }

        public async Task<Dictionary<string, Array>> GetModelOutputsAsync(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
            {
                throw new ArgumentException("Invalid input: inputData");
            }

            var modelOutputs = await GetModelOutputsAsync(new string[] { inputData });

            foreach (var outputKey in _outputKeys)
            {
                var originalArray = modelOutputs[outputKey];
                var squeezeArray = SqueezeBatchSize(originalArray);
                modelOutputs[outputKey] = squeezeArray;
            }

            return modelOutputs;
        }

        public async Task<Dictionary<string, Array>> GetModelOutputsAsync(List<string> inputData, int batchSize = 1)
        {
            if (inputData == null || !inputData.Any() || inputData.Any(m => string.IsNullOrEmpty(m)))
            {
                throw new ArgumentException("Invalid input: inputData");
            }

            return await GetModelOutputsAsync(inputData.ToArray(), batchSize);
        }

        public async Task<Dictionary<string, Array>> GetModelOutputsAsync(Array inputData, int batchSize = 1)
        {
            if (_inputKeys.Length != 1)
            {
                throw new ArgumentException("More than 1 input keys are required in model definition but only got 1 input array!");
            }

            var dict = new Dictionary<string, Array>
            {
                { _inputKeys.First(), inputData }
            };

            return await GetModelOutputsAsync(dict, batchSize);
        }

        public async Task<Dictionary<string, Array>> GetModelOutputsAsync(Dictionary<string, Array> inputData, int batchSize = 1)
        {
            if (inputData == null || !inputData.Any())
            {
                throw new ArgumentException("Invalid input: inputData");
            }

            var requiredInput = new Dictionary<string, Array>();
            foreach (var inputKey in _inputKeys)
            {
                if (inputData.ContainsKey(inputKey))
                {
                    var array = inputData[inputKey];
                    if (array == null)
                    {
                        throw new ArgumentException($"Input key = [{inputKey}] has null Array value");
                    }
                    else
                    {
                        foreach (var arrayValue in array)
                        {
                            if (arrayValue == null)
                            {
                                throw new ArgumentException($"Input key = [{inputKey}] Array contains null value");
                            }
                        }
                        requiredInput.Add(inputKey, array);
                    }
                }
                else
                {
                    throw new ArgumentException($"Missing required input key = [{inputKey}]");
                }
            }

            Console.WriteLine("Generate reuqired input.");

            var batches = BatchSplit(requiredInput, batchSize);

            Console.WriteLine("Split reuqired input to batches.");

            var batchArrays = new Dictionary<string, List<Array>>();
            foreach (var outputKey in _outputKeys)
            {
                batchArrays.Add(outputKey, new List<Array>());
            }

            foreach (var batch in batches)
            {
                var input = EncodeInput(batch);

                var request = new PredictRequest()
                {
                    ModelSpec = new ModelSpec()
                    {
                        Name = _modelName,
                        SignatureName = _signature
                    }
                };  

                foreach (var inputKey in _inputKeys)
                {
                    request.Inputs.Add(inputKey, input[inputKey]);
                }

                Console.WriteLine($"Predicting request, channel status {_channel.State}");

                var response = await _client.PredictAsync(request, deadline: DateTime.UtcNow.AddMinutes(10));

                Console.WriteLine($"Request predict succeed, channel status {_channel.State}");

                foreach (var outputKey in _outputKeys)
                {
                    var output = response.Outputs[outputKey];
                    var array = DecodeOutput(output);
                    batchArrays[outputKey].Add(array);
                }
            }

            var results = new Dictionary<string, Array>();
            foreach (var outputKey in _outputKeys)
            {
                var result = BatchMerge(batchArrays[outputKey]);
                results.Add(outputKey, result);
            }

            return results;
        }

        public static List<float[]> Convert2dArrayToFloatList(Array outputs)
        {
            if (outputs == null)
            {
                throw new ArgumentNullException($"{nameof(outputs)} must not be null");
            }

            if (outputs.Rank != 2)
            {
                throw new ArgumentException($"{nameof(outputs)}'s rank must be 2.");
            }

            var type = outputs.GetType().GetElementType();

            if (type != typeof(float))
            {
                throw new ArgumentException("Only support float matrix convert.");
            }

            var results = new List<float[]>();
            var embeddingDim = outputs.GetLength(1);
            var rowCount = outputs.GetLength(0);
            var rowSize = Marshal.SizeOf<float>() * embeddingDim;
            for (int i = 0; i < rowCount; i++)
            {
                var result = new float[embeddingDim];
                Buffer.BlockCopy(outputs, rowSize * i, result, 0, rowSize);
                results.Add(result);
            }

            return results;
        }

        private List<Dictionary<string, Array>> BatchSplit(Dictionary<string, Array> dataMatrix, int batchSize = 1)
        {
            var result = new List<Dictionary<string, Array>>();

            foreach (var pair in dataMatrix)
            {
                var batches = BatchSplit(pair.Value, batchSize);
                for (var i = 0; i < batches.Count; i++)
                {
                    if (i >= result.Count)
                    {
                        result.Add(new Dictionary<string, Array>());
                    }
                    result[i].Add(pair.Key, batches[i]);
                }
            }

            return result;
        }

        private List<Array> BatchSplit(Array dataMatrix, int batchSize = 1)
        {
            var result = new List<Array>();

            var numData = dataMatrix.GetLength(0);
            var numValues = dataMatrix.Length;
            var numValuePerData = numValues / numData;


            var batchShape = new int[dataMatrix.Rank];
            batchShape[0] = batchSize;
            for (var i = 1; i < dataMatrix.Rank; i++)
            {
                batchShape[i] = dataMatrix.GetLength(i);
            }

            var type = dataMatrix.GetType().GetElementType();
            var batchLength = numValuePerData * batchSize;
            var totalLength = numValues;
            var offset = 0;
            while (offset < totalLength)
            {
                if (offset + batchLength > totalLength)
                {
                    batchLength = totalLength - offset;
                    batchShape[0] = batchLength / numValuePerData;
                }

                var array = Array.CreateInstance(type, batchShape);
                Array.Copy(dataMatrix, offset, array, 0, batchLength);
                result.Add(array);
                offset += batchLength;
            }

            return result;
        }

        private Dictionary<string, TensorProto> EncodeInput(Dictionary<string, Array> input)
        {
            var result = new Dictionary<string, TensorProto>();

            foreach (var pair in input)
            {
                result.Add(pair.Key, EncodeInput(pair.Value));
            }

            return result;
        }

        private TensorProto EncodeInput(Array input)
        {
            var inputDataType = input.GetType().GetElementType();
            var inputTensor = new TensorProto
            {
                TensorShape = new TensorShapeProto()
            };
            if (inputDataType == typeof(System.String))
            {
                inputTensor.Dtype = DataType.DtString;
                foreach (var value in input)
                {
                    inputTensor.StringVal.Add(ByteString.CopyFrom(Convert.ToString(value), Encoding.UTF8));
                }
            }
            else if (inputDataType == typeof(System.Single))
            {
                inputTensor.Dtype = DataType.DtFloat;
                foreach (var value in input)
                {
                    inputTensor.FloatVal.Add(Convert.ToSingle(value));
                }
            }
            else if (inputDataType == typeof(System.Int32))
            {
                inputTensor.Dtype = DataType.DtInt32;
                foreach (var value in input)
                {
                    inputTensor.IntVal.Add(Convert.ToInt32(value));
                }
            }
            else if (inputDataType == typeof(System.Int64))
            {
                inputTensor.Dtype = DataType.DtInt64;
                foreach (var value in input)
                {
                    inputTensor.Int64Val.Add(Convert.ToInt64(value));
                }
            }
            else if (inputDataType == typeof(System.Int64[]))
            {
                inputTensor.TensorShape.Dim.Add(new TensorShapeProto.Types.Dim() { Size = input.Length });
                inputTensor.TensorShape.Dim.Add(new TensorShapeProto.Types.Dim() { Size = 256 });
                inputTensor.Dtype = DataType.DtInt64;
                foreach (var value in input)
                {
                    var trueValue = value as Int64[];
                    foreach (var item in trueValue)
                    {
                        inputTensor.Int64Val.Add(Convert.ToInt64(item));
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Not supported input data type {inputDataType.ToString()}");
            }

            if (inputDataType != typeof(System.Int64[]))
            {
                for (var i = 0; i < input.Rank; i++)
                {
                    inputTensor.TensorShape.Dim.Add(new TensorShapeProto.Types.Dim() { Size = input.GetLength(i) });
                }
            }

            return inputTensor;
        }

        private Array DecodeOutput(TensorProto output)
        {
            Array array;
            if (output.Dtype == DataType.DtString)
            {
                array = output.StringVal.Select(m => m.ToStringUtf8()).ToArray();
            }
            else if (output.Dtype == DataType.DtFloat)
            {
                array = output.FloatVal.Select(m => Convert.ToSingle(m)).ToArray();
            }
            else if (output.Dtype == DataType.DtInt32)
            {
                array = output.IntVal.Select(m => Convert.ToInt32(m)).ToArray();
            }
            else if (output.Dtype == DataType.DtInt64)
            {
                array = output.Int64Val.Select(m => Convert.ToInt64(m)).ToArray();
            }
            else
            {
                throw new NotSupportedException($"Not supported output data type {output.Dtype.ToString()}");
            }

            var shape = output.TensorShape.Dim.Select(m => (int)m.Size).ToArray();
            return Reshape(array, shape);
        }

        public static Array BatchMerge(List<Array> arrays)
        {
            if (arrays == null || !arrays.Any())
            {
                throw new ArgumentException("Invalid input for BatchMerge");
            }

            var sample = arrays.First();
            if (arrays.Count == 1)
            {
                return sample;
            }

            var type = sample.GetType().GetElementType();
            var rank = sample.Rank;
            var shape = new int[rank];
            for (var i = 1; i < rank; i++)
            {
                shape[i] = sample.GetLength(i);
            }

            foreach (var array in arrays)
            {
                if (array.GetType().GetElementType() != type)
                {
                    throw new ArgumentException("All arrays to merge should have the same data type");
                }

                if (array.Rank != rank)
                {
                    throw new ArgumentException("All arrays to merge should have the same rank");
                }

                for (var i = 1; i < rank; i++)
                {
                    if (array.GetLength(i) != shape[i])
                    {
                        throw new ArgumentException("All arrays to merge should have the same dimentions except for batch_size");
                    }
                }

                shape[0] += array.GetLength(0);
            }
            
            var result = Array.CreateInstance(type, shape);
            var offset = 0;
            foreach (var array in arrays)
            {
                Array.Copy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }

            return result;
        }

        private Array Reshape(Array arr, int[] newSizes)
        {
            if (arr.Rank == newSizes.Length)
            {
                var isSameShape = true;
                for (var i = 0; i < arr.Rank; i++)
                {
                    if (arr.GetLength(i) != newSizes[i])
                    {
                        isSameShape = false;
                        break;
                    }
                }
                if (isSameShape)
                {
                    return arr;
                }
            }

            var type = arr.GetType().GetElementType();
            if (type == typeof(System.String))
            {
                throw new NotImplementedException("String matrix reshape is not supported now");
            }

            var oldLength = arr.Length;
            var newLength = newSizes.Aggregate((a, x) => a * x);
            if (oldLength != newLength)
            {
                throw new ArgumentException("Reshape old array and new array should have same number of elements");
            }

            var result = Array.CreateInstance(type, newSizes);
            Buffer.BlockCopy(arr, 0, result, 0, newLength * Marshal.SizeOf(type));
            return result;
        }

        private Array SqueezeBatchSize(Array array)
        {
            if (array.GetLength(0) != 1 || array.Rank <= 1)
            {
                return array;
            }

            var squeezeShape = new int[array.Rank - 1];
            for (var i = 1; i < array.Rank; i++)
            {
                squeezeShape[i - 1] = array.GetLength(i);
            }

            var squeezeArray = Reshape(array, squeezeShape);
            return squeezeArray;
        }
    }
}
