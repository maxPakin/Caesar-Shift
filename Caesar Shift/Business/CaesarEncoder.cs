using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caesar_Shift.Business
{
    public static class CaesarEncoder
    {
        private const string AlphabetLower = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private const string AlphabetUpper = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private const string FullAlphabet = AlphabetLower + AlphabetUpper;
        private const int AlphabetLength = 33;

        private static readonly string[] OneLetterWord = { "а", "б", "в", "ж", "и", "к", "о", "с", "у", "э", "я" };
        private static readonly string[] IncorrectStartOfWords = { "ъ", "ы", "ь"};

        private static readonly float[] LetterFrecuency = { 0.0801f, 0.0159f, 0.0454f, 0.0170f, 0.0298f, 0.0845f, 0.0004f, 0.0094f, 0.0165f, 0.0735f, 0.0121f, 0.0349f, 0.0440f, 0.0321f, 0.0670f, 0.1097f, 0.0281f, 0.0473f, 0.0547f, 0.0626f, 0.0262f, 0.0026f, 0.0097f, 0.0048f, 0.0144f, 0.0073f, 0.0036f, 0.0004f, 0.0190f, 0.0174f, 0.0032f, 0.0064f, 0.0201f };

        public class Key
        {
            public int Shift { get; set; }
            public float Deflection { get; set; }
            public string Text { get; set; }
        }

        public static Key[] GetBestKeys(string text)
        {
            // Подсчёт всех букв
            int[] russianLetterCount = GetRussianLetterCount(text);

            // Подсчёт отклонения у ключей
            Key[] keys = GetKeys(text, russianLetterCount);

            // Улучшим результаты, увеличивая отклонение ключам с невозможными словами
            FindUnrealWords(keys);

            // Избавляемся от класса ключа, возвращая лишь массив ключей в порядке возрастания отклонения
            return keys.OrderBy(key => key.Deflection).ToArray();
        }

        private static int[] GetRussianLetterCount(string text)
        {
            string lower = text.ToLower();
            var russianLetterCount = new int[AlphabetLength];

            foreach (var c in lower)
            {
                int index;
                if ((index = AlphabetLower.IndexOf(c)) != -1)
                {
                    russianLetterCount[index]++;
                }
            }

            return russianLetterCount;
        }

        private static Key[] GetKeys(string text, int[] russianLetterCount)
        {
            // Определение отклонения от статистической частоты для каждого ключа
            Key[] keys = new Key[AlphabetLength];
            int russianLetterFullCount = russianLetterCount.Sum();
            // Цикл создания ключей
            for (int i = 0; i < AlphabetLength; i++)
            {
                var key = new Key { Shift = i, Deflection = 0, Text = Decryption(text, i)};
                // Цикл перебора букв
                for (int j = 0; j < AlphabetLength; j++)
                {
                    // Определяем сдвинутую букву
                    int index = (j + i) % AlphabetLength;

                    // Определение частоты буквы со сдвигом по ключу
                    float currentFrecuency = russianLetterCount[index] * 1f / russianLetterFullCount;

                    // Суммирование разниц статистической и текущей частоты
                    key.Deflection += Math.Abs(currentFrecuency - LetterFrecuency[j]);
                }
                keys[i] = key;
            }

            return keys;
        }

        private static void FindUnrealWords(Key[] keys)
        {
            foreach (var key in keys)
            {
                string[] words = key.Text.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int incorrectWordsCount = words.Count(IsUnrealWord);
                key.Deflection *= incorrectWordsCount * 10;
            }
        }

        private static bool IsUnrealWord(string word)
        {
            if (IncorrectStartOfWords.Any(word.StartsWith))
                return true;

            return word.Length == 1 && IsRussian(word[0]) && !OneLetterWord.Contains(word);
        }

        private static bool IsRussian(char c)
        {
            return FullAlphabet.Contains(c);
        }

        public static string Encryption(string text, int key)
        {
            return Shift(text, key);
        }

        public static string Decryption(string text, int key)
        {
            return Shift(text, -key);
        }

        public static string Shift(string text, int shift)
        {
            shift = shift < 0 ? AlphabetLength + shift % AlphabetLength : shift;

            string shiftText = "";
            int index;
            foreach (var c in text)
            {
                if ((index = AlphabetLower.IndexOf(c)) != -1)
                {
                    shiftText += AlphabetLower[(index + shift) % AlphabetLength];
                }
                else if ((index = AlphabetUpper.IndexOf(c)) != -1)
                {
                    shiftText += AlphabetUpper[(index + shift) % AlphabetLength];
                }
                else
                {
                    shiftText += c;
                }
            }
            return shiftText;
        }
    }
}