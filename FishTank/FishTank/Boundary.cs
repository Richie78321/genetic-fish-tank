using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImpulseEngine2;
using FishTank.Anima;
using ImpulseEngine2.Materials;
using System.Drawing;

namespace FishTank
{
    class Boundary : Entity
    {
        public override RigidBodyRef RigidBody => rigidBody;
        private RigidBodyRef rigidBody;

        public Boundary(ImpulseEngine2.RectangleF boundaryRectangle)
        {
            rigidBody = new RigidBodyRef(new RotationRectangle(boundaryRectangle), DefinedMaterials.Static, this);
        }

        public override void Draw(Tank fishTank, PaintEventArgs e)
        {
            RaycastFish.DrawPolygon(rigidBody.CollisionPolygon, Brushes.Black, e);
        }

        public override void Update(Tank fishTank)
        {
        }
    }
}
