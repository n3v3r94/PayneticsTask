using Paynetics.BusinessLogic.Models;

namespace Paynetics.BusinessLogic.Services
{
    public class TransactionGenerator
    {
        private static readonly Random random = new Random();

        public static TransactionXml GenerateRandomTransaction()
        {
            var bankNameIndex = random.Next(1, 200);
            var transaction = new TransactionXml
            {
                ExternalId = random.Next(1, 2_000_000_000).ToString(),
                CreateDate = DateTime.UtcNow.AddSeconds(-random.Next(1, 10000)),
                Amount = new Amount
                {
                    Direction = random.Next(0, 2) == 0 ? "C" : "D",
                    Value = Math.Round((decimal)(random.NextDouble() * 10000), 2),
                    Currency = "EUR"
                },
                Status = random.Next(0, 2), 
               
                Debtor = new Debtor
                {
                    BankName = $"ING BANK N.V.{bankNameIndex}",
                    BIC = "INGBNL2A",
                    IBAN = "NL68INGB5831335380"
                },
                Beneficiary = new Beneficiary
                {
                    BankName = $"Bulgarian Bank{bankNameIndex}",
                    BIC = "INGBNL2A",
                    IBAN = "BG83IORT80949736921315"
                }
            };
            return transaction;
        }

        public static string GenerateXmlForTransactions(int numberOfTransactions,string fileName)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<Operation>");
                writer.WriteLine($"  <FileDate>{DateTime.UtcNow:yyyy-MM-dd}</FileDate>");
                writer.WriteLine($"  <Transactions>");

                for (int i = 0; i < numberOfTransactions; i++)
                {
                    var transaction = GenerateRandomTransaction();

                    writer.WriteLine("    <Transaction>");
                    writer.WriteLine($"      <ExternalId>{transaction.ExternalId}</ExternalId>");
                    writer.WriteLine($"      <CreateDate>{transaction.CreateDate:yyyy-MM-ddTHH:mm:ss.fffZ}</CreateDate>");
                    writer.WriteLine("      <Amount>");
                    writer.WriteLine($"        <Direction>{transaction.Amount?.Direction}</Direction>");
                    writer.WriteLine($"        <Value>{transaction.Amount?.Value}</Value>");
                    writer.WriteLine($"        <Currency>{transaction.Amount?.Currency}</Currency>");
                    writer.WriteLine("      </Amount>");
                    writer.WriteLine($"      <Status>{transaction.Status}</Status>");
                    writer.WriteLine("      <Debtor>");
                    writer.WriteLine($"        <BankName>{transaction.Debtor?.BankName ?? string.Empty}</BankName>");
                    writer.WriteLine($"        <BIC>{transaction.Debtor?.BIC ?? string.Empty}</BIC>");
                    writer.WriteLine($"        <IBAN>{transaction.Debtor?.IBAN ?? string.Empty}</IBAN>");
                    writer.WriteLine("      </Debtor>");
                    writer.WriteLine("      <Beneficiary>");
                    writer.WriteLine($"        <BankName>{transaction.Beneficiary?.BankName ?? string.Empty}</BankName>");
                    writer.WriteLine($"        <BIC>{transaction.Beneficiary?.BIC ?? string.Empty}</BIC>");
                    writer.WriteLine($"        <IBAN>{transaction.Beneficiary?.IBAN ?? string.Empty}</IBAN>");
                    writer.WriteLine("      </Beneficiary>");
                    writer.WriteLine("    </Transaction>");
                }

                writer.WriteLine("  </Transactions>");
                writer.WriteLine("</Operation>");
            }

            return filePath;
        }
    }
}
