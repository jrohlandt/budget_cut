using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using System.Linq;

namespace BudgetApp;

public static class DB
{
    public static void SaveBudget(Budget data)
    {
        string json = JsonConvert.SerializeObject(data);

        // FileAccess file = FileAccess.Open("user://"+data.Id.ToString().Replace("-", "")+".json", FileAccess.ModeFlags.Write);
        FileAccess file = FileAccess.Open("user://budget.json", FileAccess.ModeFlags.Write);

        file.StoreString(json);
        file.Close();
    }

    public static Budget LoadBudget()
    {
        try 
        {
            FileAccess file = FileAccess.Open("user://budget.json", FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            Budget budget = JsonConvert.DeserializeObject<Budget>(json);
            return budget;
        }
        catch (Exception exception) 
        {
            Logger.Log(exception);
            Budget budget = new Budget()
            {
                Name = "Default Budget",
                Transactions = new List<Transaction>(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            SaveBudget(budget);

            return budget;
        }

    }


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

    public static void SaveCategoryGoals(List<CategoryGoal> goals)
    {
        try 
        {
            FileAccess file = FileAccess.Open("user://goals.json", FileAccess.ModeFlags.Write);
            string json = JsonConvert.SerializeObject(goals);
            file.StoreString(json);
            file.Close();
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }
    public static List<CategoryGoal> LoadCategoryGoals(Guid budgetId, Dictionary<string, TransactionCategory> categories)
    {
        try 
        {
            FileAccess file = FileAccess.Open("user://goals.json", FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            List<CategoryGoal> result = JsonConvert.DeserializeObject<List<CategoryGoal>>(json) 
                ?? throw new Exception();
            return result;
        }
        catch (Exception ex)
        {
            Logger.Log(ex);

            List<CategoryGoal> goals = new();
            foreach (TransactionCategory c in categories.Values.ToArray())
            {
                goals.Add(new CategoryGoal() {
                    BudgetId = budgetId,
                    CategoryId = c.Id,
                    Amount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                });
            }
            SaveCategoryGoals(goals);
            return goals;
        }
    }


}
