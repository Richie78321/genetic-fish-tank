using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using ImpulseEngine2;

namespace FishTank
{
    abstract class Entity
    {
        public abstract RigidBody RigidBody { get; }

        public abstract void Update(Tank fishTank);
        public abstract void Draw(Tank fishTank, PaintEventArgs e);
    }
}
