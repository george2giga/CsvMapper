using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsvMapper
{
    public class CsvMapperReflectionUtils
    {
        /// <summary>
        /// Set all the properties of the CVS row via reflection to the destination object
        /// </summary>
        /// <param name="destinationObj">Destination object</param>
        /// <param name="csvRow">CSV row to map</param>
        /// <returns>Object mapped</returns>
        public static T SetPropertiesViaReflection<T>(string[] row, Dictionary<string,int> mappingDictionary) where T : new()
        {
            var destinationObject = new T();
            Type type = destinationObject.GetType();
            foreach (var fieldMappings in mappingDictionary)
            {
                PropertyInfo prop = type.GetProperty(fieldMappings.Key);
                var propertyType = prop.PropertyType;
                var convertedValue = ChangeType(row[fieldMappings.Value], propertyType);
                prop.SetValue(destinationObject, convertedValue, null);
            }
            return destinationObject;
        }


        /// <summary>
        /// Get property name from the lambda expression
        /// </summary>
        /// <param name="propertyExpression">Property expression</param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var res = propertyExpression.Body as MemberExpression ?? ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;

            if (res == null)
            {
                throw new ArgumentException("Expression is not a valid property");
            }

            return res.Member.Name;
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
                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        /// <summary>
        /// Returns the default value of a value type (ints, shorts, doubles etc and structs such as DateTime)  otherwise returns null
        /// </summary>
        /// <param name="t">Target type</param>
        /// <returns>Default instance of target type</returns>
        private static object GetDefaultValue(Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}
