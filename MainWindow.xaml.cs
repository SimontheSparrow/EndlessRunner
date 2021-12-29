using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace EndlessRunner
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        Rect playerHitBox, obstacleHitBox, groundHitBox;

        int speed=5, force=20, score=0;

        bool gameIsOver, jumping;

        double spriteIndex=0;

        Random rand = new Random();

        ImageBrush playerSprite = new ImageBrush();
        ImageBrush backgroundSprite = new ImageBrush();
        ImageBrush obstacleSprite = new ImageBrush();

        int[] obstaclePosition = { 320, 310, 300, 305, 310 };

        public MainWindow()
        {
            InitializeComponent();
            MyCanvas.Focus();
            gameTimer.Tick += TimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(40);
            

            backgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/background.gif"));

            background1.Fill = backgroundSprite;
            background2.Fill = backgroundSprite;

            StartGame();
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            Canvas.SetLeft(background1, Canvas.GetLeft(background1) - 10);
            Canvas.SetLeft(background2, Canvas.GetLeft(background1) - 10);
            if (Canvas.GetLeft(background1)<-1262)
            {
                Canvas.SetLeft(background1, Canvas.GetLeft(background2)+ background2.Width);
            }
            if (Canvas.GetLeft(background2) < 0)
            {
                Canvas.SetLeft(background2, Canvas.GetLeft(background1) + background1.Width);
            }

            Canvas.SetTop(player, Canvas.GetTop(player) + speed);
            Canvas.SetLeft(obstacle, Canvas.GetLeft(obstacle) - 12);
            scoreLabel.Content = "Score: " + score;

            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width - 10, player.Height );
            obstacleHitBox = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);
            groundHitBox = new Rect(Canvas.GetLeft(ground), Canvas.GetTop(ground), ground.Width , ground.Height );

            if (playerHitBox.IntersectsWith(groundHitBox))
            {
                
                speed = 0;
               
                Canvas.SetTop(player, Canvas.GetTop(ground)-player.Height);
                jumping = false;
                
                spriteIndex += 1;
                if (spriteIndex>8)
                {
                    spriteIndex = 1;
                }
                RunSprite(spriteIndex);
            }
            if (jumping)
            {
                speed = -9;
                force -= 1;
            }
            else
            {
                speed = 12;
            }
            if (force < 0)
            {
                jumping = false;
            }
            if (Canvas.GetLeft(obstacle)<-50)
            {
                score++;
                Canvas.SetLeft(obstacle, rand.Next(700, 1400));
            }
            if (playerHitBox.IntersectsWith(obstacleHitBox))
            {
                gameTimer.Stop();
                gameIsOver = true;
                scoreLabel.Content = "Score: " + score + " Press enter to restart.";
            }
        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter && gameIsOver)
            {
                StartGame();
            }
        }

        private void MyCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Space && !jumping && Canvas.GetTop(player)>260)
            {
                jumping = true;
                speed = -12;
                force = 15;
                RunSprite(2);
            }
        }

        private void StartGame()
        {
            
            Canvas.SetLeft(background1, 0);
            Canvas.SetLeft(background2, 1262);

            Canvas.SetLeft(player, 110);
            Canvas.SetTop(player, 140);

            Canvas.SetLeft(obstacle, 950);

            RunSprite(1);

            obstacleSprite.ImageSource= new BitmapImage(new Uri("pack://application:,,,/Images/obstacle.png"));
            obstacle.Fill = obstacleSprite;

            jumping = false;
            score = 0;
            gameIsOver = false;
            scoreLabel.Content = "Score: " + score;
            gameTimer.Start();


        }

        private void RunSprite(double i)
        {
            playerSprite.ImageSource= new BitmapImage(new Uri("pack://application:,,,/Images/newRunner_0"+ i +".gif"));
            player.Fill = playerSprite;
        }
    }
}