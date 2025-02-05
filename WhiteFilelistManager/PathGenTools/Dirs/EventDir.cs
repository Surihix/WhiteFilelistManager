using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class EventDir
    {
        private static string ParsingErrorMsg = string.Empty;

        private static readonly List<string> _validExtensions = new List<string>()
        {
            ".xwb", ".bin", ".imgb"
        };

        public static void ProcessEventPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    EventPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    EventPathXIII2LR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void EventPathXIII(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            int zoneID;
            string zoneIDbits;

            int evID;
            string evIDbits;

            // 4 bits
            var mainTypeBits = string.Empty;

            if (virtualPathData.Length > 2)
            {
                mainTypeBits = Convert.ToString(12, 2).PadLeft(4, '0');

                // Get zone number
                if (virtualPathData[2].StartsWith("ev_comn"))
                {
                    zoneID = 0;
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

                // Get event number
                evID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                if (evID == -1)
                {
                    if (GenerationVariables.GenerationType == GenerationType.single)
                    {
                        ParsingErrorMsg = "Event number in the path is invalid";
                    }
                    else
                    {
                        ParsingErrorMsg = $"Event number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                    }

                    SharedFunctions.Error(ParsingErrorMsg);
                }

                if (evID > 999)
                {
                    if (GenerationVariables.GenerationType == GenerationType.single)
                    {
                        ParsingErrorMsg = "Event number in the path is too large. must be from 0 to 999.";
                    }
                    else
                    {
                        ParsingErrorMsg = $"Event number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                    }

                    SharedFunctions.Error(ParsingErrorMsg);
                }

                if (virtualPathData.Length > 4)
                {
                    switch (virtualPathData[3])
                    {
                        case "bin":
                            // 8 bits
                            zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(8, '0');

                            // 10 bits
                            evIDbits = Convert.ToString(evID, 2).PadLeft(10, '0');

                            // 10 bits
                            var fileIDbits = virtualPathData[4] == "lsdpack.bin" ? "0000000010" : "0000000000";

                            // Assemble bits
                            finalComputedBits += mainTypeBits;
                            finalComputedBits += zoneIDbits;
                            finalComputedBits += evIDbits;
                            finalComputedBits += fileIDbits;

                            extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                            extraInfo += $"ZoneID (8 bits): {zoneIDbits}\r\n\r\n";
                            extraInfo += $"EvID (10 bits): {evIDbits}\r\n\r\n";
                            extraInfo += $"FileID (10 bits): {fileIDbits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            break;

                        case "DataSet":
                            // 8 bits
                            zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(8, '0');

                            // 10 bits
                            evIDbits = Convert.ToString(evID, 2).PadLeft(10, '0');

                            // 9 bits
                            var dataSetIDbits = Convert.ToString(GetDataSetID(virtualPathData[4]), 2).PadLeft(9, '0');

                            // 1 bit
                            var fileTypeBit = fileExtn == ".imgb" ? "1" : "0";

                            // Assemble bits
                            finalComputedBits += mainTypeBits;
                            finalComputedBits += zoneIDbits;
                            finalComputedBits += evIDbits;
                            finalComputedBits += dataSetIDbits;
                            finalComputedBits += fileTypeBit;

                            extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                            extraInfo += $"ZoneID (8 bits): {zoneIDbits}\r\n\r\n";
                            extraInfo += $"EvID (10 bits): {evIDbits}\r\n\r\n";
                            extraInfo += $"DataSetID (9 bits): {dataSetIDbits}\r\n\r\n";
                            extraInfo += $"FileType (1 bit): {fileTypeBit}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            break;

                        default:
                            SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                            break;
                    }
                }
            }
        }
        #endregion


        #region XIII-2 and XIII-LR
        private static void EventPathXIII2LR(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            int zoneID;
            string zoneIDbits;

            int evID;
            string evIDbits;

            if (virtualPathData.Length > 2)
            {
                // Get zone number
                if (virtualPathData[2].StartsWith("ev_comn"))
                {
                    zoneID = 0;
                }
                else
                {
                    if (GenerationVariables.GenerationType == GenerationType.single)
                    {
                        GenerationFunctions.UserInput("Enter Zone ID", "Must be from 0 to 1000", 0, 1000);
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

                // Get event number
                evID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                if (evID == -1)
                {
                    if (GenerationVariables.GenerationType == GenerationType.single)
                    {
                        ParsingErrorMsg = "Event number in the path is invalid";
                    }
                    else
                    {
                        ParsingErrorMsg = $"Event number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                    }

                    SharedFunctions.Error(ParsingErrorMsg);
                }

                if (evID > 999)
                {
                    if (GenerationVariables.GenerationType == GenerationType.single)
                    {
                        ParsingErrorMsg = "Event number in the path is too large. must be from 0 to 999.";
                    }
                    else
                    {
                        ParsingErrorMsg = $"Event number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                    }

                    SharedFunctions.Error(ParsingErrorMsg);
                }

                if (virtualPathData.Length > 4)
                {
                    switch (virtualPathData[3])
                    {
                        case "bin":
                            // 12 bits
                            zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                            // 10 bits
                            evIDbits = Convert.ToString(evID, 2).PadLeft(10, '0');

                            // 10 bits
                            var fileIDbits = virtualPathData[4] == "lsdpack.bin" ? "0000000010" : "0000000000";

                            // Assemble bits
                            finalComputedBits += zoneIDbits;
                            finalComputedBits += evIDbits;
                            finalComputedBits += fileIDbits;

                            extraInfo += $"ZoneID (12 bits): {zoneIDbits}\r\n\r\n";
                            extraInfo += $"EvID (10 bits): {evIDbits}\r\n\r\n";
                            extraInfo += $"FileID (10 bits): {fileIDbits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "192";
                            break;

                        case "DataSet":
                            // 12 bits
                            zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                            // 10 bits
                            evIDbits = Convert.ToString(evID, 2).PadLeft(10, '0');

                            // 9 bits
                            var dataSetIDbits = Convert.ToString(GetDataSetID(virtualPathData[4]), 2).PadLeft(9, '0');

                            // 1 bit
                            var fileTypeBit = fileExtn == ".imgb" ? "1" : "0";

                            // Assemble bits
                            finalComputedBits += zoneIDbits;
                            finalComputedBits += evIDbits;
                            finalComputedBits += dataSetIDbits;
                            finalComputedBits += fileTypeBit;

                            extraInfo += $"ZoneID (12 bits): {zoneIDbits}\r\n\r\n";
                            extraInfo += $"EvID (10 bits): {evIDbits}\r\n\r\n";
                            extraInfo += $"DataSetID (9 bits): {dataSetIDbits}\r\n\r\n";
                            extraInfo += $"FileType (1 bit): {fileTypeBit}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "192";
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
        }
        #endregion


        private static int GetDataSetID(string fileName)
        {
            if (!GenerationFunctions.LettersList.Contains(fileName[0]))
            {
                if (GenerationVariables.GenerationType == GenerationType.single)
                {
                    ParsingErrorMsg = "Unable to get letter from filename";
                }
                else
                {
                    ParsingErrorMsg = $"Unable to get letter from filename.\n{GenerationVariables.PathErrorStringForBatch}";
                }

                SharedFunctions.Error(ParsingErrorMsg);
            }

            int numInFileName = GenerationFunctions.DeriveNumFromString(fileName);

            if (numInFileName == -1)
            {
                if (GenerationVariables.GenerationType == GenerationType.single)
                {
                    ParsingErrorMsg = "Unable to get number from filename";
                }
                else
                {
                    ParsingErrorMsg = $"Unable to get number from filename\n{GenerationVariables.PathErrorStringForBatch}";
                }

                SharedFunctions.Error(ParsingErrorMsg);
            }

            return (GenerationFunctions.LettersList.IndexOf(fileName[0]) * 100) + numInFileName + 2;
        }
    }
}