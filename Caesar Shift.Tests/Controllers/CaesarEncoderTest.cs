using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caesar_Shift.Business;
using Moq;
using NUnit.Framework;

namespace Caesar_Shift.Tests.Controllers
{
    [TestFixture, Parallelizable]
    public class CaesarEncoderTest
    {
        [TestCase("а", 1, "б")]
        [TestCase("а", -1, "я")]
        [TestCase("а", 0, "а")]
        [TestCase("а", 33, "а")]
        [TestCase("а", -33, "а")]
        [TestCase("а", 34, "б")]
        [TestCase("а", -34, "я")]
        [TestCase(":", 99, ":")]
        [TestCase(";", 99, ";")]
        [TestCase("i", 99, "i")]
        [TestCase("3", 99, "3")]
        [TestCase("ё", 1, "ж")]
        [TestCase("Ф", -330, "Ф")]
        public void OneLetterShifting(string input, int shift, string expectedResult)
        {
            string result = CaesarEncoder.Shift(input, shift);
            Assert.AreEqual(expectedResult,result);
        }

        [TestCase("Привет", 10, "Щътлоь")]
        [TestCase("Срйётвднба", -2, "Поздравляю")]
        [TestCase("123ааббвв", 1, "123ббввгг")]
        public void StringShifting(string input, int shift, string expectedResult)
        {
            string result = CaesarEncoder.Shift(input, shift);
            Assert.AreEqual(expectedResult, result);
        }

        private const string teacherExample = "Срйётвднба, фэ срнхщкн кучрёпэл фжмуф!!! Д сткпшксж српбфю, щфр фхф кусрнюйхжфуб ъкцт Шжйвтб пж рургр фтхёпр, рупрдпвб йведрйёмв уруфркф д фро, щфргэ ргэетвфю увох укфхвшка у ёжъкцтрдмрл к рстжёжнжпкжо пвствднжпкб к ъвев уёдкев.Фжсжтю ёжнр руфвнрую йв овнэо, ёрёжнвфю стретвоох ёр нрекщжумрер мрпшв, дэсрнпкфю дуж хунрдкб йвёвпкб к рсхгнкмрдвфю удра твгрфх! Орнрёжш, яфр гэнк ёруфвфрщпр фтхёпэж к кпфжтжупэж ёдв у срнрдкпрл ожубшв, пр дсжтжёк пву иёжф жыж оприжуфдр рфмтэфкл, к б пвёжаую ргыкч уджтъжпкл! Рф нкшв мросвпкк FirstLineSoftware к Хпкджтукфжфв КФОР, б твё срйётвдкфю фжгб у рцкшквнюпэо рмрпщвпкжо пвъкч мхтурд У# ёнб пвщкпваыкч! Оэ чрфко срижнвфю хусжчрд д ёвнюпжлъжо сретхижпкк д окт КФ к стретвооктрдвпкб у кусрнюйрдвпкжо уфжмв фжчпрнрекл .Net, ортж фжтсжпкб к кпфжтжупэч йвёвщ!";
        [TestCase(teacherExample, 2)]
        public void KeySearching(string input, int expectedResult)
        {
            CaesarEncoder.Key[] keys = CaesarEncoder.GetBestKeys(input);
            Assert.AreEqual(expectedResult, keys[0].Shift);
        }
    }
}
