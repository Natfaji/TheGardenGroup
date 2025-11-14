using GardenGroup;
using Model.Enums;
using Model.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using UI.ViewModel;

namespace UI.Views
{
	/// <summary>
	/// Interaction logic for CreateEditTicket.xaml
	/// </summary>
	public partial class CreateEditTicket : UserControl
	{
		//private TicketLogic ticketLogic;
		//private ScrollViewer svMainContent;
		//private PartialUser partialUser;
		public CreateEditTicket() //PartialUser user
		{
			//partialUser = user;
			//ticketLogic = new TicketLogic();
			InitializeComponent();
			//FillPriorityDropDown();
		}

		public void FillPriorityDropDown()
		{
			//foreach (Priority Priority in Enum.GetValues(typeof(Priority)))
			//{
			//	priorityDropDown.Items.Add(Priority);
			//}
		}

		private void btnSubmit_Click(object sender, RoutedEventArgs e)
		{
			//if (!IsFormValid())
			//{
			//	return;
			//}

			//Ticket newTicket = CreateTicketFromForm();

			// exception handling
			//try
			//{
			//	ticketLogic.SaveTicket(newTicket);
			//	MessageBox.Show("Ticket successfully created!", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
			//	ClearForm(); // Optionally clear the form
			//}
			//catch (Exception ex)
			//{
			//	MessageBox.Show("An error occurred while saving the _ticket: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			//}
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (DataContext is CreateEditTicketViewModel vm)
			{
				vm.ToggleAssignUserCommand?.Execute(e.AddedItems.Count > 0 ? e.AddedItems[0] : null);
			}
		}
	}
}
