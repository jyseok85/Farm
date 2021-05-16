using Farm.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace Farm.Controls
{
    public class SearchBarNoUnderline : SearchBar
    {
        void OnTextChanged(object sender, EventArgs e)
        {
            //SearchBar searchBar = (SearchBar)sender;
            //searchResults.ItemsSource = DataService.GetSearchResults(searchBar.Text);
        }
        
    }
}