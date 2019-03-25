using Xamarin.Forms;

namespace DemoApp.Views
{
    public partial class TodoListPage : ContentPage
    {
        public TodoListPage()
        {
            InitializeComponent();
        }

        private void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (ListOptions != null) ListOptions.SelectedItem = null;
        }
    }
}
