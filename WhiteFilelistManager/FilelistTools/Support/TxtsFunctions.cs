using WhiteFilelistManager.Support;

namespace WhiteFilelistManager.FilelistTools.Support
{
    internal class TxtsFunctions
    {
        public enum TxtValueTypes
        {
            Boolean,
            Byte,
            Uint,
            Ulong
        }

        public static void CheckPropertyInInfoFile(string propertyDataRead, string expectedPropertyName, TxtValueTypes valueType)
        {
            if (!propertyDataRead.StartsWith(expectedPropertyName))
            {
                SharedFunctions.Error($"The '{expectedPropertyName}' property in '#info.txt' file is invalid. Please check if the property is specified correctly as well as check if you have selected the correct game in this tool.");
            }

            var isValidVal = true;

            switch (valueType)
            {
                case TxtValueTypes.Boolean:
                    isValidVal = bool.TryParse(propertyDataRead.Split(' ')[1], out _);
                    break;

                case TxtValueTypes.Uint:
                    isValidVal = uint.TryParse(propertyDataRead.Split(' ')[1], out _);
                    break;

                case TxtValueTypes.Ulong:
                    isValidVal = ulong.TryParse(propertyDataRead.Split(' ')[1], out _);
                    break;           
            }

            if (!isValidVal)
            {
                SharedFunctions.Error($"Invalid value specified for '{expectedPropertyName}' property in the #info.txt file");
            }
        }


        public static void CheckChunkEntryData(string currentLine, TxtValueTypes entryValueType, int chunkId, int lineNo)
        {
            var isValidVal = true;

            switch (entryValueType)
            {
                case TxtValueTypes.Byte:
                    isValidVal = byte.TryParse(currentLine, out _);
                    break;

                case TxtValueTypes.Uint:
                    isValidVal = uint.TryParse(currentLine, out _);
                    break;
            }

            if (!isValidVal)
            {
                SharedFunctions.Error($"Invalid data found when parsing line_{lineNo} in 'Chunk_{chunkId}'.txt file. Please check if the line is specified correctly as well as check if you have selected the correct game in this tool");
            }
        }
    }
}