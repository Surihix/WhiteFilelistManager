using System.IO.Compression;

namespace WhiteFilelistManager.FilelistTools.FilelistHelpers
{
    internal class ZlibFunctions
    {
        public static void ZlibDecompress(Stream cmpStreamName, Stream outStreamName)
        {
            using (var decompressor = new ZLibStream(cmpStreamName, CompressionMode.Decompress))
            {
                decompressor.CopyTo(outStreamName);
            }
        }


        public static byte[] ZlibDecompressBuffer(Stream cmpStreamName)
        {
            var dcmpBuffer = Array.Empty<byte>();

            using (var outStreamName = new MemoryStream())
            {
                cmpStreamName.Seek(0, SeekOrigin.Begin);

                using (var decompressor = new ZLibStream(cmpStreamName, CompressionMode.Decompress, true))
                {
                    decompressor.CopyTo(outStreamName);
                }

                outStreamName.Seek(0, SeekOrigin.Begin);
                dcmpBuffer = outStreamName.ToArray();
            }

            return dcmpBuffer;
        }


        public static byte[] ZlibCompress(string fileToCmp)
        {
            var dataToCompressBuffer = File.ReadAllBytes(fileToCmp);
            var compressedDataBuffer = Array.Empty<byte>();

            using (var zlibDataStream = new MemoryStream())
            {
                using (var compressor = new ZLibStream(zlibDataStream, CompressionLevel.SmallestSize, true))
                {
                    compressor.Write(dataToCompressBuffer);
                }

                compressedDataBuffer = new byte[zlibDataStream.Length];
                zlibDataStream.Seek(0, SeekOrigin.Begin);
                zlibDataStream.Read(compressedDataBuffer, 0, compressedDataBuffer.Length);
            }

            return compressedDataBuffer;
        }


        public static byte[] ZlibCompressBuffer(byte[] dataToCmp)
        {
            var compressedDataBuffer = Array.Empty<byte>();

            using (var zlibDataStream = new MemoryStream())
            {
                using (var compressor = new ZLibStream(zlibDataStream, CompressionLevel.SmallestSize, true))
                {
                    compressor.Write(dataToCmp);
                }

                compressedDataBuffer = new byte[zlibDataStream.Length];
                zlibDataStream.Seek(0, SeekOrigin.Begin);
                zlibDataStream.Read(compressedDataBuffer, 0, compressedDataBuffer.Length);
            }

            return compressedDataBuffer;
        }
    }
}