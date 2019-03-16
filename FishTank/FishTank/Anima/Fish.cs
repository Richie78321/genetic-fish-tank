using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImpulseEngine2;
using ModularGenetics;

namespace FishTank.Anima
{
    abstract class Fish : Entity
    {
        public abstract string Species { get; }

        //TEMP
        public float hunger = 0;

        public override RigidBodyRef RigidBody => rigidBody;
        private ModularMember modularMember;
        public ModularMember ModularMember => modularMember;
        private RigidBodyRef rigidBody;
        public Fish(ModularMember modularMember, RigidBodyRef rigidBody)
        {
            this.rigidBody = rigidBody;
            this.modularMember = modularMember;
            rigidBody.EntityReference = this;
        }
    }
}
