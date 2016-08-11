using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csv.Test
{
    [TestClass]
    public class FileSplitterTest
    {
        private string _workingDirectory;

        [TestInitialize]
        public void Initialize()
        {
            // our working directory
            _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // remove any csv files from the working directory
            foreach (var f in new DirectoryInfo(_workingDirectory).GetFiles("*.csv"))
            {
                f.Delete();
            }
            // copy the test data into the working directory
            File.Copy(_workingDirectory + "\\..\\..\\test_data\\contacts.csv", _workingDirectory + "\\contacts.csv");
        }

        /// <summary>
        /// If the input file does not exist throw an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowWhenFileDoesNotExist()
        {
            // arrange
            var filename = _workingDirectory + "\\doesnotexist.csv";

            // act
            FileParser.SplitFile(filename);

            // Assert
        }


        /// <summary>
        /// If the maximum number of lines is less than one throw and execption
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowWhenMaxLinesLessThanOne()
        {
            // arrange
            var filename = _workingDirectory + "\\contacts.csv";

            // act
            FileParser.SplitFile(filename,0);

            // Assert
        }

        /// <summary>
        /// Split an input file into seperate files of no more than (numberOfLines) lines
        /// Remove the input file
        /// </summary>
        [TestMethod]
        public void ShouldSplitFileWhenNumberOfLinesLessThanTotalNumberOfLines()
        {
            // arrange
            var filename = _workingDirectory + "\\contacts.csv";

            // act
            FileParser.SplitFile(filename);

            // Assert
            Assert.IsTrue(File.Exists(_workingDirectory + "\\contacts0.csv"));
            Assert.IsTrue(File.Exists(_workingDirectory + "\\contacts1.csv"));
            Assert.IsTrue(File.Exists(_workingDirectory + "\\contacts2.csv"));
            Assert.IsFalse(File.Exists(_workingDirectory + "\\contacts.csv"));
        }

        /// <summary>
        /// Split an input file into seperate files of no more than (numberOfLines) lines
        /// Remove the input file
        /// </summary>
        [TestMethod]
        public void ShouldSplitFileWhenNumberOfLinesGreaterThanTotalNumberOfLines()
        {
            // arrange
            var filename = _workingDirectory + "\\contacts.csv";

            // act
            FileParser.SplitFile(filename,500);

            // Assert
            Assert.IsTrue(File.Exists(_workingDirectory + "\\contacts0.csv"));
        }
    }
}
