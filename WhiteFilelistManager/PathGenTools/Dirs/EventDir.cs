using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class EventDir
    {
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
                SharedFunctions.Error("Path does not contain a valid file extension for this root directory");
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
                    GenerationFunctions.UserInput("Enter Zone ID", "Must be from 0 to 255", 0, 255);
                    zoneID = GenerationVariables.NumInput;
                }

                // Get event number
                evID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                if (evID == -1)
                {
                    SharedFunctions.Error("Event number in the path is invalid");
                }

                if (evID > 999)
                {
                    SharedFunctions.Error("Event number in the path is too large. must be from 0 to 999.");
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
                            SharedFunctions.Error("Unable to generate filecode. check if the path starts with a valid directory.");
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
                SharedFunctions.Error("Path does not contain a valid file extension for this root directory");
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
                    GenerationFunctions.UserInput("Enter Zone ID", "Must be from 0 to 1000", 0, 1000);
                    zoneID = GenerationVariables.NumInput;
                }

                // Get event number
                evID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                if (evID == -1)
                {
                    SharedFunctions.Error("Event number in the path is invalid");
                }

                if (evID > 999)
                {
                    SharedFunctions.Error("Event number in the path is too large. must be from 0 to 999.");
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
                            SharedFunctions.Error("Unable to generate filecode. check if the path starts with a valid directory.");
                            break;
                    }
                }
                else
                {
                    SharedFunctions.Error("Unable to generate filecode. check if the path starts with a valid directory.");
                }
            }
        }
        #endregion


        private static int GetDataSetID(string fileName)
        {           
            return (GenerationFunctions.LettersList.IndexOf(fileName[0]) * 100) + GenerationFunctions.DeriveNumFromString(fileName) + 2;
        }
    }
}