using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImpulseEngine2;
using ModularGenetics;
using System.Drawing;

namespace FishTank.Anima
{
    abstract class Fish : Entity
    {
        public abstract string Species { get; }

        public float FoodValue => foodValue;
        private float foodValue;

        public float Fitness => fitness;
        private float fitness;
        public void IncrementFoodValue(float increment)
        {
            foodValue += increment;

            //Add food gain to fitness
            fitness += Math.Max(increment, 0);
        }

        public enum Modules
        {
            NeuralNet
        }

        public abstract string[] InputKey { get; }

        public override RigidBodyRef RigidBody => rigidBody;
        private ModularMember modularMember;
        public ModularMember ModularMember => modularMember;
        private RigidBodyRef rigidBody;
        public Fish(ModularMember modularMember, RigidBodyRef rigidBody, float startingFoodValue)
        {
            this.rigidBody = rigidBody;
            this.modularMember = modularMember;
            rigidBody.EntityReference = this;
            foodValue = startingFoodValue;
        }
    }
}
