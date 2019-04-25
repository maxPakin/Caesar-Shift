using System.IO;
using NUnit.Framework;

namespace Caesar_Shift.Tests.Controllers
{
    [TestFixture, Parallelizable]
    class TextServiceTest
    {
        [TestCase("Привет, я Валера 228\n и я люблю FirstLineSoftware, гы!")]
        [TestCase("Что первое на ум пришло, то и пишу, а чо нет?")]
        public void DocxWritingAndReading(string expectedResult)
        {
            byte[] bytes = Business.TextService.GetDocFileWithText(expectedResult);
            using (var ms = new MemoryStream(bytes))
            {
                string result = Business.TextService.GetTextFromDocx(ms);
                Assert.AreEqual(expectedResult, result);
            }
        }

        [TestCase("Привет, я Валера 228\n и я люблю FirstLineSoftware, гы!")]
        [TestCase("Что первое на ум пришло, то и пишу, а чо нет?")]
        public void TxtWritingAndReading(string expectedResult)
        {
            byte[] bytes = Business.TextService.GetTxtFileWithText(expectedResult);
            using (var ms = new MemoryStream(bytes))
            {
                string result = Business.TextService.GetTextFromTxt(ms);
                Assert.AreEqual(expectedResult, result);
            }
        }
    }
}
