using Godot;
using System;

namespace BudgetApp;

public partial class TransactionCreateModal : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Create modal ready");
		Button btn = GetNode<Button>("TransactionFormModal/Panel/Content/Buttons/Cancel/Button");
		GD.Print("Button text: " + btn.Text);
		btn.Pressed += Cancel;
	}

	private void Cancel()
	{
		GD.Print("Cancel Pressed");
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
