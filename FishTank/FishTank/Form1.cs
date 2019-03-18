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
            RaycastFishConfig fishConfig = new RaycastFishConfig(
                NumRaycasts: 5,
                TotalRaycastAngle: (float)Math.PI, 
                RaycastLength: 200F, 
                RaycastOneHots: new VisualRaytracer.OneHotIndicator[] { RaytraceTank.IsBoundary, RaytraceTank.IsEnemyFish, RaytraceTank.IsFood }, //Data collected from raycasts, onehot encoded
                HiddenNeurons: 8, //Neurons in hidden layer
                StartingFoodValue: 10F,
                Metabolism: .005F, //Food lost per tick
                BreedingThreshold: 15F, //Required food to breed
                BreedingCost: 10F, //Food cost of breeding
                BreedingRadius: 300F, //Radius to search for mates
                MutationRate: .025, //Rate of mutation in children
                DrawColor: null, 
                Carnivore: false, //Can eat other fish
                CarnivorousFoodValue: 2.5F //Food value from eating other fish
                );

            testTank = new RaytraceTank(Width, Height, new RaytraceTankConfig(
                numSpawn: 50, //Number of initial fish
                foodQuantity: 75, //Quantity of food nodes (this does not change)
                foodValue: 2.5F //The food value from each node
                ), fishConfig, new Random());
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
