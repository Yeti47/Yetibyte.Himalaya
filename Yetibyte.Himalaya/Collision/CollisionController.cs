using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Collision {

    public class CollisionController : EntityComponent {

        #region Constants

        private const float CONTINUOUS_DETECTION_STEP_SIZE = 10f;

        #endregion

        #region Properties

        public float GravityScale { get; set; }
        public bool IgnoreGravity { get; set; }
        public CollisionDetectionMethods CollisionDetectionMethod { get; set; } = CollisionDetectionMethods.Lazy;

        #endregion

        #region Methods

        public IEnumerable<Collider> GetAttachedColliders() => GameEntity.GetComponentsInChildren<Collider>(true, true);

        public void ApplyGravity() {



        }

        public void Move(Vector2 offset) {



        }

        #endregion

    }

}
