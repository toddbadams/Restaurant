using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csv.Test
{
    /// <summary>
    /// As a developer I want to read a CSV file and translate it to an IEnumerable
    /// where T is a POCO 
    /// </summary>
    [TestClass]
    public class CsvLineTests
    {
        /// <summary>
        /// The CsvLine Creates a Poco from a string line and an array of CsvFields
        /// </summary>
        [TestMethod]
        public void ShouldCreatePoco()
        {
            // arrange
            var csvFields = CsvColumn<ImportedPerson>.Create();
            const string line = "Shelby Macias	3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|Finland	1 66 890 3865-9584	et@eratvolutpat.ca";

            // act
            var importedPerson = CsvLine<ImportedPerson>.Create(line, csvFields);

            // assert
            Assert.IsNotNull(importedPerson);
            Assert.AreEqual("Shelby Macias", importedPerson.FullName);
            Assert.AreEqual("Shelby", importedPerson.FirstName);
            Assert.AreEqual("Macias", importedPerson.LastName);
        }

        /// <summary>
        /// The CsvLine Creates a null Poco from a string line and an array of CsvFields
        /// when a required field is null or empty
        /// </summary>
        [TestMethod]
        public void ShouldCreateNullWhenRequiredFieldIsEmpty()
        {
            // arrange
            var csvFields = CsvColumn<ImportedPerson>.Create();
            const string line = "	3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|Finland	1 66 890 3865-9584	et@eratvolutpat.ca";

            // act
            var importedPerson = CsvLine<ImportedPerson>.Create(line, csvFields);

            // assert
            Assert.IsNull(importedPerson);
        }

        /// <summary>
        /// The CsvLine Creates a null Poco from a string line and an array of CsvFields
        /// when a required field has a column number greater than the number of fields in the line
        /// 
        /// to be done
        /// </summary>
        [TestMethod]
        public void ShouldCreateNullWhenRequiredFieldColumnGreaterThanLineFields()
        {
            // arrange
            var csvFields = CsvColumn<ImportedPerson>.Create();
            const string line = "Shelby Macias";

            // act
            var importedPerson = CsvLine<ImportedPerson>.Create(line, csvFields);

            // assert
            Assert.IsNull(importedPerson);
        }
    }
}