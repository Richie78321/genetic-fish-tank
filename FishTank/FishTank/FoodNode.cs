using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImpulseEngine2;
using System.Drawing;
using Microsoft.Xna.Framework;
using ImpulseEngine2.Materials;
using FishTank.Anima;

namespace FishTank
{
    class FoodNode : Entity
    {
        private const float NODE_SIZE = 5F;

        //Object
        public override RigidBodyRef RigidBody => rigidBody;
        private RigidBodyRef rigidBody;

        private float foodValue;
        private int tickTimeout;

        public FoodNode(Vector2 position, float foodValue, int tickTimeout = 20)
        {
            rigidBody = new RigidBodyRef(new RegularPolygon(position, NODE_SIZE, 4), DefinedMaterials.Static, this);
            this.foodValue = foodValue;
            this.tickTimeout = tickTimeout;
        }

        public override void Draw(Tank fishTank, PaintEventArgs e)
        {
            RaycastFish.DrawPolygon(rigidBody.CollisionPolygon, Brushes.Orange, e);
        }

        public override void Update(Tank fishTank)
        {
        }

        public override void OnIntersection(Entity otherEntity, Tank fishTank)
        {
            base.OnIntersection(otherEntity, fishTank);

            //Make consumed
            if (otherEntity is Fish fish)
            {
                fish.IncrementFoodValue(foodValue);
                fishTank.RemoveEntity(this);
                fishTank.AddEntityIn(new FoodNode(new Vector2((float)fishTank.Random.NextDouble() * fishTank.Width, (float)fishTank.Random.NextDouble() * fishTank.Height), foodValue, tickTimeout), tickTimeout);
            }
        }
    }
}
