using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class VfxDir
    {
        private static readonly List<string> _validExtensions = new List<string>()
        {
            ".imgb", ".xfv"
        };

        public static void ProcessVfxPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    VfxPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    VfxPathXIII2LR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void VfxPathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID = 0;
            string fileExtnBits;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var mainTypeBits = string.Empty;

            if (virtualPathData.Length > 3)
            {
                switch (startingPortion)
                {
                    case "vfx/chr":
                        mainTypeBits = Convert.ToString(1, 2).PadLeft(4, '0');

                        // 5 bits
                        var chrCategoryBits = Convert.ToString(DetermineVfxChrCategory(virtualPathData[2][0]), 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (modelID == -1)
                        {
                            SharedFunctions.Error("Model number in the path is invalid");
                        }

                        if (modelID > 999)
                        {
                            SharedFunctions.Error("Model number in the path is too large. must be from 0 to 999.");
                        }

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 5 bits
                        fileExtnID = fileExtn == ".imgb" ? 2 : 3;
                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits
                        int version;
                        if (virtualPathData.Length > 3)
                        {
                            version = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

                            if (version == -1)
                            {
                                SharedFunctions.Error("Version number in the path is invalid");
                            }
                        }
                        else
                        {
                            version = 0;
                        }

                        var versionBits = Convert.ToString(version, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += versionBits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (5 bits): {chrCategoryBits}\r\n\r\n";
                        extraInfo += $"ModelID (10 bits): {modelIDbits}\r\n\r\n";
                        extraInfo += $"ModelExtension Type (5 bits): {fileExtnBits}\r\n\r\n";
                        extraInfo += $"Version (8 bits): {versionBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    default:
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        break;
                }
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        #region XIII-2 and XIII-LR
        private static void VfxPathXIII2LR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID = 0;
            string fileExtnBits;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 3)
            {
                switch (startingPortion)
                {
                    case "vfx/chr":
                        // 5 bits
                        var chrCategoryBits = Convert.ToString(DetermineVfxChrCategory(virtualPathData[2][0]), 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (modelID == -1)
                        {
                            SharedFunctions.Error("Model number in the path is invalid");
                        }

                        if (modelID > 999)
                        {
                            SharedFunctions.Error("Model number in the path is too large. must be from 0 to 999.");
                        }

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 5 bits
                        fileExtnID = fileExtn == ".imgb" ? 2 : 3;
                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits
                        int version;
                        if (virtualPathData.Length > 3)
                        {
                            version = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

                            if (version == -1)
                            {
                                SharedFunctions.Error("Version number in the path is invalid");
                            }
                        }
                        else
                        {
                            version = 0;
                        }

                        var versionBits = Convert.ToString(version, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += versionBits;

                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (5 bits): {chrCategoryBits}\r\n\r\n";
                        extraInfo += $"ModelID (10 bits): {modelIDbits}\r\n\r\n";
                        extraInfo += $"ModelExtension Type (5 bits): {fileExtnBits}\r\n\r\n";
                        extraInfo += $"Version (8 bits): {versionBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "16";
                        break;


                    default:
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        break;
                }
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        private static int DetermineVfxChrCategory(char startChara)
        {
            var categoryID = 0;

            switch (startChara)
            {
                case 'c':
                    categoryID = 2;
                    break;

                case 'f':
                    categoryID = 5;
                    break;

                case 'm':
                    categoryID = 12;
                    break;

                case 'n':
                    categoryID = 13;
                    break;

                case 's':
                    categoryID = 18;
                    break;

                case 'w':
                    categoryID = 22;
                    break;

                default:
                    SharedFunctions.Error("Unable to determine category from the filename. check if the vfx filename, starts with a valid category string.");
                    break;
            }

            return categoryID;
        }
    }
}