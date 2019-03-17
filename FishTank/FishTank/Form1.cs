using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FishTank.Anima;
using ImpulseEngine2;
using Microsoft.Xna.Framework;
using ImpulseEngine2.Materials;

namespace FishTank
{
    public partial class TankVisual : Form
    {
        public TankVisual()
        {
            InitializeComponent();
        }

        private Tank testTank;
        private void TankVisual_Load(object sender, EventArgs e)
        {
            //TEST 
            testTank = new RaytraceTank(Width, Height, new RaytraceTankConfig(50, 75, 2.5F));
        }

        private void paint(object sender, PaintEventArgs e)
        {
            testTank.Draw(e);
        }

        private void UpdateTank()
        {
            testTank.Update();
        }

        private void autoUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateTank();
            Invalidate();
        }
    }
}
