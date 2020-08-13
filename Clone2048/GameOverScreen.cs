using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.XInput;

namespace Clone2048
{
    public class GameOverScreen
    {
        int screenWidth;
        int ScreenHeight;
        int menuWidth;
        int menuHeight;
        public bool isVisible;
        SolidColorBrush backgroundColor;
        RawRectangleF menuSize;
        List<SDXMenuControl> menuControls;
        TextFormat startMenuTextFormat;
        int activeControl;

        public GameOverScreen(RenderTarget D2DRT, TextFormat tf, int width, int height, GameStateData lgsd)
        {
            isVisible = false;
            screenWidth = width;
            ScreenHeight = height;
            menuWidth = screenWidth;
            menuHeight = ScreenHeight;
            menuSize = new RawRectangleF(0, 0, menuWidth, menuHeight);
            backgroundColor = new SolidColorBrush(D2DRT, new RawColor4(0.75f, 0.75f, 0.75f, 1.0f));
            startMenuTextFormat = tf;

            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            int menuYOffset = 200;
            menuControls.Add(new SDXMenuButton(D2DRT, startMenuTextFormat, "Game Over",0,0, screenWidth, ScreenHeight));
            activeControl = 0;
            menuControls[activeControl].isActive = true;
        }

        public void ShowGameOverScreen(RenderTarget D2DRT)
        {
            foreach (SDXMenuControl s in menuControls)
            {
                s.DrawControl(D2DRT, startMenuTextFormat);
            }
        }

        public void HandleGamePadInputs(State controllerState, GameStateData lgsd, int oldPacketNumber)
        {
            if (controllerState.PacketNumber != oldPacketNumber)
            {

                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    this.isVisible = false;
                    lgsd.NewGame();
                }
            }
        }
    }
}