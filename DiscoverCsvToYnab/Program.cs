using CsvHelper;
using DiscoverCsvToYnab;
using System.Globalization;

if (args.Length == 0)
{
    Console.WriteLine($"Usage: {System.AppDomain.CurrentDomain.FriendlyName} [DiscoverFormatCsvFileName] [YnabFormatCsvFileName (optional)]");
    return -1;
}

string discoverFileName = args[0];
string ynabFileName;

if (args.Length > 1)
{
    ynabFileName = args[1];
}
else
{
    var discoverFileNameWithoutExtension = Path.GetFileNameWithoutExtension(discoverFileName);
    ynabFileName = $"{Path.GetDirectoryName(discoverFileName)}{Path.DirectorySeparatorChar}{discoverFileNameWithoutExtension}-YNAB.{Path.GetExtension(discoverFileName)}";
}

if (!File.Exists(discoverFileName))
{
    Console.WriteLine($"Cannot find Discover formatted file: {discoverFileName}");
    return -1;
}

if (File.Exists(ynabFileName))
{
    Console.WriteLine($"YNAB formatted file {ynabFileName} already exists.  Overwrite (y/N)?");
    var response = Console.ReadKey();
    if (Char.ToLower(response.KeyChar) != 'y')
    {
        return -1;
    }
}

using (var reader = new StreamReader(discoverFileName))
using (var discoverCsvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
using (var writer = new StreamWriter(ynabFileName))
using (var ynabCsvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    ynabCsvWriter.WriteHeader<YnabFormattedTransaction>();
    ynabCsvWriter.NextRecord();

    var discoverTransactions = discoverCsvReader.GetRecords<DiscoverFormattedTransaction>();

    foreach (var discoverTransaction in discoverTransactions)
    {
        var ynabTransaction = new YnabFormattedTransaction
        {
            Date = discoverTransaction.TransactionDate,
            Description = discoverTransaction.Description,
            Memo = discoverTransaction.Category
        };

        decimal ynabTransactionValue = 0;
        if (discoverTransaction.Amount < 0)
        {
            ynabTransactionValue = -discoverTransaction.Amount;
            ynabTransaction.Inflow = string.Format("{0:N2}", ynabTransactionValue);
            ynabTransaction.Outflow = string.Empty;
        }
        else
        {
            ynabTransactionValue = discoverTransaction.Amount;
            ynabTransaction.Inflow = string.Empty;
            ynabTransaction.Outflow = string.Format("{0:N2}", ynabTransactionValue);
        }

        ynabCsvWriter.WriteRecord(ynabTransaction);
        ynabCsvWriter.NextRecord();
    }

}

return 0;