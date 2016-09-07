using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CsvMapper
{
    /// <summary>
    /// Main class responsible to setup the csv source and trigger the parsing.
    /// </summary>
    /// <typeparam name="T">Type of the target class</typeparam>
    public class CsvManager<T> where T: new()
    {
        public char DefaultSeparator { get; set; }
        public List<CsvFieldTarget> CsvFieldsToMap { get; set; }
        public string FilePath { get; set; }
        private CsvReader Reader { get; set; }
        public bool IsFirstLineColumnName { get; private set; }
        public bool AutoSet { get; private set; }


        /// <summary>
        /// CSV manager constructor expecting the CSV file path,  column separator and if there is a header in the file. 
        /// </summary>
        /// <param name="filePath">Path of the CSV file</param>
        /// <param name="isFirstLineColumnName">CSV column separator</param>
        /// <param name="separator">First line contains column names</param>
        /// <param name="autoSetColumns">Infer column names and automatically map them to object properties</param>
        public CsvManager(string filePath, bool isFirstLineColumnName, char separator, bool autoSetColumns = false)
        {
            CsvFieldsToMap = new List<CsvFieldTarget>();
            //Setting default separator 
            DefaultSeparator = separator;
            IsFirstLineColumnName = isFirstLineColumnName;
            FilePath = filePath;
            AutoSet = autoSetColumns;
        }


        /// <summary>
        /// Simple constructor, the CSV contains the header row, default separator is a comma and Autoset it set to true
        /// </summary>
        /// <param name="filePath">Full file path of the spreadsheet</param>
        /// <param name="separator">Columns separator</param>
        public CsvManager(string filePath, char separator = ','):this(filePath, true , separator, true){}

        #region Public methods

        ///// <summary>
        ///// Setting fields for each property
        ///// </summary>
        ///// <param name="expression">Property to target</param>
        ///// <param name="position">CSV column position</param>
        public void SetField(Expression<Func<T, Object>> expression, int position)
        {
            //if Autoset is enabled then ignore the manual field mapping
            if (!AutoSet)
            {
                var propertyMemberInfo = GetPropertyMemberInfo(expression);
                RegisterFieldToMap(propertyMemberInfo, position);
            }
        }

        /// <summary>
        /// Retrieve the list of mapped objects
        /// </summary>
        /// <returns>Mapped objects list</returns>
        public List<T> GetObjectList()
        {
            Reader = new CsvReader(FilePath, CsvFieldsToMap, DefaultSeparator, IsFirstLineColumnName);
            //If enabled, execute Autoset
            if (AutoSet)
            {
                var headerLine = Reader.GetHeaderColumn();
                AutoSetPropertyFields(headerLine);
            }

            var csvRows = GetRowsFromFile();
            var resultList = new List<T>(csvRows.Count);
            foreach (var csvRow in csvRows)
            {
                var destinationObject = new T();
                var createdObj = SetPropertiesViaReflection(destinationObject, csvRow);
                resultList.Add(createdObj);
            }
            return resultList;
        }

        /// <summary>
        /// Cast the property value to the provided type. Uses ChangeType underneath and it can cast Nullable types
        /// </summary>
        /// <param name="value">Property value</param>
        /// <param name="conversion">Target type for the conversion</param>
        /// <returns>Property value casted to the desired type</returns>
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            //if value is empty (we are reading from a csv so it will be treated as a string first)
            //and we are on a nullable field
            //then convert it to null
            if ((string)value == string.Empty)
            {
                value = GetDefaultValue(conversion);
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        #endregion

        #region Private methods

        public void AutoSetPropertyFields(string headerLine)
        {
            if (IsFirstLineColumnName)
            {
                string[] columns = headerLine.Split(DefaultSeparator);
                Type type = typeof(T);
                for (int i = 0; i < columns.Length; i++)
                {
                    var propName = columns[i].Replace(" ", string.Empty).Replace("\"", string.Empty);
                    PropertyInfo prop = type.GetProperty(propName);
                    if (prop != null)
                    {
                        CsvFieldsToMap.Add(new CsvFieldTarget()
                        {
                            FieldName = propName,
                            Position = i
                        });
                    }
                    else
                    {
                        Console.WriteLine("Cannot autoset {0}", propName);
                    }
                }
            }
        }


        /// <summary>
        /// Creating a mapping object between the target class property and the CSV position
        /// </summary>
        /// <param name="expression">Target property</param>
        /// <param name="position">CSV column position</param>
        private MemberInfo GetPropertyMemberInfo(Expression expression)
        {
             return GetMemberInfo(expression);
        }

        private void RegisterFieldToMap(MemberInfo propertyInfo, int position)
        {
            CsvFieldsToMap.Add(new CsvFieldTarget()
            {
                FieldName = propertyInfo.Name,
                Position = position
            });
        }

        /// <summary>
        /// Get all the rows from the CSV file
        /// </summary>
        /// <returns>CSV file rows</returns>
        private List<CsvRow> GetRowsFromFile()
        {
            var csvRows = Reader.ReadCsvRows();
            return csvRows;

        }
        
        /// <summary>
        /// Set all the properties of the CVS row via reflection to the destination object
        /// </summary>
        /// <param name="destinationObj">Destination object</param>
        /// <param name="csvRow">CSV row to map</param>
        /// <returns>Object mapped</returns>
        private T SetPropertiesViaReflection(T destinationObj, CsvRow csvRow)
        {
            Type type = destinationObj.GetType();
            foreach (var csvFieldResult in csvRow.CsvFieldsResult)
            {
                PropertyInfo prop = type.GetProperty(csvFieldResult.FieldName);
                var propertyType = prop.PropertyType;
                var convertedValue = ChangeType(csvFieldResult.FieldValue, propertyType);
                prop.SetValue(destinationObj, convertedValue, null);
            }
            return destinationObj;
        }

        /// <summary>
        /// Check the type of expression and returns the member (property).
        /// </summary>
        /// <param name="method">Lambda defining the mapping between property and CSV column</param>
        /// <returns>Info about the mapped property</returns>
        private static MemberInfo GetMemberInfo(Expression method)
        {
            var lambda = method as LambdaExpression;
            if (lambda == null)
                throw new InvalidCastException("Invalid lambda expression");

            MemberExpression memberExpression = null;

            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    memberExpression =
                        ((UnaryExpression) lambda.Body).Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    memberExpression = lambda.Body as MemberExpression;
                    break;
            }

            if (memberExpression == null)
                throw new ArgumentException("Invalid expression");

            return memberExpression.Member;
        }

        private static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }

        #endregion
    }
}

