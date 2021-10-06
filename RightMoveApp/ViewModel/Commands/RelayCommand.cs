using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using EventArgs = System.EventArgs;

namespace RightMoveApp.ViewModel.Commands
{
	public class RelayCommand : ICommand
	{
		// public event EventHandler CanExecuteChanged;
		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}
		private readonly Action<object>  _executeMethod;
		private readonly Func<object, bool> _canExecuteMethod;

		public RelayCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
		{
			_executeMethod = executeMethod;
			_canExecuteMethod = canExecuteMethod;
		}



		public bool CanExecute(object parameter)
		{
			return _canExecuteMethod?.Invoke(parameter) ?? true;
		}

		public void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				_executeMethod(parameter);
			}
		}
		
		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
			// CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
