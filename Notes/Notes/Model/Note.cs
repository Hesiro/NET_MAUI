using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Model
{
    internal class Note
    {
        public string Filename { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public void Save() =>
            File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Text);
        public void Delete() =>
            File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));
        public static Note Load(string filename)
        {
            filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(filename))
                throw new FileNotFoundException("Unable to find file on local storage.", filename);

            return
                new()
                {
                    Filename = Path.GetFileName(filename),
                    Text = File.ReadAllText(filename),
                    Date = File.GetLastWriteTime(filename)
                };
        }
        public static IEnumerable<Note> LoadAll()
        {
            string appDataPath = FileSystem.AppDataDirectory;

            return Directory
                .EnumerateFiles(appDataPath, "*.notes.txt")
                .Select(filename => Note.Load(Path.GetFileName(filename)))
                .OrderByDescending(note => note.Date);
        }
        public Note()
        {
            Filename = $"{Path.GetRandomFileName()}.notes.txt";
            Date = DateTime.Now;
            Text = "";
        }
    }
}
