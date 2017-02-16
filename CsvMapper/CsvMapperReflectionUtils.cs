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
        /// Creates and instance of the destination class from a csv row and the defined property mapping.
        /// </summary>
        /// <typeparam name="T">Destination class</typeparam>
        /// <param name="row">Csv row</param>
        /// <param name="mappingDictionary">Dictionary containing the property name (key) and the position in the csv (value)</param>
        /// <returns>An initialized instance of T (1 instance per csv row)</returns>
        public static T SetPropertiesViaReflection<T>(string[] row, Dictionary<string,int> mappingDictionary) where T : new()
        {
            var destinationObject = new T();
            Type type = destinationObject.GetType();
            // iterates through the mapping list
            foreach (var fieldMappings in mappingDictionary)
            {
                // get the property from T
                PropertyInfo prop = type.GetProperty(fieldMappings.Key);
                // get the property type (ie: String, Double, DateTime)
                var propertyType = prop.PropertyType;
                // convert the value in the the csv field to the type of the target property (ie: String, Double, DateTime)
                var convertedValue = ChangeType(row[fieldMappings.Value], propertyType);
                prop.SetValue(destinationObject, convertedValue, null);
            }

            return destinationObject;
        }


        /// <summary>
        /// Get property name from the lambda expression
        /// </summary>
        /// <param name="propertyExpression">Property expression</param>
        /// <returns>Returns the property name</returns>
        public static string GetPropertyNameFromExpression<T>(Expression<Func<T, object>> propertyExpression)
        {
            var res = propertyExpression.Body as MemberExpression ?? ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;

            if (res == null)
            {
                throw new ArgumentException("Expression is not a valid property");
            }

            return res.Member.Name;
        }


        /// <summary>
        /// Returns the property name if the property exists on the target class, otherwise an empty string is returned.
        /// </summary>
        /// <typeparam name="T">Destination class</typeparam>
        /// <param name="propertyName">Property name to match</param>
        /// <returns>Returns the property name if found, empty string when not found.</returns>
        public static string GetPropertyName<T>(string propertyName)
        {
            var result = string.Empty;
            Type type = typeof(T);
            PropertyInfo prop = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop != null)
            {
                result = prop.Name;
            }

            return result;
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

            //if value is empty (we are reading from a csv so it will be treated as a string first) then assign the default value
            if ((string)value == string.Empty)
            {
                value = GetDefaultValue(conversion);
            }

            if (value == null)
            {
                return null;
            }

            //and we are on a nullable field
            //then convert it to null
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
