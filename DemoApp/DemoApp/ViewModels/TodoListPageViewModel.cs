using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DemoApp.Events;
using DemoApp.Models;
using DemoApp.Services;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace DemoApp.ViewModels
{
	public class TodoListPageViewModel : ViewModelBase
	{
	    private ObservableCollection<TodoModel> _list;
	    public ObservableCollection<TodoModel> List
	    {
	        get => _list;
	        set => SetProperty(ref _list, value);
	    }
	    private bool _isItemsTodo;
	    public bool IsItemsTodo
        {
	        get => _isItemsTodo;
	        set => SetProperty(ref _isItemsTodo, value);
	    }

        public TodoListPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ITodoService todoService) : base(navigationService, eventAggregator, todoService)
	    {
	        Init();
	    }

	    private async void Init()
	    {
	        Title = "Todos";
	        EventAggregator.GetEvent<RefreshServicesEvent>().Subscribe(Refresh);
            await RunSafe(GetAllTodos(), false, "Load...");
	    }

	    public override void Destroy()
	    {
	        base.Destroy();
	        EventAggregator.GetEvent<RefreshServicesEvent>().Unsubscribe(Refresh);
	    }

	    private ICommand _addTodoCommand;

	    public ICommand AddTodoCommand
        {
	        get
	        {
	            return _addTodoCommand ?? (_addTodoCommand = new Command(async () => { await OnAddtodoCommand(); }));
	        }
	    }

	    private ICommand _navigateCommand;

	    public ICommand NavigateCommand
	    {
	        get
	        {
	            return _navigateCommand ?? (_navigateCommand = new Command(async (value) =>
	            {
	                if (IsExecNavigation) return;

	                IsExecNavigation = true;

	                var model = (TodoModel)value;

	                var par = new NavigationParameters { { "Todo", model } };
	                await NavigationService.NavigateAsync("TodoPage", par);

	                IsExecNavigation = false;

	            }));
	        }
	    }

	    private ICommand _checkCommand;

	    public ICommand CheckCommand
	    {
	        get
	        {
	            return _checkCommand ?? (_checkCommand = new Command(async (value) =>
	                       {
	                           IsExecNavigation = true;
                               var result = (TodoModel)value;
	                           await RunSafe(ChangeStatusTodo(result.Id, !result.IsComplete), false);
	                           IsExecNavigation = false;
                           }
	                   ));
	        }
	    }

	    private ICommand _deleteCommand;

	    public ICommand DeleteCommand
	    {
	        get
	        {
	            return _deleteCommand ?? (_deleteCommand = new Command(async (value) =>
	                       {
	                           IsExecNavigation = true;
                               var result = (TodoModel)value;
	                           await RunSafe(OnDeleteCommand(result), false);
	                           IsExecNavigation = false;
                           }
	                   ));
	        }
	    }

        private async Task OnAddtodoCommand()
	    {
	        var par = new NavigationParameters { { "Todo", new TodoModel { Id="", Description = "", IsComplete = false} } };
	        await NavigationService.NavigateAsync("TodoPage", par);
	    }

	    private async Task ChangeStatusTodo(string id, bool status)
	    {
	        await TodoService.UpdateEstus(id, status);
	        await GetAllTodos();
        }

	    private async Task OnDeleteCommand(TodoModel todo)
	    {
	        var res = await TodoService.Delete(todo);
	        await GetAllTodos();
	    }

	    private async void Refresh()
	    {
	        await RunSafe(GetAllTodos(), false, "Load...");
	    }

	    private ICommand _initCommand;

	    public ICommand InitCommand
	    {
	        get
	        {
	            return _initCommand ?? (_initCommand = new Command(async () =>
	            {
	                await RunSafe(GetAllTodos(), false, "Load...");
	            }));
	        }
	    }


        private async Task GetAllTodos()
        {
            IsExecNavigation = true;
            //var res = await TodoService.AddTodo(new TodoModel {Id = null, Description = "sdsds", IsComplete = false});
	        var todos = await TodoService.GetAll();
	        IsItemsTodo = todos.Count > 0;
            Message = IsItemsTodo ? "" : "No se encontraron elementos";

            List = new ObservableCollection<TodoModel>(todos);
            IsExecNavigation = false;
        }
	}
}
