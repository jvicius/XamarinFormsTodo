using Prism.Mvvm;

namespace DemoApp.Models
{
    public class TodoModel : BindableBase
    {
        private string _id;
        private string _description;
        private bool _isComplete;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public bool IsComplete
        {
            get => _isComplete;
            set => SetProperty(ref _isComplete, value);
        }
    }
}
