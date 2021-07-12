namespace MusicKG.WebApi.ClientWrapper
{
    public class CustomerFileModel
    {
        public string FileName { get; set; }

        public long FileItemCount { get; set; }

        public byte[] Content { get; set; }
    }
}
