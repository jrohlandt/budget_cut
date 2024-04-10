using Godot;
using System;

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

	// Called when the node enters the scene tree for the first time.

	private VBoxContainer Form;

	public override void _Ready()
	{
		Form = GetNode<VBoxContainer>("TransactionFormModal/Panel/Content");
		Button cancelButton = GetNode<Button>("TransactionFormModal/Panel/Content/Buttons/Cancel/Button");
		cancelButton.Pressed += OnCancelButtonPressed;

		Button saveButton = GetNode<Button>("TransactionFormModal/Panel/Content/Buttons/Save/Button");
		saveButton.Pressed += OnSaveButtonPressed;

		// OptionButton categoriesDropdown = Form.GetNode<OptionButton>("TransactionFormModal/Panel/Content/Category/OptionButton");
		// categoriesDropdown.AddItem("food");
		// categoriesDropdown.AddItem("other");



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
		string category = "car";
		bool isIncome = Form.GetNode<CheckButton>("Income/CheckButton").ButtonPressed;

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
