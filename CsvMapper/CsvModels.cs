using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CsvMapper
{
    /// <summary>
    /// Reprents a row in the CSV file
    /// </summary>
    internal class CsvRow
    {
        public List<CsvFieldResult> CsvFieldsResult { get; private set; }

        public CsvRow(int numberOfColumns)

        {
            CsvFieldsResult = new List<CsvFieldResult>(numberOfColumns);
        }
    }

    /// <summary>
    /// Define the mapping between a field name in the target class and the position of the colum on the CSV
    /// </summary>
    public class CsvFieldTarget
    {
        public string FieldName { get; set; }
        public int Position { get; set; }
    }

    /// <summary>
    /// Result of the value in the CSV field
    /// </summary>
    internal class CsvFieldResult : CsvFieldTarget
    {
        public object FieldValue { get; set; }
    }

}

