﻿using System;
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
using ModularGenetics.AI.Dense;

namespace FishTank
{
    public partial class TankVisual : Form
    {
        public PointF RelativeMousePos => PointToClient(MousePosition);

        public TankVisual()
        {
            InitializeComponent();
        }

        private Tank currentTank;
        public Tank CurrentTank => currentTank;
        private DataGUI dataGUI;
        private void TankVisual_Load(object sender, EventArgs e)
        {
            RaycastFishConfig fishConfig = new RaycastFishConfig(
                NumRaycasts: 5,
                TotalRaycastAngle: (float)Math.PI,
                RaycastLength: 200F,
                RaycastOneHots: new VisualRaytracer.OneHotIndicator[] { RaytraceTank.IsBoundary, RaytraceTank.IsEnemyFish, RaytraceTank.IsFood }, //Data collected from raycasts, onehot encoded
                OneHotLabels: new string[] { "Is Boundary?", "Is Enemy Fish?", "Is Food?" },
                HiddenNeurons: 8, //Neurons in hidden layer
                StartingFoodValue: 10F,
                Metabolism: .005F, //Food lost per tick
                BreedingThreshold: 15F, //Required food to breed
                BreedingCost: 10F, //Food cost of breeding
                BreedingRadius: 300F, //Radius to search for mates
                MutationRate: .05, //Rate of mutation in children
                DrawColor: null, 
                Carnivore: false, //Can eat other fish
                CarnivorousFoodValue: 2.5F //Food value from eating other fish
                );

            currentTank = new RaytraceTank(Width / 2, Height, new RaytraceTankConfig(
                numSpawn: 100, //Number of initial fish
                foodQuantity: 30, //Quantity of food nodes (this does not change)
                foodValue: 2.5F //The food value from each node
                ), 
                fishConfig, 
                new Random(),
                new DataCollection(tickResolution: 10)); //Resolution of data collection

            dataGUI = new DataGUI(this, neuralPanel, layerIndexNumeric, neuronIndexNumeric, neuronInputLabel);

            this.FormClosed += TankVisual_FormClosed;
        }

        private void TankVisual_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Save data
            currentTank.DataCollector.SaveData();
        }

        private void paint(object sender, PaintEventArgs e)
        {
            currentTank.Draw(e);
        }

        private void UpdateTank()
        {
            currentTank.Update();
        }

        private void autoUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateTank();
            Invalidate();
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) autoUpdateTimer.Enabled = !autoUpdateTimer.Enabled;
        }
    }
}
