using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caesar_Shift.Business
{
    public static class Caesar
    {
        private const bool ASCII_IS_GOOD_BOY = 'а' < 'ё' && 'ё' < 'я';
        // false, because ASCII is not good boy

        public const string ALPHABET_LOWER = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private const string ALPHABET_UPPER = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        private static readonly string[] oneLetterWord = { "а", "б", "в", "ж", "и", "к", "о", "с", "у", "э", "я" };

        private static readonly float[] letterFrecuency = { 0.0801f, 0.0159f, 0.0454f, 0.0170f, 0.0298f, 0.0845f, 0.0004f, 0.0094f, 0.0165f, 0.0735f, 0.0121f, 0.0349f, 0.0440f, 0.0321f, 0.0670f, 0.1097f, 0.0281f, 0.0473f, 0.0547f, 0.0626f, 0.0262f, 0.0026f, 0.0097f, 0.0048f, 0.0144f, 0.0073f, 0.0036f, 0.0004f, 0.0190f, 0.0174f, 0.0032f, 0.0064f, 0.0201f };

        private class Key
        {
            public int Shift;
            public float Deflection;
        }

        public static int[] GetBestKeys(string text)
        {
            text = text.ToLower();

            // Подсчёт всех букв и выделение только русского текста
            int index;
            int russianLetterFullCount = 0;
            int[] russianLetterCount = new int[ALPHABET_LOWER.Length];

            string russianText = "";

            foreach (var c in text)
            {
                if ((index = ALPHABET_LOWER.IndexOf(c)) != -1)
                {
                    russianLetterCount[index]++;
                    russianLetterFullCount++;
                    russianText += c;
                }
                else if (c == ' ')
                    russianText += ' ';
            }

            // Определение отклонения от статистической частоты для каждого ключа
            float currentFrecuency;
            int keysCount = ALPHABET_LOWER.Length;
            var keys = new List<Key>(keysCount);
            // Цикл создания ключей
            for (int i = 0; i < keysCount; i++)
            {
                var key = new Key() { Shift = i, Deflection = 0 };
                // Цикл перебора букв
                for (int j = 0; j < ALPHABET_LOWER.Length; j++)
                {
                    // Определяем сдвинутую букву
                    index = (j + i) % russianLetterCount.Length;

                    // Определение частоты буквы со сдвигом по ключу
                    currentFrecuency = russianLetterCount[index] * 1f / russianLetterFullCount;

                    // Суммирование разниц статистической и текущей частоты
                    key.Deflection += Math.Abs(currentFrecuency - letterFrecuency[j]);
                }
                keys.Add(key);
            }


            // Улучшим результаты, увеличивая отклонение ключам с невозможными словами
            string shiftRussianText;
            string[] words;
            foreach (var key in keys)
            {
                shiftRussianText = Shift(russianText, -key.Shift);
                words = shiftRussianText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (word.StartsWith("ъ") || word.StartsWith("ь") || word.StartsWith("ы"))
                        key.Deflection *= 10;
                    if (word.Length == 1 && !oneLetterWord.Contains(word))
                        key.Deflection *= 10;
                }
            }

            // Избавляемся от класса ключа, возвращая лишь массив ключей в порядке возрастания отклонения
            return keys.OrderBy(key => key.Deflection).Select(x => x.Shift).ToArray();
        }

        public static string Shift(string text, int shift)
        {
            shift = shift < 0 ? 33 + shift % 33 : shift;

            string shiftText = "";
            int index;
            foreach (var c in text)
            {
                if ((index = ALPHABET_LOWER.IndexOf(c)) != -1)
                {
                    shiftText += ALPHABET_LOWER[(index + shift) % ALPHABET_LOWER.Length];
                }
                else if ((index = ALPHABET_UPPER.IndexOf(c)) != -1)
                {
                    shiftText += ALPHABET_UPPER[(index + shift) % ALPHABET_UPPER.Length];
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