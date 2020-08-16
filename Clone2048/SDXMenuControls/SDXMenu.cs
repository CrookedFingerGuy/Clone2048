using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.XInput;

namespace Clone2048.SDXMenuControls
{
    public class SDXMenu
    {
        public bool isVisible;
        public string menuName;

        protected int screenWidth;
        protected int ScreenHeight;
        protected int menuWidth;
        protected int menuHeight;
        protected SolidColorBrush backgroundColor;
        protected RawRectangleF menuSize;
        protected List<SDXMenuControl> menuControls;
        protected TextFormat startMenuTextFormat;
        protected int activeControl;
        public SDXMenu(RenderTarget D2DRT, TextFormat tf, int width, int height, string name)
        {
            isVisible = true;
            screenWidth = width;
            ScreenHeight = height;
            menuWidth = screenWidth;
            menuHeight = ScreenHeight;
            menuName = name;
            menuSize = new RawRectangleF(0, 0, menuWidth, menuHeight);
            backgroundColor = new SolidColorBrush(D2DRT, new RawColor4(0.75f, 0.75f, 0.75f, 1.0f));
            startMenuTextFormat = tf;
        }

        public virtual string HandleInputs(State controllerState, GameStateData lgsd, int oldPacketNumber)
        {
            return "start";
        }

        public virtual void ShowMenu(RenderTarget D2DRT)
        {

        }
        
    }
}
