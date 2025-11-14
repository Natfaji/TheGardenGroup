using GardenGroup;
using Logic;
using Model.Enums;
using Model.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace UI.Views
{
	/// <summary>
	/// Interaction logic for CreateEmployee.xaml
	/// </summary>
	public partial class CreateEmployee : UserControl
	{
		private UserLogic userLogic = new UserLogic();

		public PartialUser loggedinUser = App.SessionService.CurrentUser;

		public CreateEmployee()
		{
			InitializeComponent();
			userLogic = new UserLogic();
			CboxTypeOfUser.ItemsSource = Enum.GetValues(typeof(Role));
		}

		private void btnCreateUser_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(txtBoxName.Text) ||
				string.IsNullOrEmpty(txtBoxEmail.Text) ||
				string.IsNullOrEmpty(txtBoxPhoneNumber.Text) ||
				string.IsNullOrEmpty(pwdBoxPassword.Password) ||
				CboxTypeOfUser.SelectedItem == null)
			{
				MessageBox.Show("Please fill out all fields.");
				return;
			}

			string name = txtBoxName.Text;
			string email = txtBoxEmail.Text;
			string phoneNumber = txtBoxPhoneNumber.Text;
			string password = pwdBoxPassword.Password;
			Role selectedRole = (Role)CboxTypeOfUser.SelectedItem;

			userLogic.CreateUser(name, email, phoneNumber, password, selectedRole);

			MessageBox.Show("User created successfully!");

			//NavigationRequested?.Invoke(new UserManagement());
		}

		private void CboxTypeOfUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (CboxTypeOfUser.SelectedItem != null)
			{
				Role selectedRole = (Role)CboxTypeOfUser.SelectedItem;
			}
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			var parentWindow = Window.GetWindow(this);

			if (parentWindow != null)
			{
				parentWindow.Content = new UserManagement();
			}
		}
	}
}
