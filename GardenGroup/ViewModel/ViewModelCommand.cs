using System;
using System.Windows.Input;

namespace UI.ViewModel
{
	public class ViewModelCommand : ICommand
	{
		#region Fields 
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;
		#endregion Fields

		#region Constructors 
		public ViewModelCommand(Action<object> execute) : this(execute, null) { }

		public ViewModelCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");
			_execute = execute;
			_canExecute = canExecute;
		}
		#endregion Constructors

		#region ICommand Members
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute(parameter);
		}

		public void Execute(object parameter) { _execute(parameter); }
		#endregion ICommand Members
	}
}
