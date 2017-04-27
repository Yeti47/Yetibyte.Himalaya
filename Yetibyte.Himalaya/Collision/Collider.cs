using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.GameElements;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Collision {

    public abstract class Collider : EntityComponent, IBounds {

        // Fields

        // Properties

        public override bool AllowMultiple => true;

        public abstract RectangleF Bounds { get; }
        public Vector2 Offset { get; set; }
        public Vector2 Position => (this.IsAttached ? GameEntity.Transform.Position : Vector2.Zero) + Offset;

        public Vector2 Scale => this.IsAttached ? GameEntity.Transform.Scale : Vector2.One;

        public bool IsTrigger { get; set; }

        public CollisionLayers Layers { get; set; } = CollisionLayers.All;

        // Constructor


        // Methods

        public abstract bool Intersects(RectCollider otherRectCollider);

        public abstract CollisionInfo WillIntersect(Vector2 velocity, RectCollider otherRectCollider);

        /// <summary>
        /// Checks whether or not this <see cref="Collider"/> is on the same layer(s) as the given Collider.
        /// </summary>
        /// <param name="other">The other collider to compare layers with.</param>
        /// <returns>True if at least one layer matches, false otherwise. Always returns false if one or both colliders
        /// are not assigned to any layer at all.</returns>
        public bool SharesLayerWith(Collider other) => (Layers & other.Layers) != 0;

    }

}
