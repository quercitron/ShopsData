using DataCollectorCore;
using DataCollectorFramework;
using NUnit.Framework;

using TestDataCollector;

namespace ShopsData.Tests
{
    [TestFixture]
    public class WordsHelperTests
    {
        [Test]
        public void LevenshteinDistanceTest()
        {
            var distance = WordsHelper.LevenshteinDistance("sitting", "kitten");
            Assert.That(distance, Is.EqualTo(3));
            
            distance = WordsHelper.LevenshteinDistance("sunday", "saturday");
            Assert.That(distance, Is.EqualTo(3));
            
            distance = WordsHelper.LevenshteinDistance("book", "back");
            Assert.That(distance, Is.EqualTo(2));
            
            distance = WordsHelper.LevenshteinDistance("abc", "qwe");
            Assert.That(distance, Is.EqualTo(3));
            
            distance = WordsHelper.LevenshteinDistance("abc", "");
            Assert.That(distance, Is.EqualTo(3));

            distance = WordsHelper.LevenshteinDistance("dasdasasadasasd", "asdasdasdsds");
            Assert.That(distance, Is.EqualTo(6));

            distance = WordsHelper.LevenshteinDistance("dsdasdasasdasasdasasdasdasdasas", "asadasasdasadasadasasasasda");
            Assert.That(distance, Is.EqualTo(9));

            distance = WordsHelper.LevenshteinDistance("aabbb", "bbbcc");
            Assert.That(distance, Is.EqualTo(4));
        }
    }
}
