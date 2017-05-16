[assembly: Xamarin.Forms.ExportRenderer(typeof(DNAXFCircleProgress.XFCircleProgress), typeof(DNAXFCircleProgress.iOS.Renderer.XFCircleProgressRenderer))]
namespace DNAXFCircleProgress.iOS.Renderer
{
    using System;

    using Foundation;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;
    using CoreAnimation;
    using CoreGraphics;

    public class XFCircleProgressRenderer : VisualElementRenderer<XFCircleProgress>
    {
        CAShapeLayer backgroundCircle;
        CAShapeLayer indicatorCircle;
        UILabel indicatorLabel;
        CGSize indicatorLabelSize;
        int indicatorFontSize;

        double startAngle = 1.5 * Math.PI;

        public XFCircleProgressRenderer()
        {
        }

        public static void InitRender()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<XFCircleProgress> e)
        {
            base.OnElementChanged(e);

            indicatorFontSize = Element.TextSize;

            backgroundCircle = new CAShapeLayer();

            CreateBackgroundCircle();

            CreateIndicatorCircle();

            CreateIndicatorLabel();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            double radius = CreatePathAndReturnRadius();

            double heightRatio = (radius - Element.TextMargin) / indicatorLabelSize.Height;
            double widthRatio = (radius - Element.TextMargin) / indicatorLabelSize.Width;
            double ratio = 1;
            if (heightRatio < widthRatio)
                ratio = (radius - Element.TextMargin) / indicatorLabelSize.Height;
            else
                ratio = (radius - Element.TextMargin) / indicatorLabelSize.Width;

            indicatorFontSize = (int)Math.Round(indicatorFontSize * ratio, 0, MidpointRounding.ToEven);
            indicatorLabel.Font = UIFont.SystemFontOfSize(indicatorFontSize);
            indicatorLabel.InvalidateIntrinsicContentSize();
            indicatorLabelSize = indicatorLabel.IntrinsicContentSize;

            indicatorLabel.Frame = new CGRect((Frame.Width / 2) - (indicatorLabelSize.Width / 2), (Frame.Height / 2) - (indicatorLabelSize.Height / 2), indicatorLabelSize.Width, indicatorLabelSize.Height);
            this.AddSubview(indicatorLabel);
            animate();
        }

        private double CalculateValue()
        {
            double min = Element.Minimun;
            double max = Element.Maximun;
            double current = Element.Value;

            double range = max - min;

            return current / range;
        }

        private void CreateIndicatorLabel()
        {
            indicatorLabel = new UILabel();
            indicatorLabel.AdjustsFontSizeToFitWidth = true;
            indicatorLabel.Font = UIFont.SystemFontOfSize(indicatorFontSize);
            indicatorLabel.Text = Element.Text.ToString();
            indicatorLabel.TextColor = Element.TextColor.ToUIColor();
            indicatorLabel.TextAlignment = UITextAlignment.Center;
            indicatorLabelSize = indicatorLabel.IntrinsicContentSize;
        }

        private void CreateIndicatorCircle()
        {
            indicatorCircle = new CAShapeLayer();
            indicatorCircle.StrokeColor = Element.ForeColor.ToCGColor();
            indicatorCircle.FillColor = UIColor.Clear.CGColor;
            indicatorCircle.LineWidth = new nfloat(Element.BarHeight);
            indicatorCircle.Frame = this.Bounds;
            indicatorCircle.LineCap = CAShapeLayer.CapButt;
            this.Layer.AddSublayer(indicatorCircle);
        }

        private void CreateBackgroundCircle()
        {
            backgroundCircle.StrokeColor = Element.BackColor.ToCGColor();
            backgroundCircle.FillColor = UIColor.Clear.CGColor;
            backgroundCircle.LineWidth = new nfloat(Element.BarHeight);
            backgroundCircle.Frame = this.Bounds;
            this.Layer.AddSublayer(backgroundCircle);
        }

        private double CreatePathAndReturnRadius()
        {
            var radius = (Math.Min(Frame.Size.Width, Frame.Size.Height) - backgroundCircle.LineWidth - 2) / 2;
            var circlePath = new UIBezierPath();
            circlePath.AddArc(new CGPoint(Frame.Size.Width / 2, Frame.Size.Height / 2), (nfloat)radius, (nfloat)startAngle, (nfloat)(startAngle + 2 * Math.PI), true);
            backgroundCircle.Path = circlePath.CGPath;
            indicatorCircle.Path = circlePath.CGPath;
            backgroundCircle.StrokeEnd = new nfloat(1.0);
            indicatorCircle.StrokeEnd = new nfloat(CalculateValue());
            return radius;
        }

        private void animate()
        {
            var animation = new CABasicAnimation();
            animation.KeyPath = "strokeEnd";
            animation.Duration = Element.AnimationDuration / 1000;
            animation.From = new NSNumber(0.0);
            animation.To = new NSNumber(CalculateValue());
            animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
            indicatorCircle.StrokeStart = new nfloat(0.0);
            indicatorCircle.StrokeEnd = new nfloat(CalculateValue());
            indicatorCircle.AddAnimation(animation, "appear");
        }
    }
}