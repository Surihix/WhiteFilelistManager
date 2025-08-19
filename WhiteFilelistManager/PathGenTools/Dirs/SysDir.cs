using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class SysDir
    {
        public static void ProcessSysPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    SysPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    SysPathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    SysPathXIIILR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void SysPathXIII(string[] virtualPathData, string virtualPath)
        {
            var validExtensions = new List<string>
            {
                ".imgb", ".gtex"
            };

            var fileExtn = Path.GetExtension(virtualPath);

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            int fileCategory;
            string fileCategoryBits;

            var saveFileName = Path.GetFileName(virtualPath);
            int saveNameNum;
            var saveNameNumBits = string.Empty;
            int saveNameNum2;
            var saveNameNum2Bits = string.Empty;

            if (virtualPathData.Length > 5)
            {
                if (virtualPath.StartsWith("sys/x360/savedata/etc/saveicon"))
                {
                    // 8 bits
                    var mainTypeBits = Convert.ToString(72, 2).PadLeft(8, '0');

                    // 12 bits
                    fileCategory = fileExtn == ".gtex" ? 3 : 4;
                    fileCategoryBits = Convert.ToString(fileCategory, 2).PadLeft(12, '0');

                    var saveFileNameSplit = saveFileName.Split('_');

                    switch (saveFileNameSplit.Length)
                    {
                        case 3:
                            // 8 bits
                            saveNameNum = GenerationFunctions.DeriveNumFromString(saveFileNameSplit[1]);
                            GenerationFunctions.CheckDerivedNumber(saveNameNum, "save", 99);

                            saveNameNumBits = Convert.ToString(saveNameNum, 2).PadLeft(8, '0');

                            // 4 bits
                            saveNameNum2 = GenerationFunctions.DeriveNumFromString(saveFileNameSplit[2]);
                            GenerationFunctions.CheckDerivedNumber(saveNameNum, "save", 15);

                            saveNameNum2Bits = Convert.ToString(saveNameNum2, 2).PadLeft(4, '0');
                            break;

                        case 2:
                            // 8 bits
                            saveNameNum = GenerationFunctions.DeriveNumFromString(saveFileNameSplit[1]);
                            GenerationFunctions.CheckDerivedNumber(saveNameNum, "save", 99);

                            saveNameNumBits = Convert.ToString(saveNameNum, 2).PadLeft(8, '0');

                            // 4 bits
                            saveNameNum2Bits = "0000";
                            break;

                        default:
                            SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                            break;
                    }

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += fileCategoryBits;
                    finalComputedBits += saveNameNumBits;
                    finalComputedBits += saveNameNum2Bits;

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "72";
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
        private static void SysPathXIII2(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".imgb")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            string fileCategoryBits;

            var saveFileName = Path.GetFileName(virtualPath);
            int saveNameNum;
            string saveNameNumBits;

            if (virtualPathData.Length > 5)
            {
                if (virtualPath.StartsWith("sys/x360/savedata/us/saveicon"))
                {
                    // 8 bits
                    var mainTypeBits = Convert.ToString(8, 2).PadLeft(8, '0');

                    // 12 bits
                    fileCategoryBits = Convert.ToString(5, 2).PadLeft(12, '0');

                    // 8 bits
                    saveNameNum = GenerationFunctions.DeriveNumFromString(saveFileName);
                    GenerationFunctions.CheckDerivedNumber(saveNameNum, "save", 99);

                    saveNameNumBits = Convert.ToString(saveNameNum, 2).PadLeft(8, '0');

                    // 4 bits
                    var reservedBits = "0000";

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += fileCategoryBits;
                    finalComputedBits += saveNameNumBits;
                    finalComputedBits += reservedBits;

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "64";
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


        #region XIII-LR
        private static void SysPathXIIILR(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".png")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            string fileCategoryBits;

            var saveFileName = Path.GetFileName(virtualPath);
            int saveNameNum;
            string saveNameNumBits;

            if (virtualPathData.Length > 5)
            {
                if (virtualPath.StartsWith("sys/ps3/savedata/us/saveicon"))
                {
                    // 8 bits
                    var mainTypeBits = Convert.ToString(8, 2).PadLeft(8, '0');

                    // 12 bits
                    fileCategoryBits = Convert.ToString(3, 2).PadLeft(12, '0');

                    // 8 bits
                    saveNameNum = GenerationFunctions.DeriveNumFromString(saveFileName);
                    GenerationFunctions.CheckDerivedNumber(saveNameNum, "save", 99);

                    saveNameNumBits = Convert.ToString(saveNameNum, 2).PadLeft(8, '0');

                    // 4 bits
                    var reservedBits = "0000";

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += fileCategoryBits;
                    finalComputedBits += saveNameNumBits;
                    finalComputedBits += reservedBits;

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "64";
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