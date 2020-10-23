using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace REG_MARK_LIB
{
    public static class RegMark
    {
        /// <summary>
        /// Проверяет номерной знак автомобиля на подлинность.
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static bool CheckMark(string mark)
        {
            var letters = "abekmhopctyx";
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

        public static string GetNextMarkAfter(string mark)
        {
            //a123bc?11 -> "123", "abc"
            var region = new string(mark.Skip(6).ToArray());
            var letters = "abekmhopctyx";
            var markNumbers = Convert.ToInt32(new string(mark.Skip(1).Take(3).ToArray()));
            var markLetters = new StringBuilder(new string(mark.Take(1).ToArray()) + new string(mark.Skip(4).Take(2).ToArray()));
            if (markNumbers == 999)
            {
                if (markLetters[2] != 'x')
                {
                    markLetters[2] = IncrementMarkLetter(markLetters[2]);
                }
                else if (markLetters[1] != 'x')
                {
                    markLetters[1] = IncrementMarkLetter(markLetters[1]);
                    markLetters[2] = 'a';
                }
                else if (markLetters[0] != 'x')
                {
                    markLetters[0] = IncrementMarkLetter(markLetters[0]);
                    markLetters[1] = 'a';
                    markLetters[2] = 'a';
                }
                else // x999xx
                {
                    return "a000aa";
                }

                return $"{markLetters[0]}{GetMarkNumbersString(markNumbers)}{markLetters[1]}{markLetters[2]}{region}";
            }

            return $"{markLetters[0]}{GetMarkNumbersString(markNumbers + 1)}{markLetters[1]}{markLetters[2]}{region}";
        }

        public static string GetNextMarkAfterInRange(string prevMark, string rangeStart, string rangeEnd)
        {
            throw new NotImplementedException();
        }

        public static int GetCombinationsCountInRange(string mark1, string mark2)
        {
            throw new NotImplementedException();
        }
        
        private static char IncrementMarkLetter(char letter)
        {
            var letters = "abekmhopctyx";
            return letter == 'x' ? 'a' : letters[letters.IndexOf(letter) + 1];
        }

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
    }
}