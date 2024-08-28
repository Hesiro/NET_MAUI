using MonkeyFinder.Model;
using MonkeyFinder.Services;
using MonkeyFinder.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.ViewModel
{
    public partial class MonkeysViewModel : BaseViewModel
    {
        public ObservableCollection<Monkey> Monkeys { get; } = new();
        MonkeyService monkeyService;
        public Command GetMonkeysCommand { get; }
        public Command GoToDetailsCommand { get; }
        public MonkeysViewModel(MonkeyService monkeyService)
        {
            Title = "Monkey Finder";
            this.monkeyService = monkeyService;
            GetMonkeysCommand = new Command(async () => await GetMonkeyAsync());
            GoToDetailsCommand = new Command<Monkey>(async (monkey) => await GoToDetailsAsync(monkey));
        }
        async Task GetMonkeyAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var monkeys = await monkeyService.GetMonkeys();

                if(monkeys.Count != 0)
                    Monkeys.Clear();

                foreach (var monkey in monkeys)
                    Monkeys.Add(monkey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task GoToDetailsAsync(Monkey monkey)
        {
            if(monkey == null)
                return;

            await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>{
                {"Monkey", monkey }
            });
        }
        
    }
}
