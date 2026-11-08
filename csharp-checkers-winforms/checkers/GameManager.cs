using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using checkers;
using static checkers.GameBoard;


namespace checkers
{
    public partial class Form1 : Form
    {
        public GameBoard createBoard;
        
        public BoardPosition selectedPosition;
      

        public Form1()
        {
            InitializeComponent();
            createBoard = new GameBoard(this);
            createBoard.Init();
            label1.Text = "Хід: Гравець 1"; // Початкове значення
            label2.Text = $"Рахунок: \nГравець 1 - 0\nГравець 2 - 0";
        }

        private int player1Score = 0;
        private int player2Score = 0;
        private void UpdatePlayerLabel()
        {
            label1.Text = $"Хід: Гравець {(currentPlayer == CheckerType.White ? 1 : 2)}"; // Оновлення тексту
        }
        private void UpdateScoreLabel()
        {
            label2.Text = $"Рахунок: \nГравець 1 - {player1Score}\nГравець 2 - {player2Score}"; // Оновлення тексту
        }
        public bool IsValidPieceToSelect(BoardPosition position)
        {
            if (position == null)
            {
                return false;
            }

            CheckerType piece = createBoard.board[position.Row, position.Column];

            // Перевірка, чи клітинка не порожня і фішка належить поточному гравцеві
            if (piece != CheckerType.None && IsPieceOfCurrentPlayer(piece))
            {
          

                return true;
            }

            return false;
        }

   

        private bool IsPieceOfCurrentPlayer(CheckerType piece)
        {
            // Перевірка, чи фішка належить поточному гравцеві, або чи це King поточного гравця
            return (currentPlayer == CheckerType.White && (piece == CheckerType.White || piece == CheckerType.WhiteKing)) ||
                   (currentPlayer == CheckerType.Black && (piece == CheckerType.Black || piece == CheckerType.BlackKing));
        }

        public void HighlightSelectedPiece(BoardPosition position)
        {
            Button selectedButton = GetButtonAtPosition(position);
            selectedButton.BackColor = Color.Yellow;
        }

