using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace REG_MARK_LIB
{
    public static class RegMark
    {
        /// <summary>
        /// Проверяет номерной знак автомобиля на подлинность.
        /// </summary>
        /// <param name="mark">Номерной знак.</param>
        public static bool CheckMark(string mark)
        {
            var letters = "ABEKMHOPCTYX";
            var pattern = $@"[{letters}]{{1}}[0-9]{{3}}[{letters}]{{2}}[0-9]{{2,3}}";
            if (!Regex.IsMatch(mark, pattern)) return false;
            var pattern2 = "(^01$)|(^02$)|(^03$)|(^04$)|(^05$)|(^06$)|(^07$)|(^08$)|(^09$)|(^10$)|(^11$)|(^12$)|" + 
                           "(^13$)|(^14$)|(^15$)|(^16$)|(^17$)|(^18$)|(^19$)|(^21$)|(^22$)|(^23$)|(^24$)|(^25$)|" + 
                           "(^26$)|(^27$)|(^28$)|(^29$)|(^30$)|(^31$)|(^32$)|(^33$)|(^34$)|(^35$)|(^36$)|(^37$)|" + 
                           "(^38$)|(^39$)|(^40$)|(^41$)|(^42$)|(^43$)|(^44$)|(^45$)|(^46$)|(^47$)|(^48$)|(^49$)|" + 
                           "(^50$)|(^51$)|(^52$)|(^53$)|(^54$)|(^55$)|(^56$)|(^57$)|(^58$)|(^59$)|(^60$)|(^61$)|" + 
                           "(^62$)|(^63$)|(^64$)|(^65$)|(^66$)|(^67$)|(^68$)|(^69$)|(^70$)|(^71$)|(^72$)|(^73$)|" + 
                           "(^74$)|(^75$)|(^76$)|(^77$)|(^78$)|(^79$)|(^80$)|(^81$)|(^82$)|(^83$)|(^84$)|(^85$)|" + 
                           "(^86$)|(^87$)|(^88$)|(^89$)|(^90$)|(^91$)|(^92$)|(^93$)|(^95$)|(^96$)|(^97$)|(^98$)|" +
                           "(^99$)|(^102$)|(^116$)|(^118$)|(^121$)|(^123$)|(^124$)|(^125$)|(^134$)|(^136$)|(^138$)|" + 
                           "(^142$)|(^150$)|(^152$)|(^154$)|(^156$)|(^159$)|(^161$)|(^163$)|(^164$)|(^169$)|(^173$)|" + 
                           "(^174$)|(^177$)|(^178$)|(^186$)|(^190$)|(^196$)|(^197$)|(^199$)|(^716$)|(^725$)|(^750$)|(^777$)|(^788$)|(^790$)|(^797$)";
            return Regex.IsMatch(new string(mark.Skip(6).ToArray()), pattern2);
        }
        
        /// <summary>
        /// Возвращает следующий номерной знак.
        /// </summary>
        /// <param name="mark">Номерной знак.</param>
        public static string GetNextMarkAfter(string mark)
        {
            //a123bc?11 -> "123", "abc"
            var region = new string(mark.Skip(6).ToArray());
            var markNumbers = Convert.ToInt32(new string(mark.Skip(1).Take(3).ToArray()));
            var markLetters = new StringBuilder(new string(mark.Take(1).ToArray()) + new string(mark.Skip(4).Take(2).ToArray()));
            if (markNumbers == 999)
            {
                markNumbers = 0;
                if (markLetters[2] != 'X')
                {
                    markLetters[2] = IncrementMarkLetter(markLetters[2]);
                }
                else if (markLetters[1] != 'X')
                {
                    markLetters[1] = IncrementMarkLetter(markLetters[1]);
                    markLetters[2] = 'A';
                }
                else if (markLetters[0] != 'X')
                {
                    markLetters[0] = IncrementMarkLetter(markLetters[0]);
                    markLetters[1] = 'A';
                    markLetters[2] = 'A';
                }
                else // x999xx
                {
                    return $"A000AA{region}";
                }

                return $"{markLetters[0]}{GetMarkNumbersString(markNumbers)}{markLetters[1]}{markLetters[2]}{region}";
            }

            return $"{markLetters[0]}{GetMarkNumbersString(markNumbers + 1)}{markLetters[1]}{markLetters[2]}{region}";
        }
        
        /// <summary>
        /// Возвращает следующий номерной знак в заданном диапазоне.
        /// </summary>
        /// <param name="prevMark">Предыдущий знак.</param>
        /// <param name="rangeStart">Начало диапазона.</param>
        /// <param name="rangeEnd">Конец диапазона.</param>
        public static string GetNextMarkAfterInRange(string prevMark, string rangeStart, string rangeEnd)
        {
            var nextMark = GetNextMarkAfter(prevMark);
            if (CompareRegMarks(rangeStart, nextMark) <= 0 && CompareRegMarks(rangeEnd, nextMark) >= 0)
            {
                return nextMark;
            }

            return "out of stock";
        }
        
        /// <summary>
        /// Возвращает количество возможных номерных знаков в заданном диапазоне.
        /// </summary>
        public static int GetCombinationsCountInRange(string mark1, string mark2)
        {
            var count = 1;
            while (CompareRegMarks(mark1, mark2) < 0)
            {
                mark1 = GetNextMarkAfter(mark1);
                count++;
            }

            return count;
        }
        
        /// <summary>
        /// Возвращает следующую букву номерного знака.
        /// </summary>
        private static char IncrementMarkLetter(char letter)
        {
            var letters = "ABEKMHOPCTYX";
            return letter == 'X' ? 'A' : letters[letters.IndexOf(letter) + 1];
        }
        
        /// <summary>
        /// Возвращает буквы номерного знака.
        /// </summary>
        private static string GetMarkNumbersString(int nums)
        {
            var numsString = nums.ToString();

            switch (numsString.Length)
            {
                case 1:
                    return $"00{numsString}";
                case 2:
                    return $"0{numsString}";
                case 3:
                    return numsString;
                default:
                    throw new Exception("GetMarkNumbersString() error");
            }
        }
        
        /// <summary>
        /// Сравнивает две буквы из номерных знаков. >0 [l1 > l2], =0 - [l1 == l2], &lt;0 - [l1 &lt; l2].
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private static int CompareRegMarkLetters(char l1, char l2)
        {
            var letters = "ABEKMHOPCTYX";
            return letters.IndexOf(l1) - letters.IndexOf(l2);
        }

        /// <summary>
        /// Сравнивает два номерных знака. >0 [mark1 > mark2], =0 - [mark1 == mark2], &lt;0 - [mark1 &lt; mark2].
        /// </summary>
        /// <returns></returns>
        private static int CompareRegMarks(string mark1, string mark2)
        {
            // "a123bc56", "e456hk156"
            var mark1Numbers = GetNumbersFromMark(mark1);
            var mark1Letters = GetLettersFromMark(mark1);
            var mark2Numbers = GetNumbersFromMark(mark2);
            var mark2Letters = GetLettersFromMark(mark2);

            if (CompareRegMarkLetters(mark1Letters[0], mark2Letters[0]) != 0)
            {
                return CompareRegMarkLetters(mark1Letters[0], mark2Letters[0]);
            }
            if (CompareRegMarkLetters(mark1Letters[1], mark2Letters[1]) != 0)
            {
                return CompareRegMarkLetters(mark1Letters[1], mark2Letters[1]);
            }

            if (CompareRegMarkLetters(mark1Letters[2], mark2Letters[2]) != 0)
            {
                return CompareRegMarkLetters(mark1Letters[2], mark2Letters[2]);
            }

            return mark1Numbers - mark2Numbers;
        }
        
        private static int GetNumbersFromMark(string mark) =>
            Convert.ToInt32(new string(mark.Skip(1).Take(3).ToArray()));
        private static StringBuilder GetLettersFromMark(string mark) =>
            new StringBuilder(new string(mark.Take(1).ToArray()) + new string(mark.Skip(4).Take(2).ToArray()));
        private static string GetRegionFromMark(string mark) =>
            new string(mark.Skip(6).ToArray());
    }
}