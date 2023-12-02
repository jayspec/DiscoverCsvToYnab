using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoverCsvToYnab
{
    internal class DiscoverFormattedTransaction
    {
        [Name("Trans. Date")]
        public DateOnly TransactionDate { get; set; }
        [Name("Post Date")]
        public DateOnly PostDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;

    }
}
