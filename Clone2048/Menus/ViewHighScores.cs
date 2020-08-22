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
            int controlYSpacing = 45;
            //int menuYOffset = 200;
            MenuTextFormat = tf;
            tf.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            menuControls.Add(new SDXMenuButton(D2DRT, tf, "OK", screenWidth / 2 - 40, screenHeight - 100, 80, 50));
            menuControls.Add(new SDXMenuLabel(D2DRT, tf, "High Scores", screenWidth / 2 - 200, 20, 400, 100));
            TextFormat temptf = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 30);
            temptf.WordWrapping = SharpDX.DirectWrite.WordWrapping.NoWrap;
            temptf.TextAlignment = SharpDX.DirectWrite.TextAlignment.Leading;
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "1)  ", 350, controlYSpacing * 1 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "2)  ", 350, controlYSpacing * 2 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "3)  ", 350, controlYSpacing * 3 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "4)  ", 350, controlYSpacing * 4 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "5)  ", 350, controlYSpacing * 5 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "6)  ", 350, controlYSpacing * 6 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "7)  ", 350, controlYSpacing * 7 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "8)  ", 350, controlYSpacing * 8 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "9)  ", 350, controlYSpacing * 9 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "10) ", 350, controlYSpacing * 10 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 1 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 2 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 3 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 4 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 5 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 6 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 7 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 8 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 9 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 450, controlYSpacing * 10 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 1 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 2 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 3 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 4 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 5 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 6 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 7 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 8 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 9 + 75, 50, 100));
            menuControls.Add(new SDXMenuLabel(D2DRT, temptf, "", 800, controlYSpacing * 10 + 75, 50, 100));
            activeControl = 0;
            menuControls[activeControl].isActive = true;
            isVisible = false;
        }

        public override void ShowMenu(RenderTarget D2DRT)
        {
            for (int i = 2; i < lHighs.scores.Count + 2; i++)
            {
                (menuControls[i + 10] as SDXMenuLabel).label = lHighs.scores[i - 2].name;
                (menuControls[i + 20] as SDXMenuLabel).label = lHighs.scores[i - 2].score.ToString();
            }
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
                        case Key.Space:
                        case Key.Return:
                            {
                                switch (activeControl)
                                {
                                    case 0:
                                        {
                                            FileUtils.WriteToXmlFile<HighScores>(WorkingPath + @"\HighScores.sco", lHighs, false);
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
