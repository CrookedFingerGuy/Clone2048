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
    public class AreYouSureBox :SDXMenu
    {
        public AreYouSureBox(RenderTarget D2DRT, TextFormat tf, int width, int height, GameStateData gsd, BoardSpot bs, string name):base(D2DRT, tf, width, height, name)
        {
            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            int menuYOffset = 200;
            menuControls.Add(new SDXMenuButton(D2DRT, startMenuTextFormat, "Yes",
                menuWidth / 2 - 50, menuHeight / 2 - menuYOffset + controlYSpacing * menuControls.Count, 100, 50));
            menuControls.Add(new SDXMenuButton(D2DRT, startMenuTextFormat, "No",
                menuWidth / 2 - 50, menuHeight / 2 - menuYOffset + controlYSpacing * menuControls.Count, 100, 50));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "Are you sure you want to quit?", menuWidth/2-200,
                                            menuHeight/2-300, 400, 100));
            activeControl = 1;
            menuControls[activeControl].isActive = true;

        }

        public override void ShowMenu(RenderTarget D2DRT)
        {
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

                if (controllerState.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    switch (activeControl)
                    {
                        case 0:
                            {
                                //this.isVisible = false;
                                //lgsd.NewGame();
                                return "start";
                            }
                            break;
                        case 1:
                            {
                                //this.isVisible = false;
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
