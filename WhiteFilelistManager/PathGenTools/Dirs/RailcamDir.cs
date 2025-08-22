using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    class RailcamDir
    {
        public static void ProcessRailcamPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    RailcamPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    RailcamPathXIII2(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void RailcamPathXIII(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".xwp")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            int locID;
            string locIDbits;

            int fileNameNum;
            string fileNameNumBits;

            string fileCategoryBits;

            // 4 bits
            var mainTypeBits = Convert.ToString(3, 2).PadLeft(4, '0');

            if (virtualPathData.Length > 2)
            {
                if (virtualPath.StartsWith("railcam/loc"))
                {
                    // 8 bits
                    locID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    GenerationFunctions.CheckDerivedNumber(locID, "loc", 255);

                    locIDbits = Convert.ToString(locID, 2).PadLeft(8, '0');

                    // 12 bits
                    fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                    GenerationFunctions.CheckDerivedNumber(fileNameNum, "file", 999);

                    fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                    // 8 bits
                    fileCategoryBits = Convert.ToString(5, 2).PadLeft(8, '0');

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += locIDbits;
                    finalComputedBits += fileNameNumBits;
                    finalComputedBits += fileCategoryBits;

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                }
                else
                {
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                }
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        #region XIII-2
        private static void RailcamPathXIII2(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".xwp")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            int locID;
            string locIDbits;

            int fileNameNum;
            string fileNameNumBits;

            string fileCategoryBits;

            if (virtualPathData.Length > 2)
            {
                if (virtualPath.StartsWith("railcam/loc"))
                {
                    // 12 bits
                    locID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    GenerationFunctions.CheckDerivedNumber(locID, "loc", 998);

                    locIDbits = Convert.ToString(locID, 2).PadLeft(12, '0');

                    // 12 bits
                    fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                    GenerationFunctions.CheckDerivedNumber(fileNameNum, "file", 999);

                    fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                    // 8 bits
                    fileCategoryBits = Convert.ToString(5, 2).PadLeft(8, '0');

                    // Assemble bits
                    finalComputedBits += locIDbits;
                    finalComputedBits += fileNameNumBits;
                    finalComputedBits += fileCategoryBits;

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "48";
                }
                else
                {
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                }
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion
    }
}