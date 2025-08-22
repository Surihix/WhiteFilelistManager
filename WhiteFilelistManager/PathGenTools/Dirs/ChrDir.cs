using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class ChrDir
    {
        public static void ProcessChrPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    ChrPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    ChrPathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    ChrPathXIIILR(virtualPathData, virtualPath);
                    break;
            }
        }

        private static readonly Dictionary<string, int> ChrCategoryDict = new()
        {
            { "pc", 2 },
            { "exte", 4 },
            { "fa", 5 },
            { "mon", 12 },
            { "npc", 13 },
            { "summon", 18 },
            { "weapon", 22 }
        };


        #region XIII
        private static void ChrPathXIII(string[] virtualPathData, string virtualPath)
        {
            var validExtensions = new List<string>
            {
                ".trb", ".imgb"
            };

            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID;
            string fileExtnBits;

            string fileCode;

            string mainTypeBits;

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "chr/fa":
                    case "chr/mon":
                    case "chr/npc":
                    case "chr/pc":
                    case "chr/summon":
                    case "chr/weapon":
                        // 4 bits
                        mainTypeBits = Convert.ToString(1, 2).PadLeft(4, '0');

                        // 5 bits
                        if (!ChrCategoryDict.ContainsKey(virtualPathData[1]))
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                SharedFunctions.Error("Unable to determine category from the filename. check if the chr filename, starts with a valid category string.");
                            }
                            else
                            {
                                SharedFunctions.Error($"Unable to determine category from the filename. check if the chr filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        var chrCategoryBits = Convert.ToString(ChrCategoryDict[virtualPathData[1]], 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(modelID, "model", 999);

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 5 bits
                        fileExtnID = fileExtn == ".imgb" ? 0 : 1;
                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits (remaining)
                        var reservedBits = "00000000";

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += reservedBits;

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


        #region XIII-2
        private static void ChrPathXIII2(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            var validExtensions = new List<string>
            {
                ".trb", ".imgb", ".mpk"
            };

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            var fileExtnID = 0;
            string fileExtnBits;

            string fileCode;

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "chr/exte":
                    case "chr/fa":
                    case "chr/mon":
                    case "chr/npc":
                    case "chr/pc":
                    case "chr/summon":
                    case "chr/weapon":
                        // 5 bits
                        if (!ChrCategoryDict.ContainsKey(virtualPathData[1]))
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                SharedFunctions.Error("Unable to determine category from the filename. check if the chr filename, starts with a valid category string.");
                            }
                            else
                            {
                                SharedFunctions.Error($"Unable to determine category from the filename. check if the chr filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        var chrCategoryBits = Convert.ToString(ChrCategoryDict[virtualPathData[1]], 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(modelID, "model", 999);

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 5 bits
                        switch (fileExtn)
                        {
                            case ".imgb":
                                fileExtnID = 0;
                                break;

                            case ".trb":
                                fileExtnID = 1;
                                break;

                            case ".mpk":
                                fileExtnID = 4;
                                break;
                        }

                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits
                        var mpkID = 0;
                        if (fileExtn == ".mpk")
                        {
                            if (virtualPath.EndsWith("_rain.win32.mpk"))
                            {
                                mpkID = 1;
                            }
                            else if (virtualPath.EndsWith("_snow.win32.mpk"))
                            {
                                mpkID = 2;
                            }
                        }
                        var mpkIDbits = Convert.ToString(mpkID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += mpkIDbits;

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


        #region XIII-LR
        private static void ChrPathXIIILR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            var validExtensions = new List<string>
            {
                ".trb", ".imgb"
            };

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID = 0;
            string fileExtnBits;

            string fileCode;

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "chr/exte":
                    case "chr/fa":
                    case "chr/mon":
                    case "chr/npc":
                    case "chr/pc":
                    case "chr/summon":
                    case "chr/weapon":
                        // 5 bits
                        if (!ChrCategoryDict.ContainsKey(virtualPathData[1]))
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                SharedFunctions.Error("Unable to determine category from the filename. check if the chr filename, starts with a valid category string.");
                            }
                            else
                            {
                                SharedFunctions.Error($"Unable to determine category from the filename. check if the chr filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        var chrCategoryBits = Convert.ToString(ChrCategoryDict[virtualPathData[1]], 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(modelID, "model", 999);

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 5 bits
                        fileExtnID = fileExtn == ".imgb" ? 0 : 1;
                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits
                        var reserved2Bits = "00000000";

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += reserved2Bits;

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
    }
}