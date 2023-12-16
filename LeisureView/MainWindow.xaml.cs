using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        public int N = 9;

        byte impossibleTurnsInTheRow = 0;
        public bool startGame = false;
        public bool diceTheRolled = false;
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
            this.firstTurn.Text = "YOUR TURN";
            this.TurnName.Text = "TURN " + turn.ToString();
            Rounds.Add(new Round());
            field = new Field(N);
            int size = N + 2;
            rects = new Rectangle[field.amount];
            piece_rects = new Rectangle[36];
            //rects.HorizontalAlignment = HorizontalAlignment.Center;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    rects[i * size + j] = new Rectangle();
                    rects[i * size + j].MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
                }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    rects[i * size + j].Width = 40;
                    rects[i * size + j].Height = 40;

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

                    Canvas.SetLeft(rects[i * size + j], j * 50);
                    Canvas.SetTop(rects[i * size + j], i * 50);
                    GameField.Children.Add(rects[i * size + j]);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    piece_rects[i * 6 + j] = new Rectangle();
                    piece_rects[i * 6 + j].Width = 40;
                    piece_rects[i * 6 + j].Height = 40;

                    piece_rects[i * 6 + j].Stroke = Brushes.Tomato;
                    piece_rects[i * 6 + j].StrokeThickness = 10.0;

                    piece_rects[i * 6 + j].Visibility = Visibility.Hidden;

                    GameField.Children.Add(piece_rects[i * 6 + j]);
                }
            }
            #region Новый код
            /*
            if (player == 1)
            {
                piece.w = rnd.Next(1, 7);
                piece.h = rnd.Next(1, 7);
                player1.FirstRolles.Add(piece.w);
                player1.SecondRolles.Add(piece.h);
                Rounds[turn - 1].FirstMove_FirstDice = piece.w;
                Rounds[turn - 1].FirstMove_SecondDice = piece.h;
            }
            else
            {
                piece.w = rnd.Next(1, 7);
                piece.h = rnd.Next(1, 7);
                player2.FirstRolles.Add(piece.w);
                player2.SecondRolles.Add(piece.h);
                Rounds[turn - 1].SecondMove_FirstDice = piece.w;
                Rounds[turn - 1].SecondMove_SecondDice = piece.h;

            } */
            #endregion
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
                    MessageBox.Show($"Can place on {index} cell");
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
                        this.firstTurn.Text = "";
                        this.secondTurn.Text = "YOUR TURN";
                        player1.score += piece.h * piece.w;
                        this.firstScore.Text = player1.score.ToString();
                    }
                    else 
                    {
                        this.firstTurn.Text = "YOUR TURN";
                        this.secondTurn.Text = "";
                        player2.score += piece.h * piece.w;
                        this.secondScore.Text = player2.score.ToString();
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
                        Canvas.SetLeft(piece_rects[i * piece.w + j], pt.X + j * 50 - 25);
                        Canvas.SetTop(piece_rects[i * piece.w + j], pt.Y + i * 50 - 25);
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
            diceTheRolled = true;
            if (startGame == false)
            {
                #region Новый код
                startGame = true;
                if (player == 1)
                {
                    piece.w = rnd.Next(1, 7);
                    piece.h = rnd.Next(1, 7);
                    player1.FirstRolles.Add(piece.w);
                    player1.SecondRolles.Add(piece.h);
                    Rounds[turn - 1].FirstMove_FirstDice = piece.w;
                    Rounds[turn - 1].FirstMove_SecondDice = piece.h;

                    this.XText.Text = piece.w.ToString();
                    this.YText.Text = piece.h.ToString();
                }
                else
                {
                    piece.w = rnd.Next(1, 7);
                    piece.h = rnd.Next(1, 7);
                    player2.FirstRolles.Add(piece.w);
                    player2.SecondRolles.Add(piece.h);
                    Rounds[turn - 1].SecondMove_FirstDice = piece.w;
                    Rounds[turn - 1].SecondMove_SecondDice = piece.h;
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
                    piece.w = rnd.Next(1, 4);
                    piece.h = rnd.Next(1, 4);
                    player1.FirstRolles.Add(piece.w);
                    player1.SecondRolles.Add(piece.h);
                    Rounds[turn - 1].FirstMove_FirstDice = piece.w;
                    Rounds[turn - 1].FirstMove_SecondDice = piece.h;

                    this.XText.Text = piece.w.ToString();
                    this.YText.Text = piece.h.ToString();
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
                        //MessageBox.Show($"What4");
                    }
                    else
                    {
                        impossibleTurnsInTheRow++;
                        MessageBox.Show("Turn is unavailable");
                    }
                }
                else
                {
                    piece.w = rnd.Next(1, 4);
                    piece.h = rnd.Next(1, 4);
                    player2.FirstRolles.Add(piece.w);
                    player2.SecondRolles.Add(piece.h);
                    Rounds[turn - 1].SecondMove_FirstDice = piece.w;
                    Rounds[turn - 1].SecondMove_SecondDice = piece.h;
                    turn++;
                    this.TurnName.Text = "TURN " + turn.ToString();
                    this.XText.Text = piece.w.ToString();
                    this.YText.Text = piece.h.ToString();
                    Rounds.Add(new Round());

                    #region Проверка, что ход возможен

                    bool isTurnPossible = false;
                    int size = N + 2;
                    for (int m = 1; m < field.bounds; m++)
                    {
                        for (int n = 1; n < field.bounds; n++)
                        {
                            if (field.cells[m * size + n].kind == 0)
                                isTurnPossible |= field.CanPlacePiece(piece, m * size + n);
                            if (isTurnPossible)
                            {
                                //MessageBox.Show($"What1");
                                break;
                            }
                            if (isTurnPossible)
                            {
                                //MessageBox.Show($"What2");
                                break;
                            }
                        }
                    }
                    if (isTurnPossible)
                    {
                        //MessageBox.Show($"What3");
                    }
                    else
                    {
                        impossibleTurnsInTheRow++;
                        MessageBox.Show("Turn is unavailable");
                    }
                    #endregion

                }
                #endregion

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
