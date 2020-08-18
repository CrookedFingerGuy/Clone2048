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
    public class ViewHighScores:SDXMenu
    {
        public HighScores lHighs;
        string WorkingPath;
        public ViewHighScores(RenderTarget D2DRT, TextFormat tf, int width, int height, HighScores highs,string wp, string name) 
                                    : base(D2DRT, tf, width, height, name)
        {
            lHighs = highs;
            WorkingPath = wp;
            menuControls = new List<SDXMenuControl>();
            int controlYSpacing = 60;
            //int menuYOffset = 200;
            menuControls.Add(new SDXMenuButton(D2DRT, tf, "OK", screenWidth / 2 - 40, ScreenHeight -100, 80, 50));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "High Scores", screenWidth / 2 - 200, 20, 400, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "1)  ", 100, controlYSpacing * 1 + 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "2)  ", 100, controlYSpacing * 2+ 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "3)  ", 100, controlYSpacing * 3+ 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "4)  ", 100, controlYSpacing * 4+ 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "5)  ", 100, controlYSpacing * 5+ 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "6)  ", 100, controlYSpacing * 6+ 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "7)  ", 100, controlYSpacing * 7+ 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "8)  ", 100, controlYSpacing * 8+ 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "9)  ", 100, controlYSpacing * 9+ 10, 700, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "10) ", 100, controlYSpacing * 10 + 10, 700, 100));
            activeControl = 0;
            menuControls[activeControl].isActive = true;
            isVisible = false;
        }

        public override void ShowMenu(RenderTarget D2DRT)
        {
            for (int i = 2; i < lHighs.scores.Count + 2; i++)
            {
                if ((i - 1).ToString().Length == 1)
                    (menuControls[i] as SDXMenuLabel).label = (i - 1).ToString() + ")       " + lHighs.scores[i - 2].name + "    " + lHighs.scores[i - 2].score.ToString();
                else
                    (menuControls[i] as SDXMenuLabel).label = (i - 1).ToString() + ")    " + lHighs.scores[i - 2].name + "    " + lHighs.scores[i - 2].score.ToString();
            }

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
                    FileUtils.WriteToXmlFile<HighScores>(WorkingPath + @"\HighScores.sco", lHighs, false);
                    return "start";
                }
            }
            return "viewhighscores";
        }
    }
}