        public void ClearHighlights()
        {
            // Зняти виділення з усіх кнопок
            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = (BoardPosition)button.Tag == selectedPosition ? Color.Yellow : GetCellColor((BoardPosition)button.Tag);
                }
            }
        }
        public List<BoardPosition> GetValidMoves(BoardPosition position)
        {
            List<BoardPosition> validMoves = new List<BoardPosition>();

            CheckerType piece = createBoard.board[position.Row, position.Column];

            // Перевірка, чи клітинка не порожня і фішка належить поточному гравцеві
            if (piece != CheckerType.None && IsPieceOfCurrentPlayer(piece))
            {
                // Якщо клітинка належить поточному гравцеві, додаємо можливі ходи
                if (piece == CheckerType.White || piece == CheckerType.Black)
                {
                    // Перевіряємо для звичайної шашки
                    AddValidMove(validMoves, position, 1, 1);
                    AddValidMove(validMoves, position, 1, -1);
                }
                else if (piece == CheckerType.WhiteKing || piece == CheckerType.BlackKing)
                {
                    // Перевіряємо для дамки
                    // Дамка може рухатися в будь-якому напрямку по діагоналі
                    AddValidMovesInDirection(validMoves, position, 1, 1);
                    AddValidMovesInDirection(validMoves, position, 1, -1);
                    AddValidMovesInDirection(validMoves, position, -1, 1);
                    AddValidMovesInDirection(validMoves, position, -1, -1);
                }
            }

            return validMoves;
        }
        private void AddValidMove(List<BoardPosition> validMoves, BoardPosition position, int rowDirection, int colDirection)
        {
            BoardPosition targetPosition = new BoardPosition(position.Row + rowDirection, position.Column + colDirection);
            if (IsValidMove(position, targetPosition))
            {
                validMoves.Add(targetPosition);
            }
        }

        private void AddValidMovesInDirection(List<BoardPosition> validMoves, BoardPosition position, int rowDirection, int colDirection)
        {
            BoardPosition currentPos = new BoardPosition(position.Row + rowDirection, position.Column + colDirection);

            while (IsWithinBoard(currentPos.Row, currentPos.Column) && createBoard.board[currentPos.Row, currentPos.Column] == CheckerType.None)
            {
                validMoves.Add(currentPos);
                currentPos.Row += rowDirection;
                currentPos.Column += colDirection;
            }
        }
        public Color GetCellColor(BoardPosition position)
        {
            // Перевірка, чи позиція не є null перед отриманням колір клітинки
            if (position != null)
            {
                return (position.Row + position.Column) % 2 == 0 ? Color.White : Color.Gray;
            }
            else
            {
                return Color.Gray;
            }
        }

        private Button GetButtonAtPosition(BoardPosition position)
        {
            // Отримати кнопку за її позицією
            foreach (Control control in this.Controls)
            {
                if (control is Button button && (BoardPosition)button.Tag == position)
                {
                    return button;
                }
            }
            return null;
        }

        public void MovePiece(BoardPosition currentPosition, BoardPosition targetPosition)
        {
            CheckerType pieceToMove = createBoard.board[currentPosition.Row, currentPosition.Column];

            if (!IsValidMove(currentPosition, targetPosition))
            {
                MessageBox.Show("Неприпустимий хід!");
                return;
            }

            createBoard.board[currentPosition.Row, currentPosition.Column] = CheckerType.None;
            createBoard.board[targetPosition.Row, targetPosition.Column] = pieceToMove;

            if (IsCaptureMove(currentPosition, targetPosition))
            {
                BoardPosition capturedPosition = GetCapturedPosition(currentPosition, targetPosition);
                createBoard.board[capturedPosition.Row, capturedPosition.Column] = CheckerType.None;

                // Оновлення кількості побитих шашок для поточного гравця
                if (currentPlayer == CheckerType.White)
                {
                    player1Score++;
                }
                else if (currentPlayer == CheckerType.Black)
                {
                    player2Score++;
                }

                // Оновлення відображення рахунку
                UpdateScoreLabel();
            }

            createBoard.UpdateBoardUI();
            createBoard.CheckForWinner(); // Перевірка стану гри

            // перевірка, чи фішка має стати дамкою
            if (ShouldBecomeKing(targetPosition))
            {
                MakeKing(targetPosition);
            }

            ClearHighlights();
            selectedPosition = null;
            SwitchPlayer();
        }

        public bool ShouldBecomeKing(BoardPosition position)
        {
            CheckerType piece = createBoard.board[position.Row, position.Column];
            return (piece == CheckerType.White && position.Row == 7) || (piece == CheckerType.Black && position.Row == 0);
        }
        public void MakeKing(BoardPosition position)
        {
            CheckerType currentPiece = createBoard.board[position.Row, position.Column];

            if (currentPiece == CheckerType.White)
            {
                createBoard.board[position.Row, position.Column] = CheckerType.WhiteKing;
            }
            else if (currentPiece == CheckerType.Black)
            {
                createBoard.board[position.Row, position.Column] = CheckerType.BlackKing;
            }

            Console.WriteLine("Дамка");
        }

        public CheckerType currentPlayer = CheckerType.White; // Початковий гравець

        public void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == CheckerType.White) ? CheckerType.Black : CheckerType.White;
        }

        public bool IsCaptureMove(BoardPosition currentPosition, BoardPosition targetPosition)
        {
            int direction = Math.Sign(targetPosition.Row - currentPosition.Row);
            int middleRow = (currentPosition.Row + targetPosition.Row) / 2;
            int middleColumn = (currentPosition.Column + targetPosition.Column) / 2;

            if (IsWithinBoard(middleRow, middleColumn))
            {
                CheckerType middlePiece = createBoard.board[middleRow, middleColumn];

                if (IsOpponentPiece(middlePiece, currentPlayer))
                {
                  
                    return true;
              
                }
            }

            return false;
        }


        private bool IsOpponentPiece(CheckerType piece, CheckerType currentPlayer)
        {
            return piece != CheckerType.None && piece != currentPlayer;
        }

       public BoardPosition GetCapturedPosition(BoardPosition currentPosition, BoardPosition targetPosition)
        {
            // Визначаєм напрямок руху за допомогою знака
            int directionRow = Math.Sign(targetPosition.Row - currentPosition.Row);
            int directionCol = Math.Sign(targetPosition.Column - currentPosition.Column);

            // Позначаєм, чи шашка противника знаходиться на шляху
            bool opponentPieceFound = false;

            // Позначаєм позицію захопленої фішки
            BoardPosition capturedPosition = new BoardPosition(currentPosition.Row, currentPosition.Column);

            // Переміщуємось вздовж лінії руху до цільової позиції
            while (capturedPosition.Row != targetPosition.Row && capturedPosition.Column != targetPosition.Column)
            {
                // Перевірка, чи шашка противника на поточній позиції
                if (createBoard.board[capturedPosition.Row, capturedPosition.Column] != CheckerType.None && IsOpponentPiece(createBoard.board[capturedPosition.Row, capturedPosition.Column], currentPlayer))
                {
                    opponentPieceFound = true;
                    break;
                }

                // Оновляєм позицію захопленої фішки
                capturedPosition.Row += directionRow;
                capturedPosition.Column += directionCol;
            }

            // Перевірка, чи шашка противника знайдена і повертаєм позицію захопленої фішки
            return opponentPieceFound ? capturedPosition : null;
        }

        public void HandleButtonClick(BoardPosition clickedPosition)
        {
            if (selectedPosition == null)
            {
                if (IsValidPieceToSelect(clickedPosition))
                {
                    selectedPosition = clickedPosition;
                    HighlightSelectedPiece(selectedPosition);

                    // Додайте перевірку, чи обрана шашка - дамка, і викличте метод для підсвітки ходів для дамки
                    CheckerType selectedPiece = createBoard.board[selectedPosition.Row, selectedPosition.Column]; 
                }
            }
            else
            {
                if (IsValidMove(selectedPosition, clickedPosition))
                {
                    MovePiece(selectedPosition, clickedPosition);
                }
                else
                {
                    ClearHighlights();
                    if (IsValidPieceToSelect(clickedPosition))
                    {
                        selectedPosition = clickedPosition;
                        HighlightSelectedPiece(selectedPosition);

                        // Отримати можливі ходи для обраної шашки
                        List<BoardPosition> validMoves = GetValidMoves(selectedPosition);

                      
                    }
                }
            }
            UpdateScoreLabel();
            UpdatePlayerLabel();
        }

        private bool CanCaptureForKing(BoardPosition currentPosition, BoardPosition targetPosition)
        {
            int rowDirection = Math.Sign(targetPosition.Row - currentPosition.Row);
            int colDirection = Math.Sign(targetPosition.Column - currentPosition.Column);

            int currentRow = currentPosition.Row + rowDirection;
            int currentCol = currentPosition.Column + colDirection;

            int capturesInARow = 0;  // Змінна для відстеження кількості захоплень поспіль

            while (currentRow != targetPosition.Row && currentCol != targetPosition.Column)
            {
                if (createBoard.board[currentRow, currentCol] != CheckerType.None)
                {
                    // Перевірка, чи знаходиться фішка суперника на шляху
                    if (IsOpponentPiece(createBoard.board[currentRow, currentCol], currentPlayer))
                    {
                        capturesInARow++;
                    }
                    else
                    {
                        // Якщо є інша фішка, але не суперник, це неприпустимий хід
                        return false;
                    }
                }

                currentRow += rowDirection;
                currentCol += colDirection;
            }
            return capturesInARow == 1;
        }


        public bool IsValidMove(BoardPosition currentPosition, BoardPosition targetPosition)
        {
            CheckerType piece = createBoard.board[currentPosition.Row, currentPosition.Column];
            // Дамка може рухатися в будь-якому напрямку по діагоналі на будь-яку кількість вільних клітинок
            if (piece == CheckerType.WhiteKing || piece == CheckerType.BlackKing)
            {
                // Логіка для захоплення дамкою
                if (CanCaptureForKing(currentPosition, targetPosition))
                {
                    return true;
                }
                else
                {
                    // Логіка для простого ходу дамкою (без захоплення)
                    int rowDirection = Math.Sign(targetPosition.Row - currentPosition.Row);
                    int colDirection = Math.Sign(targetPosition.Column - currentPosition.Column);

                    int currentRow = currentPosition.Row + rowDirection;
                    int currentCol = currentPosition.Column + colDirection;

                    while (currentRow != targetPosition.Row && currentCol != targetPosition.Column)
                    {
                        if (createBoard.board[currentRow, currentCol] != CheckerType.None)
                        {
                            Console.WriteLine("Недійсний хід для дамки: є перешкода на шляху");
                            return false;
                        }

                        currentRow += rowDirection;
                        currentCol += colDirection;
                    }

                    return true;
                }
            }
            if (!IsWithinBoard(targetPosition.Row, targetPosition.Column))
            {
                Console.WriteLine("Поза межами дошки");
                return false;
            }

            if (createBoard.board[targetPosition.Row, targetPosition.Column] != CheckerType.None)
            {
                Console.WriteLine("Цільова клітинка не порожня");
                return false;
            }

            int rowDifference = Math.Abs(currentPosition.Row - targetPosition.Row);
            int colDifference = Math.Abs(currentPosition.Column - targetPosition.Column);

            if (rowDifference == 0 || colDifference == 0)
            {
                Console.WriteLine("Неприпустимий хід: рух по горизонталі або вертикалі");
                return false;
            }

            // Перевірка для звичайної шашки
            if (piece == CheckerType.White || piece == CheckerType.Black)
            {
                int direction = (piece == CheckerType.White) ? 1 : -1;

                // Перевірка захоплюючих ходів
                if (IsCaptureMove(currentPosition, targetPosition))
                {
                    // Додаткова логіка для захоплюючих ходів
                    BoardPosition capturedPosition = GetCapturedPosition(currentPosition, targetPosition);
                    if (createBoard.board[capturedPosition.Row, capturedPosition.Column] == CheckerType.None)
                    {
                        Console.WriteLine("Недійсний захоплюючий хід, немає фішки для захоплення.");
                        return false;
                    }
                   
                    return true;
                }

                // Дозволити рух лише вперед на одну клітинку
                if (direction * (targetPosition.Row - currentPosition.Row) == 1 && Math.Abs(targetPosition.Column - currentPosition.Column) == 1)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Недійсний хід для звичайної шашки: можливий лише рух вперед на одну клітинку");
                    return false;
                }
            }

            // Логіка для захоплення дамкою
            if (piece == CheckerType.WhiteKing || piece == CheckerType.BlackKing)
            {
                if (IsCaptureMove(currentPosition, targetPosition))
                {
                    if (CanCapture(currentPosition, targetPosition))
                    {
                        // Видаляємо здобуту фішку
                        CapturePieces(currentPosition, targetPosition);
                        // Оновлення кількості побитих шашок для поточного гравця
                        if (currentPlayer == CheckerType.White)
                        {
                            player1Score++;
                        }
                        else if (currentPlayer == CheckerType.Black)
                        {
                            player2Score++;
                        }
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Недійсний захоплюючий хід для дамки.");
                        return false;
                    }
                }
            


            // Логіка для руху дамкою (без захоплення)
            int rowDirection = Math.Sign(targetPosition.Row - currentPosition.Row);
                int colDirection = Math.Sign(targetPosition.Column - currentPosition.Column);

                int currentRow = currentPosition.Row + rowDirection;
                int currentCol = currentPosition.Column + colDirection;

                while (currentRow != targetPosition.Row && currentCol != targetPosition.Column)
                {
                    if (createBoard.board[currentRow, currentCol] != CheckerType.None)
                    {
                        Console.WriteLine("Недійсний хід для дамки: є перешкода на шляху");
                        return false;
                    }

                    currentRow += rowDirection;
                    currentCol += colDirection;
                }

                return true;
            }

            return true;
        }


        private void CapturePieces(BoardPosition currentPosition, BoardPosition targetPosition)
        {
            // Визначаєм позицію захопленої фішки
            int directionRow = Math.Sign(targetPosition.Row - currentPosition.Row);
            int directionCol = Math.Sign(targetPosition.Column - currentPosition.Column);

            // Видаляєм шашки на шляху в усіх можливих напрямках
            CaptureInDirection(currentPosition, directionRow, directionCol);
            CaptureInDirection(currentPosition, -directionRow, directionCol);
            CaptureInDirection(currentPosition, directionRow, -directionCol);
            CaptureInDirection(currentPosition, -directionRow, -directionCol);
        }

        private bool CanCapture(BoardPosition currentPosition, BoardPosition targetPosition)
        {
            int directionRow = Math.Sign(targetPosition.Row - currentPosition.Row);
            int directionCol = Math.Sign(targetPosition.Column - currentPosition.Column);

            int row = currentPosition.Row + directionRow;
            int col = currentPosition.Column + directionCol;

            while (IsWithinBoard(row, col) && createBoard.board[row, col] != CheckerType.None)
            {
                row += directionRow;
                col += directionCol;
            }

            return IsWithinBoard(row, col) && IsOpponentPiece(createBoard.board[row, col], currentPlayer);
        }

        private void CaptureInDirection(BoardPosition currentPosition, int rowDirection, int colDirection)
        {
            int row = currentPosition.Row + rowDirection;
            int col = currentPosition.Column + colDirection;

            while (IsWithinBoard(row, col) && createBoard.board[row, col] != CheckerType.None)
            {
                createBoard.board[row, col] = CheckerType.None;
                row += rowDirection;
                col += colDirection;
            }
        }

        public bool IsWithinBoard(int row, int column)
        {
            // Перевірка, чи клітинка знаходиться в межах дошки
            return row >= 0 && row < 8 && column >= 0 && column < 8;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Checkers Game";
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(550, 400);
            this.Name = "MainForm";
            this.ResumeLayout(false);
           
        }
    }
}