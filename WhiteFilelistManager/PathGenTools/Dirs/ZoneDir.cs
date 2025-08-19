using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class ZoneDir
    {
        private static readonly List<string> _validExtensions = new()
        {
            ".bin", ".bin2", ".clb", ".wdb"
        };

        public static void ProcessZonePath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    ZonePathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    ZonePathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    ZonePathXIIILR(virtualPathData, virtualPath);
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

            string reservedBits;
            string fileCode;

            string mainTypeBits;

            if (virtualPathData.Length > 2)
            {
                string categoryBits;
                string zoneIDbits;

                if (startingPortion.StartsWith("zone/z"))
                {
                    // Get zone number
                    var zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    GenerationFunctions.CheckDerivedNumber(zoneID, "zone", 255);

                    if (fileExtn == ".wdb")
                    {
                        // 8 bits
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

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "64";
                    }
                    else if (fileExtn == ".clb")
                    {
                        // 8 bits
                        mainTypeBits = Convert.ToString(96, 2).PadLeft(8, '0');

                        // 8 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(8, '0');

                        // 8 bits
                        reservedBits = "00000000";

                        // 8 bits
                        var scrID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(scrID, "scr", 255);

                        var scrIDbits = Convert.ToString(scrID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += scrIDbits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "96";
                    }
                }
                else if (startingPortion == "zone/lip")
                {
                    // 8 bits
                    mainTypeBits = Convert.ToString(97, 2).PadLeft(8, '0');

                    // 8 bits
                    var lsdpckNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                    GenerationFunctions.CheckDerivedNumber(lsdpckNameNum, "lsdpack", 255);

                    var lsdpckNameNumBits = Convert.ToString(lsdpckNameNum, 2).PadLeft(8, '0');

                    // 16 bits
                    reservedBits = "0000000000000000";

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += lsdpckNameNumBits;
                    finalComputedBits += reservedBits;

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "97";
                }
                else
                {
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                }
            }
            else if (virtualPathData[1].StartsWith("filelist_z"))
            {
                // 8 bits
                mainTypeBits = Convert.ToString(80, 2).PadLeft(8, '0');

                // 11 bits
                var filelistNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                GenerationFunctions.CheckDerivedNumber(filelistNameNum, "filelist", 255);

                var currentFilelistID = 0;

                for (int i = 0; i < filelistNameNum + 1; i++)
                {
                    if (i == filelistNameNum)
                    {
                        break;
                    }
                    else
                    {
                        currentFilelistID += 8;
                    }
                }

                var filelistIDbits = Convert.ToString(currentFilelistID, 2).PadLeft(11, '0');

                // 5 bits
                reservedBits = "00000";

                // 8 bits
                string extnTypeBits;

                if (fileExtn == ".bin")
                {
                    if (virtualPathData[1].EndsWith("u.win32.bin"))
                    {
                        extnTypeBits = Convert.ToString(1, 2).PadLeft(8, '0');
                    }
                    else
                    {
                        extnTypeBits = Convert.ToString(2, 2).PadLeft(8, '0');
                    }
                }
                else
                {
                    if (virtualPathData[1].EndsWith("u.win32.bin2"))
                    {
                        extnTypeBits = Convert.ToString(5, 2).PadLeft(8, '0');
                    }
                    else
                    {
                        extnTypeBits = Convert.ToString(6, 2).PadLeft(8, '0');
                    }
                }

                // Assemble bits
                finalComputedBits += mainTypeBits;
                finalComputedBits += filelistIDbits;
                finalComputedBits += reservedBits;
                finalComputedBits += extnTypeBits;

                fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                GenerationVariables.FileCode = fileCode;
                GenerationVariables.FileTypeID = "80";
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        #region XIII-2
        private static void ZonePathXIII2(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            // 4 bits
            var reservedBits = "0000";

            string reservedBbits;

            if (virtualPathData.Length > 2)
            {
                string categoryBits;
                string zoneIDbits;

                if (startingPortion.StartsWith("zone/z"))
                {
                    // Get zone number
                    var zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    GenerationFunctions.CheckDerivedNumber(zoneID, "zone", 998);                  

                    if (fileExtn == ".wdb")
                    {
                        // 11 bits
                        categoryBits = Convert.ToString(1, 2).PadLeft(11, '0');

                        // 5 bits
                        reservedBbits = "00000";

                        // 12 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += zoneIDbits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "64";
                    }
                    else if (fileExtn == ".clb")
                    {
                        // 8 bits
                        reservedBbits = "00000000";

                        // 12 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                        // 8 bits
                        var scrID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(scrID, "scr", 255);

                        var scrIDbits = Convert.ToString(scrID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += scrIDbits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "96";
                    }
                }
                else if (startingPortion == "zone/lip")
                {
                    // 8 bits
                    categoryBits = Convert.ToString(128, 2).PadLeft(8, '0');

                    // 12 bits
                    var lsdpckNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                    GenerationFunctions.CheckDerivedNumber(lsdpckNameNum, "lsdpack", 998);

                    var lsdpckNameNumBits = Convert.ToString(lsdpckNameNum, 2).PadLeft(12, '0');

                    // 8 bits
                    reservedBbits = "00000000";

                    // Assemble bits
                    finalComputedBits += reservedBits;
                    finalComputedBits += categoryBits;
                    finalComputedBits += lsdpckNameNumBits;
                    finalComputedBits += reservedBbits;

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "96";
                }
                else
                {
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                }
            }
            else if (virtualPathData[1].StartsWith("filelist_z"))
            {
                // 8 bits
                reservedBbits = "00000000";

                // 12 bits
                var filelistNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                GenerationFunctions.CheckDerivedNumber(filelistNameNum, "filelist", 998);

                var filelistIDbits = Convert.ToString(filelistNameNum, 2).PadLeft(12, '0');

                // 8 bits
                string extnTypeBits;

                if (fileExtn == ".bin")
                {
                    if (virtualPathData[1].EndsWith("u.win32.bin"))
                    {
                        extnTypeBits = Convert.ToString(1, 2).PadLeft(8, '0');
                    }
                    else
                    {
                        extnTypeBits = Convert.ToString(2, 2).PadLeft(8, '0');
                    }
                }
                else
                {
                    if (virtualPathData[1].EndsWith("u.win32.bin2"))
                    {
                        extnTypeBits = Convert.ToString(9, 2).PadLeft(8, '0');
                    }
                    else
                    {
                        extnTypeBits = Convert.ToString(10, 2).PadLeft(8, '0');
                    }
                }

                // Assemble bits
                finalComputedBits += reservedBits;
                finalComputedBits += reservedBbits;
                finalComputedBits += filelistIDbits;
                finalComputedBits += extnTypeBits;

                fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                GenerationVariables.FileCode = fileCode;
                GenerationVariables.FileTypeID = "80";
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        #region XIII-LR
        private static void ZonePathXIIILR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            // 4 bits
            var reservedBits = "0000";

            string reservedBbits;

            if (virtualPathData.Length > 2)
            {
                string categoryBits;
                string zoneIDbits;

                if (startingPortion.StartsWith("zone/z"))
                {
                    // Get zone number
                    var zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                    GenerationFunctions.CheckDerivedNumber(zoneID, "zone", 998);

                    if (fileExtn == ".wdb")
                    {
                        // 11 bits
                        categoryBits = Convert.ToString(1, 2).PadLeft(11, '0');

                        // 5 bits
                        reservedBbits = "00000";

                        // 12 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += zoneIDbits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "64";
                    }
                    else if (fileExtn == ".clb")
                    {
                        // 8 bits
                        reservedBbits = "00000000";

                        // 12 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(12, '0');

                        // 8 bits
                        var scrID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(scrID, "scr", 255);

                        var scrIDbits = Convert.ToString(scrID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += scrIDbits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "96";
                    }
                }
                else if (startingPortion == "zone/lip")
                {
                    // 8 bits
                    categoryBits = Convert.ToString(128, 2).PadLeft(8, '0');

                    // 12 bits
                    var lsdpckNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                    GenerationFunctions.CheckDerivedNumber(lsdpckNameNum, "lsdpack", 998);

                    var lsdpckNameNumBits = Convert.ToString(lsdpckNameNum, 2).PadLeft(12, '0');

                    // 8 bits
                    reservedBbits = "00000000";

                    // Assemble bits
                    finalComputedBits += reservedBits;
                    finalComputedBits += categoryBits;
                    finalComputedBits += lsdpckNameNumBits;
                    finalComputedBits += reservedBbits;

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "96";
                }
                else
                {
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                }
            }
            else if (virtualPathData[1].StartsWith("filelist_z"))
            {
                // 8 bits
                reservedBbits = "00000000";

                // 12 bits
                var filelistNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);
                GenerationFunctions.CheckDerivedNumber(filelistNameNum, "filelist", 998);

                var filelistIDbits = Convert.ToString(filelistNameNum, 2).PadLeft(12, '0');

                // 8 bits
                var extnTypeBits = fileExtn == ".bin" ? "00000000" : Convert.ToString(8, 2).PadLeft(8, '0');

                // Assemble bits
                finalComputedBits += reservedBits;
                finalComputedBits += reservedBbits;
                finalComputedBits += filelistIDbits;
                finalComputedBits += extnTypeBits;

                fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                GenerationVariables.FileCode = fileCode;
                GenerationVariables.FileTypeID = "80";
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion
    }
}