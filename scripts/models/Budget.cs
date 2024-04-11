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

    
	public Transaction FindTransaction(string id)
	{
        Transaction result = null;
        if (id == "") return result;
		foreach (Transaction t in Transactions)
        {
            if (t.Id.ToString() == id)
                return t;
        }
        return result;
	}

    public Transaction CreateTransaction(string name, float amount, string date, TransactionCategory category, bool isIncome)
    {
        Transaction t = new Transaction()
		{
			BudgetId = Id,
			Name = name,
			Amount = amount,
			TransactionDate = DateTime.Parse(date),
			Category = category,
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
			IsIncome = isIncome,
		};

		Transactions.Add(t);
        return t;
    }

    public Transaction UpdateTransaction(string id, string name, float amount, string date, TransactionCategory category, bool isIncome)
    {
        Transaction tr = FindTransaction(id);
        tr.Name = name;
        tr.Amount = amount;
        tr.TransactionDate = DateTime.Parse(date);
        tr.Category = category;
        tr.UpdatedAt = DateTime.Now;
        tr.IsIncome = isIncome;

        return tr;
    }

    public List<Transaction> DeleteTransaction(string id)
    {
        List<Transaction> list = new List<Transaction>();

		foreach (Transaction t in Transactions)
		{
			if (t.Id.ToString() != id) 
				list.Add(t);
		}

		Transactions = list;

        return Transactions;
    }
}
