using Godot;
using System;

namespace BudgetApp;

public partial class TransactionRow : HBoxContainer
{
	[Signal]
	public delegate void DeleteTransactionPressedEventHandler(string id);

	public Transaction CurrentTransaction;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (CurrentTransaction == null) {
			GD.Print("CurrentTransaction is null");	
			return;
		}

		GetNode<RichTextLabel>("Date").Text = CurrentTransaction.TransactionDate.ToString("yyyy/MM/dd");
		GetNode<RichTextLabel>("Name").Text = CurrentTransaction.Name;
		GetNode<RichTextLabel>("Amount").Text = CurrentTransaction.Amount.ToString();
		// GetNode<RichTextLabel>("Amount").Modulate = Color(0, 63, 63, 1);
		GetNode<RichTextLabel>("Category").Text = CurrentTransaction.Category.Name;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_delete_button_pressed()
	{
		EmitSignal(SignalName.DeleteTransactionPressed, CurrentTransaction.Id.ToString());
	}
}
