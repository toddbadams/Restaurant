using System;
using System.ComponentModel;

namespace Csv.Test
{

    public class ImportedPerson
    {
        private string _fullName;

        [CsvColumn(column: 1, fieldParserMethod: "String")]
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                // if possible split the full name into first and last
                FirstName = string.Empty;
                LastName = string.Empty;
                if (String.IsNullOrEmpty(_fullName)) return;
                var names = _fullName.Split(' ');
                FirstName = names[0];
                if (names.Length <= 1) return;
                LastName = names[1];
            }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DisplayName("foos bar")]
        public string Foobar { get; set; }

        [CsvColumn(column: 2, fieldParserMethod: "String")]
        public string Address { get; set; }
    }
}