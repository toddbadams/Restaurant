using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Csv
{
    /// <summary>
    /// Describes how to parse a CSV Field into a POCO
    /// </summary>
    /// <typeparam name="T">The POCO containing CSV attributes</typeparam>
    public class CsvColumn<T> where T : class, new()
    {
        public PropertyInfo PropertyInfo { get; set; }
        public int Column { get; set; }
        public string FieldParserMethod { get; set; }
        public bool IsRequired { get; set; }
        public MethodInfo ConvertMethod { get; set; }

        public void String(string value, T poco)
        {
            // check for empty string when required
            if (IsRequired && string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("should not be null or empty", "value");
            }
            PropertyInfo.SetValue(poco, value, null);
        }

        public void Float(string value, T poco)
        {
            // check for empty string when required
            if (IsRequired && string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("should not be null or empty", "value");
            }
            PropertyInfo.SetValue(poco, float.Parse(value), null);
        }

        public void Datetime(string value, T poco)
        {
            // check for empty string when required
            if (IsRequired && string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("should not be null or empty", "value");
            }
            PropertyInfo.SetValue(poco, DateTime.Parse(value), null);
        }

        public void Convert(string value, T poco)
        {
            ConvertMethod.Invoke(this, new object[] { value, poco });
        }

        /// <summary>
        /// Takes a poco and converts it to an array of CSV Fields
        /// </summary>
        /// <returns>an array of CSV Fields</returns>
        public static CsvColumn<T>[] Create()
        {
            var type = typeof(T);
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            IEnumerable<MemberInfo> properties = members.Where(m => m.MemberType == MemberTypes.Property).ToArray();
            return (from p in properties
                    let attributes = p.GetCustomAttributes(true)
                    where attributes.Length >= 1
                    let csvColumnAttribute = attributes[0] as CsvColumnAttribute
                    where csvColumnAttribute != null
                    select new CsvColumn<T>
                    {
                        FieldParserMethod = csvColumnAttribute.FieldParserMethod,
                        IsRequired = csvColumnAttribute.IsRequired,
                        Column = csvColumnAttribute.Column,
                        PropertyInfo = (PropertyInfo)p,
                        ConvertMethod = typeof(CsvColumn<T>).GetMethod(csvColumnAttribute.FieldParserMethod)
                    }).ToArray();
        }
    }
}