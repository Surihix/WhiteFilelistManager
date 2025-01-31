using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools
{
    internal class GenerationVariables
    {
        public static int NumInput { get; set; }

        public static string FileCode { get; set; }
        public static string FileTypeID { get; set; }
        public static GenerationType GenerationType { get; set; }
    }
}