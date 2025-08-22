using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class MotDir
    {
        private static GameID _gameID = new();

        public static void ProcessMotPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            _gameID = gameID;

            switch (gameID)
            {
                case GameID.xiii:
                    MotPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    MotPathXIII2LR(virtualPathData, virtualPath);
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
        private static void MotPathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".bin")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            // 4 bits
            string mainTypeBits;

            if (virtualPathData.Length > 3)
            {
                mainTypeBits = Convert.ToString(2, 2).PadLeft(4, '0');

                switch (startingPortion)
                {
                    case "mot/exte":
                    case "mot/fa":
                    case "mot/mon":
                    case "mot/npc":
                    case "mot/pc":
                    case "mot/summon":
                    case "mot/weapon":
                        // 5 bits
                        if (!ChrCategoryDict.ContainsKey(virtualPathData[1]))
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                SharedFunctions.Error("Unable to determine category from the filename. check if the mot filename, starts with a valid category string.");
                            }
                            else
                            {
                                SharedFunctions.Error($"Unable to determine category from the filename. check if the mot filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        var chrCategoryBits = Convert.ToString(ChrCategoryDict[virtualPathData[1]], 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2].Split('_')[1]);
                        GenerationFunctions.CheckDerivedNumber(modelID, "model", 999);

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 8 bits + 5 bits
                        string fileNameTypeBits;
                        string fileIDbits;

                        if (virtualPathData[3].StartsWith("lsdpack"))
                        {
                            fileNameTypeBits = "00000000";
                            fileIDbits = "01010";
                        }
                        else
                        {
                            var validStartChara = false;
                            if (GenerationFunctions.LettersList.Contains(virtualPathData[3][0]))
                            {
                                validStartChara = true;
                            }

                            if (validStartChara)
                            {
                                fileNameTypeBits = Convert.ToString(GenerationFunctions.LettersList.IndexOf(virtualPathData[3][0]), 2).PadLeft(8, '0');
                                fileIDbits = Convert.ToString(GenerationFunctions.DeriveNumFromString(virtualPathData[3]), 2).PadLeft(5, '0');
                            }
                            else
                            {
                                fileNameTypeBits = "00000000";
                                fileIDbits = "00000";
                            }
                        }

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileNameTypeBits;
                        finalComputedBits += fileIDbits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    default:
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        break;
                }
            }
        }
        #endregion


        #region XIII-2 and XIII-LR
        private static void MotPathXIII2LR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (_gameID == GameID.xiii2)
            {
                if (fileExtn != ".bin")
                {
                    SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
                }
            }
            else
            {
                var validExtensions = new List<string>
                {
                    ".bin", ".wpd"
                };

                if (!validExtensions.Contains(fileExtn))
                {
                    SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
                }
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            if (virtualPathData.Length > 3)
            {
                switch (startingPortion)
                {
                    case "mot/exte":
                    case "mot/fa":
                    case "mot/mon":
                    case "mot/npc":
                    case "mot/pc":
                    case "mot/summon":
                    case "mot/weapon":
                        // 9 bits
                        if (!ChrCategoryDict.ContainsKey(virtualPathData[1]))
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                SharedFunctions.Error("Unable to determine category from the filename. check if the mot filename, starts with a valid category string.");
                            }
                            else
                            {
                                SharedFunctions.Error($"Unable to determine category from the filename. check if the mot filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        var chrCategoryBits = Convert.ToString(ChrCategoryDict[virtualPathData[1]], 2).PadLeft(9, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2].Split('_')[1]);
                        GenerationFunctions.CheckDerivedNumber(modelID, "model", 999);

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 8 bits + 5 bits
                        string fileNameTypeBits;
                        string fileIDbits;

                        if (virtualPathData[3].StartsWith("lsdpack"))
                        {
                            fileNameTypeBits = "00000000";
                            fileIDbits = "01010";
                        }
                        else
                        {
                            var validStartChara = false;
                            if (GenerationFunctions.LettersList.Contains(virtualPathData[3][0]))
                            {
                                validStartChara = true;
                            }

                            if (validStartChara)
                            {
                                fileNameTypeBits = Convert.ToString(GenerationFunctions.LettersList.IndexOf(virtualPathData[3][0]), 2).PadLeft(8, '0');
                                fileIDbits = Convert.ToString(GenerationFunctions.DeriveNumFromString(virtualPathData[3]), 2).PadLeft(5, '0');
                            }
                            else
                            {
                                fileNameTypeBits = "00000000";
                                fileIDbits = "00000";
                            }
                        }

                        // Assemble bits
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileNameTypeBits;
                        finalComputedBits += fileIDbits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "32";
                        break;


                    default:
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        break;
                }
            }
        }
        #endregion
    }
}