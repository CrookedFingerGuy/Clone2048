﻿using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace Clone2048
{
    class SDXMenuIntegerBox: SDXMenuControl
    {
        string label;
        RawColor4 labelColor;
        RawColor4 activeColor;
        TextFormat tFormat;
        RawRectangleF labelRect;
        SolidColorBrush labelSCBrush;
        SolidColorBrush activeSCBrush;
        RawRectangleF valueBox;
        int valueBoxWidth;
        int valueBoxHeight;

        public SDXMenuIntegerBox(RenderTarget D2DRT, TextFormat tf, string l, int x, int y, int width, int height,int vbw,int vbh, int v):base(x,y,width,height)
        {
            label = l;
            tFormat = tf;
            value = v;
            labelRect = new RawRectangleF(x, y, x + width, y + height);
            labelColor = new RawColor4(0f, 0f, 1f, 1f);
            labelSCBrush = new SolidColorBrush(D2DRT, labelColor);

            int gapBetweenLabelAndValueBox = 15;
            valueBoxHeight = vbh;
            valueBoxWidth = vbw;
            valueBox = new RawRectangleF(x + width + gapBetweenLabelAndValueBox, y, x + width + gapBetweenLabelAndValueBox + valueBoxWidth, y + valueBoxHeight);

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
                D2DRT.DrawRectangle(valueBox, activeSCBrush);
            }
            else
            {
                D2DRT.DrawRectangle(valueBox, labelSCBrush);
            }
            D2DRT.DrawText(value.ToString(), tFormat, valueBox, labelSCBrush);
        }
    }
}
