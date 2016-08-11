using System;

namespace Csv
{
    public class CsvColumnAttribute : Attribute
    {
        protected readonly int _column;
        protected readonly string _fieldParserMethod;
        protected readonly bool _isRequired;

        public CsvColumnAttribute(int column, string fieldParserMethod, bool isRequired = true)
        {
            _column = column;
            _fieldParserMethod = fieldParserMethod;
            _isRequired = isRequired;
        }

        public int Column { get { return _column; } }
        public string FieldParserMethod { get { return _fieldParserMethod; } }
        public bool IsRequired { get { return _isRequired; } }
    }
}
