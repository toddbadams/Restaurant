using System;

namespace Csv
{
    /// <summary>
    /// Convert a string representing a CSV line into a POCO
    /// </summary>
    /// <typeparam name="T">a POCO</typeparam>
    public class CsvLine<T> where T : class, new()
    {
        private const char Seperator = '\t';

        /// <summary>
        /// convert a string representing a CSV line into a poco
        /// </summary>
        /// <param name="line">a string representing a CSV line</param>
        /// <param name="csvColumns">an array of fields representing how to parse each field of the line</param>
        /// <returns>a POCO</returns>
        public static T Create(string line, CsvColumn<T>[] csvColumns)
        {
            var result = new T();
            var values = line.Split(Seperator);

            try
            {
                foreach (var f in csvColumns)
                {
                    if (f.Column > values.Length)
                    {
                        // column required return a null POCO
                        if (f.IsRequired) return null;
                        continue;
                    }
                    f.Convert(values[f.Column - 1], result);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }
    }
}
