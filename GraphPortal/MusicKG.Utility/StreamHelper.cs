using System.IO;

namespace MusicKG.Utility
{
    public class StreamHelper
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
    }
}
