using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImpulseEngine2;
using FishTank.Anima;

namespace FishTank
{
    class Boundary : Entity
    {
        public override RigidBody RigidBody => throw new NotImplementedException();

        public override void Draw(Tank fishTank, PaintEventArgs e)
        {
        }

        public override void Update(Tank fishTank)
        {
        }
    }
}
