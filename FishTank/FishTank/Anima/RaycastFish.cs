using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModularGenetics;
using ModularGenetics.AI;
using ModularGenetics.AI.Dense;
using ImpulseEngine2;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace FishTank.Anima
{
    class RaycastFish : Fish
    {
        private const int GLOBAL_INPUTS = 2;
        private const int OUTPUT_NUM = 2;

        public static ModularMember CreateModularMember(Random random, RaycastFishConfig fishConfig)
        {
            //Create phenotypes
            SequentialModelFactory sequentialModelFactory = new SequentialModelFactory(new int[] { (fishConfig.NumRaycasts * (fishConfig.RaycastOneHots.Length + 1)) + GLOBAL_INPUTS });
            sequentialModelFactory.AddModel(new DenseLayer(fishConfig.HiddenNeurons, ComputationModel.ReLUActivation));
            sequentialModelFactory.AddModel(new DenseLayer(OUTPUT_NUM, ComputationModel.ReLUActivation));
            Phenotype[] phenotypes = new Phenotype[1];
            phenotypes[(int)Modules.NeuralNet] = sequentialModelFactory.DeployModel();

            return new ModularMember(phenotypes, random);
        }

        public static LineSegment[] CreateVisualRays(Vector2 origin, float totalRotation, RaycastFishConfig fishConfig)
        {
            float rotationAngle = totalRotation - (float)(Math.PI / 2);
            LineSegment[] visualRays = new LineSegment[fishConfig.NumRaycasts];

            int incrementLevels = (int)Math.Ceiling((double)(fishConfig.NumRaycasts + 1) / 2);
            for (int i = 0; i < incrementLevels; i++)
            {
                if (i != 0)
                {
                    for (int j = 0; j < 2 && (i * 2) + j - 1 < fishConfig.NumRaycasts; j++)
                    {
                        float angle = rotationAngle + (fishConfig.RaycastAngleDelta * i * ((j * 2) - 1));
                        Vector2 rotationPoint = new Vector2((float)Math.Cos(angle) * fishConfig.RaycastLength, (float)Math.Sin(angle) * fishConfig.RaycastLength);
                        visualRays[(i * 2) + j - 1] = new LineSegment(origin, rotationPoint + origin);
                    }
                }
                else
                {
                    Vector2 rotationPoint = new Vector2((float)Math.Cos(rotationAngle) * fishConfig.RaycastLength, (float)Math.Sin(rotationAngle) * fishConfig.RaycastLength);
                    visualRays[0] = new LineSegment(origin, rotationPoint + origin);
                }
            }

            return visualRays;
        }

        private const float TURN_DELTA = (float)Math.PI / 48;
        private const float MOVE_SPEED = 1F;

        private enum Modules
        {
            NeuralNet
        }

        public static void DrawPolygon(Polygon polygon, Brush brush, PaintEventArgs e)
        {
            Vector2[] vertices = polygon.Vertices;
            PointF[] points = new PointF[vertices.Length];
            for (int i = 0; i < points.Length; i++) points[i] = new PointF(vertices[i].X, vertices[i].Y);
            e.Graphics.FillPolygon(brush, points);
        }

        //Object
        public override string Species => species;
        private string species;

        private RaycastFishConfig fishConfig;

        public RaycastFish(RigidBody rigidBody, string species, Random random, RaycastFishConfig fishConfig) : base(CreateModularMember(random, fishConfig), rigidBody)
        {
            this.species = species;
            this.fishConfig = fishConfig;
        }

        //TEMP
        private const float DRAW_SIZE = 10F;
        public override void Draw(Tank fishTank, PaintEventArgs e)
        {
            DrawPolygon(RigidBody.CollisionPolygon, Brushes.Blue, e);
            //e.Graphics.FillEllipse(Brushes.Blue, RigidBody.CollisionPolygon.CenterPoint.X - (DRAW_SIZE / 2), RigidBody.CollisionPolygon.CenterPoint.Y - (DRAW_SIZE / 2), DRAW_SIZE, DRAW_SIZE);
            if (currentRaytraces != null)
                for (int i = 0; i < currentRaytraces.Length; i++) e.Graphics.DrawLine(Pens.Red, currentRaytraces[i].EndPoints[0].X, currentRaytraces[i].EndPoints[0].Y, currentRaytraces[i].EndPoints[1].X, currentRaytraces[i].EndPoints[1].Y);
        }

        public override void Update(Tank fishTank)
        {
            //Run network
            double[] networkInput = BuildInput(fishTank);
            double[] networkOutput = (double[])((SequentialModel)ModularMember.Phenotypes[(int)Modules.NeuralNet]).Transform(networkInput);

            //Act according to network
            if (networkOutput[0] > networkOutput[1])
            {
                //Turn left
                RigidBody.CollisionPolygon.Rotate(-TURN_DELTA, RigidBody.CollisionPolygon.CenterPoint);
            }
            else
            {
                //Turn right
                RigidBody.CollisionPolygon.Rotate(TURN_DELTA, RigidBody.CollisionPolygon.CenterPoint);
            }

            //Move forward and ensure bounds
            Vector2 targetPoint = RigidBody.CollisionPolygon.CenterPoint + (new Vector2((float)Math.Cos(RigidBody.CollisionPolygon.TotalRotation - (Math.PI / 2)), (float)Math.Sin(RigidBody.CollisionPolygon.TotalRotation - (Math.PI / 2))) * MOVE_SPEED);
            targetPoint = Vector2.Clamp(targetPoint, Vector2.Zero, new Vector2(fishTank.Width, fishTank.Height));
            RigidBody.CollisionPolygon.TranslateTo(targetPoint);
        }

        private LineSegment[] currentRaytraces = null;
        private double[] BuildInput(Tank fishTank)
        {
            List<double> inputs = new List<double>();

            //Add fish information
            inputs.Add((RigidBody.CollisionPolygon.TotalRotation % (2 * Math.PI)) / (2 * Math.PI));
            inputs.Add(hunger);

            //Add raytrace data
            currentRaytraces = CreateVisualRays(RigidBody.CollisionPolygon.CenterPoint, RigidBody.CollisionPolygon.TotalRotation, fishConfig);
            Entity[] entitiesToCheck = fishTank.ContainedEntities.Where(entity => entity != this).ToArray();
            for (int i = 0; i < currentRaytraces.Length; i++)
            {
                inputs.AddRange(VisualRaytracer.ProcessRaytrace(entitiesToCheck, currentRaytraces[i], fishConfig.RaycastOneHots));
            }

            return inputs.ToArray();
        }
    }

    struct RaycastFishConfig
    {
        public readonly int NumRaycasts;
        public readonly float TotalRaycastAngle;
        public readonly float RaycastAngleDelta;
        public readonly float RaycastLength;
        public readonly VisualRaytracer.OneHotIndicator[] RaycastOneHots;
        public readonly int HiddenNeurons;

        public RaycastFishConfig(int NumRaycasts, float TotalRaycastAngle, float RaycastLength, VisualRaytracer.OneHotIndicator[] RaycastOneHots, int HiddenNeurons)
        {
            this.NumRaycasts = NumRaycasts;
            this.TotalRaycastAngle = TotalRaycastAngle;
            this.RaycastLength = RaycastLength;
            this.RaycastOneHots = RaycastOneHots;
            this.HiddenNeurons = HiddenNeurons;
            RaycastAngleDelta = TotalRaycastAngle / (NumRaycasts - 1);
        }
    }
}
