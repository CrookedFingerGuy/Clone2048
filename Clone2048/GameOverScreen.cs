using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clone2048.SDXMenuControls;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.XInput;

namespace Clone2048
{
    public class GameOverScreen :SDXMenu
    {

        public GameOverScreen(RenderTarget D2DRT, TextFormat tf, int width, int height, GameStateData lgsd): base(D2DRT, tf, width, height)
        {
            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            int menuYOffset = 200;
            menuControls.Add(new SDXMenuButton(D2DRT, startMenuTextFormat, "Game Over",0,0, screenWidth, ScreenHeight));
            activeControl = 0;
            menuControls[activeControl].isActive = true;
            isVisible = false;
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
                    nextMenu.isVisible = true;
                    lgsd.NewGame();                    
                }
            }
        }
    }
}