using System;

namespace BudgetApp;

public class CategoryGoal
{
    public Guid Id = Guid.NewGuid();
    public Guid BudgetId;
    public Guid CategoryId;
    public float Amount;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;
}
