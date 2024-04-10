using Godot;
using System;
using System.Data;

namespace BudgetApp;
public partial class Dashboard : Control
{

	[Export]
	PackedScene transactionRow;
	VBoxContainer transactionList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		transactionList = GetNode<VBoxContainer>("LeftControl/PaddingControl/Content/VBoxContainer/TransactionsList/VBoxContainer");

		for (int i = 0; i < 5; i++)
		{
			GD.Print("hello");
			TransactionRow row = transactionRow.Instantiate<TransactionRow>();
			transactionList.AddChild(row);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
