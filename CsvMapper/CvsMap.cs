using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsvMapper
{
    public class CsvMap<T> where T : new()
    {
        private readonly bool _autoSet;
        private readonly string _filePath;
        private readonly bool _firstLineHeader;
        private readonly char _separator;

        public Dictionary<string,int> MappingDictionary{ get; private set; }

        /// <summary>
        /// Initialize class with AutoSet and infer class mapping from the spreadsheet first row (header).
        /// First line is 
        /// Default separator is ','
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isFirstLineHeader"></param>
        /// <param name="separator"></param>
        /// <param name="autoSet"></param>
        public CsvMap(string filePath, bool isFirstLineHeader = true, char separator = ',', bool autoSet = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new NullReferenceException("Missing csv source filepath");
            }
            else if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("File not found: {0} ", filePath));
            }

            MappingDictionary = new Dictionary<string, int>();
            _filePath = filePath;
            _separator = separator;
            _autoSet = autoSet;

            if (_autoSet)
            {
                InitializeAutoSet();
                _firstLineHeader = true;
            }
            else
            {
                _firstLineHeader = isFirstLineHeader;
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
        /// Define the mapping between the class property and its position in the file
        /// </summary>
        /// <param name="propertyToMap">Propery to map</param>
        /// <param name="position">Position of the property field in the csv file (ie: 1 is the first column)</param>
        /// <returns>Mapper Instance with field set</returns>
        public CsvMap<T> SetField(Expression<Func<T, object>> propertyToMap, int position)
        {
            var propertyName = CsvMapperReflectionUtils.GetPropertyName(propertyToMap);
            // remove the property from the mapping list if it already exists
            if (MappingDictionary.Any(x => x.Key == propertyName))
            {
                MappingDictionary.Remove(propertyName);
            }
            MappingDictionary.Add(propertyName, position);

            return this;
        }

      
        /// <summary>
        /// Get an iterator over the csv file
        /// </summary>
        /// <returns>Iterator over the CSV lines</returns>
        public IEnumerable<T> Load()
        {
            using (var readFile = new StreamReader(_filePath))
            {
                // if first line is header or autoset is true then skip the first line
                if (_firstLineHeader || _autoSet)
                {
                    readFile.ReadLine();
                }
                string line;

                while ((line = readFile.ReadLine()) != null)
                {
                    string[] row = line.Split(_separator);
                    var resultRow = CsvMapperReflectionUtils.SetPropertiesViaReflection<T>(row, MappingDictionary);
                    yield return resultRow;
                }
            }
        }

        /// <summary>
        /// Autoset the csv fields (case insensitive) from the header line
        /// </summary>
        /// <param name="headerLine">File header line</param>
        private void AutoSetPropertyFields(string headerLine)
        {
            string[] columns = headerLine.Split(this._separator);
            Type type = typeof(T);
            for (int i = 0; i < columns.Length; i++)
            {
                var propName = columns[i].Replace(" ", string.Empty).Replace("\"", string.Empty);
                PropertyInfo prop = type.GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                //Add property name and position to the mapping
                if (prop != null)
                {
                    MappingDictionary.Add(prop.Name,i);
                }
                else
                {
                    //we don't want to throw an exception if a column cannot be set.
                    Console.WriteLine("Cannot autoset {0}", propName);
                }
            }
        }
    }
}
