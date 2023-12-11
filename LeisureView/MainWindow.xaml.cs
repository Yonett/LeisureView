using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

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
        public Rectangle[] rects;
        public Rectangle[] piece_rects;
        public Field field;
        public Piece piece = new(1, 0, 0);
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            field = new Field(N);
            int size = N + 2;
            rects = new Rectangle[field.amount];
            piece_rects = new Rectangle[36];

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

            piece.w = rnd.Next(1, 7);
            piece.h = rnd.Next(1, 7);
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (gameState == 1)
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

                    player = player == 1 ? 2 : 1;
                    piece.kind = player;
                    piece.w = rnd.Next(1, 4);
                    piece.h = rnd.Next(1, 4);

                    for (int i = 0; i < piece.h; i++)
                    {
                        for (int j = 0; j < piece.w; j++)
                        {
                            piece_rects[i * piece.w + j].Visibility = Visibility.Visible;
                            piece_rects[i * piece.w + j].Stroke = player == 1 ? Brushes.Tomato : Brushes.LightSeaGreen;
                        }
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
            if (gameState == 1)
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
    }
}
