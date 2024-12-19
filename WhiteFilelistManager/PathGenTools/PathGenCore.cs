using WhiteFilelistManager.PathGenTools.Dirs;
using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools
{
    internal class PathGenCore
    {
        public static string FileCodeAndTypeID { get; set; }

        public static string GenerateOutput(string virtualPath, ParseType parseType, GameID gameID)
        {
            FileCodeAndTypeID = string.Empty;

            if (string.IsNullOrEmpty(virtualPath) || string.IsNullOrWhiteSpace(virtualPath))
            {
                SharedFunctions.Error("A valid virtual path was not specified. Please specify the virtual path before using this option.");
            }

            var virtualPathData = virtualPath.Split('/');

            if (virtualPathData.Length < 2)
            {
                SharedFunctions.Error("A Valid path is not specified");
            }

            switch (virtualPathData[0])
            {
                //case "chr":
                //    ChrDir.ProcessChrPath(virtualPathData, virtualPath, gameID);
                //    break;

                case "btscene":
                    BtsceneDir.ProcessBtscenePath(virtualPathData, virtualPath, gameID);
                    break;

                //case "event":
                //    EventDir.ProcessEventPath(virtualPathData, virtualPath, gameID);
                //    break;

                //case "gui":
                //    GuiDir.ProcessGuiPath(virtualPathData, virtualPath, gameID);
                //    break;

                //case "mot":
                //    MotDir.ProcessMotPath(virtualPathData, virtualPath, gameID);
                //    break;

                //case "sound":
                //    SoundDir.ProcessSoundPath(virtualPathData, virtualPath, gameID);
                //    break;

                //case "txtres":
                //    TxtresDir.ProcessTxtresPath(virtualPathData, virtualPath, gameID);
                //    break;

                //case "vfx":
                //    VfxDir.ProcessVfxPath(virtualPathData, virtualPath, gameID);
                //    break;

                //case "zone":
                //    ZoneDir.ProcessZonePath(virtualPathData, virtualPath, gameID);
                //    break;

                default:
                    SharedFunctions.Error("Valid root directory is not specified");
                    break;
            }

            if (FileCodeAndTypeID != "")
            {

            }

            if (parseType == ParseType.json)
            {
                var jsonOutput = "{\r\n     "; 

                if (gameID == GameID.xiii)
                {
                    jsonOutput += $"\"fileCode\": {FileCodeAndTypeID},\r\n     ";
                    jsonOutput += $"\"filePath\": \"0:0:0:{virtualPath}\"\r\n";
                    jsonOutput += "}";
                }
                else
                {
                    jsonOutput += $"\"fileCode\": {FileCodeAndTypeID.Split('|')[0]},\r\n     ";
                    jsonOutput += $"\"fileTypeID\": {FileCodeAndTypeID.Split('|')[1]},\r\n     ";
                    jsonOutput += $"\"filePath\": \"0:0:0:{virtualPath}\"\r\n";
                    jsonOutput += "}\r\n";
                }

                return jsonOutput;
            }
            else
            {
                var txtOutput = string.Empty;

                if (gameID == GameID.xiii)
                {
                    txtOutput += $"{FileCodeAndTypeID}|";
                    txtOutput += $"0:0:0:{virtualPath}";
                }
                else
                {
                    txtOutput += $"{FileCodeAndTypeID.Split('|')[0]}|";
                    txtOutput += $"{FileCodeAndTypeID.Split('|')[1]}|";
                    txtOutput += $"0:0:0:{virtualPath}";
                }

                return txtOutput;
            }
        }
    }
}