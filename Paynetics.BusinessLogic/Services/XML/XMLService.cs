using Paynetics.BusinessLogic.Models;
using System.Xml;

namespace Paynetics.BusinessLogic.Services.XML
{
    public class XMLService : IXMLService
    {
        public async IAsyncEnumerable<TransactionBatch> ReadTransactionBatch(Stream stream, int batchCount = 25000)
        {
            var transactionBatch = new TransactionBatch();
            var settings = new XmlReaderSettings
            {
                Async = true,
            };

            using (var reader = XmlReader.Create(stream, settings))
            {
                while (await reader.ReadAsync())
                {
                    if (reader.IsStartElement("Transaction"))
                    {
                        var transaction = await ReadTransactionAsync(reader);

                        transactionBatch.AddTransaction(transaction);
                        

                        if (transactionBatch.Transactions.Count >= batchCount)
                        {
                            yield return transactionBatch;
                            transactionBatch.ResetBatch();
                        }
                    }
                }

                if (transactionBatch.Transactions.Count > 0)
                {
                    yield return transactionBatch;
                }
            }
        }

        private async Task<TransactionXml> ReadTransactionAsync(XmlReader reader)
        {
            var transaction = new TransactionXml();

            while (await reader.ReadAsync())
            {
                if (reader.IsStartElement())
                {
                    transaction = reader.Name switch
                    {
                        "ExternalId" => await SetTransactionProperty(transaction, reader, async t => t.ExternalId = await reader.ReadElementContentAsStringAsync()),
                        "CreateDate" => await SetTransactionProperty(transaction, reader, async t => t.CreateDate = await reader.ReadContentAsDateTimeOffsetAsync()),
                        "Amount" => await SetTransactionProperty(transaction, reader, async t => t.Amount = await ReadAmountAsync(reader)),
                        "Status" => await SetTransactionProperty(transaction, reader, async t => t.Status = await reader.ReadContentAsIntAsync()),
                        "Debtor" => await SetTransactionProperty(transaction, reader, async t => t.Debtor = await ReadDebtorAsync(reader)),
                        "Beneficiary" => await SetTransactionProperty(transaction, reader, async t => t.Beneficiary = await ReadBeneficiaryAsync(reader)),
                        _ => transaction // Default case
                    };
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Transaction")
                {
                    break;
                }
            }

            return transaction;
        }

        private async Task<TransactionXml> SetTransactionProperty(TransactionXml transaction, XmlReader reader, Func<TransactionXml, Task> setProperty)
        {
            await setProperty(transaction);
            return transaction;
        }

        private async Task<Amount> ReadAmountAsync(XmlReader reader)
        {
            var amount = new Amount();

            while (await reader.ReadAsync())
            {
                if (reader.IsStartElement())
                {
                    amount = reader.Name switch
                    {
                        "Direction" => await SetAmountProperty(amount, reader, async a => a.Direction = await reader.ReadElementContentAsStringAsync()),
                        "Value" => await SetAmountProperty(amount, reader, async a => a.Value = await reader.ReadContentAsDecimalAsync()),
                        "Currency" => await SetAmountProperty(amount, reader, async a => a.Currency = await reader.ReadElementContentAsStringAsync()),
                        _ => amount // Default case
                    };
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Amount")
                {
                    break;
                }
            }

            return amount;
        }

        private async Task<Amount> SetAmountProperty(Amount amount, XmlReader reader, Func<Amount, Task> setProperty)
        {
            await setProperty(amount);
            return amount;
        }

        private async Task<Debtor> ReadDebtorAsync(XmlReader reader)
        {
            var debtor = new Debtor();

            while (await reader.ReadAsync())
            {
                if (reader.IsStartElement())
                {
                    debtor = reader.Name switch
                    {
                        "BankName" => await SetDebtorProperty(debtor, reader, async d => d.BankName = await reader.ReadElementContentAsStringAsync()),
                        "BIC" => await SetDebtorProperty(debtor, reader, async d => d.BIC = await reader.ReadElementContentAsStringAsync()),
                        "IBAN" => await SetDebtorProperty(debtor, reader, async d => d.IBAN = await reader.ReadElementContentAsStringAsync()),
                        _ => debtor
                    };
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Debtor")
                {
                    break;
                }
            }

            return debtor;
        }

        private async Task<Debtor> SetDebtorProperty(Debtor debtor, XmlReader reader, Func<Debtor, Task> setProperty)
        {
            await setProperty(debtor);
            return debtor;
        }

        private async Task<Beneficiary> ReadBeneficiaryAsync(XmlReader reader)
        {
            var beneficiary = new Beneficiary();

            while (await reader.ReadAsync())
            {
                if (reader.IsStartElement())
                {
                    beneficiary = reader.Name switch
                    {
                        "BankName" => await SetBeneficiaryProperty(beneficiary, reader, async b => b.BankName = await reader.ReadElementContentAsStringAsync()),
                        "BIC" => await SetBeneficiaryProperty(beneficiary, reader, async b => b.BIC = await reader.ReadElementContentAsStringAsync()),
                        "IBAN" => await SetBeneficiaryProperty(beneficiary, reader, async b => b.IBAN = await reader.ReadElementContentAsStringAsync()),
                        _ => beneficiary
                    };
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Beneficiary")
                {
                    break;
                }
            }

            return beneficiary;
        }

        private async Task<Beneficiary> SetBeneficiaryProperty(Beneficiary beneficiary, XmlReader reader, Func<Beneficiary, Task> setProperty)
        {
            await setProperty(beneficiary);
            return beneficiary;
        }

        private async Task ProcessBatchAsync(List<TransactionXml> transactionBatch)
        {
            await Console.Out.WriteLineAsync(transactionBatch.Count.ToString());
            // Process the batch (e.g., save to the database)
            // await _transactionRepository.SaveTransactionsAsync(transactionBatch);
        }
    }
}

