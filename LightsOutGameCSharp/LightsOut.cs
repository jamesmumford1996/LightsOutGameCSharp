using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LightsOutGameCSharp
{

    public partial class LightsOut : Form
    {
        readonly Color onColor = Color.LightGreen;
        readonly Color offColor = Color.DarkGreen;
        const int gameSize = 5;
        readonly Button[,] lightArr = new Button[gameSize, gameSize];

        public LightsOut()
        {
            InitializeComponent();
            DrawBoard();
            InitialiseGame();
        }
        private void DrawBoard()
        {
            //Create the button objects and draw them onto the window

            const int lightSpacing = 20;
            const int lightSize = 50;
            const int topOffset = 50;
            const int leftOffset = 3;

            //Create buttons for the whole array
            for (int i = 0; i < gameSize; i++)
            {
                for (int j = 0; j < gameSize; j++)
                {
                    lightArr[i, j] = new Button
                    {
                        Width = lightSize,
                        Height = lightSize,
                        Name = "btnLightX" + i + "Y" + j,
                        //Position each button in a grid
                        Location = new Point(((lightSize + lightSpacing) * i) + leftOffset, ((lightSize + lightSpacing) * j) + topOffset),
                        BackColor = offColor,
                        //Store the 2D index of the light in the button tag so we can identify them when they are clicked
                        Tag = (i, j)
                    };

                    //Draw the buttons onto the window and add event handlers
                    Controls.Add(lightArr[i, j]);
                    lightArr[i, j].Click += new EventHandler(HandleLightClick);
                }
            }

            //Size our window for the grid
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MaximizeBox = false;
        }
        private void InitialiseGame()
        {
            //Set the starting grid of lights
            //To ensure this creates a solvable puzzle, start with all lights off and click random lights

            //Set all lights to off
            for (int i = 0; i < gameSize; i++)
            {
                for (int j = 0; j < gameSize; j++)
                {
                    lightArr[i, j].BackColor = offColor;
                }
            }
            //Create a 1D list of all lights on the board
            List<int> numberList = Enumerable.Range(0, (gameSize * gameSize) - 1).ToList();
            Random randomGenerator = new Random();
            //Click a random number of lights, at least 5 so there is some challenge
            const int minSwitches = 5;
            int numberOfSwitches = randomGenerator.Next(minSwitches, numberList.Count);
            int randomLight;
            for (int index = 0; index < numberOfSwitches; index++)
            {
                //Get a random light from the list
                randomLight = randomGenerator.Next(0, numberList.Count);
                //Convert the 1D list index to a 2D grid index and toggle it
                int xPos = randomLight % gameSize;
                int yPos = randomLight / gameSize;
                ToggleLight(xPos, yPos);
                //Remove this light from the list so we don't toggle it again
                numberList.RemoveAt(randomLight);
            }
        }
        private void HandleLightClick(Object sender, EventArgs e)
        {
            //Handles a light button click and checks for winning state

            //Extract 2D index from the event
            Button btn = (Button)sender;
            (int, int) arrPos = ((int, int))btn.Tag;
            int xPos = arrPos.Item1;
            int yPos = arrPos.Item2;

            //Flip appropriate lights
            ToggleLight(xPos, yPos);

            //Check if all lights are out
            if (CheckWin())
            {
                MessageBox.Show("You Win! Try a new puzzle.", "Congratulations!");
                InitialiseGame();
            }
        }
        private void ToggleLight(int xPos, int yPos)
        {
            //Change my color
            ChangeButtonColor(xPos, yPos);
            //Check if we are at any of the edges & flip adjacent light colors
            if (xPos > 0)
            {
                ChangeButtonColor(xPos - 1, yPos);
            }
            if (xPos < gameSize - 1)
            {
                ChangeButtonColor(xPos + 1, yPos);
            }
            if (yPos > 0)
            {
                ChangeButtonColor(xPos, yPos - 1);
            }
            if (yPos < gameSize - 1)
            {
                ChangeButtonColor(xPos, yPos + 1);
            }
        }
        private void ChangeButtonColor(int xPos, int yPos)
        {
            //Changes the color of a specified button
            if (lightArr[xPos, yPos].BackColor == offColor)
            {
                lightArr[xPos, yPos].BackColor = onColor;
            }
            else
            {
                lightArr[xPos, yPos].BackColor = offColor;
            }
        }
        private bool CheckWin()
        {
            //Check if all buttons on the grid are off (dark) using a LINQ query
            bool result = lightArr.OfType<Button>().All(btnTemp => btnTemp.BackColor == offColor);
            return result;
        }
        //Event handlers for predefined objects
        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            InitialiseGame();
        }


    }
}
