using Farm.Views;
using Xamarin.Forms;

namespace Farm
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DisclosureInfoEntryPage), typeof(DisclosureInfoEntryPage));
        }

    }
}
