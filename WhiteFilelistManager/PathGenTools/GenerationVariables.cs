using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools
{
    internal class GenerationVariables
    {
        public static int NumInput { get; set; }
        public static string FileCode { get; set; }
        public static string FileTypeID { get; set; }
        public static string CommonExtnErrorMsg { get; set; }
        public static string CommonErrorMsg { get; set; }
        public static string PathErrorStringForBatch { get; set; }
        public static GenerationType GenerationType { get; set; }
        public static string IdBasedPathsTxtFile { get; set; }
        public static string[] IdBasedPathsData = new string[] { };
    }
}