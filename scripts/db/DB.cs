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
            Dictionary<string, TransactionCategory> result = new();
            Logger.Log(exception);
            List<string> categories = new List<string>{
			    "food", "car", "rent", "medical", "freelance", "other",
		    };

            foreach (string c in categories)
            {
                TransactionCategory tt = CreateTransactionCategory(result, c);
                result[tt.Name] = tt;
            }

            // foreach (TransactionCategory c in TransactionCategories.Values.ToArray())
            // {
            //     CategoryGoals.Add(new CategoryGoal() {
            //         BudgetId = currentBudget.Id,
            //         CategoryId = c.Id,
            //         Amount = 0,
            //         CreatedAt = DateTime.Now,
            //         UpdatedAt = DateTime.Now,
            //     });
            // }

                SaveCategories(result);
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


    // public void LoadCategoryGoals()
    // {
    //     FileAccess
    // }


}
