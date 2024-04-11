using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

	[Export]
	PackedScene deleteTransactionModal;
	#endregion

	VBoxContainer transactionList;

	private static Budget currentBudget;
	public static Dictionary<string, TransactionCategory> TransactionCategories = new Dictionary<string, TransactionCategory>();

	public override void _Ready()
    {
        currentBudget = GeneratePlaceholderData();
        transactionList = GetNode<VBoxContainer>("LeftControl/PaddingControl/Content/VBoxContainer/TransactionsList/VBoxContainer");

		RefreshUI();
    }

	public override void _Process(double delta) {}

	#region Refresh UI Methods
	private void RefreshUI()
	{
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
			row.DeleteTransactionPressed += OnDeleteTransactionButtonPressed; 
			row.ShowTransactionModal += ShowTransactionModal;
            transactionList.AddChild(row);
        }
    }
	#endregion

	#region Events (Click)
	private void _on_create_transaction_button_pressed()
	{
		TransactionCreateModal modal = transactionCreateModal.Instantiate<TransactionCreateModal>();
		modal.Categories = TransactionCategories;
		modal.CreateTransaction += OnCreateTransaction;
		AddChild(modal);
	}

    private void OnDeleteTransactionButtonPressed(string id)
    {
		Transaction tr = currentBudget.FindTransaction(id);
		if (tr == null) return;

		DeleteTransactionModal modal = deleteTransactionModal.Instantiate<DeleteTransactionModal>();
		modal.CurrentTransaction = tr;
		modal.DeleteTransaction += OnDeleteTransaction;
		AddChild(modal); 
    }

	private void ShowTransactionModal(string id)
    {
		TransactionCreateModal modal = transactionCreateModal.Instantiate<TransactionCreateModal>();
		modal.Categories = TransactionCategories;
		modal.CreateTransaction += OnCreateTransaction;
		Transaction t = currentBudget.FindTransaction(id);
		if (t != null) modal.CurrentTransaction = t;
		AddChild(modal);
    }
	#endregion

	#region Events (CRUD)
	private void OnDeleteTransaction(string id)
    {
		currentBudget.DeleteTransaction(id);
		RefreshUI();
    }

	private void OnCreateTransaction(string id, string name, float amount, string date, string category, bool isIncome)
    {
		TransactionCategory selectedCategory = TransactionCategories[category];

		if (id != "")
			currentBudget.UpdateTransaction(id, name, amount, date, selectedCategory, isIncome);
		else
			currentBudget.CreateTransaction(name, amount, date, selectedCategory, isIncome);

		RefreshUI();
    }
	#endregion

    

	private void CreateTransactionCategory(string name)
	{
		int listId;
		if (TransactionCategories.Count == 0)
		{
			listId = 0;
		}
		else 
		{
			int highestId = 0;
			foreach (var c in TransactionCategories.Values.ToArray())
			{
				if (c.ListId > highestId) highestId = c.ListId;
			}
			listId = highestId + 1;
		}

		TransactionCategories[name] = new TransactionCategory(){ListId = listId, Name = name, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now};
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


		List<string> categories = new List<string>{
			"food", "car", "rent", "medical", "freelance", "other",
		};

		foreach (string c in categories)
		{
			CreateTransactionCategory(c);
		}

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Hamburger patties beef 800g",
			Amount = 109.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Super Groomers Puppy Parlor PTY LTD",
			Amount = 41013f,
			Category = TransactionCategories["freelance"],
			TransactionDate = new DateTime(2024, 05, 01),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
			IsIncome = true,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Pork sausages 800g",
			Amount = 60f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});


		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Rent",
			Amount = 9000,
			Category = TransactionCategories["rent"],
			TransactionDate = new DateTime(2024, 04, 30),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});


		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Car Insurance",
			Amount = 1053,
			Category = TransactionCategories["car"],
			TransactionDate = new DateTime(2024, 04, 30),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});


		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Medical Insurance",
			Amount = 2700,
			Category = TransactionCategories["medical"],
			TransactionDate = new DateTime(2024, 04, 30),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		
		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 200f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		budget.Transactions.Add(new Transaction(){
			BudgetId = budget.Id,
			Name = "Milk",
			Amount = 20.99f,
			Category = TransactionCategories["food"],
			TransactionDate = new DateTime(2024, 04, 03),
			CreatedAt = DateTime.Now,
			UpdatedAt = DateTime.Now,
		});

		return budget;
	}
}
