using Csv;

namespace CsvService
{
    public class ImportedMenuItem
    {
        [CsvColumn(column: 1, fieldParserMethod: "String")]
        public string Name { get; set; }
        [CsvColumn(column: 2, fieldParserMethod: "Float")]
        public float Price { get; set; }

        public override string ToString()
        {
            return string.Format("{0}    {1:#.00}", Name, Price);
        }
    }
}