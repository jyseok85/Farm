using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using OganicInput.Models;

namespace OganicInput.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingView : ContentPage
    {
        public LoadingView()
        {
            InitializeComponent();
            BackgroundColor = new Color(0, 0, 0, 0.5f);
        }
        protected async override void OnAppearing()
        {
            base.OnParentSet();

            if (App.DisInfo == null)
            {
                Loading loading = new Loading();
                await loading.InitDataSetting(StatusMessage);
                await Navigation.PopModalAsync();
            }
        }
    }
}