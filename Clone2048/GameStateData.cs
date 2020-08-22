using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Clone2048
{
    public enum MoveDirection { LEFT, RIGHT, UP, DOWN };

    public class GameStateData
    {
        public int[,] boardValues = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        public Stack<int[,]> lastTurnValues;
        public int[,] testTurnValues = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        public int score;
        public int oldScore;
        public bool isGameOver;
        public bool isGameWon;
        public int gridSize;
        public bool allowUndo;
        public bool undoStored;
        public int undosRemaining;

        public GameStateData()
        {
            score = 0;
            gridSize = 4;
            undosRemaining = 0;
            isGameOver = false;
            isGameWon = false;
            allowUndo = true;
            undoStored = false;
            lastTurnValues = new Stack<int[,]>();
        }

        public bool CheckForRemainingMoves()
        {
            bool remainingMoveFound = false;

            SaveBoardState();
            remainingMoveFound = ProcessMove(MoveDirection.DOWN);            
            RestoreBoardState();
            if (remainingMoveFound)
                return remainingMoveFound;

            SaveBoardState();
            remainingMoveFound = ProcessMove(MoveDirection.LEFT);
            RestoreBoardState();
            if (remainingMoveFound)
                return remainingMoveFound;

            SaveBoardState();
            remainingMoveFound = ProcessMove(MoveDirection.RIGHT);
            RestoreBoardState();
            if (remainingMoveFound)
                return remainingMoveFound;

            SaveBoardState();
            remainingMoveFound = ProcessMove(MoveDirection.UP);
            RestoreBoardState();

            return remainingMoveFound;
        }

        void SaveBoardState()
        {
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                    testTurnValues[i, j] = boardValues[i, j];
            oldScore =score;
        }

        void RestoreBoardState()
        {
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                    boardValues[i, j] = testTurnValues[i, j];
            score = oldScore;

        }


        public void NewGame()
        {
            boardValues = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };            
            lastTurnValues.Clear();
            undosRemaining = 0;
            score = 0;
            isGameOver = false;
            isGameWon = false;
            undoStored = false;
        }

        public bool ProcessMove(MoveDirection md)
        {
            bool moveMade = false;
            switch (md)
            {
                case MoveDirection.LEFT:
                    {
                        for (int row = 0; row < gridSize; row++)
                        {
                            //Shift non-zero values
                            for (int x = 0; x < gridSize; x++)
                            {
                                for (int z = 0; z < gridSize - 1; z++)
                                {
                                    if ((boardValues[z, row] == 0) && (boardValues[z + 1, row] != 0))
                                    {
                                        boardValues[z, row] = boardValues[z + 1, row];
                                        boardValues[z + 1, row] = 0;
                                        moveMade = true;
                                    }
                                }
                            }
                            //Combing neighboring matching values
                            for (int x = 0; x < gridSize - 1; x++)
                            {
                                if (boardValues[x, row] != 0 && boardValues[x, row] == boardValues[x + 1, row])
                                {
                                    int newPiece = 2 * boardValues[x, row];
                                    score += (int)Math.Log(newPiece, 2) * newPiece;
                                    boardValues[x, row] = newPiece;
                                    boardValues[x + 1, row] = 0;
                                    moveMade = true;
                                }
                            }
                            //Shift non-zero values again
                            for (int x = 0; x < gridSize; x++)
                            {
                                for (int z = 0; z < gridSize - 1; z++)
                                {
                                    if ((boardValues[z, row] == 0) && (boardValues[z + 1, row] != 0))
                                    {
                                        boardValues[z, row] = boardValues[z + 1, row];
                                        boardValues[z + 1, row] = 0;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case MoveDirection.RIGHT:
                    {
                        for (int row = 0; row < gridSize; row++)
                        {
                            //Shift non-zero values
                            for (int x = gridSize - 1; x >= 0; x--)
                            {
                                for (int z = gridSize - 1; z > 0; z--)
                                {
                                    if ((boardValues[z, row] == 0) && (boardValues[z - 1, row] != 0))
                                    {
                                        boardValues[z, row] = boardValues[z - 1, row];
                                        boardValues[z - 1, row] = 0;
                                        moveMade = true;
                                    }
                                }
                            }
                            //Combing neighboring matching values
                            for (int x = gridSize - 1; x > 0; x--)
                            {
                                if (boardValues[x, row] != 0 && boardValues[x, row] == boardValues[x - 1, row])
                                {
                                    int newPiece = 2 * boardValues[x, row];
                                    score += (int)Math.Log(newPiece, 2) * newPiece;
                                    boardValues[x, row] = newPiece;
                                    boardValues[x - 1, row] = 0;
                                    moveMade = true;
                                }
                            }
                            //Shift non-zero values
                            for (int x = gridSize - 1; x >= 0; x--)
                            {
                                for (int z = gridSize - 1; z > 0; z--)
                                {
                                    if ((boardValues[z, row] == 0) && (boardValues[z - 1, row] != 0))
                                    {
                                        boardValues[z, row] = boardValues[z - 1, row];
                                        boardValues[z - 1, row] = 0;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case MoveDirection.UP:
                    {
                        for (int col = 0; col < gridSize; col++)
                        {
                            //Shift non-zero values
                            for (int y = 0; y < gridSize; y++)
                            {
                                for (int z = 0; z < gridSize - 1; z++)
                                {
                                    if ((boardValues[col, z] == 0) && (boardValues[col, z + 1] != 0))
                                    {
                                        boardValues[col, z] = boardValues[col, z + 1];
                                        boardValues[col, z + 1] = 0;
                                        moveMade = true;
                                    }
                                }
                            }
                            //Combing neighboring matching values
                            for (int y = 0; y < gridSize - 1; y++)
                            {
                                if (boardValues[col, y] != 0 && boardValues[col, y] == boardValues[col, y + 1])
                                {
                                    int newPiece = 2 * boardValues[col, y];
                                    score += (int)Math.Log(newPiece, 2) * newPiece;
                                    boardValues[col, y] = newPiece;
                                    boardValues[col, y + 1] = 0;
                                    moveMade = true;
                                }
                            }
                            //Shift non-zero values again
                            for (int y = 0; y < gridSize; y++)
                            {
                                for (int z = 0; z < gridSize - 1; z++)
                                {
                                    if ((boardValues[col, z] == 0) && (boardValues[col, z + 1] != 0))
                                    {
                                        boardValues[col, z] = boardValues[col, z + 1];
                                        boardValues[col, z + 1] = 0;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case MoveDirection.DOWN:
                    {
                        for (int col = 0; col < gridSize; col++)
                        {
                            //Shift non-zero values
                            for (int y = gridSize; y >= 0; y--)
                            {
                                for (int z = gridSize - 1; z > 0; z--)
                                {
                                    if ((boardValues[col, z] == 0) && (boardValues[col, z - 1] != 0))
                                    {
                                        boardValues[col, z] = boardValues[col, z - 1];
                                        boardValues[col, z - 1] = 0;
                                        moveMade = true;
                                    }
                                }
                            }
                            //Combing neighboring matching values
                            for (int y = gridSize - 1; y > 0; y--)
                            {
                                if (boardValues[col, y] != 0 && boardValues[col, y] == boardValues[col, y - 1])
                                {
                                    int newPiece = 2 * boardValues[col, y];
                                    score += (int)Math.Log(newPiece, 2) * newPiece;
                                    boardValues[col, y] = newPiece;
                                    boardValues[col, y - 1] = 0;
                                    moveMade = true;
                                }
                            }
                            //Shift non-zero values again
                            for (int y = gridSize; y >= 0; y--)
                            {
                                for (int z = gridSize - 1; z > 0; z--)
                                {
                                    if ((boardValues[col, z] == 0) && (boardValues[col, z - 1] != 0))
                                    {
                                        boardValues[col, z] = boardValues[col, z - 1];
                                        boardValues[col, z - 1] = 0;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
            return moveMade;
        }
    }
}
