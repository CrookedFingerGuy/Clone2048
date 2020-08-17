﻿using System;
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
        public int finalScore;

        public GameOverScreen(RenderTarget D2DRT, TextFormat tf, int width, int height, GameStateData lgsd,string name): base(D2DRT, tf, width, height,name)
        {
            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            int menuYOffset = 200;
            menuControls.Add(new SDXMenuButton(D2DRT, tf, "OK",screenWidth/2-100,ScreenHeight/2+50, 200, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "Game Over", screenWidth / 2-250, ScreenHeight / 2-200, 500, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "Your Score: "+finalScore, screenWidth / 2 - 350, ScreenHeight / 2 - 110, 700, 100));
            activeControl = 0;
            menuControls[activeControl].isActive = true;
            isVisible = false;
        }

        public override void ShowMenu(RenderTarget D2DRT)
        {
            (menuControls[2] as SDXMenuLabel).label = "Your Score: " + finalScore;
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
                    return "start";
                }
            }
            return "gameover";
        }
    }
}