using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.CustomControls
{
	/// <summary>
	/// Interaction logic for Pagination.xaml
	/// </summary>
	public partial class Pagination : UserControl
	{
		public int PageNumber
		{
			get { return (int)GetValue(PageNumberProperty); }
			set { SetValue(PageNumberProperty, value); }
		}

		public static readonly DependencyProperty PageNumberProperty =
			DependencyProperty.Register(nameof(PageNumber), typeof(int), typeof(Pagination), new PropertyMetadata(1));

		public ICommand NextPageCommand
		{
			get { return (ICommand)GetValue(NextPageCommandProperty); }
			set { SetValue(NextPageCommandProperty, value); }
		}

		public static readonly DependencyProperty NextPageCommandProperty =
			DependencyProperty.Register(nameof(NextPageCommand), typeof(ICommand), typeof(Pagination));

		public ICommand PreviousPageCommand
		{
			get { return (ICommand)GetValue(PreviousPageCommandProperty); }
			set { SetValue(PreviousPageCommandProperty, value); }
		}

		public static readonly DependencyProperty PreviousPageCommandProperty =
			DependencyProperty.Register(nameof(PreviousPageCommand), typeof(ICommand), typeof(Pagination));

		public ICommand ChangeItemsPerPageCommand
		{
			get { return (ICommand)GetValue(ChangeItemsPerPageCommandProperty); }
			set { SetValue(ChangeItemsPerPageCommandProperty, value); }
		}

		public static readonly DependencyProperty ChangeItemsPerPageCommandProperty =
			DependencyProperty.Register(nameof(ChangeItemsPerPageCommand), typeof(ICommand), typeof(Pagination));

		public int ItemsPerPage
		{
			get { return (int)GetValue(ItemsPerPageProperty); }
			set { SetValue(ItemsPerPageProperty, value); }
		}

		public static readonly DependencyProperty ItemsPerPageProperty =
			DependencyProperty.Register(nameof(ItemsPerPage), typeof(int), typeof(Pagination), new PropertyMetadata(10));

		public Pagination()
		{
			InitializeComponent();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ChangeItemsPerPageCommand != null && ChangeItemsPerPageCommand.CanExecute(ItemsPerPage))
			{
				ChangeItemsPerPageCommand.Execute(ItemsPerPage);
			}
		}
	}
}
