using System;
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
        Keyboard keyboard;
        KeyboardUpdate[] keyData;
        State gamePadState;

        UserInputProcessor userInputProcessor;
        Stopwatch gameInputTimer;
        TextFormat pieceTextFormat;
        TextFormat pieceTextFormat4Digits;
        RawRectangleF TestTextArea;
        Bitmap Background;
        SolidColorBrush boardColor;
        SolidColorBrush boardSpotColor;
        int screenWidth = 1024;
        int screenHeight = 768;
        int boardWidth = 500;
        int boardHeight = 500;
        int topLeftX;
        int topLeftY;
        RawRectangleF boardRect;
        Board b;
        GameStateData gsd;
        BoardSpot bs;
        bool moveSuccess = true;
        bool undid = false;
        SolidColorBrush activePieceColor;
        SolidColorBrush scoreColor;
        TextFormat scoreTextFormat;
        StartMenu startMenu;
        GameOverScreen gameOverScreen;

        public RForm(string text) : base(text)
        {
            this.ClientSize = new System.Drawing.Size(screenWidth, screenHeight);

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
            d2dRenderTarget = new RenderTarget(d2dFactory, surface, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            solidColorBrush = new SolidColorBrush(d2dRenderTarget, Color.White);
            solidColorBrush.Color = Color.Purple;
            directInput = new DirectInput();
            keyboard = new Keyboard(directInput);
            keyboard.Properties.BufferSize = 128;
            keyboard.Acquire();
            userInputProcessor = new UserInputProcessor();
            TestTextArea = new SharpDX.Mathematics.Interop.RawRectangleF(10, 10, 400, 400);
            gameInputTimer = new Stopwatch();
            gameInputTimer.Start();

            boardColor = new SolidColorBrush(d2dRenderTarget, new RawColor4(1f, 0f, 0f, 1f));
            boardSpotColor = new SolidColorBrush(d2dRenderTarget, new RawColor4(0f, 0f, 0.2f, 1f));
            activePieceColor = new SolidColorBrush(d2dRenderTarget, new RawColor4(0f, 0.4f, 0f, 1f));
            topLeftX = screenWidth / 2 - boardWidth / 2;
            topLeftY = screenHeight / 2 - boardHeight / 2;
            boardRect = new RawRectangleF(topLeftX, topLeftY,
                topLeftX + boardWidth, topLeftY + boardHeight);

            gsd= new GameStateData();
            b = new Board();
            bs = new BoardSpot(0.1);
            bs.GenerateANewPiece(gsd);
            if (bs.Value == 4)
                gsd.score += 4;
            moveSuccess = false;
            pieceTextFormat = new SharpDX.DirectWrite.TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 36);
            pieceTextFormat.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            pieceTextFormat.ParagraphAlignment = ParagraphAlignment.Center;
            pieceTextFormat4Digits = new SharpDX.DirectWrite.TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 24);
            pieceTextFormat4Digits.TextAlignment = SharpDX.DirectWrite.TextAlignment.Center;
            pieceTextFormat4Digits.ParagraphAlignment = ParagraphAlignment.Center;
            scoreColor = new SolidColorBrush(d2dRenderTarget,new RawColor4(1f, 1f, 1f, 1f));
            scoreTextFormat = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 36);

            TextFormat startMenuText = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 36);
            startMenu = new StartMenu(d2dRenderTarget,startMenuText,screenWidth,screenHeight,gsd);
            startMenu.isVisible = true;

            TextFormat gameOverMenuText = new TextFormat(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Isolated), "Gill Sans", FontWeight.UltraBold, FontStyle.Normal, 72);
            gameOverScreen =new GameOverScreen(d2dRenderTarget, gameOverMenuText, screenWidth, screenHeight, gsd);

        }

        public void rLoop()
        {
            d2dRenderTarget.BeginDraw();
            d2dRenderTarget.Clear(Color.Black);

            if (gameOverScreen.isVisible)
                gameOverScreen.ShowGameOverScreen(d2dRenderTarget);
            else if (startMenu.isVisible)
                startMenu.ShowStartMenu(d2dRenderTarget);
            else
            {
                DrawGameBoard(d2dRenderTarget);
                d2dRenderTarget.DrawText(gsd.score.ToString(), scoreTextFormat, new RawRectangleF(20f, 20f, 300f, 100f), scoreColor);
            }

            if (gameInputTimer.ElapsedMilliseconds >= 25)
            {
                userInputProcessor.oldPacketNumber = gamePadState.PacketNumber;
                gamePadState = userInputProcessor.GetGamePadState();
                gameInputTimer.Restart();
                if (gameOverScreen.isVisible)
                {
                    gameOverScreen.HandleGamePadInputs(gamePadState, gsd, userInputProcessor.oldPacketNumber);
                }
                else if (startMenu.isVisible)
                {
                    startMenu.HandleGamePadInputs(gamePadState, gsd, userInputProcessor.oldPacketNumber);
                }
                else
                {
                    HandleGameInputs();
                }
            }

            d2dRenderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
            //Thread.Sleep(100);
        }

        public void HandleGameInputs()
        {
            if (gamePadState.PacketNumber != userInputProcessor.oldPacketNumber)
            {
                if (moveSuccess)
                {
                    bs.GenerateANewPiece(gsd);
                    if (bs.Value == 4)
                        gsd.score += 4;
                    moveSuccess = false;
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            gsd.lastTurnValues[i, j] = gsd.boardValues[i, j];
                }
                if (bs.Value != 0)
                {
                    gsd.boardValues[bs.X, bs.Y] = bs.Value;
                    if (bs.Value == 4)
                        gsd.score += 4;
                }
                DrawGameBoard(d2dRenderTarget);
                if (!gsd.isGameOver)
                {

                    switch (gamePadState.Gamepad.Buttons)
                    {
                        case GamepadButtonFlags.DPadLeft:
                            {
                                moveSuccess = gsd.ProcessMove(MoveDirection.LEFT);
                            }
                            break;
                        case GamepadButtonFlags.DPadRight:
                            {
                                moveSuccess = gsd.ProcessMove(MoveDirection.RIGHT);
                            }
                            break;
                        case GamepadButtonFlags.DPadUp:
                            {
                                moveSuccess = gsd.ProcessMove(MoveDirection.UP);
                            }
                            break;
                        case GamepadButtonFlags.DPadDown:
                            {
                                moveSuccess = gsd.ProcessMove(MoveDirection.DOWN);
                            }
                            break;
                        case GamepadButtonFlags.Start:
                            {
                                //gsd.isGameOver = true;
                                startMenu.isVisible = true;
                            }
                            break;
                        case GamepadButtonFlags.A:
                            {
                                //Undo
                                for (int i = 0; i < 4; i++)
                                    for (int j = 0; j < 4; j++)
                                        gsd.boardValues[i, j] = gsd.lastTurnValues[i, j];
                                moveSuccess = false;
                                undid = true;
                            }
                            break;
                        case GamepadButtonFlags.Y:
                            {
                                gameOverScreen.isVisible = true;
                                startMenu.isVisible = true;
                            }
                            break;
                    }
                }
            }
        }

        public void DrawGameBoard(RenderTarget D2DRT)
        {
            D2DRT.FillRectangle(boardRect,boardColor);
            for(int x=0;x<4;x++)
            {
                for(int y=0; y<4;y++)
                {
                    if(gsd.boardValues[x,y]==0)
                    {
                        RawRectangleF spot = new RawRectangleF(topLeftX + (boardWidth / 4) * x + 10, topLeftY + (boardHeight / 4) * y + 10,
                            topLeftX + (boardWidth / 4) * x + (boardWidth / 4 - 10), topLeftY + (boardHeight / 4 * y) + (boardHeight / 4 - 10));
                        RoundedRectangle rSpot = new RoundedRectangle();
                        rSpot.Rect = spot;
                        rSpot.RadiusX = 15;
                        rSpot.RadiusY = 15;
                        D2DRT.FillRoundedRectangle(rSpot, activePieceColor);
                    }
                    else
                    {
                        RawRectangleF spot = new RawRectangleF(topLeftX + (boardWidth / 4) * x + 10, topLeftY + (boardHeight / 4) * y + 10,
                            topLeftX + (boardWidth / 4) * x + (boardWidth / 4 - 10), topLeftY + (boardHeight / 4 * y) + (boardHeight / 4 - 10));
                        boardSpotColor = new SolidColorBrush(d2dRenderTarget, new RawColor4(0f,0f, (float)(Math.Log(gsd.boardValues[x,y],2)/16+0.2),1f));
                        RoundedRectangle rSpot = new RoundedRectangle();
                        rSpot.Rect = spot;
                        rSpot.RadiusX = 15;
                        rSpot.RadiusY = 15;
                        D2DRT.FillRoundedRectangle(rSpot, boardSpotColor);
                        if(gsd.boardValues[x,y].ToString().Length<4)
                            D2DRT.DrawText(gsd.boardValues[x, y].ToString(), pieceTextFormat, spot, boardColor);
                        else
                            D2DRT.DrawText(gsd.boardValues[x, y].ToString(), pieceTextFormat4Digits, spot, boardColor);
                    }
                }
            }
        }

        Bitmap LoadBackground(RenderTarget renderTarget, string file)
        {
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
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

    }

}
