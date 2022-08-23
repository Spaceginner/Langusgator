using System.Text;

namespace SPlane
{
    namespace Translator
    {
        class Ugubugu
        {
            public static void Main()
            {
                Console.Write("What do you want to do?\n1. Translate to Ugu-bugu language\n2. Translate from Ugu-bugu language\n(Type [1] or [2])\n>>> ");
                var option = int.Parse(Console.ReadLine().Trim());

                Console.WriteLine("\n--------------------------------");

                switch (option)
                {
                    case 1:
                        Console.Write("\nDo you want a translated text to be in english?\n(Type [y] (Yes) or [n] (No))\n>>> ");
                        bool isTextInEnglish;
                        _ = Console.ReadLine().Trim() == "n"
                            ? isTextInEnglish = false
                            : isTextInEnglish = true;

                        Console.Write("\nType text that you want to translate to Ugu-bugu language and press enter\n>>> ");
                        var textToTranslate = Console.ReadLine();

                        Console.WriteLine("\n================================\nTranslated text: {0}", TranslateTo(textToTranslate, GetTranslationCharlist(), isTextInEnglish));
                        break;
                    case 2:
                        Console.Write("\nType text that you want to translate from Ugu-bugu language and press enter\n>>> ");
                        textToTranslate = Console.ReadLine();
                        Console.WriteLine("\n================================\nTranslated text: {0}", TranslateFrom(textToTranslate, GetTranslationCharlist()));
                        break;
                }

                Console.WriteLine("\n\nPRESS ENTER TO EXIT...");
                Console.ReadLine();
            }
            
            public static string[][] GetTranslationCharlist(ushort version = 0)
            {
                switch (version)
                {
                    case 0:
                        return new string[][]
                        {
                            new string[]
                            {
                                "0", "11", "1", "00", "0111", "101", "00110", "000111",
                                "11101", "11001", "11010", "11111", "1001", "1011", "0110",
                                "0000", "110011", "00010", "00100", "00101", "000101",
                                "10000", "01001", "110110", "110111", "1100100", "1100001",
                                "1100010", "1100011", "010100", "010101", "001010", "00000000"
                            },
                            new string[]
                            {
                                "u", "ug", "gu", "ugu", "bugu", "bug", "ug-gu",
                                "ug-bug", "bugu-u", "bugu-g", "bugu-gu", "bugu-bugu", "bu",
                                "buu", "gug", "uuu", "bug-bug", "ugu-g", "ugu-ug", "bug-buu",
                                "buu-bug", "ugub", "uug", "bu-bug", "ug-bugu", "ugug", "gu-ug",
                                "bu-ugu", "ubgu", "ubug", "ub-ub", "ub", "ugu-bugu"
                            }
                        };

                    default:
                        return new string[][]
                        {
                            new string[] { },
                            new string[] { }
                        };
                };
            }

            public static string TranslateTo(string text, string[][] translationCharlist, bool isInEnglish = true)
            {
                StringBuilder sb = new StringBuilder();

                //Convert an UTF-16 text to a binary text
                char[] charArray = text.ToCharArray();
                foreach (char ch in charArray)
                {
                    sb.Append(Convert.ToString(ch, 2).PadLeft(16, '0'));
                }

                //Sort translationCharList[0] and relevant to it translationCharList[1] by element's string length of translationCharList[0], from smallest to largestst
                Array.Sort(translationCharlist[0], translationCharlist[1], Comparer<string>.Create((x, y) => y.Length.CompareTo(x.Length)));

                //Translate binary text to a text using given translation charlist
                for (int i = 0; i < translationCharlist[0].Length; i++)
                {
                    sb.Replace(translationCharlist[0][i], translationCharlist[1][i] + " ");
                }

                //Non-latyn symbols handling
                if (isInEnglish == false)
                {
                    sb.Replace("u", "у").Replace("g", "г").Replace("b", "б");
                }

                return sb.ToString().Trim();
            }

            public static string TranslateFrom(string text, string[][] translationCharlist)
            {
                text = text.Trim() + " ";

                StringBuilder sb = new StringBuilder();
                sb.Append(text);

                //Non-latyn symbols handling
                sb.Replace("у", "u").Replace("г", "g").Replace("б", "b");

                //Sort translationCharList[0] and relevant to it translationCharList[1] by element's string length of translationCharList[0], from smallest to largest
                Array.Sort(translationCharlist[1], translationCharlist[0], Comparer<string>.Create((x, y) => y.Length.CompareTo(x.Length)));

                //Translate text to a binary text using given translation charlist
                for (int i = 0; i < translationCharlist[1].Length; i++)
                {
                    sb.Replace(translationCharlist[1][i] + " ", translationCharlist[0][i]);
                }

                var binaryText = sb.ToString();

                //Convert binary text to an UTF-16 text
                sb = new StringBuilder();
                for (int i = 0; i < binaryText.Length; i += 16)
                {
                    string currentChar = binaryText.ToString().Substring(i, 16);
                    int charCode = Convert.ToInt32(currentChar, 2);
                    sb.Append((char)charCode);
                }

                return sb.ToString();
            }
        }
    }
}
