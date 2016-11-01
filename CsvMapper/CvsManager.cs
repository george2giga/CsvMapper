using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace CsvMapper
{
    public class CsvManager<T> where T : new()
    {
        private readonly string _filePath;
        public CsvMapperConfiguration CsvMapperConfiguration { get; private set; }
        public Dictionary<string, int> MappingDictionary { get; }

        /// <summary>
        ///     Initialize class a mapper class with Autoset = false, FirstLineHeader = false and default separator is ','
        /// </summary>
        /// <param name="filePath">Spreadsheet file path</param>
        public CsvManager(string filePath)
        {
            ValidateFilePath(filePath);
            _filePath = filePath;
            MappingDictionary = new Dictionary<string, int>();
            CsvMapperConfiguration = new CsvMapperConfiguration();
        }

        /// <summary>
        ///     Initialize class a mapper class and accepts a CsvMapperConfiguration (with separator, autoset etc)
        /// </summary>
        /// <param name="filePath">Spreadsheet file path</param>
        /// <param name="csvMapperConfiguration">Mapper configuration settings (separator, header on first line, autoset) (</param>
        public CsvManager(string filePath, CsvMapperConfiguration csvMapperConfiguration) : this(filePath)
        {
            CsvMapperConfiguration = csvMapperConfiguration;
            if (CsvMapperConfiguration.AutoSet)
            {
                InitializeAutoSet();
                // The first line contains the header if autoset is set to true
                CsvMapperConfiguration.FirstLineHeader = true;
            }
        }

        private void ValidateFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new NullReferenceException("Missing csv source filepath");
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("File not found: {0} ", filePath));
            }
        }

        private void InitializeAutoSet()
        {
            using (var readFile = new StreamReader(_filePath))
            {
                var headerLine = readFile.ReadLine();
                AutoSetPropertyFields(headerLine);
            }
        }

        /// <summary>
        ///     Defines the mapping between the class property and its position in the file
        /// </summary>
        /// <param name="propertyToMap">Propery to map</param>
        /// <param name="position">Position of the property field in the csv file (ie: 1 is the first column)</param>
        /// <returns>Mapper Instance with field set</returns>
        public CsvManager<T> SetField(Expression<Func<T, object>> propertyToMap, int position)
        {
            var propertyName = CsvMapperReflectionUtils.GetPropertyNameFromExpression(propertyToMap);
            // remove the property from the mapping list if it already exists
            if (MappingDictionary.Any(x => x.Key == propertyName))
            {
                MappingDictionary.Remove(propertyName);
            }
            MappingDictionary.Add(propertyName, position);

            return this;
        }

        /// <summary>
        ///     Removes a property from the mapping list
        /// </summary>
        /// <param name="propertyToRemove">Property name to remove</param>
        /// <returns>Mapper instance with deleted property</returns>
        public CsvManager<T> RemoveFieldFromMapping(Expression<Func<T, object>> propertyToRemove)
        {
            var propertyName = CsvMapperReflectionUtils.GetPropertyNameFromExpression(propertyToRemove);
            // remove the property from the mapping list if it already exists
            if (MappingDictionary.Any(x => x.Key == propertyName))
            {
                MappingDictionary.Remove(propertyName);
            }

            return this;
        }


        /// <summary>
        ///     Get an iterator over the csv file
        /// </summary>
        /// <returns>Iterator over the CSV lines</returns>
        public IEnumerable<T> Load()
        {
            using (var readFile = new StreamReader(_filePath))
            {
                // if first line is header or autoset is true then skip the first line
                if (CsvMapperConfiguration.FirstLineHeader || CsvMapperConfiguration.AutoSet)
                {
                    readFile.ReadLine();
                }
                string line;

                while ((line = readFile.ReadLine()) != null)
                {
                    var row = line.Split(CsvMapperConfiguration.Separator);
                    var resultRow = CsvMapperReflectionUtils.SetPropertiesViaReflection<T>(row, MappingDictionary);
                    yield return resultRow;
                }
            }
        }

        /// <summary>
        ///     Autoset the csv fields (case insensitive) from the header line
        /// </summary>
        /// <param name="headerLine">File header line</param>
        private void AutoSetPropertyFields(string headerLine)
        {
            var columns = headerLine.Split(CsvMapperConfiguration.Separator);
            for (var i = 0; i < columns.Length; i++)
            {
                var propName = columns[i].Replace(" ", string.Empty).Replace("\"", string.Empty);
                // checks if the autoset property (coming from the header) is a valid property of T
                var propertyNameFound = CsvMapperReflectionUtils.GetPropertyName<T>(propName);
                // Add property name and position to the mapping
                if (!string.IsNullOrEmpty(propertyNameFound))
                {
                    MappingDictionary.Add(propertyNameFound, i);
                }
                else
                {
                    // we don't want to throw an exception if a column cannot be set.
                    Console.WriteLine("Cannot autoset {0}", propName);
                }
            }
        }
    }
}
