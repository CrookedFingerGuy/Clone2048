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


namespace Clone2048.SDXMenuControls
{
    class MadeHighScoreMenu: SDXMenu
    {
        public int finalScore;

        public MadeHighScoreMenu(RenderTarget D2DRT, TextFormat tf, int width, int height, GameStateData lgsd, string name) : base(D2DRT, tf, width, height, name)
        {
            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            int menuYOffset = 200;
            TextFormat titleFont = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 72);
            menuControls.Add(new SDXMenuButton(D2DRT, titleFont, "OK", screenWidth / 2 - 100, ScreenHeight / 2 + 100, 200, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, titleFont, "Game Over", screenWidth / 2 - 250, ScreenHeight / 2 - 200, 500, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, titleFont, "Your Made a New High Score: " + finalScore, 10, ScreenHeight / 2 - 110, 1000, 100));
            activeControl = 0;
            menuControls[activeControl].isActive = true;
            isVisible = false;
        }

        public override void ShowMenu(RenderTarget D2DRT)
        {
            (menuControls[2] as SDXMenuLabel).label = "Your Made a New High Score: " + finalScore;
            foreach (SDXMenuControl s in menuControls)
            {
                s.DrawControl(D2DRT, startMenuTextFormat);
            }
        }

        public override string HandleInputs(State controllerState, GameStateData lgsd, int oldPacketNumber)
        {
            if (controllerState.PacketNumber != oldPacketNumber)
            {

                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    return "viewhighscores";
                }
            }
            return "madehighscore";
        }

    }
}
