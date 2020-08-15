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


namespace Clone2048
{
    public class SettingsMenu : SDXMenu
    {
        public SettingsMenu(RenderTarget D2DRT, TextFormat tf, int width, int height, GameStateData lgsd) : base(D2DRT, tf, width, height)
        {
            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            int menuYOffset = 200;
            menuControls.Add(new SDXMenuButton(D2DRT, startMenuTextFormat, "Exit Settings",
                menuWidth / 2 - 90, menuHeight / 2 - menuYOffset + controlYSpacing * menuControls.Count, 180, 50));

        }


        public void ShowMenu(RenderTarget D2DRT)
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
                                this.isVisible = false;
                                nextMenu.isVisible = true;
                            }
                            break;
                    }
                }

            }

        }

    }
}
