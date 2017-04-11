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

        public float GravityScale { get; set; }

        // Constructor



        // Methods

        public IEnumerable<Collider> GetAttachedColliders() => GameEntity.GetComponentsInChildren<Collider>(true, true);

        public void ApplyGravity() {



        }

        public void Move(Vector2 offset) {

            

        }
        

    }

}
