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
        public static bool IsFood(Entity self, Entity entity)
        {
            return entity is FoodNode;
        }
        public static bool IsFish(Entity self, Entity entity)
        {
            return entity is Fish;
        }
        public static bool IsEnemyFish(Entity self, Entity entity)
        {
            return entity is RaycastFish rayFish && !((RaycastFish)self).FishConfig.DrawColor.Equals(rayFish.FishConfig.DrawColor);
        }
        public static bool IsBoundary(Entity self, Entity entity)
        {
            return entity is Boundary;
        }

        //Object
        private RaytraceTankConfig tankConfig;
        private RaycastFishConfig fishConfig;

        public RaytraceTank(float width, float height, RaytraceTankConfig tankConfig, RaycastFishConfig fishConfig, Random random) : base(width, height, random)
        {
            this.tankConfig = tankConfig;
            this.fishConfig = fishConfig;
            InitialSpawn();
        }

        private void InitialSpawn()
        {
            for (int i = 0; i < tankConfig.NumSpawn; i++)
            {
                RaycastFishConfig instanceConfig = fishConfig;
                instanceConfig.DrawColor = new SolidBrush(System.Drawing.Color.FromArgb(Random.Next(0, 256), Random.Next(0, 256), Random.Next(0, 256)));
                Fish newFish = new RaycastFish(RaycastFish.GetStandardBody(this), "Test Fish", Random, instanceConfig);
                AddEntity(newFish);
            }

            for (int i = 0; i < tankConfig.FoodQuantity; i++)
            {
                AddEntity(new FoodNode(new Vector2((float)Random.NextDouble() * Width, (float)Random.NextDouble() * Height), tankConfig.FoodValue, tickTimeout: 150));
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
