using System;

namespace BudgetApp;

public class TransactionCategory
{
    public Guid Id = Guid.NewGuid();
    public string Name;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;
}
