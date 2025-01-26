using System.Threading.Tasks;
using System.Windows.Input;

namespace RightMove.Desktop.ViewModel.Commands
{
	public interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync(object parameter);

		void RaiseCanExecuteChanged();
	}
}
