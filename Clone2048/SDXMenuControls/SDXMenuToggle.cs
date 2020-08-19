using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;


namespace Clone2048.SDXMenuControls
{
    public class SDXMenuToggle:SDXMenuControl
    {
        string label;
        RawColor4 labelColor;
        RawColor4 activeColor;
        TextFormat tFormat;
        RawRectangleF labelRect;
        SolidColorBrush labelSCBrush;
        SolidColorBrush activeSCBrush;
        RawRectangleF toggleBox;
        int toggleBoxWidth;
        int toggleBoxHeight;

        public SDXMenuToggle(RenderTarget D2DRT, TextFormat tf, string l, int x, int y, int width, int height,int tbw,int tbh,int v)
                                : base(x, y, width, height)
        {
            label = l;
            tFormat = tf;
            value = v; //0 is off 1 is on
            labelRect = new RawRectangleF(x, y, x + width, y + height);
            labelColor = new RawColor4(0f, 0f, 1f, 1f);
            labelSCBrush = new SolidColorBrush(D2DRT, labelColor);

            int gapBetweenLabelAndValueBox = 15;
            toggleBoxWidth = tbw;
            toggleBoxHeight = tbh;
            toggleBox = new RawRectangleF(x + width + gapBetweenLabelAndValueBox, y, x + width + gapBetweenLabelAndValueBox + toggleBoxWidth, y + toggleBoxHeight);

            activeColor = new RawColor4(1f, 0f, 0f, 1f);
            activeSCBrush = new SolidColorBrush(D2DRT, activeColor);
            isActive = false;
            isSelectable = true;
        }

        public override void DrawControl(RenderTarget D2DRT, TextFormat tf)
        {
            D2DRT.DrawText(label, tFormat, labelRect, labelSCBrush);
            if (isActive)
            {
                D2DRT.DrawRectangle(toggleBox, activeSCBrush);
            }
            else
            {
                D2DRT.DrawRectangle(toggleBox, labelSCBrush);
            }
            if(value==0)
                D2DRT.DrawText("OFF", tFormat, toggleBox, labelSCBrush);
            else
                D2DRT.DrawText("ON", tFormat, toggleBox, labelSCBrush);
        }
    }
}
