namespace Notes.View;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class NotePage : ContentPage
{
	string fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");
	public NotePage()
	{
		InitializeComponent();
		
		string appDataPath = FileSystem.AppDataDirectory;
		string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

		LoadNote(Path.Combine(appDataPath, randomFileName));
	}
	private void LoadNote(string fileName)
	{
		Model.Note noteModel = new Model.Note();
		noteModel.Filename = fileName;

		if (File.Exists(fileName))
		{
			noteModel.Date = File.GetCreationTime(fileName);
			noteModel.Text = File.ReadAllText(fileName);
		}

		BindingContext = noteModel;
	}
	private async void SaveButton_Clicked(object sender, EventArgs e)
	{
		if(BindingContext is Model.Note note)
			File.WriteAllText(note.Filename, TextEditor.Text);

		await Shell.Current.GoToAsync("..");
	}
	private async void DeleteButton_Clicked(Object sender, EventArgs e)
	{
		if(BindingContext is Model.Note note)
		{
            if (File.Exists(note.Filename))
                File.Delete(note.Filename);
        }

        await Shell.Current.GoToAsync("..");
    }
    public string ItemId
    {
        set { LoadNote(value); }
    }
}