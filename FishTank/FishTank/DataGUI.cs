using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModularGenetics.AI.Dense;
using System.Drawing;
using System.Windows.Forms;
using FishTank.Anima;
using Microsoft.Xna.Framework;
using ModularGenetics.AI;

namespace FishTank
{
    class DataGUI
    {
        private const float NEURON_DRAW_SCALE = .5F;
        public static void DrawDenseLayers(DenseLayer[] denseLayers, float drawWidth, float drawHeight, PaintEventArgs e)
        {
            int[] neuronCounts = new int[denseLayers.Length + 1];
            neuronCounts[0] = denseLayers[0].InputShape[0];
            for (int i = 0; i < denseLayers.Length; i++) neuronCounts[i + 1] = denseLayers[i].Neurons + Convert.ToInt32(i != denseLayers.Length - 1);

            float layerDrawWidth = drawWidth / neuronCounts.Length;
            float neuronHeightBound = drawHeight / neuronCounts.Max();
            float neuronDrawSize = Math.Min(neuronHeightBound, layerDrawWidth) * NEURON_DRAW_SCALE;

            for (int i = 0; i < neuronCounts.Length; i++)
            {
                for (int j = 0; j < neuronCounts[i]; j++)
                {
                    PointF drawCenter = new PointF((layerDrawWidth * i) + (layerDrawWidth / 2), (neuronHeightBound * j) + (neuronHeightBound / 2));
                    e.Graphics.FillEllipse(Brushes.Blue, drawCenter.X - (neuronDrawSize / 2), drawCenter.Y - (neuronDrawSize / 2), neuronDrawSize, neuronDrawSize);
                }
            }
        }

        //Object
        private TankVisual tankForm;
        private Panel neuralPanel;

        public DataGUI(TankVisual tankForm, Panel neuralPanel)
        {
            this.tankForm = tankForm;
            this.neuralPanel = neuralPanel;

            neuralPanel.Paint += paintNeuralPanel;
            tankForm.MouseClick += mouseClick;
        }

        private DenseLayer[] selectedBrain = null;
        private void paintNeuralPanel(object sender, PaintEventArgs e)
        {
            if (selectedBrain != null)
            {
                DrawDenseLayers(selectedBrain, neuralPanel.Width, neuralPanel.Height, e);
            }
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            PointF clientMousePos = tankForm.RelativeMousePos;
            Vector2 mousePosition = new Vector2(clientMousePos.X, clientMousePos.Y);

            //Check for fish selection
            Entity[] entities = tankForm.CurrentTank.ContainedEntities.Where(t => t is Fish).ToArray();
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].RigidBody.CollisionPolygon.ContainsPoint(mousePosition))
                {
                    Fish fish = ((Fish)entities[i]);
                    SelectFish(fish);
                    break;
                }
            }
        }

        public void SelectFish(Fish fish)
        {
            //Get Neural Net
            SequentialModel fishNN = (SequentialModel)fish.ModularMember.Phenotypes[(int)Fish.Modules.NeuralNet];
            selectedBrain = new DenseLayer[fishNN.Phenotypes.Length];
            for (int i = 0; i < fishNN.Phenotypes.Length; i++) selectedBrain[i] = (DenseLayer)fishNN.Phenotypes[i];

            //Redraw neural visual
            neuralPanel.Invalidate();
        }
    }
}
