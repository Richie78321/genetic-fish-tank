using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModularGenetics;
using ModularGenetics.AI;
using ModularGenetics.AI.Dense;
using ImpulseEngine2;

namespace FishTank.Anima
{
    class RaycastFish : Fish
    {
        private const int NUM_RAYCASTS = 4;
        private const int RAYCAST_CATEGORIES = 5;

        private const int OTHER_INPUTS = 1;

        private const int HIDDEN_NEURONS = 25;
        private const int OUTPUT_NUM = 2;

        public static ModularMember CreateModularMember(Random random)
        {
            //Create phenotypes
            SequentialModelFactory sequentialModelFactory = new SequentialModelFactory(new int[] { (NUM_RAYCASTS * RAYCAST_CATEGORIES) + OTHER_INPUTS });
            sequentialModelFactory.AddModel(new DenseLayer(HIDDEN_NEURONS, ComputationModel.ReLUActivation));
            sequentialModelFactory.AddModel(new DenseLayer(OUTPUT_NUM, ComputationModel.ReLUActivation));
            Phenotype[] phenotypes = new Phenotype[] { sequentialModelFactory.DeployModel() };

            return new ModularMember(phenotypes, random);
        }

        //Object
        public override string Species => species;
        private string species;

        public RaycastFish(RigidBody rigidBody, string species, Random random) : base(CreateModularMember(random), rigidBody)
        {
            this.species = species;
        }

        public override void Draw(Tank fishTank, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Update(Tank fishTank)
        {
            throw new NotImplementedException();
        }
    }
}
