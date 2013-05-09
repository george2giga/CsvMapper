using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvMapper
{
    /// <summary>
    /// Reader class for the CSV file
    /// </summary>
    internal class CsvReader
    {
        #region Public members

        public List<CsvFieldTarget> CsvFieldTargets { get; private set; }
        public List<CsvRow> CsvRowsResult { get; private set; }
        public char Separator { get; private set; }
        public bool IsFirstLineColumnName { get; private set; }
        public string FilePath { get; private set; }
        
        #endregion

        #region Constructors
        /// <summary>
        /// CSV reader constructor expecting the CSV file path, mappings, column separator and if there is a header in the file. 
        /// </summary>
        /// <param name="filePath">Path of the CSV file</param>
        /// <param name="csvFieldTargets">Mappings between object properties and CSV columns</param>
        /// <param name="separator">CSV column separator</param>
        /// <param name="isFirstLineColumnName">First line contains column names</param>
        public CsvReader(string filePath, List<CsvFieldTarget> csvFieldTargets, char separator, bool isFirstLineColumnName)
        {
            FilePath = filePath;
            CsvFieldTargets = csvFieldTargets;
            Separator = separator;
            IsFirstLineColumnName = isFirstLineColumnName;
            CsvRowsResult = new List<CsvRow>();
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Reads all the CSV skipping the first line if there's a separator
        /// </summary>
        /// <returns>CSV rows</returns>
        public List<CsvRow> ReadCsvRows()
        {
            CsvRowsResult = IsFirstLineColumnName ? GetCsvRowIterator().Skip(1).ToList() : GetCsvRowIterator().ToList();
            return CsvRowsResult;
        }

        #endregion

        #region Private methods


        /// <summary>
        /// Get an iterator over the csv file
        /// </summary>
        /// <returns>Iterator over the CSV lines</returns>
        private IEnumerable<CsvRow> GetCsvRowIterator()
        {
            using (var readFile = new StreamReader(FilePath))
            {
                string line;

                while ((line = readFile.ReadLine()) != null)
                {
                    string[] row = line.Split(Separator);
                    var resultRow = BuildCsvRow(row);
                    yield return resultRow;
                }
            }
        }

        /// <summary>
        /// Helper method to create a CSV row
        /// </summary>
        /// <param name="row">Array of row fields</param>
        /// <returns>CSV row</returns>
        private CsvRow BuildCsvRow(string[] row)
        {
            var resultRow = new CsvRow(CsvFieldTargets.Count);
            foreach (var csvField in CsvFieldTargets)
            {
                var field = new CsvFieldResult()
                {
                    FieldName = csvField.FieldName,
                    Position = csvField.Position,
                    FieldValue = row[csvField.Position]
                };
                resultRow.CsvFieldsResult.Add(field);
            }

            return resultRow;
          
        }

        #endregion

    }
}
