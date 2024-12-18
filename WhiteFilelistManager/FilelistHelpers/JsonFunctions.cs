using System.Text.Json;

namespace WhiteFilelistManager.FilelistHelpers
{
    internal class JsonFunctions
    {
        public static void CheckJSONProperty(ref Utf8JsonReader jsonReader, string tokenType, string propertyName)
        {
            CheckTokenType("PropertyName", ref jsonReader, propertyName);
            CheckPropertyName(ref jsonReader, propertyName);
            CheckTokenType(tokenType, ref jsonReader, propertyName);
        }


        private static void CheckTokenType(string tokenType, ref Utf8JsonReader jsonReader, string property)
        {
            _ = jsonReader.Read();

            switch (tokenType)
            {
                case "Array":
                    if (jsonReader.TokenType != JsonTokenType.StartArray)
                    {
                        SharedFunctions.Error($"Specified {property} property's value is not a number");
                    }
                    break;

                case "Bool":
                    if (jsonReader.TokenType != JsonTokenType.True)
                    {
                        if (jsonReader.TokenType != JsonTokenType.False)
                        {
                            SharedFunctions.Error($"Specified {property} property's value is not a boolean");
                        }
                    }
                    break;

                case "Number":
                    if (jsonReader.TokenType != JsonTokenType.Number)
                    {
                        SharedFunctions.Error($"Specified {property} property's value is not a number");
                    }
                    break;

                case "Object":
                    if (jsonReader.TokenType != JsonTokenType.StartObject)
                    {
                        SharedFunctions.Error($"Specified {property} property's value is not a start object");
                    }
                    break;

                case "PropertyName":
                    if (jsonReader.TokenType != JsonTokenType.PropertyName)
                    {
                        SharedFunctions.Error($"{property} type is not a valid PropertyName");
                    }
                    break;

                case "String":
                    if (jsonReader.TokenType != JsonTokenType.String)
                    {
                        SharedFunctions.Error($"Specified {property} property's value is not a string");
                    }
                    break;
            }
        }


        private static void CheckPropertyName(ref Utf8JsonReader jsonReader, string propertyName)
        {
            if (jsonReader.GetString() != propertyName)
            {
                SharedFunctions.Error($"Missing {propertyName} property at expected position");
            }
        }
    }
}