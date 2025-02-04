using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class ZoneDir
    {
        private static readonly List<string> _validExtensions = new List<string>()
        {
            ".wdb", ".clb"
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

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var mainTypeBits = string.Empty;

            if (virtualPathData.Length > 2)
            {
                if (startingPortion.StartsWith("zone/z"))
                {
                    string categoryBits;
                    string reservedBits;
                    string zoneIDbits;

                    // Get zone number
                    var zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    if (zoneID == -1)
                    {
                        SharedFunctions.Error("Zone number in the wdb filename is invalid");
                    }

                    if (zoneID > 255)
                    {
                        SharedFunctions.Error("Zone number in the wdb filename is too large. must be from 0 to 255.");
                    }

                    if (fileExtn == ".wdb")
                    {
                        mainTypeBits = Convert.ToString(4, 2).PadLeft(4, '0');

                        // 11 bits
                        categoryBits = Convert.ToString(1, 2).PadLeft(11, '0');

                        // 9 bits
                        reservedBits = "000000000";

                        // 8 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += zoneIDbits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (9 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"ZoneID (8 bits): {zoneIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                    }
                    else if (fileExtn == ".clb")
                    {
                        mainTypeBits = Convert.ToString(6, 2).PadLeft(4, '0');

                        // 12 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                        // 8 bits
                        reservedBits = "00000000";

                        // 8 bits
                        var scrID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (scrID == -1)
                        {
                            SharedFunctions.Error("scr number in the clb filename is invalid or not specified");
                        }

                        if (scrID > 255)
                        {
                            SharedFunctions.Error("scr number in the clb filename is too large. must be from 0 to 255.");
                        }

                        var scrIDbits = Convert.ToString(scrID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += scrIDbits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"ZoneID (12 bits): {zoneIDbits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"ScrID (8 bits): {scrIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                    }
                }
                else
                {
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                }
            }
            else
            {
                // Assume its a filelist file
                SharedFunctions.Error("not implemented");
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

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 2)
            {
                if (startingPortion.StartsWith("zone/z"))
                {
                    string categoryBits;
                    string reserved2Bits;
                    string zoneIDbits;

                    // Get zone number
                    var zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    if (zoneID == -1)
                    {
                        SharedFunctions.Error("Number in the zone folder name, is invalid or not specified");
                    }

                    if (zoneID > 1000)
                    {
                        SharedFunctions.Error("Number in the zone folder name, is too large. must be from 0 to 1000.");
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
                            SharedFunctions.Error("scr number in the clb filename is invalid or not specified");
                        }

                        if (scrID > 255)
                        {
                            SharedFunctions.Error("scr number in the clb filename is too large. must be from 0 to 255.");
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
                else
                {
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                }
            }
            else
            {
                // Assume its a filelist file
                SharedFunctions.Error("not implemented");
            }
        }
        #endregion
    }
}