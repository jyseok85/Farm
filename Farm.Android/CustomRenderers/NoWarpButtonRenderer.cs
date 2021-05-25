using OganicInput.Controls;
using OganicInput.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(NoWarpButtonRenderer))]
namespace OganicInput.Droid.CustomRenderers
{
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
    public class NoWarpButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetMaxLines(1);
            }
        }
    }
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
}