using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RightMoveApp.ViewModel.Commands
{
	public abstract class AsyncCommandBase : IAsyncCommand
	{
		public abstract bool CanExecute(object parameter);

		public abstract Task ExecuteAsync(object parameter);

		public async void Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}
	}
}
