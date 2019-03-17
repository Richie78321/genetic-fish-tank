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
using ImpulseEngine2.Materials;

namespace FishTank.Anima
{
    class RaycastFish : Fish
    {
        private const int GLOBAL_INPUTS = 3;
        private const int OUTPUT_NUM = 2;

        public static ModularMember CreateModularMember(Random random, RaycastFishConfig fishConfig)
        {
            //Create phenotypes
            SequentialModelFactory sequentialModelFactory = new SequentialModelFactory(new int[] { (fishConfig.NumRaycasts * (fishConfig.RaycastOneHots.Length + 1)) + GLOBAL_INPUTS });
            sequentialModelFactory.AddModel(new DenseLayer(fishConfig.HiddenNeurons, ComputationModel.ReLUActivation));
            sequentialModelFactory.AddModel(new DenseLayer(OUTPUT_NUM, ComputationModel.LinearActivation));
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

        private const float TURN_DELTA = (float)Math.PI / 128;
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

        private const float BOUNDARY_PADDING = .01F;

        public static RigidBodyRef GetStandardBody(Tank fishTank)
        {
            return new RigidBodyRef(new RegularPolygon(new Vector2((float)fishTank.Random.NextDouble() * fishTank.Width, (float)fishTank.Random.NextDouble() * fishTank.Height), 10, 3), DefinedMaterials.Rubber, null);
        }

        //Object
        public override string Species => species;
        private string species;

        public readonly RaycastFishConfig FishConfig;
        public bool CanBreed => FoodValue >= FishConfig.BreedingThreshold;

        public RaycastFish(RigidBodyRef rigidBody, string species, Random random, RaycastFishConfig fishConfig) : base(CreateModularMember(random, fishConfig), rigidBody, fishConfig.StartingFoodValue)
        {
            this.species = species;
            this.FishConfig = fishConfig;
        }

        public RaycastFish(RigidBodyRef rigidBody, string species, ModularMember modularMember, RaycastFishConfig fishConfig) : base(modularMember, rigidBody, fishConfig.StartingFoodValue)
        {
            this.species = species;
            this.FishConfig = fishConfig;
        }

        //TEMP
        private const float DRAW_SIZE = 10F;
        public override void Draw(Tank fishTank, PaintEventArgs e)
        {
            DrawPolygon(RigidBody.CollisionPolygon, FishConfig.DrawColor, e);
            //e.Graphics.FillEllipse(Brushes.Blue, RigidBody.CollisionPolygon.CenterPoint.X - (DRAW_SIZE / 2), RigidBody.CollisionPolygon.CenterPoint.Y - (DRAW_SIZE / 2), DRAW_SIZE, DRAW_SIZE);
            //if (currentRaytraces != null)
            //{
            //    for (int i = 0; i < currentRaytraces.Length; i++)
            //    {
            //        Pen drawPen = Pens.Green;
            //        if (currentRaytraceData[i][0] != 1) drawPen = Pens.Red;
            //        e.Graphics.DrawLine(drawPen, currentRaytraces[i].EndPoints[0].X, currentRaytraces[i].EndPoints[0].Y, currentRaytraces[i].EndPoints[1].X, currentRaytraces[i].EndPoints[1].Y);
            //    }
            //}
        }

        public override void Update(Tank fishTank)
        {
            //Apply metabolism
            FoodValue -= FishConfig.Metabolism;
            if (FoodValue <= 0) HandleDeath(fishTank);

            //Check breeding
            if (CanBreed) HandleBreeding(fishTank);

            GetNextMove(fishTank);
        }

        private void HandleDeath(Tank fishTank)
        {
            //TEMP add extinction stuff
            fishTank.RemoveEntity(this);
        }

        private void HandleBreeding(Tank fishTank)
        {
            //Find eligible mates
            Entity[] mates = fishTank.ContainedEntities.Where(entity => entity != this && entity is RaycastFish rayFish && species.Equals(rayFish.species) && rayFish.CanBreed && LineSegment.Distance(rayFish.RigidBody.CollisionPolygon.CenterPoint, RigidBody.CollisionPolygon.CenterPoint) <= FishConfig.BreedingRadius).ToArray();
            Array.Sort(mates, (Entity x, Entity y) => { return ((Fish)x).FoodValue.CompareTo(((Fish)y).FoodValue); });

            if (mates.Length > 0)
            {
                //Mate
                Breed((RaycastFish)mates[0], fishTank);
            }
        }

        private void Breed(RaycastFish otherFish, Tank fishTank)
        {
            Vector2 spawnPoint = (otherFish.RigidBody.CollisionPolygon.CenterPoint + RigidBody.CollisionPolygon.CenterPoint) / 2;
            float spawnRadius = LineSegment.Distance(RigidBody.CollisionPolygon.CenterPoint, spawnPoint);

            List<ModularMember> childrenModules = new List<ModularMember>(4);
            childrenModules.AddRange(ModularMember.BreedMembers(ModularMember, otherFish.ModularMember, FishConfig.MutationRate, fishTank.Random));
            childrenModules.AddRange(ModularMember.BreedMembers(ModularMember, otherFish.ModularMember, FishConfig.MutationRate, fishTank.Random));

            //Brush familyColor = new SolidBrush(System.Drawing.Color.FromArgb(fishTank.Random.Next(0, 256), fishTank.Random.Next(0, 256), fishTank.Random.Next(0, 256)));

            for (int i = 0; i < childrenModules.Count; i++)
            {
                RigidBodyRef standardBody = GetStandardBody(fishTank);

                float deviationMagnitude = (float)fishTank.Random.NextDouble() * spawnRadius;
                float deviationAngle = (float)(fishTank.Random.NextDouble() * Math.PI * 2);
                standardBody.CollisionPolygon.TranslateTo(spawnPoint + new Vector2(deviationMagnitude * (float)Math.Cos(deviationMagnitude), deviationMagnitude * (float)Math.Sin(deviationMagnitude)));

                //Make dominant color by food
                RaycastFishConfig newConfig = FishConfig;
                if (otherFish.FoodValue > FoodValue) newConfig.DrawColor = otherFish.FishConfig.DrawColor;

                RaycastFish childFish = new RaycastFish(standardBody, species, childrenModules[i], newConfig);
                fishTank.AddEntity(childFish);
            }
            FoodValue -= FishConfig.BreedingCost;
            otherFish.FoodValue -= otherFish.FishConfig.BreedingCost;
        }

        private bool lastMovedLeft = true;
        private void GetNextMove(Tank fishTank)
        {
            //Run network
            double[] networkInput = BuildInput(fishTank);
            double[] networkOutput = (double[])((SequentialModel)ModularMember.Phenotypes[(int)Modules.NeuralNet]).Transform(networkInput);
            networkOutput = ComputationModel.SoftmaxFunction(networkOutput);

            //Act according to network
            if (networkOutput[0] > networkOutput[1])
            {
                //Turn left
                RigidBody.CollisionPolygon.Rotate(-TURN_DELTA, RigidBody.CollisionPolygon.CenterPoint);
                lastMovedLeft = true;
            }
            else
            {
                //Turn right
                RigidBody.CollisionPolygon.Rotate(TURN_DELTA, RigidBody.CollisionPolygon.CenterPoint);
                lastMovedLeft = false;
            }

            //Move forward and ensure bounds
            Vector2 targetPoint = RigidBody.CollisionPolygon.CenterPoint + (new Vector2((float)Math.Cos(RigidBody.CollisionPolygon.TotalRotation - (Math.PI / 2)), (float)Math.Sin(RigidBody.CollisionPolygon.TotalRotation - (Math.PI / 2))) * MOVE_SPEED);
            targetPoint = Vector2.Clamp(targetPoint, Vector2.Zero + (Vector2.One * BOUNDARY_PADDING), new Vector2(fishTank.Width, fishTank.Height) - (Vector2.One * BOUNDARY_PADDING));
            RigidBody.CollisionPolygon.TranslateTo(targetPoint);
        }

        private LineSegment[] currentRaytraces = null;
        private double[][] currentRaytraceData = null;
        private double[] BuildInput(Tank fishTank)
        {
            List<double> inputs = new List<double>();

            //Add fish information
            inputs.Add((RigidBody.CollisionPolygon.TotalRotation % (2 * Math.PI)) / (2 * Math.PI));
            inputs.Add(ComputationModel.SigmoidActivation(FoodValue));
            inputs.Add(Convert.ToDouble(lastMovedLeft));

            //Add raytrace data
            currentRaytraces = CreateVisualRays(RigidBody.CollisionPolygon.CenterPoint, RigidBody.CollisionPolygon.TotalRotation, FishConfig);
            Entity[] entitiesToCheck = fishTank.ContainedEntities.Where(entity => entity != this && LineSegment.Distance(entity.RigidBody.CollisionPolygon.CenterPoint, RigidBody.CollisionPolygon.CenterPoint) <= entity.RigidBody.CollisionPolygon.MaximumRadius + FishConfig.RaycastLength).ToArray();
            currentRaytraceData = new double[currentRaytraces.Length][];
            for (int i = 0; i < currentRaytraces.Length; i++)
            {
                double[] raytraceOutput = VisualRaytracer.ProcessRaytrace(this, entitiesToCheck, currentRaytraces[i], FishConfig.RaycastOneHots);
                currentRaytraceData[i] = raytraceOutput;
                inputs.AddRange(raytraceOutput);
            }

            return inputs.ToArray();
        }

        public override void OnIntersection(Entity otherEntity, Tank fishTank)
        {
            if (FishConfig.Carnivore && otherEntity is RaycastFish otherFish && !otherFish.FishConfig.DrawColor.Equals(FishConfig.DrawColor) && otherFish.FoodValue < FoodValue)
            {
                //Eat fish
                FoodValue += FishConfig.CarnivorousFoodValue;
                fishTank.RemoveEntity(otherEntity);
            }
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

        public readonly float StartingFoodValue;
        public readonly float Metabolism;

        public readonly float BreedingThreshold;
        public readonly float BreedingCost;
        public readonly float BreedingRadius;

        public readonly double MutationRate;

        public readonly bool Carnivore;
        public readonly float CarnivorousFoodValue;

        public Brush DrawColor;

        public RaycastFishConfig(int NumRaycasts, float TotalRaycastAngle, float RaycastLength, VisualRaytracer.OneHotIndicator[] RaycastOneHots, int HiddenNeurons, float StartingFoodValue, float Metabolism, float BreedingThreshold, float BreedingCost, float BreedingRadius, double MutationRate, Brush DrawColor, bool Carnivore, float CarnivorousFoodValue)
        {
            this.NumRaycasts = NumRaycasts;
            this.TotalRaycastAngle = TotalRaycastAngle;
            this.RaycastLength = RaycastLength;
            this.RaycastOneHots = RaycastOneHots;
            this.HiddenNeurons = HiddenNeurons;
            RaycastAngleDelta = TotalRaycastAngle / (NumRaycasts - 1);

            this.StartingFoodValue = StartingFoodValue;
            this.Metabolism = Metabolism;
            this.BreedingThreshold = BreedingThreshold;
            this.BreedingCost = BreedingCost;
            this.BreedingRadius = BreedingRadius;
            this.MutationRate = MutationRate;
            this.DrawColor = DrawColor;
            this.Carnivore = Carnivore;
            this.CarnivorousFoodValue = CarnivorousFoodValue;
        }
    }
}
