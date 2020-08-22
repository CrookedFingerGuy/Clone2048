using Clone2048.SDXMenuControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.XInput;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace Clone2048
{
    public class SettingsMenu : SDXMenu
    {
        BoardSpot lbs;
        GameStateData lgsd;
        public SettingsMenu(RenderTarget D2DRT, TextFormat tf, int width, int height, GameStateData gsd, string name) : base(D2DRT, tf, width, height,name)
        {
            lgsd = gsd;
            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            int menuYOffset = 200;
            tf.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            menuControls.Add(new SDXMenuIntegerBox(D2DRT, tf, "Grid Size", menuWidth / 2 - 125, menuHeight / 2 - menuYOffset + controlYSpacing * menuControls.Count, 250, 50, 60, 50, 4));
            menuControls.Add(new SDXMenuToggle(D2DRT, tf, "Undos", menuWidth / 2 - 80, menuHeight / 2 - menuYOffset + controlYSpacing * menuControls.Count, 160, 50, 90, 50, 1));
            menuControls.Add(new SDXMenuButton(D2DRT, MenuTextFormat, "Exit Settings",
                menuWidth / 2 - 200, menuHeight / 2 - menuYOffset + controlYSpacing * menuControls.Count, 400, 50));
            activeControl = 0;
            menuControls[activeControl].isActive = true;

        }


        public override void ShowMenu(RenderTarget D2DRT)
        {
            foreach (SDXMenuControl s in menuControls)
            {
                s.DrawControl(D2DRT, MenuTextFormat);
            }
        }

        public override string HandleInputs(State controllerState, GameStateData gsd, int oldPacketNumber, KeyboardUpdate[] keyData)
        {
            lgsd = gsd;
            foreach (var state in keyData)
            {
                if (state.IsPressed)
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
                        case Key.Down:
                            {
                                if (activeControl < menuControls.Count - 1)
                                {
                                    menuControls[activeControl].isActive = false;
                                    activeControl++;
                                    menuControls[activeControl].isActive = true;
                                }
                            }
                            break;
                        case Key.Left:
                            {
                                if (activeControl == 0)
                                {
                                    menuControls[activeControl].value = 3;
                                }
                            }
                            break;
                        case Key.Right:
                            {
                                if (activeControl == 0)
                                {
                                    menuControls[activeControl].value = 4;
                                }
                            }
                            break;
                        case Key.Space:
                        case Key.Return:
                            {
                                switch (activeControl)
                                {
                                    case 1:
                                        {
                                            if (menuControls[1].value == 0)
                                            {
                                                menuControls[1].value = 1;
                                                lgsd.allowUndo = true;
                                            }
                                            else
                                            {
                                                menuControls[1].value = 0;
                                                lgsd.allowUndo = false;
                                            }
                                        }
                                        break;
                                    case 2:
                                        {
                                            lgsd.gridSize = menuControls[0].value;
                                            return "start";
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                }
            }


            if (controllerState.PacketNumber != oldPacketNumber)
            {
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.Start)
                {
                }

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
                        menuControls[activeControl].isActive = false;
                        activeControl++;
                        menuControls[activeControl].isActive = true;
                    }
                }
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.DPadLeft)
                {
                    if (activeControl == 0)
                    {
                        menuControls[activeControl].value = 3;
                    }
                }
                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.DPadRight)
                {
                    if (activeControl == 0)
                    {
                        menuControls[activeControl].value = 4;
                    }
                }


                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    switch (activeControl)
                    {
                        case 1:
                            {
                                if (menuControls[1].value == 0)
                                {
                                    menuControls[1].value = 1;
                                    lgsd.allowUndo = true;
                                }
                                else
                                {
                                    menuControls[1].value = 0;
                                    lgsd.allowUndo = false;
                                }
                            }
                            break;
                        case 2:
                            {
                                lgsd.gridSize = menuControls[0].value;
                                lbs.gridSize= menuControls[0].value;
                                //lgsd.NewGame();
                                return "start";
                            }
                            break;
                    }
                }

            }
            return "settings";
        }

    }
}
