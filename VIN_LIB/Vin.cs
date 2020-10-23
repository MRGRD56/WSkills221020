using System;
using System.Collections.Generic;
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
            var code = new string(vin.Take(2).ToArray());

            foreach (var t in countriesData)
            {
                if (string.IsNullOrWhiteSpace(t)) break;
                var (pattern, country) = GetCountryPair(t);
                if (Regex.IsMatch(code, pattern))
                {
                    return country;
                }
            }

            return "unknown country";
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

        private static List<string> countriesData =
            "A[A-H] ЮАР;A[J-N] Котд’Ивуар;B[A-E] Ангола;B[F-K] Кения;B[L-R] Танзания;C[A-E] Бенин;C[F-K] Мадагаскар;C[L-R] Тунис;D[A-E] Египет;D[F-K] Марокко;D[L-R] Замбия;E[A-E] Эфиопия;E[F-K] Мозамбик;F[A-E] Гана;F[F-K] Нигерия;J[A-T] Япония;K[A-E] Шри Ланка;K[F-K] Израиль;K[L-R] Южная Корея;K[S-Z|0-9] Казахстан;L[A-Z|0-9] Китай;M[A-E] Индия;M[F-K] Индонезия;M[L-R] Таиланд;N[F-K] Пакистан;N[L-R] Турция;P[A-E] Филиппины;P[F-K] Сингапур;P[L-R] Малайзия;R[A-E] ОАЭ;R[F-K] Тайвань;R[L-R] Вьетнам;R[S-Z|0-9] Саудовская Аравия;S[A-M] Великобритания;S[N-T] Германия;S[U-Z] Польша;S[1-4] Латвия;T[A-H] Швейцария;T[J-P] Чехия;T[R-V] Венгрия;T[W-Z|1] Португалия;U[H-M] Дания;U[N-T] Ирландия;U[U-Z] Румыния;U[5-7] Словакия;V[A-E] Австрия;V[F-R] Франция;V[S-W] Испания;V[X-Z|1-2] Сербия;V[3-5] Хорватия;V[6-9|0] Эстония;W[A-Z|0-9] Германия;X[A-E] Болгария;X[F-K] Греция;X[L-R] Нидерланды;X[S-W] СССР/СНГ;X[X-Z|1-2] Люксембург;X[3-9|0] Россия;Y[A-E] Бельгия;Y[F-K] Финляндия;Y[L-R] Мальта;Y[S-W] Швеция;Y[X-Z|1-2] Норвегия;Y[3-5] Беларусь;Y[6-9|0] Украина;Z[A-R] Италия;Z[X-Z|1-2] Словения;Z[3-5] Литва;Z[6-9|0] Россия;1[A-Z|0-9] США;2[A-Z|0-9] Канада;3[A-W] Мексика;3[X-Z|1-7] Коста Рика;3[8-9|0] Каймановы острова;4[A-Z|0-9] США;5[A-Z|0-9] США;6[A-W] Австралия;7[A-E] Новая Зеландия;8[A-E] Аргентина;8[F-K] Чили;8[L-R] Эквадор;8[S-W] Перу;8[X-Z|1-2] Венесуэла;9[A-E] Бразилия;9[F-K] Колумбия;9[L-R] Парагвай;9[S-W] Уругвай;9[X-Z|1-2] Тринидад и Тобаго;9[3-9] Бразилия;"
                .Split(';')
                .ToList();
    }
}