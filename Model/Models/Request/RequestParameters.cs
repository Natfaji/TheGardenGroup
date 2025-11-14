using System.ComponentModel;

namespace Model.Models.Request
{
	public class RequestParameters : INotifyPropertyChanged
	{
		const int maxPageSize = 100;
		private int _pageNumber = 1;
		private int _itemsPerPage = 10;

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public int ItemsPerPage
		{
			get { return _itemsPerPage; }
			set { _itemsPerPage = value > maxPageSize ? maxPageSize : value; }
		}

		public int PageNumber
		{
			get { return _pageNumber; }
		}

		public void NextPage()
		{
			_pageNumber++;
		}

		public void PreviousPage()
		{
			if (_pageNumber > 1)
			{
				_pageNumber--;
			}
		}
	}
}
