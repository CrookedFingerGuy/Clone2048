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
using SharpDX.DirectInput;

namespace Clone2048
{
    public class AreYouSureBox :SDXMenu
    {
        public AreYouSureBox(RenderTarget D2DRT, TextFormat tf, int width, int height, string name):base(D2DRT, tf, width, height, name)
        {
            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            int menuYOffset = 200;
            tf.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            menuControls.Add(new SDXMenuButton(D2DRT, MenuTextFormat, "Yes",
                menuWidth / 2 - 50, menuHeight / 2 - menuYOffset + controlYSpacing * menuControls.Count, 100, 50));
            menuControls.Add(new SDXMenuButton(D2DRT, MenuTextFormat, "No",
                menuWidth / 2 - 50, menuHeight / 2 - menuYOffset + controlYSpacing * menuControls.Count, 100, 50));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "Are you sure you want to quit?", menuWidth/2-200,
                                            menuHeight/2-300, 400, 100));
            activeControl = 1;
            menuControls[activeControl].isActive = true;
            menuControls[2].isSelectable = false;
        }

        public override void ShowMenu(RenderTarget D2DRT)
        {
            foreach (SDXMenuControl s in menuControls)
            {
                s.DrawControl(D2DRT, MenuTextFormat);
            }
        }

        public override string HandleInputs(State controllerState, GameStateData lgsd, int oldPacketNumber, KeyboardUpdate[] keyData)
        {
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
                        case Key.Space:
                        case Key.Return:
                            {
                                switch (activeControl)
                                {
                                    case 0:
                                        {
                                            return "start";
                                        }
                                        break;
                                    case 1:
                                        {
                                            //Returning empty string sends the control to the game
                                            return "";
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
                        case 0:
                            {
                                return "start";
                            }
                            break;
                        case 1:
                            {
                                //Returning empty string sends the control to the game
                                return "";
                            }
                            break;
                    }
                }

            }
            return "areyousure";
        }

    }
}
