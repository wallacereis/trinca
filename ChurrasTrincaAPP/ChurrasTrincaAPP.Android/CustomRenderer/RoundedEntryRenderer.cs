using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Content;
using ChurrasTrincaAPP.CustomRenderer;
using ChurrasTrincaAPP.Droid.CustomRenderer;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]
namespace ChurrasTrincaAPP.Droid.CustomRenderer
{
    public class RoundedEntryRenderer : EntryRenderer
    {
        public RoundedEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                //Control.SetBackgroundResource(Resource.Layout.rounded_shape);
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(10f);
                gradientDrawable.SetStroke(1, Android.Graphics.Color.ParseColor("#000000"));
                gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
                Control.SetBackground(gradientDrawable);

                Control.SetPadding(20, 0, 0, 5);
                //Control.SetCompoundDrawablesRelativeWithIntrinsicBounds(Resource.Drawable.usuario, 0, 0, 0);
            }
        }
    }
}