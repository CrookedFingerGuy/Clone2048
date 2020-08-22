using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.DirectInput;
using SharpDX.XInput;


using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.DXGI.Factory;
using System.Windows.Forms;
using Clone2048.SDXMenuControls;

namespace Clone2048
{
    public class RForm : RenderForm
    {
        
        SwapChainDescription desc;
        Device device;
        SwapChain swapChain;
        SharpDX.Direct2D1.Factory d2dFactory;
        Factory factory;
        Texture2D backBuffer;
        RenderTargetView renderView;
        Surface surface;
        RenderTarget d2dRenderTarget;
        SolidColorBrush solidColorBrush;
        DirectInput directInput;
        public Keyboard keyboard;
        KeyboardUpdate[] keyData;
        KeyboardState keys;
        State gamePadState;

        UserInputProcessor userInputProcessor;
        Stopwatch gameInputTimer;
        int screenWidth = 1280;
        int screenHeight = 720;
        int boardWidth = 500;
        int boardHeight = 500;
        int topLeftX;
        int topLeftY;

        TextFormat pieceTextFormat;
        TextFormat pieceTextFormat4Digits;
        TextFormat scoreTextFormat;
        TextFormat viewHighScoresTF;
        SolidColorBrush boardColor;
        SolidColorBrush boardSpotColor;
        SolidColorBrush boardValueColor;
        List<SolidColorBrush> activePieceColors;
        SolidColorBrush scoreColor;
        RawRectangleF boardRect;
        GameStateData gsd;
        int gridSize;
        bool moveSuccess = true;

        StartMenu startMenu;
        GameOverScreen gameOverScreen;
        MadeHighScoreMenu madeHighScoreMenu;
        SettingsMenu settingsMenu;
        AreYouSureBox areYouSureBox;
        ViewHighScores viewHighScores;
        SDXSceneFlow sceneFlow;
        string currentMenu;

        HighScores highs;
        string WorkingPath;


