using Farm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Farm.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class DisclosureInfoEntryPage : ContentPage
    {
        public string ItemId
        {
            set
            {
                LoadDisclosureInfomation(value);
            }
        }


        public DisclosureInfoEntryPage()
        {
            InitializeComponent();

            // Set the BindingContext of the page to a new Note.
            BindingContext = new DisclosureInfomation();

        }

        

        void LoadDisclosureInfomation(string filename)
        {
            try
            {
                // Retrieve the note and set it as the BindingContext of the page.
                DisclosureInfomation note = new DisclosureInfomation
                {
                    //Filename = filename,
                    //Text = File.ReadAllText(filename),
                    //Date = File.GetCreationTime(filename)
                };

                BindingContext = note;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load note.");
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var info = (DisclosureInfomation)BindingContext;

            //if (string.IsNullOrWhiteSpace(note.Filename))
            {
                // Save the file.
                var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes.txt");
               // File.WriteAllText(filename, note.Text);
            }
            //else
            {
                // Update the file.
               // File.WriteAllText(note.Filename, note.Text);
            }

            //이전페이지로 이동
            await Shell.Current.GoToAsync("..");
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (DisclosureInfomation)BindingContext;

            //// Delete the file.
            //if (File.Exists(note.Filename))
            //{
            //    File.Delete(note.Filename);
            //}

            // Navigate backwards
            await Shell.Current.GoToAsync("..");
        }
    }
}