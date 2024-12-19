using WhiteFilelistManager.Support;

namespace WhiteFilelistManager.PathGenTools
{
    internal class GenerationFunctions
    {
        public static readonly List<char> LettersList = new List<char>
        {
            'a','b','c','d','e','f','g','h','i','j','k','l','m',
            'n','o','p','q','r','s','t','u','v','w','x','y','z'
        };


        public static int DeriveNumFromString(string numberedString)
        {
            var foundNumsList = new List<int>();

            for (int i = 0; i < numberedString.Length; i++)
            {
                if (numberedString[i] == '.')
                {
                    break;
                }

                if (numberedString[i] == ' ')
                {
                    SharedFunctions.Error("Number contains spaces");
                    break;
                }

                if (char.IsDigit(numberedString[i]))
                {
                    foundNumsList.Add(int.Parse(Convert.ToString(numberedString[i])));
                }
            }

            var foundNumStr = "";
            foreach (var n in foundNumsList)
            {
                foundNumStr += n;
            }

            var hasParsed = int.TryParse(foundNumStr, out int foundNum);

            if (hasParsed)
            {
                return foundNum;
            }
            else
            {
                return -1;
            }
        }


        public static void UserInput(string formTitle, string rangeTxt, int min, int max)
        {
            var numInputForm = new InputForm(formTitle, rangeTxt, min, max);
            System.Media.SystemSounds.Asterisk.Play();
            numInputForm.ShowDialog();
        }
    }
}