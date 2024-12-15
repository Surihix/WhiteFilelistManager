namespace WhiteFilelistManager.FilelistHelpers
{
    internal class TxtsFunctions
    {
        public static void TxtsUnpackProcess(FilelistVariables filelistVariables)
        {
            var extractedFilelistDir = Path.Combine(filelistVariables.MainFilelistDirectory, "_" + filelistVariables.FilelistOutName);

        }
    }
}