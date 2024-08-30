using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Model
{
    internal class AllNotes
    {
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note> ();
        public void LoadNotes()
        {
            Notes.Clear ();
            string appDataPath = FileSystem.AppDataDirectory;
            IEnumerable<Note> notes = Directory
                .EnumerateFiles(appDataPath, "*.notes.txt")
                .Select(filename => new Note()
                {
                    Filename = filename,
                    Text = File.ReadAllText(filename),
                    Date = File.GetLastWriteTime(filename)
                })
                .OrderBy(note => note.Date);

            foreach(Note note in notes)
                Notes.Add (note);
        }

        public AllNotes() => LoadNotes();
    }
}
