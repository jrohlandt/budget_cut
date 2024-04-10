using Godot;
using System;

namespace BudgetApp;

public partial class DeleteTransactionModal : Control
{

	[Signal]
	public delegate void DeleteTransactionEventHandler(string id);

	public Transaction CurrentTransaction;

	public override void _Ready()
	{
		GetNode<Label>("Panel/VBoxContainer/Name/Label").Text = CurrentTransaction.Name;
		GetNode<Button>("Panel/VBoxContainer/Buttons/CancelButton").Pressed += Cancel;
		GetNode<Button>("Panel/VBoxContainer/Buttons/DeleteButton").Pressed += Delete;

	}

    private void Delete()
    {
		EmitSignal(SignalName.DeleteTransaction, CurrentTransaction.Id.ToString());
		QueueFree();
    }


    private void Cancel()
    {
        QueueFree();
    }

    public override void _Process(double delta)
	{
	}
}
