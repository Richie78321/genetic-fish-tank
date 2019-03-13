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

        //TEST
        private bool TestOneHot(Entity entity) { return true; }

        private Tank testTank;
        private void TankVisual_Load(object sender, EventArgs e)
        {
            //TEST 
            testTank = new Tank(Width, Height);
            testTank.AddEntity(new RaycastFish(new RigidBody(new RegularPolygon(new Vector2(Width / 2, Height / 2), 25, 3), DefinedMaterials.Wood), "Test Fish", new Random(), new RaycastFishConfig(7, (float)Math.PI, 125F, new VisualRaytracer.OneHotIndicator[] { TestOneHot, TestOneHot, TestOneHot }, 20)));
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
