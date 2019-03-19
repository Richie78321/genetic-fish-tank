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

        public float FoodValue;

        public enum Modules
        {
            NeuralNet
        }

        public override RigidBodyRef RigidBody => rigidBody;
        private ModularMember modularMember;
        public ModularMember ModularMember => modularMember;
        private RigidBodyRef rigidBody;
        public Fish(ModularMember modularMember, RigidBodyRef rigidBody, float startingFoodValue)
        {
            this.rigidBody = rigidBody;
            this.modularMember = modularMember;
            rigidBody.EntityReference = this;
            FoodValue = startingFoodValue;
        }
    }
}
