using Farm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

            await App.GetInitialData(StatusMessage);
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

        public void aaa()
        {
            SearchLabel.IsVisible = false;
        }

        void SearchBarNoUnderline_Focused(object sender, EventArgs e)
        {
            //SearchBar searchBar = (SearchBar)sender;
            //searchResults.ItemsSource = DataService.GetSearchResults(searchBar.Text);

            //((FarmPage)Shell.Current.CurrentPage).aaa();
            Default = SearchLabel.Bounds;
            CloseLabel();
        }

        private async Task CloseLabel()
        {
            while(SearchLabel.HeightRequest > 0)
            {
                if (SearchLabel.Margin.Top > 0)
                {
                    SearchLabel.Margin = new Thickness(20, SearchLabel.Margin.Top - 2, 0, 0);
                }

                if(SearchLabel.HeightRequest > 0)
                {
                    SearchLabel.HeightRequest -= 2;
                }
                await Task.Delay(1);
                if (CancelButton.Width.Value < 1)
                {
                    GridLength grid = new GridLength(CancelButton.Width.Value + 0.1, GridUnitType.Star);
                    CancelButton.Width = grid;
                }
            }

        }

        private async Task ShowLabel()
        {
            while (SearchLabel.HeightRequest < 36)
            {
                if (SearchLabel.Margin.Top < 20)
                {
                    SearchLabel.Margin = new Thickness(20, SearchLabel.Margin.Top + 2, 0, 0);
                }

                if (SearchLabel.HeightRequest < 36)
                {
                    SearchLabel.HeightRequest += 2;
                }
                await Task.Delay(1);

                if (CancelButton.Width.Value > 0 )
                {
                    double targetValue = CancelButton.Width.Value - 0.1;
                    if(targetValue >= 0)
                    { 
                        GridLength grid = new GridLength(targetValue, GridUnitType.Star);
                        CancelButton.Width = grid;
                    }
                }

            }
        }

                                                                        
        Rectangle Default = Rectangle.Zero;

        private void SearchBarNoUnderline_Unfocused(object sender, FocusEventArgs e)
        {
            ShowLabel();
        }
    }
}
