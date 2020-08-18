using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace Clone2048
{
    public class SDXMenuLetterInput:SDXMenuControl
    {
        public string letter;
        RawColor4 labelColor;
        RawColor4 activeColor;
        TextFormat tFormat;
        SolidColorBrush labelSCBrush;
        SolidColorBrush activeSCBrush;
        RawRectangleF valueBox;
        int valueBoxWidth;
        int valueBoxHeight;
        public SDXMenuLetterInput(RenderTarget D2DRT, TextFormat tf, int x, int y, int width, int height) 
                                : base(x, y, width, height)
        {
            tFormat = tf;
            value = -1;
            labelColor = new RawColor4(0f, 0f, 1f, 1f);
            labelSCBrush = new SolidColorBrush(D2DRT, labelColor);
            letter = "A";

            int gapBetweenLabelAndValueBox = 15;
            valueBoxHeight = width;
            valueBoxWidth = height;
            valueBox = new RawRectangleF(x + width + gapBetweenLabelAndValueBox, y, x + width + gapBetweenLabelAndValueBox + valueBoxWidth, y + valueBoxHeight);

            activeColor = new RawColor4(1f, 0f, 0f, 1f);
            activeSCBrush = new SolidColorBrush(D2DRT, activeColor);
            isActive = false;
            isSelectable = true;
        }

        public override void DrawControl(RenderTarget D2DRT, TextFormat tf)
        {
            if (isActive)
            {
                D2DRT.DrawRectangle(valueBox, activeSCBrush);
            }
            else
            {
                D2DRT.DrawRectangle(valueBox, labelSCBrush);
            }
            D2DRT.DrawText(letter, tFormat, valueBox, labelSCBrush);
        }
    }
}