        public RForm(string text) : base(text)
        {
            this.ClientSize = new System.Drawing.Size(screenWidth, screenHeight);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RForm));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            //SharpDX variable initialization
            desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(this.ClientSize.Width, this.ClientSize.Height, new Rational(144, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = this.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new SharpDX.Direct3D.FeatureLevel[] { SharpDX.Direct3D.FeatureLevel.Level_10_0 }, desc, out device, out swapChain);
            d2dFactory = new SharpDX.Direct2D1.Factory();
            factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(this.Handle, WindowAssociationFlags.IgnoreAll);
            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderView = new RenderTargetView(device, backBuffer);
            surface = backBuffer.QueryInterface<Surface>();
            d2dRenderTarget = new RenderTarget(d2dFactory,surface,new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown,AlphaMode.Premultiplied)));
            solidColorBrush = new SolidColorBrush(d2dRenderTarget, Color.White);
            directInput = new DirectInput();
            keyboard = new Keyboard(directInput);
            keyboard.Properties.BufferSize = 128;
            keyboard.Acquire();
            keys = new KeyboardState();
            userInputProcessor = new UserInputProcessor();
            gameInputTimer = new Stopwatch();
            gameInputTimer.Start();
            
            //Colors            
            boardColor = new SolidColorBrush(d2dRenderTarget, new RawColor4(0.9215686f, 0.4235294f, 0.07058824f, 1.0f));
            boardSpotColor = new SolidColorBrush(d2dRenderTarget, new RawColor4(0.1882353f, 0.2431373f, 0.2862745f, 1.0f));
            boardValueColor = new SolidColorBrush(d2dRenderTarget, Color.Black);
            activePieceColors = new List<SolidColorBrush>();
            activePieceColors.Add(new SolidColorBrush(d2dRenderTarget, new RawColor4(0.3960784f, 0.3960784f, 0.372549f, 1.0f)));
            activePieceColors.Add(new SolidColorBrush(d2dRenderTarget, new RawColor4(0.7411765f, 0.5843138f, 0.4f, 1.0f)));
            activePieceColors.Add(new SolidColorBrush(d2dRenderTarget, new RawColor4(0.9176471f, 0.6784314f, 0.3921569f, 1.0f)));
            activePieceColors.Add(new SolidColorBrush(d2dRenderTarget, new RawColor4(0.972549f, 0.627451f, 0.1137255f, 1.0f)));
            activePieceColors.Add(new SolidColorBrush(d2dRenderTarget, new RawColor4(0.9137255f, 0.7411765f, 0.2235294f, 1.0f)));
            activePieceColors.Add(new SolidColorBrush(d2dRenderTarget, new RawColor4(0.427451f, 0.7843137f, 0.654902f, 1.0f)));
            activePieceColors.Add(new SolidColorBrush(d2dRenderTarget, new RawColor4(0.6627451f, 0.7686275f, 0.3411765f, 1.0f)));

            //Gameboard drawing initialization
            topLeftX = screenWidth / 2 - boardWidth / 2;
            topLeftY = screenHeight / 2 - boardHeight / 2;
            boardRect = new RawRectangleF(topLeftX, topLeftY,topLeftX + boardWidth, topLeftY + boardHeight);
            pieceTextFormat = new SharpDX.DirectWrite.TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 36);
            pieceTextFormat.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            pieceTextFormat.ParagraphAlignment = ParagraphAlignment.Center;
            pieceTextFormat4Digits = new SharpDX.DirectWrite.TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 24);
            pieceTextFormat4Digits.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            pieceTextFormat4Digits.ParagraphAlignment = ParagraphAlignment.Center;
            scoreColor = new SolidColorBrush(d2dRenderTarget, new RawColor4(1f, 1f, 1f, 1f));
            scoreTextFormat = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 36);


            //Game variable initialization
            gsd = new GameStateData();
            gridSize = 4;
            gsd.gridSize = gridSize;
            gsd.bs.gridSize = gridSize;
            gsd.bs.GenerateANewPiece(gsd.boardValues);
            if (gsd.bs.Value == 4)
                gsd.score += 4;
            moveSuccess = false;


            //Menu Initialization and font setup
            TextFormat startMenuText = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 36);
            startMenu = new StartMenu(d2dRenderTarget,startMenuText,screenWidth,screenHeight,"start");  
            TextFormat gameOverMenuText = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 72);
            gameOverScreen =new GameOverScreen(d2dRenderTarget, gameOverMenuText, screenWidth, screenHeight, gsd,"gameover");
            TextFormat settingsMenuText = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 36);
            settingsMenu = new SettingsMenu(d2dRenderTarget, settingsMenuText, screenWidth, screenHeight, gsd, "settings");
            TextFormat sureMenuText = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 36);
            areYouSureBox = new AreYouSureBox(d2dRenderTarget, sureMenuText, screenWidth, screenHeight, "areyousure");

            //Load high scores and initialize the high score related menus
            WorkingPath = Directory.GetCurrentDirectory();
            if (File.Exists(WorkingPath + @"\HighScores.sco"))
            {
                highs = FileUtils.ReadFromXmlFile<HighScores>(WorkingPath + @"\HighScores.sco");
            }
            else
            {
                highs = new HighScores();
            }
            viewHighScoresTF = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 30);
            viewHighScoresTF.WordWrapping = SharpDX.DirectWrite.WordWrapping.NoWrap;
            viewHighScoresTF.TextAlignment = SharpDX.DirectWrite.TextAlignment.Leading;
            viewHighScores = new ViewHighScores(d2dRenderTarget, viewHighScoresTF, screenWidth, screenHeight,highs,WorkingPath , "viewhighscores");
            madeHighScoreMenu = new MadeHighScoreMenu(d2dRenderTarget, settingsMenuText, screenWidth, screenHeight, gsd, "madehighscore", highs);

            //Link all the menus together
            sceneFlow = new SDXSceneFlow();
            sceneFlow.menuList.Add(startMenu);
            sceneFlow.menuList.Add(gameOverScreen);
            sceneFlow.menuList.Add(settingsMenu);
            sceneFlow.menuList.Add(areYouSureBox);
            sceneFlow.menuList.Add(viewHighScores);
            sceneFlow.menuList.Add(madeHighScoreMenu);
            currentMenu = "start";
            sceneFlow.activeMenu = sceneFlow.NextMenu(currentMenu);
        }

        public void rLoop()
        {
            d2dRenderTarget.BeginDraw();
            d2dRenderTarget.Clear(Color.Black);


            gridSize = gsd.gridSize;
            gsd.bs.gridSize = gridSize;

            //If currentMenu is "" then the game shown and has control of the input.
            sceneFlow.NextMenu(currentMenu);
            if(currentMenu!="")
                sceneFlow.menuList[sceneFlow.activeMenu].ShowMenu(d2dRenderTarget);
            else
            {
                DrawGameBoard(d2dRenderTarget);
            }

            if (gameInputTimer.ElapsedMilliseconds >= 25)
            {
                userInputProcessor.oldPacketNumber = gamePadState.PacketNumber;
                gamePadState = userInputProcessor.GetGamePadState();
                keyboard.Poll();
                keyData = keyboard.GetBufferedData();
                gameInputTimer.Restart();

                if (currentMenu != "")
                    currentMenu = sceneFlow.menuList[sceneFlow.activeMenu].HandleInputs(gamePadState, gsd, userInputProcessor.oldPacketNumber,keyData);
                else
                    HandleGameInputs();
            }

            d2dRenderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
        }

        public void HandleGameInputs()
        {
            if (!gsd.isGameOver)
            {
                if (moveSuccess)
                {
                    gsd.bs.GenerateANewPiece(gsd.boardValues);
                    if (gsd.bs.Value == 4)
                        gsd.score += 4;
                    moveSuccess = false;
                    if(gsd.undosRemaining<5)
                        gsd.undosRemaining++;
                }
                if (gsd.bs.Value != 0)
                {
                    gsd.boardValues[gsd.bs.X, gsd.bs.Y] = gsd.bs.Value;
                }
                DrawGameBoard(d2dRenderTarget);
                gsd.isGameOver = !gsd.CheckForRemainingMoves();

                foreach (var state in keyData)
                {
                    if (state.IsPressed)
                    {
                        switch (state.Key)
                        {
                            case Key.Left:
                                {
                                    gsd.SaveUndoState();
                                    moveSuccess = gsd.ProcessMove(MoveDirection.LEFT);
                                }
                                break;
                            case Key.Right:
                                {
                                    gsd.SaveUndoState();
                                    moveSuccess = gsd.ProcessMove(MoveDirection.RIGHT);
                                }
                                break;
                            case Key.Up:
                                {
                                    gsd.SaveUndoState();
                                    moveSuccess = gsd.ProcessMove(MoveDirection.UP);
                                }
                                break;
                            case Key.Down:
                                {
                                    gsd.SaveUndoState();
                                    moveSuccess = gsd.ProcessMove(MoveDirection.DOWN);
                                }
                                break;
                            case Key.Escape:
                                {
                                    currentMenu = "areyousure";
                                }
                                break;
                            case Key.Space:
                                {
                                    if (gsd.allowUndo && gsd.undoStored && gsd.undosRemaining > 0)
                                    {
                                        gsd.RestoreUndoState();
                                        gsd.undosRemaining--;
                                    }
                                    moveSuccess = false;

                                }
                                break;
                        }
                    }
                }

                if (gamePadState.PacketNumber != userInputProcessor.oldPacketNumber)
                {
                    switch (gamePadState.Gamepad.Buttons)
                    {
                        case GamepadButtonFlags.DPadLeft:
                            {
                                gsd.SaveUndoState();
                                moveSuccess = gsd.ProcessMove(MoveDirection.LEFT);
                            }
                            break;
                        case GamepadButtonFlags.DPadRight:
                            {
                                gsd.SaveUndoState();
                                moveSuccess = gsd.ProcessMove(MoveDirection.RIGHT);
                            }
                            break;
                        case GamepadButtonFlags.DPadUp:
                            {
                                gsd.SaveUndoState();
                                moveSuccess = gsd.ProcessMove(MoveDirection.UP);
                            }
                            break;
                        case GamepadButtonFlags.DPadDown:
                            {
                                gsd.SaveUndoState();
                                moveSuccess = gsd.ProcessMove(MoveDirection.DOWN);
                            }
                            break;
                        case GamepadButtonFlags.Start:
                            {
                                currentMenu = "areyousure";
                            }
                            break;
                        case GamepadButtonFlags.A:
                            {
                                //Undo
                                if (gsd.allowUndo && gsd.undoStored && gsd.undosRemaining > 0)
                                {
                                    gsd.RestoreUndoState();
                                    gsd.undosRemaining--;
                                }
                                moveSuccess = false;
                            }
                            break;
                        case GamepadButtonFlags.Y:
                            {
                                if (highs.CheckIfNewHighScore(gsd.score))
                                {
                                    madeHighScoreMenu.finalScore = gsd.score;
                                    currentMenu = "madehighscore";
                                }
                                else
                                {
                                    gameOverScreen.finalScore = gsd.score;
                                    currentMenu = "gameover";
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                if (highs.CheckIfNewHighScore(gsd.score))
                {
                    madeHighScoreMenu.finalScore = gsd.score;
                    currentMenu = "madehighscore";
                }
                else
                {
                    gameOverScreen.finalScore = gsd.score;
                    currentMenu = "gameover";
                }
            }                        
        }

        public void DrawGameBoard(RenderTarget D2DRT)
        {
            RoundedRectangle rBoardRect = new RoundedRectangle();
            rBoardRect.Rect = boardRect;
            rBoardRect.RadiusX = 15;
            rBoardRect.RadiusY = 15;
            D2DRT.FillRoundedRectangle(rBoardRect, boardColor);
            for(int x=0;x< gridSize; x++)
            {
                for(int y=0; y< gridSize; y++)
                {
                    if(gsd.boardValues[x,y]==0)
                    {
                        RawRectangleF spot = new RawRectangleF(topLeftX + (boardWidth / gridSize) * x + 10, topLeftY + (boardHeight / gridSize) * y + 10,
                            topLeftX + (boardWidth / gridSize) * x + (boardWidth / gridSize - 10), topLeftY + (boardHeight / gridSize * y) + (boardHeight / gridSize - 10));
                        RoundedRectangle rSpot = new RoundedRectangle();
                        rSpot.Rect = spot;
                        rSpot.RadiusX = 15;
                        rSpot.RadiusY = 15;
                        D2DRT.FillRoundedRectangle(rSpot, boardSpotColor);
                    }
                    else
                    {
                        RawRectangleF spot = new RawRectangleF(topLeftX + (boardWidth / gridSize) * x + 10, topLeftY + (boardHeight / gridSize) * y + 10,
                            topLeftX + (boardWidth / gridSize) * x + (boardWidth / gridSize - 10), topLeftY + (boardHeight / gridSize * y) + (boardHeight / gridSize - 10));
                        RoundedRectangle rSpot = new RoundedRectangle();
                        rSpot.Rect = spot;
                        rSpot.RadiusX = 15;
                        rSpot.RadiusY = 15;
                        D2DRT.FillRoundedRectangle(rSpot, activePieceColors[(int)(Math.Log(gsd.boardValues[x, y], 2)%activePieceColors.Count)]);
                        if (gsd.boardValues[x, y].ToString().Length < gridSize)
                            D2DRT.DrawText(gsd.boardValues[x, y].ToString(), pieceTextFormat, spot, boardValueColor);
                        else
                            D2DRT.DrawText(gsd.boardValues[x, y].ToString(), pieceTextFormat4Digits, spot, boardValueColor);                        
                    }
                }
            }
            d2dRenderTarget.DrawText("Score:", scoreTextFormat, new RawRectangleF(20f, 20f, 300f, 100f), scoreColor);
            d2dRenderTarget.DrawText(gsd.score.ToString(), scoreTextFormat, new RawRectangleF(20f, 56f, 300f, 100f), scoreColor);
            if (gsd.allowUndo)
            {
                d2dRenderTarget.DrawText("Undos:", scoreTextFormat, new RawRectangleF(20f, 118f, 300f, 140f), scoreColor);
                d2dRenderTarget.DrawText(gsd.undosRemaining.ToString(), scoreTextFormat, new RawRectangleF(20f, 154f, 300f, 200f), scoreColor);
                d2dRenderTarget.DrawRectangle(new RawRectangleF(10f, 10f, 230f, 200f), scoreColor, 4f);
            }
            else
            {
                d2dRenderTarget.DrawRectangle(new RawRectangleF(10f, 10f, 230f, 102f), scoreColor, 4f);
            }
        }

        ~RForm()
        {
            keyboard.Unacquire();
            keyboard.Dispose();
            directInput.Dispose();
            renderView.Dispose();
            backBuffer.Dispose();
            device.ImmediateContext.ClearState();
            device.ImmediateContext.Flush();
            device.Dispose();
            device.Dispose();
            swapChain.Dispose();
            factory.Dispose();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RForm));
            this.SuspendLayout();
            // 
            // RForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RForm";
            this.Text = "2048";
            this.ResumeLayout(false);

        }
    }

}
