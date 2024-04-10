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

    public float CalculateIncome()
    {
        float result = 0;
        foreach (Transaction t in Transactions)
        {
            if (t.IsIncome) result += t.Amount;
        }
        return result;
    }

    
    public float CalculateExpenses()
    {
        float result = 0;
        foreach (Transaction t in Transactions)
        {
            if (!t.IsIncome) result += t.Amount;
        }
        return result;
    }
}
