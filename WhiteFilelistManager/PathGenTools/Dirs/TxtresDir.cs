using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class TxtresDir
    {
        private static string ParsingErrorMsg = string.Empty;

        public static void ProcessTxtresPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            if (Path.GetExtension(virtualPath) != ".ztr")
            {
                SharedFunctions.Error("Path does not contain a valid file extension");
            }

            switch (gameID)
            {
                case GameID.xiii:
                    TxtresPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    TxtresPathXIII2LR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void TxtresPathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];

            var finalComputedBits = string.Empty;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            string langIDbits;
            int zoneID;
            string zoneIDbits;

            // 8 bits
            var mainTypeBits = string.Empty;

            if (virtualPathData.Length > 2)
            {
                if (virtualPathData[2].StartsWith("ac_comn") || virtualPathData[2].StartsWith("ev_comn"))
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
                            if (GenerationVariables.IdBasedPathsDataDict[virtualPath].Count > 1)
                            {
                                GenerationVariables.NumInput = int.Parse(GenerationVariables.IdBasedPathsDataDict[virtualPath][1]);
                                hasZoneID = true;
                            }
                            else
                            {
                                hasZoneID = false;
                            }
                        }

                        if (!hasZoneID)
                        {
                            SharedFunctions.Error($"Unable to determine Zone ID for a file.\n{GenerationVariables.PathErrorStringForBatch}");
                        }
                    }

                    zoneID = GenerationVariables.NumInput;
                }

                switch (startingPortion)
                {
                    case "txtres/ac":
                        mainTypeBits = Convert.ToString(228, 2).PadLeft(8, '0');

                        // 4 bits
                        langIDbits = Convert.ToString(GetLangID(Path.GetFileName(virtualPath)), 2).PadLeft(4, '0');

                        // 10 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(10, '0');

                        // 10 bits
                        var acID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                        if (acID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "ac number in the path is invalid";
                            }
                            else
                            {
                                ParsingErrorMsg = $"ac number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (acID > 999)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "ac number in the path is too large. must be from 0 to 999.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"ac number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var acIDbits = Convert.ToString(acID, 2).PadLeft(10, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += langIDbits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += acIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"LanguageID (4 bits): {langIDbits}\r\n\r\n";
                        extraInfo += $"ZoneID (10 bits): {zoneIDbits}\r\n\r\n";
                        extraInfo += $"AcID (10 bits): {acIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "txtres/event":
                        mainTypeBits = Convert.ToString(227, 2).PadLeft(8, '0');

                        // 4 bits
                        langIDbits = Convert.ToString(GetLangID(Path.GetFileName(virtualPath)), 2).PadLeft(4, '0');

                        // 10 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(10, '0');

                        // 10 bits
                        var evID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

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

                        var evIDbits = Convert.ToString(evID, 2).PadLeft(10, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += langIDbits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += evIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"LanguageID (4 bits): {langIDbits}\r\n\r\n";
                        extraInfo += $"ZoneID (10 bits): {zoneIDbits}\r\n\r\n";
                        extraInfo += $"EvID (10 bits): {evIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "txtres/zone":
                        mainTypeBits = Convert.ToString(229, 2).PadLeft(8, '0');

                        // 4 bits
                        langIDbits = Convert.ToString(GetLangID(Path.GetFileName(virtualPath)), 2).PadLeft(4, '0');

                        // 10 bits
                        var reservedBits = "0000000000";

                        // 10 bits
                        zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                        if (zoneID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "Zone number in the path is invalid";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Zone number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(10, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += langIDbits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += zoneIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"LanguageID (4 bits): {langIDbits}\r\n\r\n";
                        extraInfo += $"Reserved (10 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"ZoneID (10 bits): {zoneIDbits}";
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
        private static void TxtresPathXIII2LR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];

            var finalComputedBits = string.Empty;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            string langIDbits;
            string categoryBits;

            int zoneID;
            string zoneIDbits;

            string reservedBits;

            if (virtualPathData.Length > 2)
            {
                if (virtualPathData[2].StartsWith("ac_comn") || virtualPathData[2].StartsWith("ev_comn"))
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
                            if (GenerationVariables.HasIdPathsTxtFile && GenerationVariables.IdBasedPathsDataDict.ContainsKey(virtualPath))
                            {
                                GenerationVariables.NumInput = int.Parse(GenerationVariables.IdBasedPathsDataDict[virtualPath][1]);
                                hasZoneID = true;
                            }
                            else
                            {
                                hasZoneID = false;
                            }
                        }

                        if (!hasZoneID)
                        {
                            SharedFunctions.Error($"Unable to determine Zone ID for a file.\n{GenerationVariables.PathErrorStringForBatch}");
                        }
                    }

                    zoneID = GenerationVariables.NumInput;
                }

                switch (startingPortion)
                {
                    case "txtres/ac":
                        // 4 bits
                        langIDbits = Convert.ToString(GetLangID(Path.GetFileName(virtualPath)), 2).PadLeft(4, '0');

                        // 4 bits
                        categoryBits = Convert.ToString(4, 2).PadLeft(4, '0');

                        // 4 bits
                        reservedBits = "0000";

                        // 10 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(10, '0');

                        // 10 bits
                        var acID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                        if (acID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "ac number in the path is invalid";
                            }
                            else
                            {
                                ParsingErrorMsg = $"ac number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (acID > 999)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "ac number in the path is too large. must be from 0 to 999.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"ac number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var acIDbits = Convert.ToString(acID, 2).PadLeft(10, '0');

                        // Assemble bits
                        finalComputedBits += langIDbits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += acIDbits;

                        extraInfo += $"LanguageID (4 bits): {langIDbits}\r\n\r\n";
                        extraInfo += $"Category (4 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"ZoneID (10 bits): {zoneIDbits}\r\n\r\n";
                        extraInfo += $"AcID (10 bits): {acIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "224";
                        break;


                    case "txtres/event":
                        // 4 bits
                        langIDbits = Convert.ToString(GetLangID(Path.GetFileName(virtualPath)), 2).PadLeft(4, '0');

                        // 4 bits
                        categoryBits = Convert.ToString(3, 2).PadLeft(4, '0');

                        // 4 bits
                        reservedBits = "0000";

                        // 10 bits
                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(10, '0');

                        // 10 bits
                        var evID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

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

                        var evIDbits = Convert.ToString(evID, 2).PadLeft(10, '0');

                        // Assemble bits
                        finalComputedBits += langIDbits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += zoneIDbits;
                        finalComputedBits += evIDbits;

                        extraInfo += $"LanguageID (4 bits): {langIDbits}\r\n\r\n";
                        extraInfo += $"Category (4 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"ZoneID (10 bits): {zoneIDbits}\r\n\r\n";
                        extraInfo += $"EvID (10 bits): {evIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "224";
                        break;


                    case "txtres/zone":
                        // 4 bits
                        langIDbits = Convert.ToString(GetLangID(Path.GetFileName(virtualPath)), 2).PadLeft(4, '0');

                        // 4 bits
                        categoryBits = Convert.ToString(5, 2).PadLeft(4, '0');

                        // 14 bits
                        reservedBits = "00000000000000";

                        // 10 bits
                        zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                        if (zoneID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "Zone number in the path is invalid";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Zone number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(10, '0');

                        // Assemble bits
                        finalComputedBits += langIDbits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += zoneIDbits;

                        extraInfo += $"LanguageID (4 bits): {langIDbits}\r\n\r\n";
                        extraInfo += $"Category (4 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (14 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"ZoneID (10 bits): {zoneIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "224";
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


        private static int GetLangID(string fileName)
        {
            var langID = 0;

            switch (fileName)
            {
                case "txtres_jp.ztr":
                    langID = 0;
                    break;

                case "txtres_us.ztr":
                    langID = 1;
                    break;

                case "txtes_uk.ztr":
                    langID = 2;
                    break;

                case "txtres_it.ztr":
                    langID = 3;
                    break;

                case "txtres_gr.ztr":
                    langID = 4;
                    break;

                case "txtres_fr.ztr":
                    langID = 5;
                    break;

                case "txtres_sp.ztr":
                    langID = 6;
                    break;

                case "txtres_ru.ztr":
                    langID = 7;
                    break;

                case "txtres_kr.ztr":
                    langID = 8;
                    break;

                case "txtres_ck.ztr":
                    langID = 9;
                    break;

                case "txtres_ch.ztr":
                    langID = 10;
                    break;

                default:
                    SharedFunctions.Error("Unable to determine the language from the filename. check if the ztr filename, starts with a valid language id.");
                    break;
            }

            return langID;
        }
    }
}