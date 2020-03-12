using System;
using System.Collections.Generic;
using System.IO;

namespace TableOfRecordsTask
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var records = new List<FileCabinetRecord>
            {
                new FileCabinetRecord
                {
                    Id = 1,
                    FirstName = "asdjernjewrjwejr",
                    LastName = "asdas",
                    DateOfBirth = new DateTime(2020, 1, 1),
                    Sex = 'F',
                    NumberOfReviews = 23,
                    Salary = 200000000,
                },
                new FileCabinetRecord
                {
                    Id = 2,
                    FirstName = "sadqwe",
                    LastName = "xczvzxasdhfgyadsgfyuadsgfyudg",
                    DateOfBirth = DateTime.Now,
                    Sex = 'M',
                    NumberOfReviews = 100,
                    Salary = 10000,
                },
            };

            var notes = new List<Note>
            {
                new Note("test", "sadqadeqr"),
                new Note("test2", "asdfewfwefwergfwrfew"),
                new Note("test3", "dsa"),
            };

            TableView.GetTableRepresentation(records, Console.Out);
        }
    }
}
