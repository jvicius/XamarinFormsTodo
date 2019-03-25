using Acr.UserDialogs;
using DemoApp.Services;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DemoApp.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; }
        protected IEventAggregator EventAggregator { get; }
        protected ITodoService TodoService { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _hasError;

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private bool _isExecNavigation;
        public bool IsExecNavigation
        {
            get => _isExecNavigation;
            set => SetProperty(ref _isExecNavigation, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ViewModelBase(INavigationService navigationService , IEventAggregator eventAggregator, ITodoService todoService)
        {
            NavigationService = navigationService;
            EventAggregator = eventAggregator;
            TodoService = todoService;
        }

        public ICommand BackCommand
        {
            get
            {
                return new Command(async (value) =>
                {
                    try
                    {
                        await NavigationService.GoBackAsync();
                    }
                    catch (Exception e)
                    {
                        while (e.InnerException != null)
                        {
                            e = e.InnerException;
                        }
                    }
                });
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new Command(async (value) =>
                {
                    try
                    {
                        await NavigationService.GoBackToRootAsync();
                    }
                    catch (Exception e)
                    {
                        while (e.InnerException != null)
                        {
                            e = e.InnerException;
                        }
                    }
                });
            }
        }

        public async Task RunSafe(Task task, bool showLoading = true, string loadinMessage = null)
        {
            try
            {
                if (IsBusy) return;

                IsBusy = true;

                if (showLoading) UserDialogs.Instance.ShowLoading(loadinMessage ?? "Load...");

                await task;
            }
            catch (TaskCanceledException)
            {
                IsBusy = false;
            }
            catch (Exception e)
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }

                await UserDialogs.Instance.AlertAsync("Error", "Internet conection lost",
                    "Ok");
            }
            finally
            {
                IsBusy = false;
                if (showLoading) UserDialogs.Instance.HideLoading();
            }
        }

        public static void Toast(string msg)
        {
            var toastConfig = new ToastConfig(msg)
            {
                Duration = TimeSpan.FromSeconds(3),
                Position = ToastPosition.Top
            };

            UserDialogs.Instance.Toast(toastConfig);
        }

        public async Task<DatePromptResult> DatePromptAsync(DateTime time)
        {
            var date = new DatePromptConfig
            {
                Title = Title,
                OkText = "Ok",
                CancelText = "Cancel",
                SelectedDate = time
            };
            return await UserDialogs.Instance.DatePromptAsync(date);
        }

        public async Task Alert(string msg)
        {
            var c = new AlertConfig
            {
                Title = Title,
                Message = msg,
                OkText = "Ok"
            };
            await UserDialogs.Instance.AlertAsync(c);
        }

        public async Task<bool> Confirm(string msg)
        {
            var c = new ConfirmConfig
            {
                Title = Title,
                Message = msg,
                OkText = "Ok",
                CancelText = "Cancel"
            };
            return await UserDialogs.Instance.ConfirmAsync(c);
        }

        public async Task<string> ActionSheetAsync(string[] buttons, string cancel)
        {
            return await UserDialogs.Instance.ActionSheetAsync(Title, cancel, null, null, buttons);
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
