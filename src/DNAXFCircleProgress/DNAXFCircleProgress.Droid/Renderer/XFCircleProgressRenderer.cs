[assembly: Xamarin.Forms.ExportRenderer(typeof(DNAXFCircleProgress.XFCircleProgress), typeof(DNAXFCircleProgress.Droid.Renderer.XFCircleProgressRenderer))]
namespace DNAXFCircleProgress.Droid.Renderer
{
    using Android.Widget;
    using Xamarin.Forms.Platform.Android;
    using Android.Graphics.Drawables;
    using Android.Support.V4.Graphics.Drawable;
    using Android.Graphics;
    using Android.Text;
    using Android.Animation;
    using Android.Views.Animations;

    public class XFCircleProgressRenderer : ViewRenderer<XFCircleProgress, ProgressBar>
    {
        private ProgressBar pBar;
        private Drawable pBarBackDrawable;
        private Drawable pBarForeDrawable;
        public XFCircleProgressRenderer()
        {
            SetWillNotDraw(false);
        }

        public static void InitRender()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XFCircleProgress> e)
        {

            base.OnElementChanged(e);
            if (Control == null)
            {
                pBar = CreateNativeControl();
                SetNativeControl(pBar);
                CreateAnimation();
            }
        }

        protected override ProgressBar CreateNativeControl()
        {
            pBarBackDrawable = DrawableCompat.Wrap(Resources.GetDrawable("CircularProgress_background"));
            pBarForeDrawable = DrawableCompat.Wrap(Resources.GetDrawable("CircularProgress_drawable"));

            DrawableCompat.SetTint(pBarBackDrawable, Element.BackColor.ToAndroid());
            DrawableCompat.SetTint(pBarForeDrawable, Element.ForeColor.ToAndroid());

            var nativeControl = new ProgressBar(this.Context, null, global::Android.Resource.Attribute.ProgressBarStyleHorizontal)
            {
                Indeterminate = false,
                Max = Element.Maximun,
                ProgressDrawable = pBarForeDrawable,
                Rotation = -90f,
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent),
            };

            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Kitkat)
                nativeControl.SetBackgroundDrawable(pBarBackDrawable);
            else
                nativeControl.SetBackground(pBarBackDrawable);

            return nativeControl;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            Rect bounds = new Rect();
            TextPaint paint = new TextPaint();
            paint.Color = Element.TextColor.ToAndroid();
            paint.TextSize = Element.TextSize;
            paint.GetTextBounds(Element.Text.ToString(), 0, Element.Text.ToString().Length, bounds);
            if (((this.Width / 2) - (Element.TextMargin * 4)) < bounds.Width())
            {
                float ratio = (float)((this.Width / 2) - Element.TextMargin * 4) / (float)bounds.Width();
                paint.TextSize = paint.TextSize * ratio;
                paint.GetTextBounds(Element.Text.ToString(), 0, Element.Text.ToString().Length, bounds);
            }

            int x = this.Width / 2 - bounds.CenterX();
            int y = this.Height / 2 - bounds.CenterY();
            canvas.DrawText(Element.Text.ToString(), x, y, paint);
        }

        private void CreateAnimation()
        {
            ObjectAnimator anim = ObjectAnimator.OfInt(pBar, "progress", Element.Minimun, Element.Value);
            anim.SetDuration(Element.AnimationDuration);
            anim.SetInterpolator(new DecelerateInterpolator());
            anim.Start();
        }
    }
}