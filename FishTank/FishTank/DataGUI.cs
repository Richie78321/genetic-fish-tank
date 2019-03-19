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
        public static PointF[][] DrawDenseLayers(DenseLayer[] denseLayers, float drawWidth, float drawHeight, PaintEventArgs e)
        {
            int[] neuronCounts = new int[denseLayers.Length + 1];
            neuronCounts[0] = denseLayers[0].InputShape[0] + 1;
            for (int i = 0; i < denseLayers.Length; i++) neuronCounts[i + 1] = denseLayers[i].Neurons + Convert.ToInt32(i != denseLayers.Length - 1);

            float layerDrawWidth = drawWidth / neuronCounts.Length;
            float neuronHeightBound = drawHeight / neuronCounts.Max();
            float neuronDrawSize = Math.Min(neuronHeightBound, layerDrawWidth) * NEURON_DRAW_SCALE;

            //Draw neurons and find positions
            PointF[][] neuronPositions = new PointF[neuronCounts.Length][];
            for (int i = 0; i < neuronCounts.Length; i++)
            {
                float layerHeightOffset = (drawHeight - (neuronHeightBound * neuronCounts[i])) / 2;
                neuronPositions[i] = new PointF[neuronCounts[i]];
                for (int j = 0; j < neuronCounts[i]; j++)
                {
                    PointF drawCenter = new PointF((layerDrawWidth * i) + (layerDrawWidth / 2), (neuronHeightBound * j) + (neuronHeightBound / 2) + layerHeightOffset);
                    neuronPositions[i][j] = drawCenter;
                    e.Graphics.FillEllipse(Brushes.Blue, drawCenter.X - (neuronDrawSize / 2), drawCenter.Y - (neuronDrawSize / 2), neuronDrawSize, neuronDrawSize);
                }
            }

            //Draw weights
            for (int i = 1; i < neuronCounts.Length; i++)
            {
                int biasOffset = Convert.ToInt32(i != neuronCounts.Length - 1);
                for (int j = biasOffset; j < neuronCounts[i]; j++)
                {
                    for (int k = 0; k < neuronCounts[i - 1]; k++)
                    {
                        DrawWeight(neuronPositions[i - 1][k], neuronPositions[i][j], denseLayers[i - 1].BakedParameterValues[((j - biasOffset) * neuronCounts[i - 1]) + k], e);
                    }
                }
            }

            return neuronPositions;
        }

        private const float MIN_LINE_WEIGHT = .1F, MAX_LINE_WEIGHT = 5F;
        public static void DrawWeight(PointF neuron1, PointF neuron2, double weightValue, PaintEventArgs e)
        {
            System.Drawing.Color drawColor = System.Drawing.Color.Green;
            if (weightValue < 0) drawColor = System.Drawing.Color.Red;
            Brush brush = new SolidBrush(drawColor);

            Pen drawPen = new Pen(brush, MIN_LINE_WEIGHT + ((MAX_LINE_WEIGHT - MIN_LINE_WEIGHT) * (float)Math.Abs(weightValue)));
            e.Graphics.DrawLine(drawPen, neuron1, neuron2);
        }

        //Object
        private TankVisual tankForm;
        private Panel neuralPanel;

        private NumericUpDown layerNumeric;
        private NumericUpDown neuronNumeric;

        public DataGUI(TankVisual tankForm, Panel neuralPanel, NumericUpDown layerNumeric, NumericUpDown neuronNumeric)
        {
            this.tankForm = tankForm;
            this.neuralPanel = neuralPanel;
            this.layerNumeric = layerNumeric;
            this.neuronNumeric = neuronNumeric;

            neuralPanel.Paint += paintNeuralPanel;
            tankForm.MouseClick += mouseClick;
        }

        private DenseLayer[] selectedBrain = null;
        private PointF[][] neuronPositions = null;
        private void paintNeuralPanel(object sender, PaintEventArgs e)
        {
            if (selectedBrain != null)
            {
                neuronPositions = DrawDenseLayers(selectedBrain, neuralPanel.Width, neuralPanel.Height, e);
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
