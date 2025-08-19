using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class AcDir
    {
        private static readonly List<string> _validExtensions = new()
        {
            ".bin", ".imgb", ".xwb"
        };

        public static void ProcessAcPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    AcPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    AcPathXIII2(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void AcPathXIII(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            int zoneID;
            string zoneIDbits;

            int acID;
            string acIDbits;

            string mainTypeBits;

            if (virtualPathData.Length > 2)
            {
                // 4 bits
                mainTypeBits = Convert.ToString(13, 2).PadLeft(4, '0');

                // Get zone number
                if (virtualPathData[2].StartsWith("ac_comn"))
                {
                    zoneID = 0;
                }
                else
                {
                    var zoneName = virtualPathData[2].Split("_")[1];

                    if (ZoneMapping.XIIIZones.ContainsKey(zoneName))
                    {
                        zoneID = ZoneMapping.XIIIZones[zoneName];
                    }
                    else
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            GenerationFunctions.UserInput("Enter Zone ID", "Must be from 0 to 255", 0, 255);
                        }
                        else
                        {
                            var hasZoneID = false;

                            if (GenerationVariables.HasIdPathsTxtFile && GenerationVariables.IdBasedPathsDataDict.ContainsKey(virtualPath))
                            {
                                if (GenerationVariables.IdBasedPathsDataDict[virtualPath].Count > 0)
                                {
                                    GenerationVariables.NumInput = int.TryParse(GenerationVariables.IdBasedPathsDataDict[virtualPath][0], out int result) ? result : 0;
                                    hasZoneID = true;
                                }
                            }

                            if (!hasZoneID)
                            {
                                SharedFunctions.Error($"Unable to determine Zone ID for a file.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        zoneID = GenerationVariables.NumInput;
                    }
                }

                // Get ac number
                acID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                GenerationFunctions.CheckDerivedNumber(acID, "ac", 999);

                if (virtualPathData.Length > 4)
                {
                    switch (virtualPathData[3])
                    {
                        case "bin":
                            // 8 bits
                            zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(8, '0');

                            // 10 bits
                            acIDbits = Convert.ToString(acID, 2).PadLeft(10, '0');

                            // 10 bits
                            var fileIDbits = virtualPathData[4] == "lsdpack.bin" ? "0000000010" : "0000000000";

                            // Assemble bits
                            finalComputedBits += mainTypeBits;
                            finalComputedBits += zoneIDbits;
                            finalComputedBits += acIDbits;
                            finalComputedBits += fileIDbits;

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            break;

                        case "DataSet":
                            // 8 bits
                            zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(8, '0');

                            // 10 bits
                            acIDbits = Convert.ToString(acID, 2).PadLeft(10, '0');

                            // 9 bits
                            var dataSetIDbits = Convert.ToString(GetDataSetID(virtualPathData[4]), 2).PadLeft(9, '0');

                            // 1 bit
                            var fileTypeBit = fileExtn == ".imgb" ? "1" : "0";

                            // Assemble bits
                            finalComputedBits += mainTypeBits;
                            finalComputedBits += zoneIDbits;
                            finalComputedBits += acIDbits;
                            finalComputedBits += dataSetIDbits;
                            finalComputedBits += fileTypeBit;

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "13";
                            break;

                        default:
                            SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                            break;
                    }
                }
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        #region XIII-2
        private static void AcPathXIII2(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            var zoneID = 0;
            string zoneIDbits;

            int acID;
            string acIDbits;

            if (virtualPathData.Length > 2)
            {
                // Get zone number
                if (virtualPathData[2].StartsWith("ac_comn"))
                {
                    zoneID = 0;
                }
                else
                {
                    var zoneName = virtualPathData[2].Split("_")[1];
                    var isExistingZone = false;

                    if (ZoneMapping.XIII2Zones.ContainsKey(zoneName))
                    {
                        isExistingZone = true;
                        zoneID = ZoneMapping.XIII2Zones[zoneName];
                    }

                    if (!isExistingZone)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            GenerationFunctions.UserInput("Enter Zone ID", "Must be from 0 to 998", 0, 998);
                        }
                        else
                        {
                            var hasZoneID = false;

                            if (GenerationVariables.HasIdPathsTxtFile && GenerationVariables.IdBasedPathsDataDict.ContainsKey(virtualPath))
                            {
                                if (GenerationVariables.IdBasedPathsDataDict[virtualPath].Count > 0)
                                {
                                    GenerationVariables.NumInput = int.TryParse(GenerationVariables.IdBasedPathsDataDict[virtualPath][0], out int result) ? result : 0;
                                    hasZoneID = true;
                                }
                            }

                            if (!hasZoneID)
                            {
                                SharedFunctions.Error($"Unable to determine Zone ID for a file.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        zoneID = GenerationVariables.NumInput;
                    }
                }

                // Get ac number
                acID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                GenerationFunctions.CheckDerivedNumber(acID, "ac", 999);

                if (virtualPathData.Length > 4)
                {
                    switch (virtualPathData[3])
                    {
                        case "bin":
                            // 12 bits
                            zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                            // 10 bits
                            acIDbits = Convert.ToString(acID, 2).PadLeft(10, '0');

                            // 10 bits
                            var fileIDbits = virtualPathData[4] == "lsdpack.bin" ? "0000000010" : "0000000000";

                            // Assemble bits
                            finalComputedBits += zoneIDbits;
                            finalComputedBits += acIDbits;
                            finalComputedBits += fileIDbits;

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "208";
                            break;

                        case "DataSet":
                            // 12 bits
                            zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                            // 10 bits
                            acIDbits = Convert.ToString(acID, 2).PadLeft(10, '0');

                            // 9 bits
                            var dataSetIDbits = Convert.ToString(GetDataSetID(virtualPathData[4]), 2).PadLeft(9, '0');

                            // 1 bit
                            var fileTypeBit = fileExtn == ".imgb" ? "1" : "0";

                            // Assemble bits
                            finalComputedBits += zoneIDbits;
                            finalComputedBits += acIDbits;
                            finalComputedBits += dataSetIDbits;
                            finalComputedBits += fileTypeBit;

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "208";
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
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        private static int GetDataSetID(string fileName)
        {
            if (!GenerationFunctions.LettersList.Contains(fileName[0]))
            {
                if (GenerationVariables.GenerationType == GenerationType.single)
                {
                    SharedFunctions.Error("Unable to get letter from filename");
                }
                else
                {
                    SharedFunctions.Error($"Unable to get letter from filename.\n{GenerationVariables.PathErrorStringForBatch}");
                }
            }

            int numInFileName = GenerationFunctions.DeriveNumFromString(fileName);

            if (numInFileName == -1)
            {
                if (GenerationVariables.GenerationType == GenerationType.single)
                {
                    SharedFunctions.Error("Unable to get number from filename");
                }
                else
                {
                    SharedFunctions.Error($"Unable to get number from filename\n{GenerationVariables.PathErrorStringForBatch}");
                }
            }

            return (GenerationFunctions.LettersList.IndexOf(fileName[0]) * 100) + numInFileName + 2;
        }
    }
}