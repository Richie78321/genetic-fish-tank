using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FishTank
{
    class Tank
    {
        public Tank()
        {

        }

        private List<Entity> containedEntities = new List<Entity>();
        public void Update()
        {
            //Update entities
            Entity[] currentContainedEntities = containedEntities.ToArray();
            for (int i = 0; i < currentContainedEntities.Length; i++) currentContainedEntities[i].Update(this);
        }

        public void Draw(PaintEventArgs e)
        {
            //Draw entities
            Entity[] currentContainedEntities = containedEntities.ToArray();
            for (int i = 0; i < currentContainedEntities.Length; i++) currentContainedEntities[i].Draw(this, e);
        }
    }
}
