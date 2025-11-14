using Logic;
using Model.Enums;
using Model.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace UI.Views
{
	/// <summary>
	/// Interaction logic for EditUser.xaml
	/// </summary>
	public partial class EditUser : UserControl
	{
		private readonly UserLogic userLogic;
		private readonly PartialUser selectedUser;

		public EditUser(PartialUser user)
		{
			InitializeComponent();
			userLogic = new UserLogic();
			selectedUser = user;

			LoadUserData();
		}

		private void LoadUserData()
		{
			txtBoxEditName.Text = selectedUser.Name;
			txtBoxEditEmail.Text = selectedUser.Email;
			txtBoxEditPhonenumber.Text = selectedUser.Phone_number;
			cmBoxEditRole.ItemsSource = Enum.GetValues(typeof(Role));
			cmBoxEditRole.SelectedItem = selectedUser.Role;
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			PartialUser newPartialUser = new PartialUser();
			newPartialUser._id = selectedUser._id;
			newPartialUser.Name = txtBoxEditName.Text;
			newPartialUser.Email = txtBoxEditEmail.Text;
			newPartialUser.Phone_number = txtBoxEditPhonenumber.Text;
			newPartialUser.Role = (Role)cmBoxEditRole.SelectedItem;

			userLogic.Update(selectedUser._id, newPartialUser);

			MessageBox.Show("User updated successfully!");

			//NavigationRequested?.Invoke(new UserManagement());
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			//NavigationRequested?.Invoke(new UserManagement());
		}
	}
}
