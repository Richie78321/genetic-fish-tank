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
        public static void DrawDenseLayers(DenseLayer[] denseLayers, float drawWidth, float drawHeight, int selectedLayer, int selectedNeuron, PaintEventArgs e)
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
            int layerSelection = MathHelper.Clamp(selectedLayer, 0, neuronCounts.Length - 1);
            int neuronSelection = MathHelper.Clamp(selectedNeuron, 0, neuronCounts[layerSelection] - 1);
            if (layerSelection - 1 >= 0 && (layerSelection == neuronCounts.Length - 1 || neuronSelection > 0))
            {
                int neuronOffset = Convert.ToInt32(layerSelection != neuronCounts.Length - 1);
                //Draw input weights (not input and not bias neuron)
                for (int i = 0; i < neuronCounts[layerSelection - 1]; i++)
                {
                    DrawWeight(neuronPositions[layerSelection - 1][i], neuronPositions[layerSelection][neuronSelection], denseLayers[layerSelection - 1].BakedParameterValues[((neuronSelection - neuronOffset) * neuronCounts[layerSelection - 1]) + i], e);
                }
            }
            if (layerSelection + 1 < neuronCounts.Length)
            {
                int neuronOffset = Convert.ToInt32(layerSelection + 1 != neuronCounts.Length - 1);
                //Draw output weights (not output)
                for (int i = neuronOffset; i < neuronCounts[layerSelection + 1]; i++)
                {
                    DrawWeight(neuronPositions[layerSelection][neuronSelection], neuronPositions[layerSelection + 1][i], 
                        denseLayers[layerSelection].BakedParameterValues[(neuronCounts[layerSelection] * (i - neuronOffset)) + neuronSelection], e);
                }
            }
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
        private Label neuronInputLabel;

        public DataGUI(TankVisual tankForm, Panel neuralPanel, NumericUpDown layerNumeric, NumericUpDown neuronNumeric, Label neuronInputLabel)
        {
            this.tankForm = tankForm;
            this.neuralPanel = neuralPanel;
            this.layerNumeric = layerNumeric;
            this.neuronNumeric = neuronNumeric;
            this.neuronInputLabel = neuronInputLabel;

            neuralPanel.Paint += paintNeuralPanel;
            tankForm.MouseClick += mouseClick;

            layerNumeric.ValueChanged += NeuronNumeric_ValueChanged;
            neuronNumeric.ValueChanged += NeuronNumeric_ValueChanged;
        }

        private void NeuronNumeric_ValueChanged(object sender, EventArgs e)
        {
            neuralPanel.Invalidate();
        }

        private DenseLayer[] selectedBrain = null;
        private string[] selectedNeuronInputLabels = null;
        private void paintNeuralPanel(object sender, PaintEventArgs e)
        {
            if (selectedBrain != null)
            {
                //Update neuron input label
                if (layerNumeric.Value == 0 && neuronNumeric.Value != 0) neuronInputLabel.Text = selectedNeuronInputLabels[MathHelper.Clamp((int)neuronNumeric.Value - 1, 0, selectedNeuronInputLabels.Length - 1)];
                else neuronInputLabel.Text = "Not Input";

                DrawDenseLayers(selectedBrain, neuralPanel.Width, neuralPanel.Height, (int)layerNumeric.Value, (int)neuronNumeric.Value, e);
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

            //Get neuron input labels
            selectedNeuronInputLabels = fish.InputKey;

            //Redraw neural visual
            neuralPanel.Invalidate();
        }
    }
}
