using System;

namespace BudgetApp;

public class TransactionCategory
{
    public Guid Id = Guid.NewGuid();
    public int ListId; // id used in Godot OptionButton
    public string Name;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;
}
