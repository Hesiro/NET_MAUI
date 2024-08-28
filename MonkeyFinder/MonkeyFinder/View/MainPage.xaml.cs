using MonkeyFinder.ViewModel;

namespace MonkeyFinder.View
{
    public partial class MainPage : ContentPage
    {
        //int count = 0;

        public MainPage(MonkeysViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}
    }

}
