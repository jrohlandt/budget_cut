using Godot;
using System;
using System.Data;
using System.Collections.Generic;

namespace BudgetApp;
public partial class AppManager : Control
{

	#region Exports
	[Export]
	PackedScene transactionRow;

	[Export]
	PackedScene transactionCreateModal;

	[Export]
	PackedScene transactionEditModal;

	#endregion

	VBoxContainer transactionList;

	Budget currentBudget;

	public override void _Ready()
	{
		currentBudget = GeneratePlaceholderData();
		GD.Print("CurrentBudget: " + currentBudget.StartDate);
		transactionList = GetNode<VBoxContainer>("LeftControl/PaddingControl/Content/VBoxContainer/TransactionsList/VBoxContainer");

		for (int i = 0; i < currentBudget.Transactions.Count; i++)
		{
			TransactionRow row = transactionRow.Instantiate<TransactionRow>();
			row.CurrentTransaction = currentBudget.Transactions[i];
			transactionList.AddChild(row);
		}
	}

	public override void _Process(double delta)
	{
	}

	private void _on_create_transaction_button_pressed()
	{
		GD.Print("Pressed");
		// GetNode<Control>("TransactionFormModal").Visible = true;
		TransactionCreateModal modal = transactionCreateModal.Instantiate<TransactionCreateModal>();
		AddChild(modal);
		
	}


	private Budget GeneratePlaceholderData()
	{
		Budget budget = new Budget()
		{
			Name = "Monthly Budget",
			StartDate = new DateTime(2024, 04, 01),
			EndDate = new DateTime(2024, 04, 30),
			Transactions = new List<Transaction>(),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		};



		TransactionCategory foodCategory = new TransactionCategory(){Name = "food", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now};
		TransactionCategory rentCategory = new TransactionCategory(){Name = "rent", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now};
		TransactionCategory carCategory = new TransactionCategory(){Name = "car", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now};
		TransactionCategory medicalCategory = new TransactionCategory(){Name = "medical", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now};


		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Hamburger patties beef 800g",
			Amount = 109.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Pork sausages 800g",
			Amount = 60f,
			Category = rentCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});


		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Rent",
			Amount = 9000,
			Category = rentCategory,
			TransactionDate = new DateTime(2024, 04, 30),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});


		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Car Insurance",
			Amount = 1053,
			Category = carCategory,
			TransactionDate = new DateTime(2024, 04, 30),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});


		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Medical Insurance",
			Amount = 2700,
			Category = medicalCategory,
			TransactionDate = new DateTime(2024, 04, 30),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		
		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 200f,
			Category = carCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = foodCategory,
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		return budget;
	}
}
