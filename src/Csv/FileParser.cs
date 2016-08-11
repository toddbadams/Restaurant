using System;
using System.IO;

namespace Csv
{
    public class FileParser
    {
        /// <summary>
        /// Splits a file at End of Line (EOL) where each file is not greater than the
        /// number of lines given.
        /// </summary>
        /// <param name="inputFile">a fully qualified filename</param>
        /// <param name="numberOfLines">The maximum of lines permittid in each output file, defaults to 100</param>
        public static void SplitFile(string inputFile, int numberOfLines = 100)
        {
            if (!File.Exists(inputFile)) throw new ArgumentException(inputFile + "does not exist", "inputFile");
            if (numberOfLines < 1) throw new ArgumentException("number of lines must be at least 1", "numberOfLines");
            var fileInfo = new FileInfo(inputFile);

            using (var file = new StreamReader(inputFile))
            {
                var index = 0;
                var line = file.ReadLine();
                while (line != null)
                {
                    var outputFile = fileInfo.Directory + "\\"
                        + fileInfo.Name.Split('.')[0]
                        + index
                        + fileInfo.Extension;

                    using (var output = new StreamWriter(outputFile))
                    {
                        var i = 0;
                        while (i++ < numberOfLines && line != null)
                        {
                            output.WriteLine(line);
                            line = file.ReadLine();
                        }
                    }
                    index++;
                }
            }

            // all done delete input file
            File.Delete(inputFile);
        }
    }
}
