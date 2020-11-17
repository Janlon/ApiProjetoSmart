namespace WEBAPI_VOPAK.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;


    /// <summary>
    /// Orígens de placas veiculares.
    /// </summary>
    public enum VehicularPlateLocation
    {
        UK,
        India,
        Brazil,
        Mercosul
    }

    /// <summary>
    /// Caixa para textos.
    /// </summary>
    public enum StringCases
    {
        Lower,
        Upper,
        Title,
        NoChanges
    }

    /// <summary>
    /// Mensagenss de regras de comprimento.
    /// </summary>
    public enum ValidLenghtResult
    {
        Ok = 0,
        IsNull = 1,
        IsEmpty = 2,
        IsLower = 3,
        IsGreater = 4,
        IsInvalid = 5
    }
    public static class StringExtensions
    {
        #region Levenshtein
        /// <summary>
        /// Altera entre comparado e comparação.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Swap(this string source, ref string target)
        {
            string str = target;
            target = source;
            source = str;
        }
        /// <summary>
        /// Retorna a faixa restante
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        internal static double Invert(this double min, ref double max)
        {
            return max - min;
        }
        /// <summary>
        /// Retorna a taxa de comparação (score) entre os textos "a" e "b".
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal static float Compare(this string a, ref string b)
        {
            float num = 0.0f;
            try
            {
                string[] strArray = new string[10] { " ", "-", ".", ")", "_", "(", "]", "[", "<", ">" };
                foreach (string oldValue in strArray)
                    b = b.Replace(oldValue, "");
                double min = (double)a.LevenshteinDistance(ref b);
                if (min == 0.0)
                    return (float)byte.MaxValue;
                double max = (double)Math.Max(a.Length, b.Length);
                if (min == max)
                    return 0.0f;
                num = (float)(byte)(min.Invert(ref max) / max * (double)byte.MaxValue) / (float)byte.MaxValue * 100f;
            }
            catch (Exception) { return 0.0f; }
            return num;
        }
        /// <summary>
        /// Retorna a distância entre origem e destino.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static int LevenshteinDistance(this string source, ref string target)
        {
            int num1 = 0;
            try
            {
                if (string.IsNullOrEmpty(source))
                    num1 = string.IsNullOrEmpty(target) ? 0 : target.Length;
                if (string.IsNullOrEmpty(target))
                    return source.Length;
                if (source.Length > target.Length)
                    source.Swap(ref target);
                int length1 = target.Length;
                int length2 = source.Length;
                int[,] numArray = new int[2, length1 + 1];
                for (int index = 1; index <= length1; ++index)
                    numArray[0, index] = index;
                int index1 = 0;
                for (int index2 = 1; index2 <= length2; ++index2)
                {
                    index1 = index2 & 1;
                    numArray[index1, 0] = index2;
                    int index3 = index1 ^ 1;
                    for (int index4 = 1; index4 <= length1; ++index4)
                    {
                        int num2 = (int)target[index4 - 1] == (int)source[index2 - 1] ? 0 : 1;
                        numArray[index1, index4] = Math.Min(Math.Min(numArray[index3, index4] + 1, numArray[index1, index4 - 1] + 1), numArray[index3, index4 - 1] + num2);
                    }
                }
                num1 = numArray[index1, length1];
            }
            catch (Exception) { }
            return num1;
        }
        #endregion
        /// <summary>
        /// Tenta regulamentar um padrão para a gravação de textos.
        /// </summary>
        /// <param name="value">Este texto.</param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Normalized(this string value, StringCases pattern = StringCases.Upper)
        {
            string ret = ("" + ((value == null) ? "" : value) + "").Trim();
            TextInfo ti = new CultureInfo("pt-BR").TextInfo;
            switch (pattern)
            {
                case StringCases.Lower: ret = ret.ToLower(); break;
                case StringCases.Upper: ret = ret.ToUpper(); break;
                case StringCases.Title: ret = ti.ToTitleCase(ret); break;
                default: break;
            }
            return ret;
        }

        /// <summary>
        /// Retorna verdadeiro se este texto é maior do que o tamanho indicado. Opcionalmente pode verificar também se é nulo.
        /// </summary>
        /// <param name="value">Este texto.</param>
        /// <param name="size">Tamanho.</param>
        /// <param name="required">Booleano. Indica se deve retornar como sendo verdadeiro (ou seja, um erro) quando estiver nulo ou em branco.</param>
        /// <returns>Booleano.</returns>
        public static bool IsBiggerThan(this string value, int size = 1, bool required = false)
        {
            string temp = "" + value + "";
            bool ret = false;
            if ((string.IsNullOrEmpty(temp)) && (required))
                ret = true;
            else
            {
                if (!string.IsNullOrEmpty(temp.Trim()))
                    ret = (temp.Length > size);
            }
            return ret;
        }

        /// <summary>
        /// Retorna se este texto é uma sigla válida para UFs do Brasil.
        /// </summary>
        /// <param name="value">Este texto.</param>
        /// <returns>Booleano.</returns>
        public static bool IsBrUF(this string value)
        {
            value = ("" + value + "").Trim().ToUpper();
            if ((string.IsNullOrEmpty(value)) || (string.IsNullOrWhiteSpace(value))) return false;
            if (value.Length > 2) return false;
            string[] ufs = { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            return ufs.Contains(value);
        }


        /// <summary>
        /// Retorna o conteúdo desta variável tipo texto, ou um valor padrão, caso seja nulo ou vazio.
        /// </summary>
        /// <param name="value">Este texto.</param>
        /// <param name="defaultValue">Valor padrão.</param>
        /// <returns>Conteúdo da variável tipo texto.</returns>
        public static string DefaultIfNull(this string value, string defaultValue)
        {
            string ret = defaultValue;
            try
            {
                if (!string.IsNullOrEmpty(value))
                    if (!string.IsNullOrWhiteSpace(value))
                        ret = value;
            }
            catch { ret = defaultValue; }
            return ret;
        }
        /// <summary>
        /// Retorna se este texto existe, não está vazio e possui um comprimento  
        /// entre os valores mínimo e máximo de caracteres informados.
        /// </summary>
        /// <param name="value">Este texto.</param>
        /// <param name="min">Mínimo de tamanho.</param>
        /// <param name="max">Máximo de tamanho.</param>
        /// <returns>Valor da enumeração <see cref="ValidLenghtResult"/>, indicando se este texto cumpre as restrições.</returns>
        public static ValidLenghtResult HasValidLenght(this string value, int min = 0, int max = int.MaxValue, string valids = "", bool mustTrim = true)
        {
            ValidLenghtResult ret = ValidLenghtResult.Ok;
            if ((value is null) || (value == null))
                ret = ValidLenghtResult.IsNull;
            else
            {
                if (mustTrim) TrimMe(ref value);
                if (!string.IsNullOrEmpty(valids))
                    if (!valids.Contains(value)) ret = ValidLenghtResult.IsInvalid;
                if (string.IsNullOrWhiteSpace(value)) ret = ValidLenghtResult.IsEmpty;
                if (value.Trim().Length == 0) ret = ValidLenghtResult.IsEmpty;
                if (value.Trim().Length < min) ret = ValidLenghtResult.IsLower;
                if (value.Trim().Length > max) ret = ValidLenghtResult.IsGreater;
            }
            return ret;
        }

        private static void TrimMe(ref string value) { value = value.Trim(); }

        /// <summary>
        /// Verifica se este texto contém um CEP brasileiro válido.
        /// </summary>
        /// <param name="value">Este texto.</param>
        /// <param name="performRealValidation">Define se deve ou não efetuar a pesquisa online.</param>
        /// <returns>Booleano.</returns>
        public static bool IsBrZIP(this string value, bool required = false, bool performRealValidation = true)
        {
            bool ret = false;
            // O cep foi informado?
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return !required;
            // Garantir que só existam números:
            string cep = value.JustNumbers();
            // O valor informado pode ser um CEP?
            if (cep.Length == 8)
                // O valor informado "cabe" na máscara de um CEP?
                ret = new Regex("^\\d{8}$").IsMatch(cep);
            //if ((ret) && (performRealValidation))
            //{
            //    // Tenta confirmar o CEP como válido:
            //    try
            //    {
            //        string json = "";
            //        string url = $"https://viacep.com.br/ws/{cep}/json/";
            //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //        request.AllowAutoRedirect = false;
            //        using (HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse())
            //            if (ChecaServidor.StatusCode == HttpStatusCode.OK)
            //            {
            //                using (Stream stm = ChecaServidor.GetResponseStream())
            //                    if (stm != null)
            //                        using (StreamReader responseReader = new StreamReader(stm))
            //                            json = responseReader.ReadToEnd();
            //                if (!string.IsNullOrEmpty(json))
            //                    if (json.ToUpperInvariant().Contains(@"""ERRO"))
            //                        ret = false;
            //            }
            //    }
            //    catch { ret = (performRealValidation); }
            //}
            return ret;
        }

        public static bool IsBase64(this string value)
        {
            bool flag;
            try
            {
                Image image = (Image)new ImageConverter()
                    .ConvertTo((object)Convert
                    .FromBase64String(value), typeof(Image));
                flag = true;
            }
            catch { flag = false; }
            return flag;
        }

        public static bool IsBrCpf(this string value)
        {
            bool ret = false;
            string cpf = ("" + value + "").JustNumbers();
            if (cpf.Length == 11)
            {
                if (cpf != new string(cpf.Substring(0, 1).ToCharArray()[0], cpf.Length))
                {
                    int[] numArray1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                    int[] numArray2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                    string str1 = cpf.Substring(0, 9);
                    int num1 = 0;
                    for (int index = 0; index < 9; ++index)
                        num1 += int.Parse(str1[index].ToString()) * numArray1[index];
                    int num2 = (num1 % 11);
                    int num3 = ((num2 >= 2) ? (11 - num2) : 0);
                    string str2 = num3.ToString();
                    string str3 = (str1 + str2);
                    int num4 = 0;
                    for (int index = 0; index < 10; ++index)
                        num4 += int.Parse(str3[index].ToString()) * numArray2[index];
                    num3 = (num4 % 11);
                    num3 = ((num3 >= 2) ? (11 - num3) : 0);
                    string str4 = (str2 + num3.ToString());
                    ret = cpf.EndsWith(str4);
                }
            }
            return ret;
        }

        public static bool IsBrCnpj(this string value)
        {
            bool ret = false;
            string cnpj = value.JustNumbers();
            if (cnpj.Length == 14)
            {
                if (cnpj != new string(cnpj.Substring(0, 1).ToCharArray()[0], cnpj.Length))
                {
                    int[] numArray1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                    int[] numArray2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                    string str1 = cnpj.Substring(0, 12);
                    int num1 = 0;
                    for (int index = 0; index < 12; ++index)
                        num1 += int.Parse(str1[index].ToString()) * numArray1[index];
                    int num2 = (num1 % 11);
                    int num3 = ((num2 >= 2) ? (11 - num2) : 0);
                    string str2 = num3.ToString();
                    string str3 = (str1 + str2);
                    int num4 = 0;
                    for (int index = 0; index < 13; ++index)
                        num4 += int.Parse(str3[index].ToString()) * numArray2[index];
                    num3 = (num4 % 11);
                    num3 = ((num3 >= 2) ? (11 - num3) : 0);
                    string str4 = str2 + num3.ToString();
                    ret = cnpj.EndsWith(str4);
                }
            }
            return ret;
        }

        public static string JustNumbers(this string value)
        {
            string ret = "";
            foreach (char c in value)
                if (char.IsDigit(c))
                    ret += c;
            return ret.Trim();
        }

        public static bool IsVehicularPlate(this string value, VehicularPlateLocation location = VehicularPlateLocation.Brazil)
        {
            string pattern;
            switch (location)
            {
                case VehicularPlateLocation.UK:
                    pattern = @"^([A-Z]{3}\s?(\d{3}|\d{2}|d{1})\s?[A-Z])|([A-Z]\s?(\d{3}|\d{2}|\d{1})\s?[A-Z]{3})|(([A-HK-PRSVWY][A-HJ-PR-Y])\s?([0][2-9]|[1-9][0-9])\s?[A-HJ-PR-Z]{3})$";
                    break;
                case VehicularPlateLocation.India:
                    pattern = @"^[a-zA-Z]{2}[a-zA-Z0-9]{0,6}[0-9]{3}$";
                    break;
                case VehicularPlateLocation.Mercosul:
                    pattern = @"^(?=(?:.*[0-9]){3})(?=(?:.*[A-Z]){4})[A-Z0-9]{7}$";
                    break;
                default:
                    pattern = @"^[a-zA-Z]{3}\\-\\d{4}$";
                    break;
            }
            return
                Regex.IsMatch(value,
                pattern);
        }

        public static bool IsEmail(this string email)
        {
            bool ret = false;
            try
            {
                if (!ret)
                    ret = (string.IsNullOrWhiteSpace(email));
                if (!ret)
                    ret = (string.IsNullOrEmpty(email));
                if (!ret)
                    ret = (email.Trim().Length <= 3);
                if (!ret)
                    ret = (!email.Contains("@"));
                if (!ret)
                    ret = ("0123456789".Contains(email.Substring(0, 1)));
                if (!ret)
                    ret = !(new EmailAddressAttribute()).IsValid(email);
                if (ret) { ret = false; return ret; }
                if (!ret)
                    try
                    {
                        email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                              RegexOptions.None,
                                              TimeSpan.FromMilliseconds(200));
                        string DomainMapper(Match match)
                        {
                            var idn = new IdnMapping();
                            var domainName = idn.GetAscii(match.Groups[2].Value);
                            return (match.Groups[1].Value + domainName);
                        }
                    }
                    catch (RegexMatchTimeoutException e) { e.Log(); ret = false; }
                    catch (ArgumentException e) { e.Log(); return false; }
                if (!ret)
                    try
                    {
                        ret = Regex.IsMatch(email,
                            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                            RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                    }
                    catch (RegexMatchTimeoutException) { ret = false; }
            }
            catch { ret = false; }
            return ret;
        }

        /// <summary>
        /// Retorna se este texto é uma data válida.
        /// </summary>
        /// <param name="strDate">Este texto.</param>
        /// <param name="ci">Objeto do tipo <see cref="CultureInfo"/>.</param>
        /// <returns>Booleano.</returns>
        public static bool IsDate(this string strDate, CultureInfo ci = null)
        {
            bool ret = false;
            DateTime temp;
            if (ci != null)
                ret = DateTime.TryParse(strDate, ci, DateTimeStyles.AdjustToUniversal, out temp);
            else
                ret = DateTime.TryParse(strDate, out temp);
            return ret;
        }
    }
}
