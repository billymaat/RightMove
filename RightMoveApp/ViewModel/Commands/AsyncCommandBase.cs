using System;
using System.Threading.Tasks;

namespace RightMove.Desktop.ViewModel.Commands
{
	public abstract class AsyncCommandBase : IAsyncCommand
	{
		public abstract bool CanExecute(object parameter);

		public abstract Task ExecuteAsync(object parameter);

		public async void Execute(object parameter)
		{
			await ExecuteAsync(parameter);
		}

		//public event EventHandler CanExecuteChanged
		//{
		//	add
		//	{
		//		CommandManager.RequerySuggested += value;
		//	}
		//	remove
		//	{
		//		CommandManager.RequerySuggested -= value;
		//	}
		//}

		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged()
		{
			//CommandManager.InvalidateRequerySuggested();
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, new EventArgs());
			}
		}
	}
}
