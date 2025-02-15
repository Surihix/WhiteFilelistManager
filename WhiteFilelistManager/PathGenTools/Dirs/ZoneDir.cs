using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class ZoneDir
    {
        private static string ParsingErrorMsg = string.Empty;

        private static readonly List<string> _validExtensions = new List<string>()
        {
            ".wdb", ".clb", ".bin"
        };

        public static void ProcessZonePath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    ZonePathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    ZonePathXIII2LR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void ZonePathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            string extraInfo = string.Empty;

            // 8 bits
            string mainTypeBits;

            if (virtualPathData.Length > 2)
            {
                string categoryBits;
                string reservedBits;
                string zoneIDbits;

                if (startingPortion.StartsWith("zone/z"))
                {
                    // Get zone number
                    var zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    if (zoneID == -1)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = "Zone number in the wdb filename is invalid";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Zone number in the wdb filename is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (zoneID > 255)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = "Zone number in the wdb filename is too large. must be from 0 to 255.";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Zone number in the wdb filename is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (fileExtn == ".wdb")
                    {
                        mainTypeBits = Convert.ToString(64, 2).PadLeft(8, '0');

                        // 8 bits
                        categoryBits = Convert.ToString(2, 2).PadLeft(8, '0');

                        // 8 bits
                        reservedBits = "00000000";

                        // 8 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += zoneIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"ZoneID (8 bits): {zoneIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                    }
                    else if (fileExtn == ".clb")
                    {
                        mainTypeBits = Convert.ToString(96, 2).PadLeft(8, '0');

                        // 8 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(8, '0');

                        // 8 bits
                        reservedBits = "00000000";

                        // 8 bits
                        var scrID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (scrID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "scr number in the clb filename is invalid or not specified";
                            }
                            else
                            {
                                ParsingErrorMsg = $"scr number in the clb filename is invalid or not specified.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (scrID > 255)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "scr number in the clb filename is too large. must be from 0 to 255.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"scr number in the clb filename is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var scrIDbits = Convert.ToString(scrID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += scrIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"ZoneID (8 bits): {zoneIDbits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"ScrID (8 bits): {scrIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                    }
                }
                else if (startingPortion.StartsWith("zone/lip"))
                {
                    mainTypeBits = Convert.ToString(97, 2).PadLeft(8, '0');

                    // 8 bits
                    var fileNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                    if (fileNameNum == -1)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"Unable to determine file number from path";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Unable to determine file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (fileNameNum > 255)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 255.";
                        }
                        else
                        {
                            ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    var fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(8, '0');

                    // 16 bits
                    reservedBits = "0000000000000000";

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += fileNameNumBits;
                    finalComputedBits += reservedBits;

                    extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                    extraInfo += $"File number (8 bits): {fileNameNumBits}\r\n\r\n";
                    extraInfo += $"Reserved (16 bits): {reservedBits}";
                    finalComputedBits.Reverse();

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


        #region XIII-2 and XIII-LR
        private static void ZonePathXIII2LR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            var extraInfo = string.Empty;

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 2)
            {
                string categoryBits;
                string reserved2Bits;
                string zoneIDbits;

                if (startingPortion.StartsWith("zone/z"))
                {
                    // Get zone number
                    var zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    if (zoneID == -1)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = "Number in the zone folder name, is invalid or not specified";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Number in the zone folder name, is invalid or not specified.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (zoneID > 998)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = "Number in the zone folder name, is too large. must be from 0 to 998.";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Number in the zone folder name, is too large. must be from 0 to 998.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (fileExtn == ".wdb")
                    {
                        // 11 bits
                        categoryBits = Convert.ToString(1, 2).PadLeft(11, '0');

                        // 5 bits
                        reserved2Bits = "00000";

                        // 12 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reserved2Bits;
                        finalComputedBits += zoneIDbits;

                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Reserved2 (5 bits): {reserved2Bits}\r\n\r\n";
                        extraInfo += $"ZoneID (12 bits): {zoneIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "64";
                    }
                    else if (fileExtn == ".clb")
                    {
                        // 8 bits
                        reserved2Bits = "00000000";

                        // 12 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                        // 8 bits
                        var scrID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (scrID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "scr number in the clb filename is invalid or not specified";
                            }
                            else
                            {
                                ParsingErrorMsg = $"scr number in the clb filename is invalid or not specified.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (scrID > 255)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "scr number in the clb filename is too large. must be from 0 to 255.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"scr number in the clb filename is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var scrIDbits = Convert.ToString(scrID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += reserved2Bits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += scrIDbits;

                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Reserved2 (8 bits): {reserved2Bits}\r\n\r\n";
                        extraInfo += $"ZoneID (12 bits): {zoneIDbits}\r\n\r\n";
                        extraInfo += $"ScrID (8 bits): {scrIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "96";
                    }
                }
                else if (startingPortion.StartsWith("zone/lip"))
                {
                    // 8 bits
                    categoryBits = Convert.ToString(128, 2).PadLeft(8, '0');

                    // 12 bits
                    var fileNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                    if (fileNameNum == -1)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"Unable to determine file number from path";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Unable to determine file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (fileNameNum > 998)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 998.";
                        }
                        else
                        {
                            ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 998.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    var fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                    // 8 bits
                    reserved2Bits = "00000000";

                    // Assemble bits
                    finalComputedBits += reservedBits;
                    finalComputedBits += categoryBits;
                    finalComputedBits += fileNameNumBits;
                    finalComputedBits += reserved2Bits;

                    extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                    extraInfo += $"Category (8 bits): {categoryBits}\r\n\r\n";
                    extraInfo += $"File number (12 bits): {fileNameNumBits}\r\n\r\n";
                    extraInfo += $"Reserved2 (8 bits): {reserved2Bits}";
                    finalComputedBits.Reverse();

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "96";
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