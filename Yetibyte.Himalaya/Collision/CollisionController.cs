using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Collision {

    public class CollisionController : EntityComponent {

        // Properties

        public List<Collider> Colliders { get; private set; } = new List<Collider>();
        public float GravityScale { get; set; }

        // Constructor

        

        // Methods
        
        /// <summary>
        /// Checks whether or not this <see cref="CollisionController"/> is in control of the given <see cref="Collider"/>.
        /// </summary>
        /// <param name="collider">An instance of a sub class of Collider.</param>
        /// <returns>True if the collider was found, false otherwise.</returns>
        public bool HasCollider(Collider collider) => Colliders.Contains(collider);

        /*

        /// <summary>
        /// Recursively iterates through all children of the <see cref="Yetibyte.Himalaya.GameElements.GameEntity"/> attached to this 
        /// <see cref="CollisionController"/> and returns a collection of all the <see cref="CollisionController"/>s found.
        /// </summary>
        /// <param name="includeSelf">Should the <see cref="CollisionController"/> of the root Game Entity be included in the collection?</param>
        /// <returns>A list of the collision controllers found in all of the Game Entitiy's children.</returns>
        public List<CollisionController> GetAllChildControllers(bool includeSelf) {

            List<CollisionController> collisionControllers = new List<CollisionController>();

            if (GameEntity != null) {

                if (includeSelf)
                    collisionControllers.Add(this);

                foreach (GameEntity childEntity in GameEntity.ChildEntities) {

                    collisionControllers.Add(childEntity.CollisionController);
                    collisionControllers = collisionControllers.Union(childEntity.CollisionController.GetAllChildControllers(false)).ToList();

                }
                
            }
            
            return collisionControllers;

        }

        */

    }

}
