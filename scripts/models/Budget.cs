using System;
using System.Collections.Generic;
using System.Transactions;

namespace BudgetApp;

public class Budget
{
    public Guid Id = Guid.NewGuid();
    public string Name;
    public DateTime StartDate;
    public DateTime EndDate;
    public List<Transaction> Transactions;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;
}
