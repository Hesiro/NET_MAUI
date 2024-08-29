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
        IConnectivity connectivity;
        IGeolocation geolocation;
        bool isRefreshing;
        public Command GetMonkeysCommand { get; }
        public Command GoToDetailsCommand { get; }
        public Command GetClosestMonkeyCommand {  get; }
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                if (isRefreshing == value)
                    return;
                isRefreshing = value;
                OnPropertyChanged();                
            }
        }
        public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
        {
            Title = "Monkey Finder";
            this.monkeyService = monkeyService;
            this.connectivity = connectivity;
            this.geolocation = geolocation;
            GetMonkeysCommand = new Command(async () => await GetMonkeyAsync());
            GoToDetailsCommand = new Command<Monkey>(async (monkey) => await GoToDetailsAsync(monkey));
            GetClosestMonkeyCommand = new Command(async () => await GetClosestMonkey());
        }
        async Task GetMonkeyAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if(connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "Ok");
                    return;
                }

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
                IsRefreshing = false;
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
        async Task GetClosestMonkey()
        {
            if(IsBusy || Monkeys.Count == 0) 
                return;

            try
            {
                var location = await geolocation.GetLastKnownLocationAsync();
                if(location == null)
                {
                    location = await geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(30)
                        });
                }

                var first = Monkeys.OrderBy(m => location.CalculateDistance(
                    new Location(m.Latitude, m.Longitude), DistanceUnits.Miles)).FirstOrDefault();

                await Shell.Current.DisplayAlert("",first.Name + " " + first.Location, "OK");
            }
            catch(Exception ex) 
            {
                Debug.WriteLine($"Unable to query location: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!",ex.Message, "OK");
            }
        }
    }
}
