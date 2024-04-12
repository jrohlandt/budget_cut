using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using System.Linq;

namespace BudgetApp;

public static class DB
{
    #region Budget
    public static void SaveBudget(Budget budget)
    {
        try 
        {
            string fileName = budget.Id.ToString().Replace("-", "") + "_budget.json";

            string json = JsonConvert.SerializeObject(budget);

            FileAccess file = FileAccess.Open("user://"+fileName, FileAccess.ModeFlags.Write);

            file.StoreString(json);
            file.Close();
        }
        catch (Exception e) 
        {
            Logger.Log(e);
        }
    }

    public static Budget CreateBudget(string name, string startDate, string endDate)
    {
        try 
        {
            Budget budget = new Budget()
            {
                Name = name,
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(endDate),
                Transactions = new List<Transaction>(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            string fileName = budget.Id.ToString().Replace("-", "") + "_budget.json";

            FileAccess file = FileAccess.Open("user://"+fileName, FileAccess.ModeFlags.Write);
            string json = JsonConvert.SerializeObject(budget);
            file.StoreString(json);
            file.Close();

            AddBudgetToList(budget);
            return budget;
        }
        catch (Exception exception) 
        {
            Logger.Log(exception);
            return null;
        }

    }

    public static Budget LoadBudget(string budgetId)
    {
        try 
        {
            string fileName = budgetId.Replace("-", "") + "_budget.json";

            FileAccess file = FileAccess.Open("user://"+fileName, FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            file.Close();
            Budget budget = JsonConvert.DeserializeObject<Budget>(json);
            return budget;
        }
        catch (Exception exception) 
        {
            Logger.Log(exception);
            return null;
        }
    }

    public static Budget LoadBudget(Guid budgetId)
    {
        return LoadBudget(budgetId.ToString());
    }
    
    public static void CreateEmptyBudgetList()
    {
        try 
        {
            if (FileAccess.FileExists("user://budgets.json"))
            {
                return;
            }

            FileAccess file = FileAccess.Open("user://budgets.json", FileAccess.ModeFlags.Write);
            List<string> budgetList = new() {"empty"};
            file.StoreString(JsonConvert.SerializeObject(budgetList));
            file.Close();
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }

    }

    public static void AddBudgetToList(Budget budget)
    {
        try 
        {
            if (!FileAccess.FileExists("user://budgets.json"))
            {
                CreateEmptyBudgetList();
            }

            FileAccess file = FileAccess.Open("user://budgets.json", FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            List<string> budgetList = JsonConvert.DeserializeObject<List<string>>(json);
            if (budgetList[0] == "empty")
                budgetList[0] = budget.Id.ToString();
            else
                budgetList.Add(budget.Id.ToString());

            FileAccess file2 = FileAccess.Open("user://budgets.json", FileAccess.ModeFlags.Write);
            file2.StoreString(JsonConvert.SerializeObject(budgetList));
            file2.Close();
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }

    }

     public static List<string> LoadBudgetList()
    {
        try 
        {
            FileAccess file = FileAccess.Open("user://budgets.json", FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            file.Close();
            List<string> budgetList = JsonConvert.DeserializeObject<List<string>>(json);
            return budgetList;
        }
        catch (Exception exception) 
        {
            Logger.Log(exception);
            return null;
        }
    }
    #endregion

    #region Planned

    public static void CreatePlanned(Guid budgetId, Dictionary<string, TransactionCategory> categories)
    {
        List<CategoryGoal> planned = new();
        foreach (TransactionCategory c in categories.Values.ToArray())
        {
            planned.Add(new CategoryGoal() {
                BudgetId = budgetId,
                CategoryId = c.Id,
                Amount = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            });
        }
        SavePlanned(budgetId, planned);
    }

    public static void SavePlanned(Guid budgetId, List<CategoryGoal> goals)
    {
        try 
        {
            string fileName = budgetId.ToString().Replace("-", "") + "_planned.json";

            FileAccess file = FileAccess.Open("user://"+fileName, FileAccess.ModeFlags.Write);
            string json = JsonConvert.SerializeObject(goals);
            file.StoreString(json);
            file.Close();
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }
    public static List<CategoryGoal> LoadPlanned(Guid budgetId)
    {
        try 
        {
            string fileName = budgetId.ToString().Replace("-", "") + "_planned.json";

            FileAccess file = FileAccess.Open("user://"+fileName, FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            List<CategoryGoal> result = JsonConvert.DeserializeObject<List<CategoryGoal>>(json) 
                ?? throw new Exception();
            return result;
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            return null;
        }
    }
    #endregion

    #region Categories
    public static void SaveCategories(Dictionary<string, TransactionCategory> data)
    {
        string json = JsonConvert.SerializeObject(data);

        FileAccess file = FileAccess.Open("user://categories.json", FileAccess.ModeFlags.Write);

        file.StoreString(json);
        file.Close();
    }

    public static Dictionary<string, TransactionCategory> LoadCategories()
    {
        try 
        {
            FileAccess file = FileAccess.Open("user://categories.json", FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            Dictionary<string, TransactionCategory> result = JsonConvert.DeserializeObject<Dictionary<string, TransactionCategory>>(json);
            return result;
        }
        catch (Exception exception) 
        {
            Dictionary<string, TransactionCategory> categories = new();
            Logger.Log(exception);
            List<string> categoryNames = new List<string>{
			    "food", "car", "rent", "medical", "freelance", "other",
		    };

            foreach (string c in categoryNames)
            {
                TransactionCategory tt = CreateTransactionCategory(categories, c);
                categories[tt.Name] = tt;
            }

            SaveCategories(categories);
            return LoadCategories();
            }

    }

    private static TransactionCategory CreateTransactionCategory(Dictionary<string, TransactionCategory> categories, string name)
	{
		int listId;
		if (categories.Count == 0)
		{
			listId = 0;
		}
		else 
		{
			int highestId = 0;
			foreach (var c in categories.Values.ToArray())
			{
				if (c.ListId > highestId) highestId = c.ListId;
			}
			listId = highestId + 1;
		}

		return new TransactionCategory(){ListId = listId, Name = name, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now};
	}
    #endregion

}
