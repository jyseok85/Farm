using Farm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// 페이지가 표시되면 
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Loading loading = new Loading();
            await loading.InitDataSetting(StatusMessage);
          

            // Set the data source for the CollectionView to a
            // sorted collection of notes.
            //collectionView.ItemsSource = notes
            //    .OrderBy(d => d.등재일자)
            //    .ToList();
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            // Navigate to the NoteEntryPage, without passing any data.
            await Shell.Current.GoToAsync(nameof(DisclosureInfoEntryPage));
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                // Navigate to the NoteEntryPage, passing the filename as a query parameter.
                DisclosureInfomation note = (DisclosureInfomation)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(DisclosureInfoEntryPage)}?{nameof(DisclosureInfoEntryPage.ItemId)}={note.공시ID}");
            }
        }
    }
}