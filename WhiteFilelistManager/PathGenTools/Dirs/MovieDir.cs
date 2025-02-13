using System.Text;
using WhiteFilelistManager.Support;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class MovieDir
    {
        public static void ProcessMoviePath(string[] virtualPathData, string virtualPath)
        {
            var fileName = Path.GetFileName(virtualPath).Replace("_us", "");

            if (Path.GetExtension(fileName) != ".bik")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var fileNameNoExtn = Encoding.UTF8.GetBytes(Path.GetFileNameWithoutExtension(fileName));

            uint hash = 0x811C9DC5;

            for (int s = 0; s < fileNameNoExtn.Length; s++)
            {
                hash *= 0x01000193;
                hash ^= fileNameNoExtn[s];
            }

            // Assemble bits
            var hashIDbits = Convert.ToString(hash, 2).PadLeft(32, '0');
            hashIDbits.Reverse();

            var extraInfo = $"Movie Hash (32 bits): {hashIDbits}";

            var fileCode = hashIDbits.BinaryToUInt(0, 32).ToString();
            GenerationVariables.FileCode = fileCode;
            GenerationVariables.FileTypeID = "255";
        }
    }
}