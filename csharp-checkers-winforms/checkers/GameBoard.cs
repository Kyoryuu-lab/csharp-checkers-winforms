using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace checkers
{
    public class GameBoard
    {
        public enum GameState
        {
            InProgress,
            WhiteWins,
            BlackWins
        }

        public enum CheckerType
        {
            None,
            White,
            Black,
            WhiteKing,
            BlackKing
        }

        public const int cellSize = 50;
        public CheckerType[,] board = new CheckerType[8, 8];
        private Form1 f;

        public GameBoard(Form1 form)
        {
            f = form;
        }

        public GameState CurrentGameState { get; private set; }

        public void CheckForWinner()
        {
            bool whitePiecesRemaining = false;
            bool blackPiecesRemaining = false;

            foreach (CheckerType piece in board)
            {
                if (piece == CheckerType.White || piece == CheckerType.WhiteKing)
                {
                    whitePiecesRemaining = true;
                }
                else if (piece == CheckerType.Black || piece == CheckerType.BlackKing)
                {
                    blackPiecesRemaining = true;
                }
            }

            if (!whitePiecesRemaining)
            {
                CurrentGameState = GameState.BlackWins;
                MessageBox.Show("Гравець з чорними шашками переміг!");
            }
            else if (!blackPiecesRemaining)
            {
                CurrentGameState = GameState.WhiteWins;
                MessageBox.Show("Гравець з білими шашками переміг!");
            }
        }

        public void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        if (i < 3)
                        {
                            board[i, j] = CheckerType.White;
                        }
                        else if (i > 4)
                        {
                            board[i, j] = CheckerType.Black;
                        }
                        else
                        {
                            board[i, j] = CheckerType.None;
                        }
                    }
                    else
                    {
                        board[i, j] = CheckerType.None;
                    }
                }
            }
        }

        public void Init()
        {
            InitializeBoard();
            CreateBoardUI();

        }

        public void CreateBoardUI()
        {
            int boardSize = 8;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Button button = new Button();
                    button.Size = new Size(cellSize, cellSize);
                    button.Location = new Point(j * cellSize, i * cellSize);
                    button.Tag = new BoardPosition(i, j);

                    bool isDarkCell = (i + j) % 2 == 1;

                    // Встановлюємо колір клітинки
                    button.BackColor = isDarkCell ? Color.Gray : Color.White;

                    // Встановлюємо зображення відповідно до фішки
                    switch (board[i, j])
                    {
                        case CheckerType.White:
                            button.Image = new Bitmap(new Bitmap(@"C:\Users\Zylietta\Downloads\Telegram Desktop\checkers (4)\checkers\white.png"), new Size(cellSize - 10, cellSize - 10));
                            break;
                        case CheckerType.Black:
                            button.Image = new Bitmap(new Bitmap(@"C:\Users\Zylietta\Downloads\Telegram Desktop\checkers (4)\checkers\black.png"), new Size(cellSize - 10, cellSize - 10));
                            break;
                        case CheckerType.WhiteKing:
                            button.Image = new Bitmap(new Bitmap(@"C:\Users\Zylietta\Downloads\Telegram Desktop\checkers (4)\checkers\white_king.png"), new Size(cellSize - 10, cellSize - 10));
                            break;
                        case CheckerType.BlackKing:
                            button.Image = new Bitmap(new Bitmap(@"C:\Users\Zylietta\Downloads\Telegram Desktop\checkers (4)\checkers\black_king.png"), new Size(cellSize - 10, cellSize - 10));
                            break;
                    }

                    // Додаємо обробник події для клікання на кнопку
                    button.Click += Button_Click;

                   

                    // Додаємо кнопку до форми
                    f.Controls.Add(button);
                }
            }
        }


        public void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                BoardPosition clickedPosition = (BoardPosition)clickedButton.Tag;
                f.HandleButtonClick(clickedPosition);
            }
        }

        public void UpdateBoardUI()
        {
            foreach (Control control in f.Controls)
            {
                if (control is Button button && button.Tag is BoardPosition position)
                {
                    CheckerType piece = board[position.Row, position.Column];

                    if (piece == CheckerType.White)
                    {
                        button.Image = new Bitmap(new Bitmap(@"C:\Users\Zylietta\Downloads\Telegram Desktop\checkers (4)\checkers\white.png"), new Size(cellSize - 10, cellSize - 10));
                    }
                    else if (piece == CheckerType.Black)
                    {
                        button.Image = new Bitmap(new Bitmap(@"C:\Users\Zylietta\Downloads\Telegram Desktop\checkers (4)\checkers\black.png"), new Size(cellSize - 10, cellSize - 10));
                    }
                    else if (piece == CheckerType.WhiteKing )
                    {
                        button.Image = new Bitmap(new Bitmap(@"C:\Users\Zylietta\Downloads\Telegram Desktop\checkers (4)\checkers\white_king.png"), new Size(cellSize - 10, cellSize - 10));
                    }
                    else if ( piece == CheckerType.BlackKing)
                    {
                        button.Image = new Bitmap(new Bitmap(@"C:\Users\Zylietta\Downloads\Telegram Desktop\checkers (4)\checkers\black_king.png"), new Size(cellSize - 10, cellSize - 10));
                    }
                    else
                    {
                        button.Image = null;
                    }

                    button.BackColor = (position.Row + position.Column) % 2 == 1 ? Color.White : Color.Gray;
                }
            }
        }

    }
}
