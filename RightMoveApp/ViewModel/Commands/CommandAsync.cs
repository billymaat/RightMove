using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RightMoveApp.ViewModel.Commands
{
	public class CommandAsync<T> : ICommand

	{
		private CancellationTokenSource _cancellationTokenSource;
		private readonly Func<T, CancellationToken, Task> _executeTask;

		private readonly Predicate<T> _canExecute;

		private bool _locked;

		public CommandAsync(Func<T, CancellationToken, Task> executeTask) : this(executeTask, o => true)
		{ 
		}

		public CommandAsync(Func<T, CancellationToken, Task> executeTask, Predicate<T> canExecute)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_executeTask = executeTask;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute.Invoke((T)parameter);
		}

		public async void Execute(object parameter)
		{
			try
			{
				if (_locked)
				{
					_cancellationTokenSource.Cancel();
					_cancellationTokenSource = new CancellationTokenSource();
				}

				_locked = true;

				CanExecuteChanged?.Invoke(this, EventArgs.Empty);
				await _executeTask.Invoke((T) parameter, _cancellationTokenSource.Token);
			}
			finally
			{
				_locked = false;
				CanExecuteChanged?.Invoke(this, EventArgs.Empty);
				// _cancellationTokenSource.Dispose();
			}
		}

		public event EventHandler CanExecuteChanged;

		public void ChangeCanExecute()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

	}
}
