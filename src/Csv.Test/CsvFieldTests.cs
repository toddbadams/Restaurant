using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csv.Test
{
    /// <summary>
    /// As a developer I want to read a CSV file and translate it to an IEnumerable
    /// where T is a POCO 
    /// </summary>
    [TestClass]
    public class CsvFieldTests
    {
        /// <summary>
        /// The CsvColumn creates from a POCO
        /// </summary>
        [TestMethod]
        public void ShouldCreateArrayOfCsvFields()
        {
            // arrange

            // act
            var array = CsvColumn<ImportedPerson>.Create();

            // assert
            Assert.IsNotNull(array);
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[0].Column);
            Assert.AreEqual("String", array[0].FieldParserMethod);
            Assert.AreEqual(true, array[0].IsRequired);
        }

    }
}