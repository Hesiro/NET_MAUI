using MonkeyFinder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.ViewModel
{
    [QueryProperty(nameof(Monkey), "Monkey")]
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        public MonkeyDetailsViewModel()
        {
            
        }
        
        Monkey monkey;
        public Monkey Monkey
        {
            get => monkey;
            set
            {
                if (monkey == value)
                    return;
                monkey = value;
                OnPropertyChanged();
            }
        }
    }
}
