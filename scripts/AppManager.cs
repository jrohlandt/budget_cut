using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApp;
public partial class AppManager : Control
{

	#region Exports

	[Export]
	PackedScene createBudgetScreen;

	[Export]
	PackedScene allBudgetsScreen;

	[Export]
	PackedScene transactionRow;

	[Export]
	PackedScene transactionCreateModal;

	[Export]
	PackedScene transactionEditModal;

	[Export]
	PackedScene deleteTransactionModal;

	[Export]
	PackedScene categoryRow;

	#endregion

	VBoxContainer transactionList;
	private static Budget currentBudget;
	public static Dictionary<string, TransactionCategory> TransactionCategories = new Dictionary<string, TransactionCategory>();
	public static List<CategoryGoal> CategoryGoals = new List<CategoryGoal>();

	public override void _Ready()
    {
        transactionList = GetNode<VBoxContainer>("LeftControl/PaddingControl/Content/VBoxContainer/TransactionsList/VBoxContainer");
		GetNode<Button>("CreateNewBudgetButton").Pressed += ShowCreateBudgetScreen;
		GetNode<Button>("ShowAllBudgetsButton").Pressed += ShowAllBudgetsList;

		List<string> budgetList = DB.LoadBudgetList();
		if (budgetList == null) {
			ShowCreateBudgetScreen(false);
			return;
		}
		else
		{
			// load most recently added budget
			string budgetId = budgetList[budgetList.Count - 1];
			currentBudget = DB.LoadBudget(budgetId);
			TransactionCategories = DB.LoadCategories();
			CategoryGoals = DB.LoadPlanned(currentBudget.Id);
			RefreshUI();
		}
    }

    public override void _Process(double delta) {}

	#region Refresh UI Methods

	private void ShowCreateBudgetScreen()
	{
		ShowCreateBudgetScreen(true);
	}
	
	private void ShowCreateBudgetScreen(bool showCancelButton = true)
	{
		CreateBudgetScreen bScreen = createBudgetScreen.Instantiate<CreateBudgetScreen>();
		bScreen.CreateBudget += OnCreateBudget;
		bScreen.ShowCancelButton = showCancelButton;
		AddChild(bScreen);
	}

	private void ShowAllBudgetsList()
    {
        AllBudgetsScreen bscreen = allBudgetsScreen.Instantiate<AllBudgetsScreen>();
		bscreen.LoadBudget += LoadBudget;
		AddChild(bscreen);
		
    }

    private void LoadBudget(string budgetId)
    {
        currentBudget = DB.LoadBudget(budgetId);
		TransactionCategories = DB.LoadCategories();
		CategoryGoals = DB.LoadPlanned(currentBudget.Id);
		RefreshUI();
    }

    private void RefreshUI()
	{
		RefreshTotals();
		RefreshTransactionList();
		RefreshCategoriesList();
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

	private void RefreshCategoriesList()
	{
		VBoxContainer rows = GetNode<VBoxContainer>("RightControl/PaddingControl/BudgetBreakdown/Content/CategoriesList/Control/ScrollContainer/Rows");
		foreach (Node r in rows.GetChildren())
		{
			r.QueueFree();
		}
		foreach (TransactionCategory c in TransactionCategories.Values.ToArray())
		{
			float actual = 0;
			float planned = 0;
			foreach (CategoryGoal g in CategoryGoals)
			{
				if (g.CategoryId == c.Id)
					planned = g.Amount;
			}

			foreach (Transaction t in currentBudget.Transactions)
			{
				if (c.Id == t.Category.Id)
					actual += t.Amount;
			}
			CategoryRow row = categoryRow.Instantiate<CategoryRow>();
			row.Actual = actual;
			row.CurrentCategory = c;
			row.Planned = planned;
			row.SaveCategoryGoal += SaveCategoryGoal;
			rows.AddChild(row);
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

	private void OnCreateBudget(string name, string startDate, string endDate)
	{
		DB.CreateBudget(name, startDate, endDate);

		List<string> budgetList = DB.LoadBudgetList();
		string budgetId = budgetList[budgetList.Count - 1];
		currentBudget = DB.LoadBudget(budgetId);

		TransactionCategories = DB.LoadCategories();
		
		DB.CreatePlanned(currentBudget.Id, TransactionCategories);
		CategoryGoals = DB.LoadPlanned(currentBudget.Id);
		RefreshUI();
	}

	private void OnDeleteTransaction(string id)
    {
		currentBudget.DeleteTransaction(id);
		RefreshUI();
		DB.SaveBudget(currentBudget);
    }

	private void OnCreateTransaction(string id, string name, float amount, string date, string category, bool isIncome)
    {
		TransactionCategory selectedCategory = TransactionCategories[category];

		if (id != "")
			currentBudget.UpdateTransaction(id, name, amount, date, selectedCategory, isIncome);
		else
			currentBudget.CreateTransaction(name, amount, date, selectedCategory, isIncome);

		RefreshUI();
		DB.SaveBudget(currentBudget);
    }

	  private void SaveCategoryGoal(string categoryId, float planned)
    {
        foreach (CategoryGoal g in CategoryGoals)
		{
			if (g.CategoryId.ToString() == categoryId)
				g.Amount = planned;
				g.UpdatedAt = DateTime.Now;
		}
		RefreshCategoriesList();
		DB.SavePlanned(currentBudget.Id, CategoryGoals);
    }

	#endregion

}
