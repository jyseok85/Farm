using OganicInput.Controls;
using OganicInput.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBarNoUnderline), typeof(SearchBarNoUnderlineRenderer))]
namespace OganicInput.Droid.CustomRenderers
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    public class SearchBarNoUnderlineRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
                var plate = Control.FindViewById(plateId);
                plate.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}