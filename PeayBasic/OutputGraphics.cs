using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PeayBasic
{
    public partial class OutputGraphics : Form
    {
        private PictureBox[,] cells;
        private List<Tuple<int, int>> changes;

        public OutputGraphics(List<Tuple<int, int>> change)
        {
            InitializeComponent();

            changes = change;
        }

        private void OutputGraphics_Load(object sender, EventArgs e)
        {
            cells = new PictureBox[20, 15];
            for (int k = 0; k < 15; k++)
            {
                for (int j = 0; j < 20; j++)
                {
                    cells[j, k] = new PictureBox();
                    cells[j, k].Size = new Size(32, 32);
                    cells[j, k].Left = j * 32;
                    cells[j, k].Top = k * 32;
                    cells[j, k].BackColor = Color.Black;
                    this.Controls.Add(cells[j, k]);
                }
            }

            foreach (Tuple<int, int> tile in changes)
            {
                if(cells[tile.Item1, tile.Item2].BackColor != Color.Black)
                    cells[tile.Item1, tile.Item2].BackColor = Color.Black;
                else
                    cells[tile.Item1, tile.Item2].BackColor = Color.White;
            }
        }
    }
}
