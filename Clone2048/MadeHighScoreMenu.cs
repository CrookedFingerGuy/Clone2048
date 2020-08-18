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
        HighScores newHighs;
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";

        public MadeHighScoreMenu(RenderTarget D2DRT, TextFormat tf, int width, int height, GameStateData lgsd, string name,HighScores hs) 
                                    : base(D2DRT, tf, width, height, name)
        {
            menuControls = new List<SDXMenuControl>();
            newHighs = hs;
            int controlYSpacing = 60;
            int controlXSpacing = 15;
            int menuXOffset = 200;
            int menuYOffset = 200;
            TextFormat titleFont = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 72);
            menuControls.Add(new SDXMenuLetterInput(D2DRT, tf, menuXOffset, ScreenHeight / 2 + 100, 60, 50));
            menuControls.Add(new SDXMenuLetterInput(D2DRT, tf, menuXOffset+controlXSpacing+60, ScreenHeight / 2 + 100, 60, 50));
            menuControls.Add(new SDXMenuLetterInput(D2DRT, tf, menuXOffset+controlXSpacing*2+120, ScreenHeight / 2 + 100, 60, 50));
            menuControls.Add(new SDXMenuButton(D2DRT, tf, "Done", menuXOffset+ controlXSpacing * 3 + 280, ScreenHeight / 2 + 100, 140, 60));
            menuControls.Add(new SDXMenuLabel(D2DRT, titleFont, "Game Over", screenWidth / 2 - 250, ScreenHeight / 2 - 200, 500, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "You Made a New High Score: " + finalScore, 10, ScreenHeight / 2 - 110, 1000, 100));
            activeControl = 0;
            menuControls[activeControl].isActive = true;
            isVisible = false;
        }

        public override void ShowMenu(RenderTarget D2DRT)
        {
            (menuControls[menuControls.Count-1] as SDXMenuLabel).label = "You Made a New High Score: " + finalScore;
            foreach (SDXMenuControl s in menuControls)
            {
                s.DrawControl(D2DRT, startMenuTextFormat);
            }
        }

        public override string HandleInputs(State controllerState, GameStateData lgsd, int oldPacketNumber)
        {
            if (controllerState.PacketNumber != oldPacketNumber)
            {
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.Start)
                {
                }

                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.DPadLeft)
                {
                    if (activeControl > 0)
                    {
                        menuControls[activeControl].isActive = false;
                        activeControl--;
                        menuControls[activeControl].isActive = true;
                    }
                }
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.DPadRight)
                {
                    if (activeControl < menuControls.Count - 1)
                    {
                        menuControls[activeControl].isActive = false;
                        activeControl++;
                        menuControls[activeControl].isActive = true;
                    }
                }
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.DPadUp)
                {
                    switch(activeControl)
                    {

                        case 0:
                        case 1:
                        case 2:
                            {
                               int currentLeterIndex = letters.IndexOf((menuControls[activeControl] as SDXMenuLetterInput).letter);
                               if (currentLeterIndex == letters.Length - 1)
                                    currentLeterIndex = 0;
                                else
                                    currentLeterIndex++;


                                (menuControls[activeControl] as SDXMenuLetterInput).letter= letters[currentLeterIndex].ToString();
                            }break;
                    }
                }
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.DPadDown)
                {
                    switch (activeControl)
                    {

                        case 0:
                        case 1:
                        case 2:
                            {
                                int currentLeterIndex = letters.IndexOf((menuControls[activeControl] as SDXMenuLetterInput).letter);
                                if (currentLeterIndex == 0)
                                    currentLeterIndex = letters.Length-1;
                                else
                                    currentLeterIndex--;

                                (menuControls[activeControl] as SDXMenuLetterInput).letter = letters[currentLeterIndex].ToString();
                            }
                            break;
                    }
                }


                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    switch (activeControl)
                    {
                        case 3:
                            {
                                newHighs.InsertNewHighScore(finalScore, (menuControls[0] as SDXMenuLetterInput).letter
                                                                + (menuControls[1] as SDXMenuLetterInput).letter
                                                                + (menuControls[2] as SDXMenuLetterInput).letter);
                                return "viewhighscores";
                            }
                            break;
                    }
                }

            }

            return "madehighscore";
        }

    }
}
