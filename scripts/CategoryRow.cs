using BudgetApp;
using Godot;
using System;

public partial class CategoryRow : Button
{
	public TransactionCategory CurrentCategory;
	public float Actual = 0;
	public float Planned = 0;

	public override void _Ready()
	{
		RefreshUI();
		LineEdit le = GetNode<LineEdit>("Panel/Items/Planned");
		// le.TextChanged += PlannedTextChanged;
		le.TextSubmitted += PlannedTextChanged;
	}

    private void RefreshUI()
    {
        GetNode<Label>("Panel/Items/Name").Text = CurrentCategory.Name;
		GetNode<Label>("Panel/Items/Actual").Text = Actual.ToString();
		GetNode<Label>("Panel/Items/Difference").Text = (Planned - Actual).ToString();
    }


    private void PlannedTextChanged(string newText)
    {
		try {
			Planned = newText.ToFloat();
			RefreshUI();
		}
		catch(Exception exception)
		{
			GD.Print(exception.GetType().FullName);
		}
    }

    public override void _Process(double delta)
	{
	}
}
