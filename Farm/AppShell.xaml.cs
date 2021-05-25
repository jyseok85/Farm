using OganicInput.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OganicInput
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

        public AppShell()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);
            RegisterRoutes();
            //Routing.RegisterRoute(nameof(DisclosureInfoEntryPage), typeof(DisclosureInfoEntryPage));
            //Routing.RegisterRoute(nameof(OganicInputPage), typeof(OganicInputPage));
            //Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            //Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));

        }

        void RegisterRoutes()
        {
            //페이지 등록을 하면, GoToAsync 메서드를 통해 (URI 기반 탐색을 사용하여 ) 이 페이지로 이동할 수 있다.
            //Routes.Add("DisclosureInfoEntryPage", typeof(DisclosureInfoEntryPage));
            //Routes.Add("OganicInputPage", typeof(OganicInputPage));
            Routes.Add("OganicInputDetailPage", typeof(OganicInputDetailPage));

            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

    }
}
