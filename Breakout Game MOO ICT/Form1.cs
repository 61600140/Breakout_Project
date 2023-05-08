using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout_Game_MOO_ICT
{
    public partial class Form1 : Form
    {

        //Lessons for learning WinForms courtesy of Moo ICT & ChatGPT
        //Game difficulty/longevity can be changed on line 28, only use integers

        bool goLeft;
        bool goRight;
        bool isGameOver;
        
        int score;
        int ballx;
        int bally;
        int playerSpeed;

        int level = 1;
        //changes difficulty here
        int longevity = 1;

        Random rnd = new Random();

        PictureBox[] blockArray;

        public Form1()
        {
            InitializeComponent();

            PlaceBlocks();
        }

        private void setupGame()
        {
            //init everything
            isGameOver = false;
            score = 0;
            ballx = 5 + level * 2;
            bally = 5+level*2;
            playerSpeed = 12;
            txtScore.Text = "Score: " + score;
            lvlScore.Text = "Level: " + level;

            ball.Left = 376;
            ball.Top = 328;

            player.Left = 347;



            gameTImer.Start();
            //random colored blocks
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }
        }

        //call when ball doesn't hit paddle on the bottom segment
        private void gameOver(string message)
        {
            isGameOver = true;
            gameTImer.Stop();

            txtScore.Text = "Score: " + score + " " + message;
        }

        //to be initialized when blocks need to be constructed
        private void PlaceBlocks()
        
        {
            
            blockArray = new PictureBox[15*(longevity)] ;

            int a = 0;

            int top = 10;
            int left = 25/longevity;

            for(int i = 0; i < blockArray.Length; i++)
            {
                blockArray[i] = new PictureBox();
                blockArray[i].Height = 40/longevity;
                blockArray[i].Width = 140/longevity;
                blockArray[i].Tag = "blocks";
                blockArray[i].BackColor = Color.White;


                if(a == 5*longevity)
                {
                    top = top + 50/longevity;
                    left = 25/longevity;
                    a = 0;
                }

                if(a < 5 * longevity)
                {
                    a++;
                    blockArray[i].Left = left;
                    blockArray[i].Top = top;
                    this.Controls.Add(blockArray[i]);
                    left = left + 155/longevity;
                }

            }
            setupGame();
        }


        //when the game is over, this clears all blocks in the array
        private void removeBlocks()
        {
            foreach(PictureBox x in blockArray)
            {
                this.Controls.Remove(x);
            }
        }


        //all of the logic
        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            lvlScore.Text = "Level: " + level;

            //bounce physics
            if (goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }

            if (goRight == true && player.Left < 700)
            {
                player.Left += playerSpeed;
            }

            ball.Left += ballx;
            ball.Top += bally;

            if (ball.Left < 0 || ball.Left > 775)
            {
                ballx = -ballx;
            }
            if (ball.Top < 0)
            {
                bally = -bally;
            }
            //intersect logic
            if (ball.Bounds.IntersectsWith(player.Bounds))
            {

                ball.Top = 463;

                bally = rnd.Next(5+(level-1)*2, 12 + (level - 1) * 2) * -1;

                if (ballx < 0)
                {
                    ballx = rnd.Next(5 + (level - 1) * 2, 12 + (level - 1) * 2) * -1;
                }
                else
                {
                    ballx = rnd.Next(5 + (level - 1) * 2, 12 + (level - 1) * 2);
                }
            }
            //intersect removal
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    if(ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;

                        bally = -bally;

                        this.Controls.Remove(x);
                    }
                }

            }

            //game over calling
            if (score == 15 *longevity)
            {
                level++;
                PlaceBlocks();
            }

            if(ball.Top > 580)
            {
                gameOver("You Lose!! Press Enter to try again");
                level = 1;
            }



        }
        //controls
        private void keyisdown(object sender, KeyEventArgs e)
        {

            if(e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                goRight = true;
            }

        }
        //controls vol. 2
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if(e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeBlocks();
                PlaceBlocks();
            }
        }
        //loads Form1
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
