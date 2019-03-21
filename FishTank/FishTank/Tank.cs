using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImpulseEngine2;
using Microsoft.Xna.Framework;

namespace FishTank
{
    public abstract class Tank
    {
        public readonly Random Random;

        public readonly DataCollection DataCollector;

        public readonly float Width, Height;
        public Tank(float width, float height, Random random, DataCollection dataCollector)
        {
            Width = width;
            Height = height;
            AddBoundaries();
            this.Random = random;
            this.DataCollector = dataCollector;

            collisionEventHandler.OnIntersection += CollisionEventHandler_OnIntersection;
        }

        private void CollisionEventHandler_OnIntersection(object sender, EventArgs e)
        {
            IntersectionEventArgs intersectionArgs = (IntersectionEventArgs)e;
            RigidBodyRef body1 = (RigidBodyRef)intersectionArgs.Body1;
            RigidBodyRef body2 = (RigidBodyRef)intersectionArgs.Body2;
            body1.EntityReference.OnIntersection(body2.EntityReference, this);
            body2.EntityReference.OnIntersection(body1.EntityReference, this);
        }

        private const float BOUNDARY_SIZE = 10F;
        private void AddBoundaries()
        {
            AddEntity(new Boundary(new ImpulseEngine2.RectangleF(-BOUNDARY_SIZE, 0, BOUNDARY_SIZE, Height)));
            AddEntity(new Boundary(new ImpulseEngine2.RectangleF(-BOUNDARY_SIZE, -BOUNDARY_SIZE, Width + BOUNDARY_SIZE * 2, BOUNDARY_SIZE)));
            AddEntity(new Boundary(new ImpulseEngine2.RectangleF(Width, 0, BOUNDARY_SIZE, Height)));
            AddEntity(new Boundary(new ImpulseEngine2.RectangleF(-BOUNDARY_SIZE, Height, Width + BOUNDARY_SIZE * 2, BOUNDARY_SIZE)));
        }

        private void SpawnFoodInitial()
        {

        }

        private CollisionEventHandler<RigidBodyRef> collisionEventHandler = new CollisionEventHandler<RigidBodyRef>();
        private List<Entity> containedEntities = new List<Entity>();
        public void AddEntity(Entity entity)
        {
            containedEntities.Add(entity);
            collisionEventHandler.AddBody(entity.RigidBody);
        }

        private List<EntityAddTimer> entityAddTimers = new List<EntityAddTimer>();
        public void AddEntityIn(Entity entity, int numTicks)
        {
            entityAddTimers.Add(new EntityAddTimer(entity, numTicks));
        }

        public void RemoveEntity(Entity entity)
        {
            containedEntities.Remove(entity);
            collisionEventHandler.RemoveBody(entity.RigidBody);
        }

        public Entity[] ContainedEntities => containedEntities.ToArray();
        public void Update()
        {
            //Update timers
            EntityAddTimer[] addTimers = entityAddTimers.ToArray();
            for (int i = 0; i < addTimers.Length; i++)
            {
                if (--addTimers[i].NumTicks <= 0)
                {
                    AddEntity(addTimers[i].Entity);
                    entityAddTimers.Remove(addTimers[i]);
                }
            }

            //Update handler
            collisionEventHandler.Update(null);

            //Update entities
            Entity[] currentContainedEntities = containedEntities.ToArray();
            for (int i = 0; i < currentContainedEntities.Length; i++) currentContainedEntities[i].Update(this);

            //Update collector
            DataCollector.UpdateCollector(this);
        }

        public void Draw(PaintEventArgs e)
        {
            //Draw entities
            Entity[] currentContainedEntities = containedEntities.ToArray();
            for (int i = 0; i < currentContainedEntities.Length; i++) currentContainedEntities[i].Draw(this, e);
        }
    }

    class EntityAddTimer
    {
        public readonly Entity Entity;
        public int NumTicks;

        public EntityAddTimer(Entity entity, int numTicks)
        {
            this.Entity = entity;
            this.NumTicks = numTicks;
        }
    }
}
