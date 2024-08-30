using Notes.Model;
using Notes.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.ViewModel
{
    internal class NotesViewModel : IQueryAttributable
    {
        public ObservableCollection<NoteViewModel> AllNotes { get; }
        public Command NewCommand {  get; }
        public Command SelectNoteCommand {  get; }

        public NotesViewModel()
        {
            AllNotes = new ObservableCollection<NoteViewModel>(
                Note.LoadAll()
                .Select(n => new NoteViewModel(n))
                );
            NewCommand = new Command(async() => await NewNoteAsync());
            SelectNoteCommand = new Command<NoteViewModel>(async(note) => await SelectNoteAsync(note));
        }

        async Task NewNoteAsync()
        {
            await Shell.Current.GoToAsync(nameof(NotePage));
        }
        async Task SelectNoteAsync(NoteViewModel note)
        {
            if (note != null)
                await Shell.Current.GoToAsync($"{nameof(NotePage)}?load={note.Identifier}");
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string noteId = query["deleted"].ToString();
                NoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();
                if (matchedNote != null)
                    AllNotes.Remove(matchedNote);                
            }
            else if (query.ContainsKey("saved"))
            {
                string noteId = query["saved"].ToString();
                NoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();
                if (matchedNote != null)
                {
                    matchedNote.Reload();
                    AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
                }
                else
                    AllNotes.Insert(0, new NoteViewModel(Note.Load(noteId)));
            }
        }
    }
}
