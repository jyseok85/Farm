using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Farm.Views
{
    public partial class FarmPage : ContentPage
    {
        //readonly string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");


        public FarmPage()
        {
            InitializeComponent();
            //// Read the file.
            //if (File.Exists(_fileName))
            //{
            //    editor.Text = File.ReadAllText(_fileName);
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var notes = new List<Note>();

            // Create a Note object from each file.
            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");
            foreach (var filename in files)
            {
                notes.Add(new Note
                {
                    Filename = filename,
                    Text = File.ReadAllText(filename),
                    Date = File.GetCreationTime(filename)
                });
            }

            // Set the data source for the CollectionView to a
            // sorted collection of notes.
            collectionView.ItemsSource = notes
                .OrderBy(d => d.Date)
                .ToList();
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            // Navigate to the NoteEntryPage, without passing any data.
            await Shell.Current.GoToAsync(nameof(NoteEntryPage));
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                // Navigate to the NoteEntryPage, passing the filename as a query parameter.
                Note note = (Note)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(NoteEntryPage)}?{nameof(NoteEntryPage.ItemId)}={note.Filename}");
            }
        }
    }
}