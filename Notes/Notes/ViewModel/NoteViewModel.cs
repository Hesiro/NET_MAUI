using Notes.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.ViewModel
{
    internal class NoteViewModel : BaseViewModel, IQueryAttributable
    {
        private Note note;

        public string Text
        {
            get => note.Text;
            set
            {
                if (note.Text != value)
                {
                    note.Text = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime Date => note.Date;
        public string Identifier => note.Filename;
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public NoteViewModel()
        {
            note = new Note();
            SaveCommand = new Command(async() => await Save());
            DeleteCommand = new Command(async() => await Delete());
        }
        public NoteViewModel(Note note)
        {
            this.note = note;
            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(async () => await Delete());
        }

        async Task Save()
        {
            note.Date = DateTime.Now;
            note.Save();
            await Shell.Current.GoToAsync($"..?saved={note.Filename}");
        }
        async Task Delete()
        {
            note.Delete();
            await Shell.Current.GoToAsync($"..?deleted={note.Filename}");
        }
        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("load"))
            {
                note = Note.Load(query["load"].ToString());
                RefreshProperties();
            }
        }
        public void Reload()
        {
            note = Note.Load(note.Filename);
            RefreshProperties();
        }
        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(Date));
        }
    }
}
