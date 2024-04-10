using System;

namespace BudgetApp;

public class Transaction
{
    private string _table = "transactions";

    public Guid Id = Guid.NewGuid();
    public Guid BudgetId;
    public string Name;
    public float Amount;
    public TransactionCategory Category;
    public bool IsIncome;
    public DateTime TransactionDate;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;


}
