using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TableOfRecordsTask
{
    /// <summary>Table view.</summary>
    public static class TableView
    {
        /// <summary>Gets the table representation.</summary>
        /// <typeparam name="T">Type with public properties.</typeparam>
        /// <param name="records">The records.</param>
        /// <param name="writer">The writer.</param>
        /// <exception cref="ArgumentNullException">Thrown when records
        /// or
        /// writer
        /// is null.</exception>
        public static void GetTableRepresentation<T>(IEnumerable<T> records, TextWriter writer)
        {
            Validation(records, writer);

            var columns = typeof(T).GetProperties();
            if (columns.Length == 0)
            {
                throw new ArgumentException($"{nameof(records)} doesn't have public properties.");
            }

            var stringsTable = new List<string[]>();
            var maxLengthsOfColumns = new List<int>();
            maxLengthsOfColumns = columns.Select(c => c.Name.Length).ToList();

            foreach (var record in records)
            {
                var stringsRow = new string[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i].PropertyType == typeof(DateTime))
                    {
                        var date = (DateTime)record.GetType().GetProperty(columns[i].Name).GetValue(record);
                        var dateString = date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        stringsRow[i] = dateString;
                    }
                    else
                    {
                        var stringValue = record.GetType().GetProperty(columns[i].Name).GetValue(record).ToString();
                        stringsRow[i] = stringValue;
                    }

                    if (stringsRow[i].Length > maxLengthsOfColumns[i])
                    {
                        maxLengthsOfColumns[i] = stringsRow[i].Length;
                    }
                }

                stringsTable.Add(stringsRow);
            }

            maxLengthsOfColumns.ForEach(length => writer.Write("+-" + new string('-', length) + '-'));
            writer.WriteLine("+");

            var line = string.Empty;
            for (int i = 0; i < columns.Length; i++)
            {
                line += "| " + columns[i].Name.PadRight(maxLengthsOfColumns[i]) + ' ';
            }

            writer.WriteLine(line + "|");
            maxLengthsOfColumns.ForEach(length => writer.Write("+-" + new string('-', length) + '-'));
            writer.WriteLine("+");

            if (!records.Any())
            {
                return;
            }

            foreach (var row in stringsTable)
            {
                line = string.Empty;
                for (int i = 0; i < columns.Length; i++)
                {
                    if (columns[i].PropertyType == typeof(string) || columns[i].PropertyType == typeof(char))
                    {
                        line += "| " + row[i].PadRight(maxLengthsOfColumns[i]) + ' ';
                    }
                    else
                    {
                        line += "| " + row[i].PadLeft(maxLengthsOfColumns[i]) + ' ';
                    }
                }

                writer.WriteLine(line + '|');
            }

            maxLengthsOfColumns.ForEach(length => writer.Write("+-" + new string('-', length) + '-'));
            writer.WriteLine("+");
        }

        private static void Validation<T>(IEnumerable<T> records, TextWriter writer)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
        }
    }
}
