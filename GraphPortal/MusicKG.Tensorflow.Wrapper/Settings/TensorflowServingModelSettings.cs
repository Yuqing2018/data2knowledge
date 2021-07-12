namespace MusicKG.Tensorflow.Wrapper.Settings
{
    public class TensorflowServingModelSettings
    {
        public string Host { get; set; }

        public string Port { get; set; }

        public string ModelName { get; set; }

        public string ModelSignature { get; set; }

        public string[] ModelInputs { get; set; }

        public string[] ModelOutputs { get; set; }

        public int MillisecondsTimeout { get; set; }
    }
}
