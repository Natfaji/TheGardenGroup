using Model.Models;
using System.ComponentModel;

namespace UI.Services
{
	public class SessionService : INotifyPropertyChanged
	{
		private PartialUser? _currentUser;
		public PartialUser? CurrentUser
		{
			get => _currentUser;
			set
			{
				_currentUser = value;
				OnPropertyChanged(nameof(CurrentUser));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}
