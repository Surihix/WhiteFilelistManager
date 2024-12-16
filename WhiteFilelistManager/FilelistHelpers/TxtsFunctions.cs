using static WhiteFilelistManager.CoreForm;

namespace WhiteFilelistManager.FilelistHelpers
{
    internal class TxtsFunctions
    {
        public static void TxtsUnpackProcess(FilelistVariables filelistVariables, GameCode gameCode)
        {
            var extractedFilelistDir = Path.Combine(filelistVariables.MainFilelistDirectory, "_" + filelistVariables.FilelistOutName);

        }
    }
}