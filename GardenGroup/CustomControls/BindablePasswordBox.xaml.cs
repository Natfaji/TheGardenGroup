using System;
using System.Windows;
using System.Windows.Controls;

namespace UI.CustomControls
{
	/// <summary>
	/// Interaction logic for BindablePasswordBox.xaml
	/// </summary>
	public partial class BindablePasswordBox : UserControl
	{
		public string Password
		{
			get { return (string)GetValue(PasswordProperty); }
			set { SetValue(PasswordProperty, value); }
		}

		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.Register(nameof(Password), typeof(string), typeof(BindablePasswordBox));

		public BindablePasswordBox()
		{
			InitializeComponent();
			txtPasswordBox.PasswordChanged += onPasswordChanged;
		}

		private void onPasswordChanged(object sender, RoutedEventArgs e)
		{
			Password = txtPasswordBox.Password;
		}
	}
}
