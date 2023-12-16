using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;
using System.Collections.Generic;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using System.Reflection;
using System.Drawing;
using System.Security.Policy;

namespace LeisureView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int gameState = 1;
        public int player = 1;
        //public int N = 9;
        public int N = 20;
        byte impossibleTurnsInTheRow = 0;
        public bool startGame = false;
        public bool diceTheRolled = false;
        public double xSize, ySize = 40;
        public int numberOfMissedMoves = 0;
        bool isTurnPossible, isTurnProceed;
        public Rectangle[] rects;
        public Rectangle[] piece_rects;
        public Field field;
        public Piece piece = new(1, 0, 0);
        public Player player1 = new Player();
        public Player player2 = new Player();
        public List<Round> Rounds = new List<Round>();
        public int turn = 0;
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            turn++;
            this.turnHistory.Inlines.Clear();
            this.playerHistory.Inlines.Clear();
            this.diceHistory.Inlines.Clear();
            this.scoreHistory.Inlines.Clear();

            this.turnHistory.Inlines.Add(new Run("Ход\n"));
            this.playerHistory.Inlines.Add(new Run("Игрок\n"));
            this.diceHistory.Inlines.Add(new Run("бросок\n"));
            this.scoreHistory.Inlines.Add(new Run("Очки\n"));

            /*
            this.turnHistory.Text = "Ход";
            this.playerHistory.Text = "Игрок";
            this.first_diceHistory.Text = "1 бросок";
            this.second_diceHistory.Text = "2 бросок";
            this.scoreHistory.Text = "Очки";
            */
            this.firstTurn.Text = "YOUR TURN";
            this.TurnName.Text = "TURN " + turn.ToString();
            Rounds.Add(new Round());
            field = new Field(N);
            int size = N + 2;
            rects = new Rectangle[field.amount];
            //rects.HorizontalAlignment = HorizontalAlignment.Center;
            piece_rects = new Rectangle[36];
            //rects.VerticalAlignment = VerticalAlignment.Top;
            //rects.HorizontalAlignment = HorizontalAlignment.Center;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    rects[i * size + j] = new Rectangle();
                    //rects[i * size + j].HorizontalAlignment = HorizontalAlignment.Center;
                    rects[i * size + j].MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
                }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (N < 10)
                    {
                        xSize = ySize = 40;
                        rects[i * size + j].Width = 40;
                        rects[i * size + j].Height = 40;
                    }
                    else
                    {
                        xSize = ySize = 540.0 / (1.25 * N + 2.25);
                        rects[i * size + j].Width = xSize;
                        rects[i * size + j].Height = ySize;
                    }

                    switch (field.cells[i * size + j].kind)
                    {
                        case 0:
                            rects[i * size + j].Fill = Brushes.White;
                            break;
                        case 1:
                            rects[i * size + j].Fill = Brushes.Tomato;
                            break;
                        case 2:
                            rects[i * size + j].Fill = Brushes.LightSeaGreen;
                            break;
                        case 3:
                            rects[i * size + j].Fill = Brushes.Black;
                            break;
                    }
                    
                    Canvas.SetLeft(rects[i * size + j], j * (xSize+xSize/4.0));
                    Canvas.SetTop(rects[i * size + j], i * (xSize + xSize / 4.0));
                    GameField.Children.Add(rects[i * size + j]);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    piece_rects[i * 6 + j] = new Rectangle();
                    piece_rects[i * 6 + j].Width = xSize;
                    piece_rects[i * 6 + j].Height = ySize;

                    piece_rects[i * 6 + j].Stroke = Brushes.Tomato;
                    piece_rects[i * 6 + j].StrokeThickness = xSize/4;

                    piece_rects[i * 6 + j].Visibility = Visibility.Hidden;

                    GameField.Children.Add(piece_rects[i * 6 + j]);
                }
            }
            #region Новый код
            #endregion
            //MessageBoxCustomWindow result = new MessageBoxCustomWindow();
            //piece.w = rnd.Next(1, 7);
            //piece.h = rnd.Next(1, 7);
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (gameState == 1 && diceTheRolled == true)
            {
                int index = Array.IndexOf(rects, sender);

                if (field.CanPlacePiece(piece, index))
                {
                    //MessageBox.Show($"Can place on {index} cell");
                    for (int i = 0; i < piece.h; i++)
                        for (int j = 0; j < piece.w; j++)
                        {
                            rects[index + i * (N + 2) + j].Fill = player == 1 ? Brushes.Tomato : Brushes.LightSeaGreen;
                            field.cells[index + i * (N + 2) + j].kind = piece.kind;
                            piece_rects[i * piece.w + j].Visibility = Visibility.Hidden;
                        }

                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            piece_rects[i * 6 + j].Visibility = Visibility.Hidden;
                        }
                    }
                    diceTheRolled = false;
                    if (player == 1)
                    {
                        player1.FirstRolles.Add(piece.w);
                        player1.SecondRolles.Add(piece.h);
                        Rounds[turn - 1].FirstMove_FirstDice = piece.w;
                        Rounds[turn - 1].FirstMove_SecondDice = piece.h;
                        this.firstTurn.Text = "";
                        this.secondTurn.Text = "YOUR TURN";
                        player1.score += piece.h * piece.w;
                        this.firstScore.Text = player1.score.ToString();

                        this.turnHistory.Inlines.Add(new Run("  " + turn.ToString() + "\n"));
                        this.playerHistory.Inlines.Add(new Run("  " + player.ToString()+"\n") { Foreground= Brushes.Tomato });
                        this.diceHistory.Inlines.Add(new Run("  " + piece.h.ToString()+"-"+ piece.w.ToString() + "\n"));
                        this.scoreHistory.Inlines.Add(new Run("  " + player1.score.ToString()+"\n"));

                        /*
                        this.turnHistory.Text += "\n" + turn.ToString();
                        this.playerHistory.Text += "\n" + player.ToString();
                        this.first_diceHistory.Text += "\n" + piece.h.ToString();
                        this.second_diceHistory.Text += "\n" + piece.w.ToString();
                        this.scoreHistory.Text += "\n" + player1.score.ToString();*/
                    }
                    else 
                    {
                        player2.FirstRolles.Add(piece.w);
                        player2.SecondRolles.Add(piece.h);
                        Rounds[turn - 1].SecondMove_FirstDice = piece.w;
                        Rounds[turn - 1].SecondMove_SecondDice = piece.h;

                        this.firstTurn.Text = "YOUR TURN";
                        this.secondTurn.Text = "";
                        player2.score += piece.h * piece.w;
                        this.secondScore.Text = player2.score.ToString();

                        this.turnHistory.Inlines.Add(new Run("  " + turn.ToString() + "\n"));
                        this.playerHistory.Inlines.Add(new Run("  "+player.ToString() + "\n") { Foreground = Brushes.LightSeaGreen }) ;
                        this.diceHistory.Inlines.Add(new Run("  " + piece.h.ToString() + "-" + piece.w.ToString() + "\n"));
                        this.scoreHistory.Inlines.Add(new Run("  " + player2.score.ToString() + "\n"));

                        /*
                        this.turnHistory.Text += "\n" + (turn).ToString();
                        this.playerHistory.Text += "\n" + player.ToString();
                        this.first_diceHistory.Text += "\n" + piece.h.ToString();
                        this.second_diceHistory.Text += "\n" + piece.w.ToString();
                        this.scoreHistory.Text += "\n" + player2.score.ToString();*/
                        turn++;
                        Rounds.Add(new Round());
                    }

                }
                else
                {
                   //MessageBox.Show($"Cannot place on {index} cell");
                }
            } 
        }

        private void GameField_MouseMove(object sender, MouseEventArgs e)
        {
            if (gameState == 1)
            {
                Point pt = e.GetPosition(GameField);

                for (int i = 0; i < piece.h; i++)
                {
                    for (int j = 0; j < piece.w; j++)
                    {
                        Canvas.SetLeft(piece_rects[i * piece.w + j], pt.X + j * (xSize + xSize / 4.0) - (xSize + xSize / 4.0)/2);
                        Canvas.SetTop(piece_rects[i * piece.w + j], pt.Y + i * (xSize + xSize / 4.0) - (xSize + xSize / 4.0)/2);
                    }
                }
            }
        }

        private void GameField_MouseLeave(object sender, MouseEventArgs e)
        {
            if (gameState == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        piece_rects[i * piece.w + j].Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void GameField_MouseEnter(object sender, MouseEventArgs e)
        {
            if (gameState == 1 && diceTheRolled == true)
            {
                for (int i = 0; i < piece.h; i++)
                {
                    for (int j = 0; j < piece.w; j++)
                    {
                        piece_rects[i * piece.w + j].Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void RollTheDice_Click(object sender, RoutedEventArgs e)
        {
            
            if (diceTheRolled == false)
            {
                diceTheRolled = true;
                bool isTurnPossible = false;
                if (startGame == false)
                {
                    #region Новый код
                    startGame = true;
                    if (player == 1)
                    {
                        piece.w = rnd.Next(1, 7);
                        piece.h = rnd.Next(1, 7);

                        this.XText.Text = piece.w.ToString();
                        this.YText.Text = piece.h.ToString();
                    }
                    else
                    {
                        piece.w = rnd.Next(1, 7);
                        piece.h = rnd.Next(1, 7);
                        this.XText.Text = piece.w.ToString();
                        this.YText.Text = piece.h.ToString();
                        
                    }
                    #endregion
                }
                else
                {
                    player = player == 1 ? 2 : 1;
                    piece.kind = player;
                    #region Новый код
                    if (player == 1)
                    {
                        piece.w = rnd.Next(1, 7);
                        piece.h = rnd.Next(1, 7);
                        this.XText.Text = piece.w.ToString();
                        this.YText.Text = piece.h.ToString();

                        #region Проверка, что ход возможен

                        int size = N + 2;
                        for (int m = 1; m < field.bounds; m++)
                        {
                            for (int n = 1; n < field.bounds; n++)
                            {
                                if (field.cells[m * size + n].kind == 0)
                                    isTurnPossible |= field.CanPlacePiece(piece, m * size + n);
                                if (isTurnPossible)
                                    break;
                            }
                            if (isTurnPossible)
                                break;
                        }

                        if (isTurnPossible)
                        {
                            if (impossibleTurnsInTheRow > 0)
                                impossibleTurnsInTheRow--;
                            isTurnProceed = true;
                            
                        }
                        else
                        {
                            impossibleTurnsInTheRow++;
                            new MessageBoxCustomWindow("Ход игрока 1 невозможен!").ShowDialog();
                            this.turnHistory.Inlines.Add(new Run("  " + turn.ToString() + "\n") { Background = Brushes.Red });
                            this.playerHistory.Inlines.Add(new Run("  " + player.ToString() + "\n") { Foreground = Brushes.Tomato, Background = Brushes.Red });
                            this.diceHistory.Inlines.Add(new Run("  " + piece.h.ToString() + "-" + piece.w.ToString() + "\n") { Background = Brushes.Red });
                            this.scoreHistory.Inlines.Add(new Run("  " + player1.score.ToString() + "\n") { Background = Brushes.Red });
                            diceTheRolled = false;
                            if (impossibleTurnsInTheRow > 1) {
                                if (player1.score > player2.score)
                                {
                                    new MessageBoxCustomWindow("ВЫИГРАЛ ИГРОК 1!").ShowDialog();
                                }
                                if (player1.score == player2.score)
                                {
                                    new MessageBoxCustomWindow("НИЧЬЯ!").ShowDialog();
                                }
                                if (player1.score < player2.score)
                                {
                                    new MessageBoxCustomWindow("ВЫИГРАЛ ИГРОК 2!").ShowDialog();
                                }
                            }
                            //передаём ход другому игроку:
                        }
                        #endregion
                    }

                    else
                    {
                        piece.w = rnd.Next(1, 7);
                        piece.h = rnd.Next(1, 7);
                        
                        this.TurnName.Text = "TURN " + turn.ToString();
                        this.XText.Text = piece.w.ToString();
                        this.YText.Text = piece.h.ToString();
                        

                        #region Проверка, что ход возможен

                        
                        int size = N + 2;
                        for (int m = 1; m < field.bounds; m++)
                        {
                            for (int n = 1; n < field.bounds; n++)
                            {
                                if (field.cells[m * size + n].kind == 0)
                                    isTurnPossible |= field.CanPlacePiece(piece, m * size + n);
                                if (isTurnPossible)
                                {
                                    break;
                                }
                            }
                            if (isTurnPossible)
                                break;
                        }
                        if (isTurnPossible)
                        {
                            if (impossibleTurnsInTheRow > 0)
                                impossibleTurnsInTheRow--;
                            isTurnProceed = true;
                            
                            //MessageBox.Show($"What3");
                        }
                        else //если 2-ому игроку негде поставить фигуру
                        {
                            
                            impossibleTurnsInTheRow++;
                            new MessageBoxCustomWindow("Ход игрока 2 невозможен!").ShowDialog();
                            this.turnHistory.Inlines.Add(new Run("  " + turn.ToString() + "\n") { Background = Brushes.Red });
                            this.playerHistory.Inlines.Add(new Run("  " + player.ToString() + "\n") { Foreground = Brushes.LightSeaGreen, Background = Brushes.Red });
                            this.diceHistory.Inlines.Add(new Run("  " + piece.h.ToString() + "-" + piece.w.ToString() + "\n") { Background = Brushes.Red });
                            this.scoreHistory.Inlines.Add(new Run("  " + player2.score.ToString() + "\n") { Background = Brushes.Red });
                            diceTheRolled = false;
                            Rounds.Add(new Round());
                            turn++;
                            if (impossibleTurnsInTheRow > 1) {
                                if(player1.score > player2.score)
                                {
                                    new MessageBoxCustomWindow("ВЫИГРАЛ ИГРОК 1!").ShowDialog();
                                }
                                if (player1.score == player2.score)
                                {
                                    new MessageBoxCustomWindow("НИЧЬЯ!").ShowDialog();
                                }
                                if (player1.score < player2.score)
                                {
                                    new MessageBoxCustomWindow("ВЫИГРАЛ ИГРОК 2!").ShowDialog();
                                }
                            }
                            //передаём ход другому игроку:

                        }
                        #endregion

                    }
                    #endregion

                    if (isTurnPossible)
                    {
                        for (int i = 0; i < piece.h; i++)
                        {
                            for (int j = 0; j < piece.w; j++)
                            {
                                piece_rects[i * piece.w + j].Visibility = Visibility.Visible;
                                piece_rects[i * piece.w + j].Stroke = player == 1 ? Brushes.Tomato : Brushes.LightSeaGreen;
                            }
                        }
                    }

                }
            }
        } 
    }
}
