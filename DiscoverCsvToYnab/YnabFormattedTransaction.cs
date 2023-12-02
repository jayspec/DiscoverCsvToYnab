using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoverCsvToYnab
{
    internal class YnabFormattedTransaction
    {
        [Index(0)]
        public DateOnly Date { get; set; }
        [Index(1)]
        public string Description { get; set; } = string.Empty;
        [Index(2)]
        public string Memo { get; set; } = string.Empty;
        [Index(3)]
        public string Outflow { get; set; } = string.Empty;
        [Index(4)]
        public string Inflow { get; set; } = string.Empty;
    }
}
