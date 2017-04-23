using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Collision {

    public struct CollisionInfo {

        private static CollisionInfo _default = new CollisionInfo(false, null, Vector2.Zero);

        public static CollisionInfo Default => _default;

        public bool Intersects { get; }
        public Collider OtherCollider { get; }
        public Vector2 Penetration { get; }

        public CollisionInfo(bool intersects, Collider otherCollider, Vector2 penetration) {

            Intersects = intersects;
            OtherCollider = otherCollider;
            Penetration = penetration;

        }

    }
}
