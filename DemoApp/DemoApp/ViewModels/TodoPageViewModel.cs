using DemoApp.Events;
using DemoApp.Models;
using DemoApp.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoApp.ViewModels
{
    public class TodoPageViewModel : ViewModelBase
	{
	    public ICommand SaveCommand { get; set; }

	    private string _id;
	    public string Id
	    {
	        get => _id;
	        set => SetProperty(ref _id, value);
	    }

        private string _description;
	    public string Description
	    {
	        get => _description;
	        set => SetProperty(ref _description, value);
	    }

	    private bool _isComplete;
	    public bool IsComplete
        {
	        get => _isComplete;
	        set => SetProperty(ref _isComplete, value);
	    }

        public TodoPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ITodoService todoService) : base(navigationService, eventAggregator, todoService)
	    {
	        Init();
	    }

	    private void Init()
	    {
	        Title = "Todo";
	        SaveCommand = new DelegateCommand(async () => await OnSaveCommand(), () => !IsExecNavigation && !string.IsNullOrEmpty(Description)).ObservesProperty(() => IsExecNavigation).ObservesProperty(() => Description);
        }

	    private async Task OnSaveCommand()
	    {
	        IsExecNavigation = true;

	        await RunSafe(Save(), true, "Load...");

	        if (!string.IsNullOrEmpty(Message))
	        {
	            await Alert(Message);
	        }

	        if (!HasError)
	        {
	            EventAggregator.GetEvent<RefreshServicesEvent>().Publish();
	            await NavigationService.GoBackAsync();
	        }

	        IsExecNavigation = false;
	    }

	    private async Task Save()
	    {
	        IsExecNavigation = true;

	        var todo = new TodoModel
	        {
	            Id = Id,
	            Description = Description,
	            IsComplete = IsComplete
	        };

	        var res = (string.IsNullOrEmpty(todo.Id)) ?  await TodoService.AddTodo(todo):await TodoService.UpdateTodo(todo);

            Message = res ? "Todo Save!!!" : "Error in Todo Save!!!";
	        IsExecNavigation = false;
	    }

	    public override async void OnNavigatedTo(INavigationParameters parameters)
	    {
	        var par1 = parameters["Todo"];

	        if (par1 == null)
	        {
	            await NavigationService.GoBackAsync();
	            return;
	        }

	        var todo = (TodoModel)par1;

            Title = string.IsNullOrEmpty(todo.Id) ? "New Todo" : "Update Todo";

	        Description = todo.Description;
	        Id = todo.Id;
	        IsComplete = todo.IsComplete;
	    }
    }
}