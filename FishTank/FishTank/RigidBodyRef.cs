using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpulseEngine2;
using ImpulseEngine2.Materials;

namespace FishTank
{
    public class RigidBodyRef : RigidBody
    {
        public Entity EntityReference;
        public RigidBodyRef(Polygon collisionPolygon, Material material, Entity entityReference, int collisionLevel = 0) : base(collisionPolygon, material, collisionLevel)
        {
            this.EntityReference = entityReference;
        }

        public RigidBodyRef(Polygon collisionPolygon, float Bounce, float Mass, float StaticFriction, float DynamicFriction, Entity entityReference, int collisionLevel = 0) : base(collisionPolygon, Bounce, Mass, StaticFriction, DynamicFriction, collisionLevel)
        {
            this.EntityReference = entityReference;
        }
    }
}
