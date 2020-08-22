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
    class SDXMenuTextInput: SDXMenuControl
    {
        string label;
        private string input;
        public string Input 
        { 
            get
            { 
                return input; 
            } 
            set 
            {
                if (value.Length <= maxInputLength)                
                    input = value;
            } 
        }
        public int maxInputLength;
        RawColor4 labelColor;
        RawColor4 activeColor;
        TextFormat tFormat;
        RawRectangleF labelRect;
        SolidColorBrush labelSCBrush;
        SolidColorBrush activeSCBrush;
        RawRectangleF inputBox;
        int inputBoxWidth;
        int inputBoxHeight;

        public SDXMenuTextInput(RenderTarget D2DRT, TextFormat tf, string l, int x, int y, int width, int height, int vbw, int vbh, string v) : base(x, y, width, height)
        {
            label = l;
            tFormat = tf;
            input = v;
            labelRect = new RawRectangleF(x, y, x + width, y + height);
            labelColor = new RawColor4(0f, 0f, 1f, 1f);
            labelSCBrush = new SolidColorBrush(D2DRT, labelColor);
            maxInputLength = 10;

            int gapBetweenLabelAndValueBox = 15;
            inputBoxWidth = vbw;
            inputBoxHeight = vbh;
            inputBox = new RawRectangleF(x + width + gapBetweenLabelAndValueBox, y, x + width + gapBetweenLabelAndValueBox + inputBoxWidth, y + inputBoxHeight);

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
                D2DRT.DrawRectangle(inputBox, activeSCBrush);
            }
            else
            {
                D2DRT.DrawRectangle(inputBox, labelSCBrush);
            }
            D2DRT.DrawText(Input, tFormat, inputBox, labelSCBrush);
        }
    }
}
