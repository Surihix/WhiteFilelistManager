namespace WhiteFilelistManager
{
    internal class SharedFunctions
    {
        public static void IfFileExistsDel(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}