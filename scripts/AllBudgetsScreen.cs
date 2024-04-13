using Godot;
using System;
using System.Collections.Generic;

namespace BudgetApp;

public partial class AllBudgetsScreen : Control
{
	[Signal]
	public delegate void LoadBudgetEventHandler(string budgetId);
	
	[Export]
	PackedScene allBudgetsListItem; // Wtf not showing in inspector of all_budgets_screen.tscn

	
	VBoxContainer UI;
	public List<Budget> budgets;

	public override void _Ready()
	{
		UI = GetNode<VBoxContainer>("UI");

		UI.GetNode<Button>("Cancel/Button").Pressed += CancelButtonPressed;

		VBoxContainer list = UI.GetNode<VBoxContainer>("ScrollContainer/List");

		budgets = DB.LoadAllBudgets();

		foreach (Budget b in budgets)
		{
			AllBudgetsListItem item = allBudgetsListItem.Instantiate<AllBudgetsListItem>();
			item.CurrentBudget = b;
			item.LoadBudget += Load;
			list.AddChild(item);
		}
		
	}


    private void Load(string budgetId)
    {
       EmitSignal(SignalName.LoadBudget, budgetId);
	   QueueFree();
    }


    private void CancelButtonPressed()
    {
        QueueFree();
    }
}
