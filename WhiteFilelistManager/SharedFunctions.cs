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


        public static void Error(string errorMsg)
        {
            CoreForm.CoreFormInstance.Invoke(new Action(() => MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
            throw new Exception("Handled");
        }
    }
}