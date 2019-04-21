using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caesar_Shift.Business
{
    public static class CaesarEncoder
    {
        private const bool ASCII_IS_GOOD_BOY = 'а' < 'ё' && 'ё' < 'я';
        // false, because ASCII is not good boy

        private const string ALPHABET_LOWER = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private const string ALPHABET_UPPER = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private const int ALPHABET_LENGTH = 33;

        private static readonly string[] oneLetterWord = { "а", "б", "в", "ж", "и", "к", "о", "с", "у", "э", "я" };

        private static readonly float[] letterFrecuency = { 0.0801f, 0.0159f, 0.0454f, 0.0170f, 0.0298f, 0.0845f, 0.0004f, 0.0094f, 0.0165f, 0.0735f, 0.0121f, 0.0349f, 0.0440f, 0.0321f, 0.0670f, 0.1097f, 0.0281f, 0.0473f, 0.0547f, 0.0626f, 0.0262f, 0.0026f, 0.0097f, 0.0048f, 0.0144f, 0.0073f, 0.0036f, 0.0004f, 0.0190f, 0.0174f, 0.0032f, 0.0064f, 0.0201f };

        private class Key
        {
            public int Shift { get; set; }
            public float Deflection { get; set; }
        }

        public static int[] GetBestKeys(string text)
        {
            text = text.ToLower();

            // Подсчёт всех букв и выделение только русского текста
            GetRussianTextWithStatistic(text, out string russianText, out int[] russianLetterCount);

            // Подсчёт отклонения у ключей
            Key[] keys = GetKeys(text, russianLetterCount);

            // Улучшим результаты, увеличивая отклонение ключам с невозможными словами
            GiveBigDeflectionToUnrealWordKeys(keys, russianText);

            // Избавляемся от класса ключа, возвращая лишь массив ключей в порядке возрастания отклонения
            return keys.OrderBy(key => key.Deflection).Select(x => x.Shift).ToArray();
        }

        private static void GiveBigDeflectionToUnrealWordKeys(Key[] keys, string russianText)
        {
            foreach (var key in keys)
            {
                string shiftRussianText = Shift(russianText, -key.Shift);
                string[] words = shiftRussianText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (word.StartsWith("ъ") || word.StartsWith("ь") || word.StartsWith("ы"))
                        key.Deflection *= 10;
                    if (word.Length == 1 && !oneLetterWord.Contains(word))
                        key.Deflection *= 10;
                }
            }
        }

        private static Key[] GetKeys(string text, int[] russianLetterCount)
        {
            // Определение отклонения от статистической частоты для каждого ключа
            Key[] keys = new Key[ALPHABET_LENGTH];
            int russianLetterFullCount = russianLetterCount.Sum();
            // Цикл создания ключей
            for (int i = 0; i < ALPHABET_LENGTH; i++)
            {
                var key = new Key() { Shift = i, Deflection = 0 };
                // Цикл перебора букв
                for (int j = 0; j < ALPHABET_LENGTH; j++)
                {
                    // Определяем сдвинутую букву
                    int index = (j + i) % ALPHABET_LENGTH;

                    // Определение частоты буквы со сдвигом по ключу
                    float currentFrecuency = russianLetterCount[index] * 1f / russianLetterFullCount;

                    // Суммирование разниц статистической и текущей частоты
                    key.Deflection += Math.Abs(currentFrecuency - letterFrecuency[j]);
                }
                keys[i] = key;
            }

            return keys;
        }
        
        private static void GetRussianTextWithStatistic(string text, out string russianText, out int[] russianLetterCount)
        {
            russianText = "";
            russianLetterCount = new int[ALPHABET_LENGTH];

            foreach (var c in text)
            {
                int index;
                if ((index = ALPHABET_LOWER.IndexOf(c)) != -1)
                {
                    russianLetterCount[index]++;
                    russianText += c;
                }
                else if (c == ' ')
                    russianText += ' ';
            }
        }

        public static string Shift(string text, int shift)
        {
            shift = shift < 0 ? ALPHABET_LENGTH + shift % ALPHABET_LENGTH : shift;

            string shiftText = "";
            int index;
            foreach (var c in text)
            {
                if ((index = ALPHABET_LOWER.IndexOf(c)) != -1)
                {
                    shiftText += ALPHABET_LOWER[(index + shift) % ALPHABET_LENGTH];
                }
                else if ((index = ALPHABET_UPPER.IndexOf(c)) != -1)
                {
                    shiftText += ALPHABET_UPPER[(index + shift) % ALPHABET_LENGTH];
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