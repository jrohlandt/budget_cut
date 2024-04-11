using BudgetApp;
using Godot;
using System;

public partial class CategoryRow : Button
{
	[Signal]
	public delegate void SaveCategoryGoalEventHandler(string categoryId, float planned);

	public TransactionCategory CurrentCategory;
	public float Actual = 0;
	public float Planned = 0;

	public override void _Ready()
	{
		RefreshUI();
		LineEdit le = GetNode<LineEdit>("Panel/Items/Planned");
		le.Text = Planned.ToString();
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
			// No need to refresh UI here.
			// row gets queue freed and recreated when CategoryGoal gets saved in AppManager.
			EmitSignal(
				SignalName.SaveCategoryGoal, 
				CurrentCategory.Id.ToString(),
				Planned
			);
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
