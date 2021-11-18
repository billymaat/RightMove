using RightMoveApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RightMoveApp.ViewModel.Commands
{
	public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
	{
		private readonly Func<Task<TResult>> _command;
		private NotifyTaskCompletion<TResult> _execution;
		private readonly Func<bool> _canExecute;

		public AsyncCommand(Func<Task<TResult>> command, Func<bool> canExecute = null)
		{
			_command = command;
			_canExecute = canExecute;
		}

		public override bool CanExecute(object parameter)
		{
			return _canExecute != null ? _canExecute() : true;
		}

		public override Task ExecuteAsync(object parameter)
		{
			Execution = new NotifyTaskCompletion<TResult>(_command());
			return Execution.TaskCompletion;
		}

		public NotifyTaskCompletion<TResult> Execution
		{
			get { return _execution; }
			private set
			{
				_execution = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}

	public static class AsyncCommand
	{
		public static AsyncCommand<object> Create(Func<Task> command)
		{
			return new AsyncCommand<object>(async () => { await command(); return null; });
		}

		public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command, Func<bool> canExecute = null)
		{
			return new AsyncCommand<TResult>(command, canExecute);
		}
	}
}
