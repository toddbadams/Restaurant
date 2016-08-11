using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csv.Test
{
    /// <summary>
    /// As a developer I want to read a CSV file and translate it to an IEnumerable
    /// where T is a POCO 
    /// 
    /// </summary>
    [TestClass]
    public class CsvPocoTests
    {
        /// <summary>
        /// Each POCO property has an attribute that describe what column to associate with a given property
        /// </summary>
        [TestMethod]
        public void PocoHasColumnAttribute()
        {
            // arrange
            var property = GetProperties(typeof(ImportedPerson)).First(p => p.Name == "FullName");

            // act
            var attrs = property.GetCustomAttributes(true);

            // assert
            Assert.IsNotNull(attrs);
            Assert.AreEqual(1, attrs.Length);
            var attr = attrs.ToArray()[0] as CsvColumnAttribute;
            Assert.IsNotNull(attr);
            Assert.AreEqual(1, attr.Column);
        }

        /// <summary>
        /// Each POCO property has an attribute that indicates what method to use for import and validation
        /// </summary>
        [TestMethod]
        public void PocoHasFieldParserMethodAttribute()
        {
            // arrange
            var property = GetProperties(typeof(ImportedPerson)).First(p => p.Name == "FullName");

            // act
            var attrs = property.GetCustomAttributes(true);

            // assert
            Assert.IsNotNull(attrs);
            Assert.AreEqual(1, attrs.Length);
            var attr = attrs.ToArray()[0] as CsvColumnAttribute;
            Assert.IsNotNull(attr);
            Assert.AreEqual("String", attr.FieldParserMethod);
        }

        /// <summary>
        /// Each POCO property has an is optional required attribute, when true the field is required or
        /// the importer skips its' row.  The default value is true.
        /// </summary>
        [TestMethod]
        public void PocoHasSkipOnFailAttribute()
        {
            // arrange
            var property = GetProperties(typeof(ImportedPerson)).First(p => p.Name == "FullName");

            // act
            var attrs = property.GetCustomAttributes(true);

            // assert
            Assert.IsNotNull(attrs);
            Assert.AreEqual(1, attrs.Length);
            var attr = attrs.ToArray()[0] as CsvColumnAttribute;
            Assert.IsNotNull(attr);
            Assert.AreEqual(true, attr.IsRequired);
        }

        private static IEnumerable<MemberInfo> GetProperties(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var properties = members.Where(m => m.MemberType == MemberTypes.Property);
            return properties;
        }
    }
}
