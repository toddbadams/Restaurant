using Csv;

namespace CsvService
{
    public class ImportedMenuItem
    {
        [CsvColumn(column: 1, fieldParserMethod: "String")]
        public string Name { get; set; }
        [CsvColumn(column: 2, fieldParserMethod: "Double")]
        public string Price { get; set; }
    }
}