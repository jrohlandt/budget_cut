using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp;

public partial class TransactionCreateModal : Control
{
	[Signal]
	public delegate void CreateTransactionEventHandler(
		string name, 
		float amount, 
		string date, 
		string category, 
		bool isIncome
	);

	public Dictionary<string, TransactionCategory> Categories;

	private VBoxContainer Form;

	public override void _Ready()
    {
        Form = GetNode<VBoxContainer>("TransactionFormModal/Panel/Content");
        Button cancelButton = GetNode<Button>("TransactionFormModal/Panel/Content/Buttons/Cancel/Button");
        cancelButton.Pressed += OnCancelButtonPressed;

        Button saveButton = GetNode<Button>("TransactionFormModal/Panel/Content/Buttons/Save/Button");
        saveButton.Pressed += OnSaveButtonPressed;

        PopulateCategoryDropdown();
    }

    private void PopulateCategoryDropdown()
    {
        OptionButton categoriesDropdown = Form.GetNode<OptionButton>("Category/OptionButton");

        foreach (TransactionCategory c in Categories.Values.ToArray())
        {
            categoriesDropdown.AddItem(c.Name, c.ListId);
            // Default to "other"
            if (c.Name == "other")
                categoriesDropdown.Selected = c.ListId;
        }
    }

    private void OnCancelButtonPressed()
	{
		QueueFree();
	}

	private void OnSaveButtonPressed()
	{
		string name = GetLineEditText("Name");
		float amount = GetLineEditText("Amount").ToFloat();
		string date = "2099-12-31"; //GetLineEditText("Date");
		int categoryId = Form.GetNode<OptionButton>("Category/OptionButton").GetSelectedId();
		bool isIncome = Form.GetNode<CheckButton>("Income/CheckButton").ButtonPressed;

		string category = "other";
		foreach (var c in Categories.Values.ToArray())
		{
			if (c.ListId == categoryId) category = c.Name;
		}

		EmitSignal(
			SignalName.CreateTransaction, 
			name,
			amount,
			date,
			category,
			isIncome
		);

		QueueFree();
	}

	private string GetLineEditText(string fieldName)
	{
		return Form.GetNode<LineEdit>(fieldName + "/LineEdit").Text;
	}
}
