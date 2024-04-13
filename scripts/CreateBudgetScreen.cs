using Godot;
using System;

namespace BudgetApp;

public partial class CreateBudgetScreen : Control
{
	[Signal]
	public delegate void CreateBudgetEventHandler(string name, string startDate, string endDate);

	VBoxContainer InitialUI;
	Control FormContainer;
	public bool ShowCancelButton = true;
	Button CancelButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		InitialUI = GetNode<VBoxContainer>("InitialUI");
		FormContainer = GetNode<Control>("Control");
		InitialUI.Visible = true;
		FormContainer.Visible = false;

		GetNode<Button>("InitialUI/HBoxContainer/LoadFormButton").Pressed += ShowForm;

		CancelButton = InitialUI.GetNode<Button>("Cancel/Button");
		if (ShowCancelButton)
		{
			CancelButton.Visible = true;
			CancelButton.Pressed += CancelButtonPressed;
		}
		else 
		{
			CancelButton.Visible = false;
			
		}
	}

    private void CancelButtonPressed()
    {
        QueueFree();
    }


    private void ShowForm()
    {
		InitialUI.Visible = false;
        FormContainer.Visible = true;
		FormContainer.GetNode<Button>("PaddingControl/Form/Buttons/CancelButton").Pressed += FormCancelButtonPressed;
		FormContainer.GetNode<Button>("PaddingControl/Form/Buttons/CreateButton").Pressed += FormCreateButtonPressed;
    }

    private void FormCreateButtonPressed()
    {
        string name = FormContainer.GetNode<LineEdit>("PaddingControl/Form/Name/LineEdit").Text;
        string startDate = FormContainer.GetNode<LineEdit>("PaddingControl/Form/StartDate/LineEdit").Text;
        string endDate = FormContainer.GetNode<LineEdit>("PaddingControl/Form/EndDate/LineEdit").Text;

		EmitSignal(SignalName.CreateBudget, name, startDate, endDate);
		QueueFree();
    }


    private void FormCancelButtonPressed()
    {
       	InitialUI.Visible = true;
		FormContainer.Visible = false;
    }
}
