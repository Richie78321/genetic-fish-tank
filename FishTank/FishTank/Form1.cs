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

namespace FishTank
{
    public partial class TankVisual : Form
    {
        public TankVisual()
        {
            InitializeComponent();
        }

        private void TankVisual_Load(object sender, EventArgs e)
        {
            //TEST 
            RaycastFish fish = new RaycastFish(new RigidBody(new RegularPolygon(Ve)))
        }

        private void paint(object sender, PaintEventArgs e)
        {

        }

        private void UpdateTank()
        {

        }

        private void autoUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateTank();
        }
    }
}
