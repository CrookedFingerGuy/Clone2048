using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clone2048.SDXMenuControls;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.XInput;
using SharpDX.DirectInput;


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
            int controlXSpacing = 25;
            int menuXOffset = 200;
            int menuYOffset = 200;
            TextFormat titleFont = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 72);
            titleFont.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            menuControls.Add(new SDXMenuTextInput(D2DRT, tf,"Enter Name" ,menuXOffset-100, screenHeight / 2 + 100, 300, 50,500,60,""));
            menuControls.Add(new SDXMenuButton(D2DRT, tf, "Done", menuXOffset+ controlXSpacing * 3 + 200, screenHeight / 2 + 125+controlYSpacing, 140, 60));
            menuControls.Add(new SDXMenuLabel(D2DRT, titleFont, "Game Over", 0, screenHeight / 2 - 200, screenWidth, 100));
            tf.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "You Made a New High Score: " + finalScore, 0, screenHeight / 2 - 110, screenWidth, 100));
            activeControl = 0;
            menuControls[activeControl].isActive = true;
            menuControls[2].isSelectable = false;
            menuControls[3].isSelectable = false;
            isVisible = false;
        }

        public override void ShowMenu(RenderTarget D2DRT)
        {
            (menuControls[menuControls.Count-1] as SDXMenuLabel).label = "You Made a New High Score: " + finalScore;
            foreach (SDXMenuControl s in menuControls)
            {
                s.DrawControl(D2DRT, MenuTextFormat);
            }
        }

        public override string HandleInputs(State controllerState, GameStateData lgsd, int oldPacketNumber, KeyboardUpdate[] keyData)
        {
            foreach (var state in keyData)
            {
                if (state.IsPressed && activeControl == 0)
                {
                    switch (state.Key)
                    {
                        case Key.A:
                        case Key.B:
                        case Key.C:
                        case Key.D:
                        case Key.E:
                        case Key.F:
                        case Key.G:
                        case Key.H:
                        case Key.I:
                        case Key.J:
                        case Key.K:
                        case Key.L:
                        case Key.M:
                        case Key.N:
                        case Key.O:
                        case Key.P:
                        case Key.Q:
                        case Key.R:
                        case Key.S:
                        case Key.T:
                        case Key.U:
                        case Key.V:
                        case Key.W:
                        case Key.X:
                        case Key.Y:
                        case Key.Z:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += state.Key.ToString();
                            }
                            break;
                        case Key.Back:
                            {
                                if ((menuControls[0] as SDXMenuTextInput).Input.Length > 0)
                                {
                                    (menuControls[0] as SDXMenuTextInput).Input =
                                    (menuControls[0] as SDXMenuTextInput).Input.Remove((menuControls[0] as SDXMenuTextInput).Input.Length - 1);
                                }
                            }
                            break;
                        case Key.Space:
                            {
                               (menuControls[0] as SDXMenuTextInput).Input += " ";
                            }
                            break;
                        case Key.D0:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "0";
                            }
                            break;
                        case Key.D1:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "1";
                            }
                            break;
                        case Key.D2:
                            { 
                                (menuControls[0] as SDXMenuTextInput).Input += "2";
                            }
                            break;
                        case Key.D3:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "3";
                            }
                            break;
                        case Key.D4:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "4";
                            }
                            break;
                        case Key.D5:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "5";
                            }
                            break;
                        case Key.D6:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "6";
                            }
                            break;
                        case Key.D7:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "7";
                            }
                            break;
                        case Key.D8:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "8";
                            }
                            break;
                        case Key.D9:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input += "9";
                            }
                            break;
                        case Key.Down:
                            {
                                if (activeControl < menuControls.Count - 1)
                                {
                                    if (menuControls[activeControl + 1].isSelectable)
                                    {
                                        menuControls[activeControl].isActive = false;
                                        activeControl++;
                                        menuControls[activeControl].isActive = true;
                                    }
                                }
                            }
                            break;
                    }
                }

                if (state.IsPressed && activeControl == 1)
                {
                    switch (state.Key)
                    {
                        case Key.Up:
                            {
                                if (activeControl > 0)
                                {
                                    menuControls[activeControl].isActive = false;
                                    activeControl--;
                                    menuControls[activeControl].isActive = true;
                                }
                            }
                            break;
                        case Key.Return:
                            {
                                newHighs.InsertNewHighScore(finalScore, (menuControls[0] as SDXMenuTextInput).Input);
                                return "viewhighscores";
                            }
                            break;

                    }
                }
            }
            if (controllerState.PacketNumber != oldPacketNumber)
            {
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.DPadUp)
                {
                    if (activeControl > 0)
                    {
                        menuControls[activeControl].isActive = false;
                        activeControl--;
                        menuControls[activeControl].isActive = true;
                    }
                }
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.DPadDown)
                {
                    if (activeControl < menuControls.Count - 1)
                    {
                        if (menuControls[activeControl + 1].isSelectable)
                        {
                            menuControls[activeControl].isActive = false;
                            activeControl++;
                            menuControls[activeControl].isActive = true;
                        }
                    }

                }


                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    switch (activeControl)
                    {
                        case 1:
                            {
                                (menuControls[0] as SDXMenuTextInput).Input.PadRight(12);
                                newHighs.InsertNewHighScore(finalScore, (menuControls[0] as SDXMenuTextInput).Input);
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
