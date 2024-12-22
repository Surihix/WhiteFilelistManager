using System.Text.Json;
using WhiteFilelistManager.Support;

namespace WhiteFilelistManager.FilelistTools.Support
{
    internal class JsonFunctions
    {
        public enum TokenTypes
        {
            Array,
            Bool,
            Number,
            Object,
            PropertyName,
            String
        }

        public static void CheckJSONProperty(ref Utf8JsonReader jsonReader, TokenTypes tokenType, string propertyName)
        {
            CheckTokenType(TokenTypes.PropertyName, ref jsonReader, propertyName);

            if (jsonReader.GetString() != propertyName)
            {
                SharedFunctions.Error($"Missing {propertyName} property at expected position");
            }

            CheckTokenType(tokenType, ref jsonReader, propertyName);
        }

        public static void CheckTokenType(TokenTypes tokenType, ref Utf8JsonReader jsonReader, string property)
        {
            _ = jsonReader.Read();

            switch (tokenType)
            {
                case TokenTypes.Array:
                    if (jsonReader.TokenType != JsonTokenType.StartArray)
                    {
                        SharedFunctions.Error($"Specified {property} property's value is not a number");
                    }
                    break;

                case TokenTypes.Bool:
                    if (jsonReader.TokenType != JsonTokenType.True)
                    {
                        if (jsonReader.TokenType != JsonTokenType.False)
                        {
                            SharedFunctions.Error($"Specified {property} property's value is not a boolean");
                        }
                    }
                    break;

                case TokenTypes.Number:
                    if (jsonReader.TokenType != JsonTokenType.Number)
                    {
                        SharedFunctions.Error($"Specified {property} property's value is not a number");
                    }
                    break;

                case TokenTypes.Object:
                    if (jsonReader.TokenType != JsonTokenType.StartObject)
                    {
                        SharedFunctions.Error($"Specified {property} property's value is not a start object");
                    }
                    break;

                case TokenTypes.PropertyName:
                    if (jsonReader.TokenType != JsonTokenType.PropertyName)
                    {
                        SharedFunctions.Error($"{property} type is not a valid PropertyName");
                    }
                    break;

                case TokenTypes.String:
                    if (jsonReader.TokenType != JsonTokenType.String)
                    {
                        SharedFunctions.Error($"Specified {property} property's value is not a string");
                    }
                    break;
            }
        }
    }
}