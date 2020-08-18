
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace Clone2048
{
    class SDXMenuLabel:SDXMenuControl
    {
        public string label;
        RawColor4 labelColor;
        SolidColorBrush labelSCBrush;
        RawRectangleF labelRect;
        public TextFormat tFormat;

        public SDXMenuLabel(RenderTarget D2DRT, TextFormat tf, string l, int x,int y,int width,int height):base(x,y,width,height)
        {
            label = l;
            tFormat = tf; 
            labelRect = new RawRectangleF(x, y, x + width, y + height);
            labelColor = new RawColor4(0f, 0f, 1f, 1f);
            labelSCBrush = new SolidColorBrush(D2DRT, labelColor);
            isSelectable = false;
        }
        public override void DrawControl(RenderTarget D2DRT, TextFormat tf)
        {
            D2DRT.DrawText(label, tFormat, labelRect, labelSCBrush);
        }

    }
}
