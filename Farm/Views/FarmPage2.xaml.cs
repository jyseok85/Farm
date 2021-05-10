using Farm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Farm.Views
{
    public partial class FarmPage2 : ContentPage
    {
        //readonly string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");


        public FarmPage2()
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
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var notes = new List<DisclosureInfomation>();
            //설치버전기준 최소 카운트도 비교한다.

            //서버에서 데이터를 조회한다.(카운트)

            //마지막 저장된 숫자보다 클 경우에 추가된 데이터를 조회한다. 

            //Json 파일로 저장을 한다.

            //파일을 읽어서 CollectionView 에 데이터를 넣는다.
            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");
            foreach (var filename in files)
            {
                notes.Add(new DisclosureInfomation
                {
                    //Filename = filename,
                    //Text = File.ReadAllText(filename),
                    //Date = File.GetCreationTime(filename)
                });
            }

            // Set the data source for the CollectionView to a
            // sorted collection of notes.
            collectionView.ItemsSource = notes
                .OrderBy(d => d.등재일자)
                .ToList();
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