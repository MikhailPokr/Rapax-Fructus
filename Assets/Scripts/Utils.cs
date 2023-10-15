using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RapaxFructus
{
    /// <summary>
    /// Разные вспомогательные штуки
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Принимает выражение и возвращает ответ.
        /// </summary>
        /// <param name="expression">
        /// Просто выражение, которое поддерживает + - * / () ^ и числа. (никаких переменных)
        /// </param>
        /// <returns></returns>
        public static float GetExpressionResult(string expression)
        {
            List<char> charsList = expression.ToCharArray().ToList();
            expression = expression.Replace(" ", "");

            //скобки
            string extraExpression = "";
            while (charsList.Count > 0)
            {
                string currentChar = charsList[0].ToString();

                extraExpression += currentChar;
                if (currentChar == "(")
                {
                    extraExpression = "";
                }
                if (currentChar == ")")
                {
                    extraExpression = extraExpression.Replace(")", "");
                    expression = expression.Replace("(" + extraExpression + ")", GetExpressionResult(extraExpression).ToString(CultureInfo.InvariantCulture));
                    charsList = expression.ToCharArray().ToList();
                    continue;
                }
                charsList.RemoveAt(0);
            }

            charsList = expression.ToCharArray().ToList();

            //степень
            string num = "";
            float numFloat = 0;
            while (charsList.Count > 0)
            {
                string currentChar = charsList[0].ToString();

                if (int.TryParse(currentChar, out int number) || currentChar == "." || currentChar == "," || (currentChar == "-" && num.Length == 0))
                {
                    if (currentChar != "f")
                        num += currentChar;
                }
                else if (currentChar == "^")
                {
                    charsList.RemoveAt(0);
                    string currentCharExtra = charsList[0].ToString();
                    string numExtra = "";
                    while (int.TryParse(currentCharExtra, out number) || currentCharExtra == "." || (currentCharExtra == "-" && numExtra.Length == 0))
                    {
                        numExtra += currentCharExtra;
                        charsList.RemoveAt(0);
                        if (charsList.Count != 0)
                            currentCharExtra = charsList[0].ToString();
                        else
                            break;
                    }
                    numFloat = float.Parse(num, CultureInfo.InvariantCulture);
                    float numExtraFloat = float.Parse(numExtra, CultureInfo.InvariantCulture);
                    expression = expression.Replace(num + currentChar + numExtra, MathF.Pow(numFloat, numExtraFloat).ToString(CultureInfo.InvariantCulture));
                    charsList = expression.ToCharArray().ToList();
                    num = "";
                    continue;
                }
                else
                {
                    num = "";
                }
                charsList.RemoveAt(0);
            }

             charsList = expression.ToCharArray().ToList();

            //умножение и деление
            num = "";
            while (charsList.Count > 0)
            {
                string currentChar = charsList[0].ToString();

                if (int.TryParse(currentChar, out int number) || currentChar == "." || (currentChar == "-" && num.Length == 0))
                {
                    if (currentChar != "f")
                        num += currentChar;
                }
                else if (currentChar == "*" || currentChar == "/")
                {
                    charsList.RemoveAt(0);
                    string currentCharExtra = charsList[0].ToString();
                    string numExtra = "";
                    while (int.TryParse(currentCharExtra, out number) || currentCharExtra == "." || (currentCharExtra == "-" && numExtra.Length == 0))
                    {
                        numExtra += currentCharExtra;
                        charsList.RemoveAt(0);
                        if (charsList.Count != 0)
                            currentCharExtra = charsList[0].ToString();
                        else
                            break;
                    }
                    numFloat = float.Parse(num, CultureInfo.InvariantCulture);
                    float numExtraFloat = float.Parse(numExtra, CultureInfo.InvariantCulture);
                    expression = expression.Replace(num + currentChar + numExtra, (currentChar == "*" ? numFloat * numExtraFloat : numFloat / numExtraFloat).ToString(CultureInfo.InvariantCulture));
                    charsList = expression.ToCharArray().ToList();
                    num = "";
                    continue;
                }
                else
                {
                    num = "";
                }
                charsList.RemoveAt(0);
            }

            charsList = expression.ToCharArray().ToList();

            //сложение и вычитание
            num = "";
            while (charsList.Count > 0)
            {
                string currentChar = charsList[0].ToString();

                if (int.TryParse(currentChar, out int number) || currentChar == "." || (currentChar == "-" && num.Length == 0))
                {
                    if (currentChar != "f")
                        num += currentChar;
                }
                else if ((currentChar == "-" && num != "") || currentChar == "+")
                {
                    charsList.RemoveAt(0);
                    string currentCharExtra = charsList[0].ToString();
                    string numExtra = "";
                    while (int.TryParse(currentCharExtra, out number) || currentCharExtra == "." || (currentCharExtra == "-" && numExtra.Length == 0))
                    {
                        numExtra += currentCharExtra;
                        charsList.RemoveAt(0);
                        if (charsList.Count != 0)
                            currentCharExtra = charsList[0].ToString();
                        else
                            break;
                    }
                    numFloat = float.Parse(num, CultureInfo.InvariantCulture);
                    float numExtraFloat = float.Parse(numExtra, CultureInfo.InvariantCulture);
                    expression = expression.Replace(num + currentChar + numExtra, (currentChar == "+" ? numFloat + numExtraFloat : numFloat - numExtraFloat).ToString(CultureInfo.InvariantCulture));
                    charsList = expression.ToCharArray().ToList();
                    num = "";
                    continue;
                }
                else
                {
                    num = "";
                }
                charsList.RemoveAt(0);
            }

            charsList = expression.ToCharArray().ToList();

            //конец
            num = "";
            while (charsList.Count > 0)
            {
                string currentChar = charsList[0].ToString();

                if (int.TryParse(currentChar, out int number) || currentChar == "." || currentChar == "-")
                {
                    num += currentChar;
                    charsList.RemoveAt(0);
                    if (charsList.Count != 0)
                        currentChar = charsList[0].ToString();
                    else
                        break;
                }
                else
                    charsList.RemoveAt(0);
            }

            return float.Parse(num, CultureInfo.InvariantCulture);
        }
    }
}