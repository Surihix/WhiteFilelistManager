using WhiteFilelistManager.PathGenTools.Dirs;
using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools
{
    internal class PathGenCore
    {
        public static string GenerateOutput(string virtualPath, ParseType parseType, GameID gameID)
        {
            GenerationVariables.FileCode = string.Empty;
            GenerationVariables.FileTypeID = string.Empty;

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
                case "btscene":
                    BtsceneDir.ProcessBtscenePath(virtualPathData, virtualPath, gameID);
                    break;

                case "chr":
                    ChrDir.ProcessChrPath(virtualPathData, virtualPath, gameID);
                    break;

                case "event":
                    EventDir.ProcessEventPath(virtualPathData, virtualPath, gameID);
                    break;

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

            if (parseType == ParseType.json)
            {
                var jsonOutput = "{\r\n     "; 

                if (gameID == GameID.xiii)
                {
                    jsonOutput += $"\"fileCode\": {GenerationVariables.FileCode},\r\n     ";
                    jsonOutput += $"\"filePath\": \"0:0:0:{virtualPath}\"\r\n";
                    jsonOutput += "}";
                }
                else
                {
                    jsonOutput += $"\"fileCode\": {GenerationVariables.FileCode},\r\n     ";
                    jsonOutput += $"\"fileTypeID\": {GenerationVariables.FileTypeID},\r\n     ";
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
                    txtOutput += $"{GenerationVariables.FileCode}|";
                    txtOutput += $"0:0:0:{virtualPath}";
                }
                else
                {
                    txtOutput += $"{GenerationVariables.FileCode}|";
                    txtOutput += $"{GenerationVariables.FileTypeID}|";
                    txtOutput += $"0:0:0:{virtualPath}";
                }

                return txtOutput;
            }
        }
    }
}