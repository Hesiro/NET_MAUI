namespace Notes.View;

public partial class AllNotesPage : ContentPage
{
	public AllNotesPage()
	{
		InitializeComponent();
		BindingContext = new Model.AllNotes();
	}
    protected override void OnAppearing()
    {
        ((Model.AllNotes)BindingContext).LoadNotes();
    }
	private async void Add_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(NotePage));
	}
	private async void notesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if(e.CurrentSelection.Count != 0)
		{
			var note = (Model.Note)e.CurrentSelection[0];
			await Shell.Current.GoToAsync($"{nameof(NotePage)}?{nameof(NotePage.ItemId)}={note.Filename}");
			notesCollection.SelectedItem = null;
		}
	}
}