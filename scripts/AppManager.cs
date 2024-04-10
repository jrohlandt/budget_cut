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

	private static Budget currentBudget;

	public override void _Ready()
    {
        currentBudget = GeneratePlaceholderData();
        GD.Print("CurrentBudget: " + currentBudget.StartDate);
        transactionList = GetNode<VBoxContainer>("LeftControl/PaddingControl/Content/VBoxContainer/TransactionsList/VBoxContainer");

		RefreshTotals();
        RefreshTransactionList();
    }

	private void RefreshTotals()
	{
		HBoxContainer header = GetNode<HBoxContainer>("LeftControl/PaddingControl/TotalsHeader");

		float income = currentBudget.CalculateIncome();
		float expenses = currentBudget.CalculateExpenses();
		header.GetNode<RichTextLabel>("Income/Value").Text = "[center][b]" + income.ToString();
		header.GetNode<RichTextLabel>("Expenses/Value").Text = "[center][b]" + expenses.ToString();
		TextureProgressBar piechart = header.GetNode<TextureProgressBar>("PieChart/TextureProgressBar");
		piechart.MaxValue = income;
		piechart.Value = expenses;
	}

    private void RefreshTransactionList()
    {
		foreach (Node c in transactionList.GetChildren())
		{
			c.QueueFree();
		}

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
		TransactionCreateModal modal = transactionCreateModal.Instantiate<TransactionCreateModal>();
		modal.CreateTransaction += OnCreateTransaction;
		AddChild(modal);
		
	}

    private void OnCreateTransaction(string name, float amount, string date, string category, bool isIncome)
    {
        Transaction t = new Transaction()
		{
			BudgetId = currentBudget.Id,
			Name = name,
			Amount = amount,
			TransactionDate = DateTime.Parse(date),
			Category = new TransactionCategory() {Name = category, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now},
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
			IsIncome = isIncome,
		};

		currentBudget.Transactions.Add(t);
		RefreshTotals();
		RefreshTransactionList();
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
		TransactionCategory freelanceCategory = new TransactionCategory(){Name = "freelance", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now};


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
			Name = "Super Groomers Puppy Parlor PTY LTD",
			Amount = 41013f,
			Category = freelanceCategory,
			TransactionDate = new DateTime(2024, 05, 01),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
			IsIncome = true,
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
