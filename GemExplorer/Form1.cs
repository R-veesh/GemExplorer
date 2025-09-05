using System;
using System.Drawing;
using System.Windows.Forms;

namespace GemExplorer
{
    public partial class Form1 : Form
    {
        private const int gridSize = 10;
        private const int cellSize = 50;
        private Button[,] gridButtons = new Button[gridSize, gridSize];
        private Random rand = new Random();
        private int playerX = 0, playerY = 0;
        private int score = 0;
        private int totalGems = 0;


        public Form1()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            // Panel settings
            panel1.Size = new Size(gridSize * cellSize, gridSize * cellSize);
            panel1.Location = new Point(20, 60);

            // Label settings
            label1.Text = "Score: 0";
            label1.Font = new Font("Arial", 14, FontStyle.Bold);
            label1.Location = new Point(20, 20);

            // Button settings
            BtnStart.Text = "Start Game";
            BtnStart.Location = new Point(200, 20);
            BtnStart.Click += BtnStart_Click;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            score = 0;
            playerX = 0; playerY = 0;
            InitGrid();
            UpdatePlayerPosition();
            label1.Text = "Score: 0";
        }

        private void InitGrid()
        {
            totalGems = 0; // reset gem count

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(cellSize, cellSize);
                    btn.Location = new Point(x * cellSize, y * cellSize);
                    btn.Tag = "empty";
                    btn.BackgroundImageLayout = ImageLayout.Stretch;

                    // Randomly place gems
                    if (rand.Next(0, 100) < 15)
                    {
                        btn.Tag = "diamond";
                        btn.BackgroundImage = Properties.Resources.gem;
                        totalGems++;
                    }

                    gridButtons[x, y] = btn;
                    panel1.Controls.Add(btn);
                }
            }
        }


        private void UpdatePlayerPosition()
        {
            // Reset images
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (gridButtons[x, y].Tag.ToString() == "diamond")
                        gridButtons[x, y].BackgroundImage = Properties.Resources.gem;
                    else
                        gridButtons[x, y].BackgroundImage = null;
                }
            }

            // Collect gem
            if (gridButtons[playerX, playerY].Tag.ToString() == "diamond")
            {
                score++;
                totalGems--; // one gem collected
                gridButtons[playerX, playerY].Tag = "empty";
                label1.Text = $"Score: {score}";

                // Check for game over
                if (totalGems == 0)
                {
                    MessageBox.Show("🎉 Game Over! You collected all gems!", "Gem Explorer");
                }
            }


            // Show player
            gridButtons[playerX, playerY].BackgroundImage = Properties.Resources.player; // player.png in Resources
        }

  

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up: if (playerY > 0) playerY--; break;
                case Keys.Down: if (playerY < gridSize - 1) playerY++; break;
                case Keys.Left: if (playerX > 0) playerX--; break;
                case Keys.Right: if (playerX < gridSize - 1) playerX++; break;
            }

            UpdatePlayerPosition();
            return true;
        }
    }
}
