using System;
using System.ComponentModel;
using UI.Stores;

namespace UI.ViewModel
{
	public class ViewModelBase : INotifyPropertyChanged, IDisposable
	{
		protected NavigationStore _navigationStore;

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public virtual void Dispose() { }
	}
}
