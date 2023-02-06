/*
 * Description:     A basic PONG simulator
 * Author:           
 * Date:            
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 10);
        SolidBrush goldBrush = new SolidBrush(Color.Gold);
        SolidBrush purpleBrush = new SolidBrush(Color.Purple);
        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean wKeyDown, sKeyDown, upKeyDown, downKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball values
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        const int BALL_SPEED = 4;
        const int BALL_WIDTH = 1;
        const int BALL_HEIGHT = 1;
        Rectangle ball;

        //player values
        const int PADDLE_SPEED = 6;
        const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            
        const int PADDLE_WIDTH = 10;
        const int PADDLE_HEIGHT = 40;
        Rectangle player1, player2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 3;  // number of points needed to win game

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.Escape:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //player start positions
            player1 = new Rectangle(PADDLE_EDGE, this.Height / 2 - PADDLE_HEIGHT / 2, PADDLE_WIDTH, PADDLE_HEIGHT);
            player2 = new Rectangle(this.Width - PADDLE_EDGE - PADDLE_WIDTH, this.Height / 2 - PADDLE_HEIGHT / 2, PADDLE_WIDTH, PADDLE_HEIGHT);

            // TODO create a ball rectangle in the middle of screen
            ball = new Rectangle(this.Width / 2 - BALL_WIDTH, this.Height / 2, BALL_HEIGHT, BALL_WIDTH);

            UpdateScore();   

        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void UpdateScore()
        {
            player1ScoreLabel.Text = player1Score.ToString();
            plaery2ScoreLabel.Text = player2Score.ToString();
        }
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if (ballMoveRight)
            {
                ball.X += BALL_SPEED;
            }
            else { ball.X -= BALL_SPEED; }

            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED
            if (ballMoveDown)
            { ball.Y += BALL_SPEED; }
            else { ball.Y -= BALL_SPEED; }
            #endregion

            #region update paddle positions

            if (wKeyDown == true && player1.Y > 0)
            {
                player1.Y -= PADDLE_SPEED;
            }

            // TODO create an if statement and code to move player 1 down 
            if (sKeyDown == true && player1.Y > 0)
            {
                player1.Y += PADDLE_SPEED;
            }
            // TODO create an if statement and code to move player 2 up
            if (upKeyDown == true && player1.Y > 0)
            {
                player2.Y -= PADDLE_SPEED;
            }
            // TODO create an if statement and code to move player 2 down
            if (downKeyDown == true && player1.Y > 0)
            {
                player2.Y += PADDLE_SPEED;
            }
            #endregion

            #region ball collision with top and bottom lines




            if (ball.Y < 0) // if ball hits top line
            {
                // TODO use ballMoveDown boolean to change direction 
                ballMoveDown = true;
                collisionSound.Play();
                // TODO play a collision sound


            }
            // TODO In an else if statement check for collision with bottom line
            else if (ball.Y > this.Height - BALL_HEIGHT)
            {
                collisionSound.Play();
                ballMoveDown = false;
            }
            // If true use ballMoveDown boolean to change direction
            if (ball.Y < 0)
            { ballMoveDown = true; }
            if (ball.Y >= this.Height - BALL_HEIGHT)
            { ballMoveDown = false; }

            #endregion

            #region ball collision with paddles

            // TODO create if statment that checks if player1 collides with ball and if it does
            if (ball.IntersectsWith(player1) || ball.IntersectsWith(player2))
            {
                collisionSound.Play();

            }// --- play a "paddle hit" sound and
             // --- use ballMoveRight boolean to change direction
            if (ball.IntersectsWith(player1))
            { ballMoveRight = true; }
            else if (ball.IntersectsWith(player2))
            {
                ballMoveRight = false;
            }
            // TODO create if statment that checks if player2 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction

            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                // TODO
                // --- play score sound
                scoreSound.Play();
                // --- update player 2 score and display it to the label
                player2Score++;
                UpdateScore();
                // TODO use if statement to check to see if player 2 has won the game. If true run 
                if (player2Score >= gameWinScore)
                {
                    GameOver("Player 2");

                }
                else
                {
                    ballMoveRight = !ballMoveRight;
                    SetParameters();
                }

                // GameOver() method. Else change direction of ball and call SetParameters() method.

            }
            #endregion 
            // TODO same as above but this time check for collision with the right wall

            if (ball.X >= this.Width)
            {
                scoreSound.Play();
                player1Score++;
                UpdateScore();

                if (player1Score >= gameWinScore)
                {
                    GameOver("Player 1");
                }
                else
                {
                    ballMoveRight = !ballMoveRight;
                    SetParameters();
                }
            }



            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }

        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            startLabel.Text = $"{winner}\nWinnnnsssssss!!!!\nPlayer Again\n Space to play again\n ESC to stop";
            gameUpdateLoop.Enabled = false;
            startLabel.Visible = true;
            newGameOk = true;
            Refresh();

            // TODO create game over logic
            // --- stop the gameUpdateLoop
            // --- show a message on the startLabel to indicate a winner, (may need to Refresh).
            // --- use the startLabel to ask the user if they want to play again

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw player2 using FillRectangle
            e.Graphics.FillRectangle(goldBrush, player1);
            e.Graphics.FillRectangle(purpleBrush, player2);
            // TODO draw ball using FillRectangle
            e.Graphics.FillRectangle(whiteBrush, ball);

        }

    }
}
