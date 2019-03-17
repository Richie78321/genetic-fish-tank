using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishTank.Anima;
using ImpulseEngine2;
using ImpulseEngine2.Materials;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace FishTank
{
    class RaytraceTank : Tank
    {
        public static bool IsFood(Entity entity)
        {
            return entity is FoodNode;
        }
        public static bool IsFish(Entity entity)
        {
            return entity is Fish;
        }
        public static bool IsBoundary(Entity entity)
        {
            return entity is Boundary;
        }

        //Object
        private RaytraceTankConfig tankConfig;
        public RaytraceTank(float width, float height, RaytraceTankConfig tankConfig) : base(width, height, new Random())
        {
            this.tankConfig = tankConfig;
            InitialSpawn();
        }

        private void InitialSpawn()
        {
            for (int i = 0; i < tankConfig.NumSpawn; i++)
            {
                Fish newFish = new RaycastFish(RaycastFish.GetStandardBody(this), "Test Fish", Random, 
                    new RaycastFishConfig(5, (float)Math.PI, 200F, new VisualRaytracer.OneHotIndicator[] { IsBoundary, IsFish, IsFood }, 20, 10F, .01F, 15F, 10F, 300F, .02, Brushes.Blue));
                AddEntity(newFish);
            }

            for (int i = 0; i < tankConfig.FoodQuantity; i++)
            {
                AddEntity(new FoodNode(new Vector2((float)Random.NextDouble() * Width, (float)Random.NextDouble() * Height), tankConfig.FoodValue));
            }
        }
    }

    struct RaytraceTankConfig
    {
        public readonly int NumSpawn;
        public readonly int FoodQuantity;
        public readonly float FoodValue;

        public RaytraceTankConfig(int numSpawn, int foodQuantity, float foodValue)
        {
            NumSpawn = numSpawn;
            FoodQuantity = foodQuantity;
            FoodValue = foodValue;
        }
    }
}
