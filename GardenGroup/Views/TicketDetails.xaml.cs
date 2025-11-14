using System.Windows;
using System.Windows.Controls;

namespace UI.Views
{
	public partial class TicketDetails : UserControl
	{
		public TicketDetails()
		{
			InitializeComponent();
		}

		private void AddCommentButton_Click(object sender, RoutedEventArgs e)
		{
			//if (string.IsNullOrWhiteSpace(NewCommentTextBox.Text) && selectedEmployee == null)
			//{
			//	MessageBox.Show("Please enter a comment.");
			//	return;
			//}
			//else if (string.IsNullOrWhiteSpace(NewCommentTextBox.Text) && selectedEmployee != null)
			//{
			//	UpdateAssignTo();
			//}
			//else
			//{
			//	// Create new comment
			//	var newComment = new Comment
			//	{
			//		Content = NewCommentTextBox.Text,
			//		Author = loggedInUser,
			//		Created_at = DateTime.UtcNow
			//	};

			//	// Voeg de opmerking toe aan het _ticket
			//	commentLogic.AddComment(ticketId, newComment);
			//	LinkedComments.Add(newComment);
			//	NewCommentTextBox.Clear();
			//	if (selectedEmployee != null)
			//	{
			//		UpdateAssignTo();
			//	}
			//}
		}

		//private async void EnableAssignToPerson(PartialUser loggedInUser)
		//{
		//	if (loggedInUser.Role == Role.ServiceDesk)
		//	{
		//		RequestParameters requestParameters = new RequestParameters();
		//		employees = await userLogic.GetAllUsers(requestParameters);
		//		labelPersonInCharge.Visibility = Visibility.Visible;
		//		ComboBoxEmployee.Visibility = Visibility.Visible;
		//		ComboBoxEmployee.ItemsSource = employees;
		//		ComboBoxEmployee.DisplayMemberPath = "Name";
		//		ComboBoxEmployee.SelectedValuePath = "_id";

		//	}
		//}
	}
}