using BudgetApp;
using Godot;
using System;

public partial class AllBudgetsListItem : HBoxContainer
{

	[Signal]
	public delegate void LoadBudgetEventHandler(string budgetId);

	public Budget CurrentBudget;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string startDate = CurrentBudget.StartDate.ToString("yyyy/MM/dd");
		string endtDate = CurrentBudget.EndDate.ToString("yyyy/MM/dd");

		GetNode<Label>("Name").Text = CurrentBudget.Name;
		GetNode<Label>("Duration").Text = startDate + " - " + endtDate;
		GetNode<Button>("OpenButton/Button").Pressed += Load;
	}

    private void Load()
    {
       EmitSignal(SignalName.LoadBudget, CurrentBudget.Id.ToString());
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
