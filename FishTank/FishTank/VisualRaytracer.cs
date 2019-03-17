using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpulseEngine2;
using Microsoft.Xna.Framework;

namespace FishTank
{
    class VisualRaytracer
    {
        public delegate bool OneHotIndicator(Entity self, Entity intersectedEntity);

        public static double[] ProcessRaytrace(Entity self, Entity[] entities, LineSegment raytrace, OneHotIndicator[] oneHotIndicators)
        {
            //Find closest intersection with entities
            Entity intersectedEntity = null;
            float intersectionDistance = raytrace.Length;
            for (int i = 0; i < entities.Length; i++)
            {
                Vector2 potentialIntersectionPoint;
                if (entities[i].RigidBody.CollisionPolygon.RayIntersection(raytrace, out potentialIntersectionPoint))
                {
                    float distance = LineSegment.Distance(raytrace.EndPoints[0], potentialIntersectionPoint);
                    if (intersectedEntity == null || intersectionDistance > distance)
                    {
                        intersectedEntity = entities[i];
                        intersectionDistance = distance;
                    }
                }
            }

            double[] outputs = new double[1 + oneHotIndicators.Length];
            outputs[0] = intersectionDistance / raytrace.Length;
            for (int i = 0; i < oneHotIndicators.Length; i++)
            {
                outputs[i + 1] = Convert.ToDouble(oneHotIndicators[i](self, intersectedEntity));
            }
            return outputs;
        }
    }
}
