using Microsoft.Extensions.Configuration;

namespace MusicKG.HondaPlugins.PredictWrapper.Settings
{
    public class VehicleFaultPredictSettings
    {
        public string Host { get; set; }

        public string Port { get; set; }

        public string PartModelName { get; set; }

        public string SyndromeModelName { get; set; }

        public string ModelSignature { get; set; }

        public string[] ModelInputs { get; set; }

        public string[] PartModelOutputs { get; set; }

        public string[] SyndromeModelOutputs { get; set; }

        public int MillisecondsTimeout { get; set; }
    }
}
