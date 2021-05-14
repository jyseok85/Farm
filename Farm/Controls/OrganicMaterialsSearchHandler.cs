using Farm;
using Farm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Farm.Controls
{
    public class OrganicMaterialsSearchHandler: SearchHandler
    {
        //https://docs.microsoft.com/ko-kr/xamarin/xamarin-forms/app-fundamentals/shell/search
        public Type SelectedItemNavigationTarget { get; set; }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if(App.DisInfo != null && App.DisInfo.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(newValue))
                {
                    ItemsSource = null;
                }
                else
                {
                    ItemsSource = App.DisInfo
                        .Where(x => x.공시ID.ToLower().Contains(newValue.ToLower()))
                        .ToList<DisclosureInfomation>();
                }
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            await Task.Delay(1000);

            ShellNavigationState state = (App.Current.MainPage as Shell).CurrentState;
            // The following route works because route names are unique in this application.
            await Shell.Current.GoToAsync($"{GetNavigationTarget()}?name={((DisclosureInfomation)item).공시ID}");
        }

        string GetNavigationTarget()
        {
            return null; //(Shell.Current as AppShell).Routes.FirstOrDefault(route => route.Value.Equals(SelectedItemNavigationTarget)).Key;
        }
    }
}