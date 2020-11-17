using System.Text.RegularExpressions;

namespace WebApiBusiness.Util
{
    public static class StringExtensions
    {
        public static bool IsPlate(this string plate)
        {
            return new Regex("^[a-zA-Z]{3}\\d{4}$").IsMatch(plate);
        }

        public static bool IsCpf(this string cpf)
        {
            int[] numArray1 = new int[9]
            {
        10,
        9,
        8,
        7,
        6,
        5,
        4,
        3,
        2
            };
            int[] numArray2 = new int[10]
            {
        11,
        10,
        9,
        8,
        7,
        6,
        5,
        4,
        3,
        2
            };
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            string str1 = cpf.Substring(0, 9);
            int num1 = 0;
            char ch;
            for (int index = 0; index < 9; ++index)
            {
                int num2 = num1;
                ch = str1[index];
                int num3 = int.Parse(ch.ToString()) * numArray1[index];
                num1 = num2 + num3;
            }
            int num4 = num1 % 11;
            string str2 = (num4 >= 2 ? 11 - num4 : 0).ToString();
            string str3 = str1 + str2;
            int num5 = 0;
            for (int index = 0; index < 10; ++index)
            {
                int num2 = num5;
                ch = str3[index];
                int num3 = int.Parse(ch.ToString()) * numArray2[index];
                num5 = num2 + num3;
            }
            int num6 = num5 % 11;
            int num7 = num6 >= 2 ? 11 - num6 : 0;
            string str4 = str2 + num7.ToString();
            return cpf.EndsWith(str4);
        }

        public static bool OnlyNumbers(this string value)
        {
            return new Regex("^[0-9]+$").IsMatch(value);
        }
    }
}