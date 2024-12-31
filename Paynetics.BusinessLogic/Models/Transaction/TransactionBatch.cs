namespace Paynetics.BusinessLogic.Models
{
    public class TransactionBatch
    {
        public List<TransactionXml> Transactions { get; set; } = new List<TransactionXml>();
        public HashSet<string?> Benefits { get; set; } = new HashSet<string?>();
        public HashSet<string?> Debts { get; set; } = new HashSet<string?>();

        public void ResetBatch()
        {
            Transactions.Clear();
            Benefits.Clear();
            Debts.Clear();
        }

        public void AddTransaction(TransactionXml transaction)
        {
            Transactions.Add(transaction);

            if (transaction?.Beneficiary != null)
                Benefits.Add(transaction.Beneficiary.BankName);

            if (transaction?.Debtor != null)
                Debts.Add(transaction.Debtor.BankName);
        }
    }
}
