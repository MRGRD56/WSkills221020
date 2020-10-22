using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace VIN_LIB
{
    public static class Vin
    {
        /// <summary>
        /// Проверяет VIN на подлинность.
        /// </summary>
        /// <param name="vin">Идентификационный номер ТС.</param>
        public static bool CheckVIN(string vin) =>
            ContainsCorrectCharacters(vin) &&
            IsCorrectCountryCode(vin) &&
            IsCorrectYear(vin);
        
        /// <summary>
        /// Вовзращает страну производства транспортного средства.
        /// </summary>
        /// <param name="vin">Идентификационный номер ТС.</param>\\
        public static string GetVINCountry(string vin)
        {
            using (StreamReader sr = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}\..\..\..\CountriesCodes.dat"))
            {
                var code = new string(vin.Take(2).ToArray());
                string line;
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    var pair = GetCountryPair(line);
                    var pattern = pair.Item1;
                    var country = pair.Item2;
                    if (Regex.IsMatch(code, pattern))
                    {
                        return country;
                    }
                }

                return "unknown country";
            }
        }
        
        /// <summary>
        /// Возвращает год производства транспортного средства.
        /// </summary>
        /// <param name="vin">Идентификационный номер ТС.</param>
        public static int GetTransportYear(string vin)
        {
            var currentYear = DateTime.Today.Year;
            var chars = "ABCDEFGHJKLMNPRSTVWXY123456789";
            var yearCode = vin[10 - 1].ToString().ToUpper();

            var i = chars.IndexOf(yearCode, StringComparison.Ordinal);
            var year = 1980 + i;
            while (true)
            {
                //F - 1985 -> F - 2015
                if (year + 30 <= currentYear)
                {
                    year += 30;
                    continue;
                }

                return year;
            }
        }

        private static bool ContainsCorrectCharacters(string vin)
        {
            var pattern = @"[0-9|ABCDEFGHJKLMNPRSTUVWXYZ]{8}[0-9|X]{1}[0-9|ABCDEFGHJKLMNPRSTUVWXYZ]{4}[0-9]{4}";
            return Regex.IsMatch(vin, pattern);
        }

        private static bool IsCorrectCountryCode(string vin)
        {
            var cc = new string(vin.Take(2).ToArray());
            var isIncorrect = Regex.IsMatch(cc, "(A[P-Z|0-9])") ||
                              Regex.IsMatch(cc, "(B[S-Z|0-9])") ||
                              Regex.IsMatch(cc, "(C[S-Z|0-9])") ||
                              Regex.IsMatch(cc, "(D[S-Z|0-9])") ||
                              Regex.IsMatch(cc, "(E[L-Z|0-9])") ||
                              Regex.IsMatch(cc, "(F[L-Z|0-9])") ||
                              Regex.IsMatch(cc, "(G[L-Z|0-9])") ||
                              Regex.IsMatch(cc, "(H[A-Z|0-9])") ||
                              Regex.IsMatch(cc, "(M[S-Z|0-9])") ||
                              Regex.IsMatch(cc, "(N[T-Z|0-9])") ||
                              Regex.IsMatch(cc, "(P[S-Z|0-9])") ||
                              Regex.IsMatch(cc, "(T[2-9|0])") ||
                              Regex.IsMatch(cc, "(U[A-G])") ||
                              Regex.IsMatch(cc, "(U[1-4])") ||
                              Regex.IsMatch(cc, "(U[8-9|0])") ||
                              Regex.IsMatch(cc, "(6[X-Z|0])") ||
                              Regex.IsMatch(cc, "(7[F-Z|0])") ||
                              Regex.IsMatch(cc, "(8[3-9|0])") ||
                              Regex.IsMatch(cc, "(90)");
            return !isIncorrect;
        }

        private static bool IsCorrectYear(string vin)
        {
            var yearCode = vin[10 - 1].ToString();
            return yearCode != "U" && yearCode != "Z" && yearCode != "0";
        }
        
        /// <summary>
        /// Возвращает объект класса Tuple, где первый элемент -
        /// регулярное выражение, второй - название страны.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static Tuple<string, string> GetCountryPair(string line)
        {
            // "K[S-Z|0-9] Казахстан" -> ("K[S-Z|0-9]", "Казахстан")
            line = line.Trim();
            var arr = line.Split(' ');
            return new Tuple<string, string>(arr[0], arr[1]);
        }
    }
}