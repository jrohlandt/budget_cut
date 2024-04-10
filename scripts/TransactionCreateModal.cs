using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp;

public partial class TransactionCreateModal : Control
{
	[Signal]
	public delegate void CreateTransactionEventHandler(
		string id,
		string name, 
		float amount, 
		string date, 
		string category, 
		bool isIncome
	);

	private bool editMode = false;
	public Transaction CurrentTransaction = null;

	public Dictionary<string, TransactionCategory> Categories;

	private VBoxContainer Form;

	public override void _Ready()
    {
		if (CurrentTransaction != null)
		{
			editMode = true;
		}

        Form = GetNode<VBoxContainer>("TransactionFormModal/Panel/Content");
        Button cancelButton = GetNode<Button>("TransactionFormModal/Panel/Content/Buttons/Cancel/Button");
        cancelButton.Pressed += OnCancelButtonPressed;

        Button saveButton = GetNode<Button>("TransactionFormModal/Panel/Content/Buttons/Save/Button");
        saveButton.Pressed += OnSaveButtonPressed;

        PopulateCategoryDropdown();
		
		RichTextLabel heading = GetNode<RichTextLabel>("TransactionFormModal/Panel/Content/RichTextLabel");
		heading.Text = "Add Transaction";

		if (editMode)
		{
			heading.Text = "Edit Transaction";
			PopulateFormFields();
		}
    }

    private void PopulateFormFields()
    {
		Form.GetNode<LineEdit>("Name/LineEdit").Text = CurrentTransaction.Name;
		Form.GetNode<LineEdit>("Amount/LineEdit").Text = CurrentTransaction.Amount.ToString();
		Form.GetNode<LineEdit>("Date/LineEdit").Text = CurrentTransaction.TransactionDate.ToString("yyyy-MM-dd");
		Form.GetNode<CheckButton>("Income/CheckButton").SetPressedNoSignal(CurrentTransaction.IsIncome);
    }


    private void PopulateCategoryDropdown()
    {
        OptionButton categoriesDropdown = Form.GetNode<OptionButton>("Category/OptionButton");

        foreach (TransactionCategory c in Categories.Values.ToArray())
        {
            categoriesDropdown.AddItem(c.Name, c.ListId);
			// default to "other"
            if (c.Name == "other")
                categoriesDropdown.Selected = c.ListId;
        }

		if (editMode)
			categoriesDropdown.Selected = CurrentTransaction.Category.ListId;
    }

    private void OnCancelButtonPressed()
	{
		QueueFree();
	}

	private void OnSaveButtonPressed()
	{
		string id = editMode ? 	CurrentTransaction.Id.ToString() : "";
		
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
			id,
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
