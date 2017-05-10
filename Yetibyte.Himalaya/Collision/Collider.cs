using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.GameElements;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Collision {

    public abstract class Collider : EntityComponent, IBounds {

        #region Properties

        /// <summary>
        /// Whether or not multiple instances of this <see cref="EntityComponent"/> can be attached to a <see cref="GameEntity"/>.
        /// </summary>
        public override bool AllowMultiple => true;

        /// <summary>
        /// Returns the axis aligned bounding box of this Collider.
        /// </summary>
        public abstract RectangleF Bounds { get; }

        /// <summary>
        /// The offset added to this Collider's <see cref="Position"/>.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// The position of this collider. Directly linked to the position of the <see cref="GameEntity"/> this Collider is attached to.
        /// Always equal to <see cref="Offset"/> if not attached to any entity.
        /// </summary>
        public Vector2 Position => (this.IsAttached ? GameEntity.Transform.Position : Vector2.Zero) + Offset;

        /// <summary>
        /// The scale of this Collider. Directly linked to the scale of the <see cref="GameEntity"/> this Collider is attached to. Always returns 
        /// (1,1) if not attached to any entity.
        /// </summary>
        public Vector2 Scale => this.IsAttached ? GameEntity.Transform.Scale : Vector2.One;

        /// <summary>
        /// Whether or not this Collider acts as a trigger instead of an actual Collider.
        /// </summary>
        public bool IsTrigger { get; set; }

        /// <summary>
        /// A bitmask that determines what <see cref="CollisionLayers"/> this Collider lives on. 
        /// Colliders usually only interact with other Colliders that have at least one matching layer.
        /// </summary>
        public CollisionLayers Layers { get; set; } = CollisionLayers.All;

        internal List<Collider> IntersectingColliders { get; set; } = new List<Collider>();
        internal List<Collider> PreviouslyIntersectingColliders { get; set; } = new List<Collider>();

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether this <see cref="Collider"/> and the given <see cref="RectCollider"/> overlap.
        /// </summary>
        /// <param name="otherRectCollider">The <see cref="RectCollider"/> to check intersection with.</param>
        /// <returns>True if the Colliders intersect, false otherwise.</returns>
        public abstract bool Intersects(RectCollider otherRectCollider);

        /// <summary>
        /// Checks whether or not this collider will intersect with the given <see cref="RectCollider"/> if
        /// it moves with the given velocity. Intersection is only checked for colliders with matching layers.
        /// </summary>
        /// <param name="velocity">The velocity of this collider.</param>
        /// <param name="otherRectCollider">The <see cref="RectCollider"/> to check intersection with.</param>
        /// <returns>An instance of <see cref="CollisionInfo"/> which contains information on whether or not
        /// an intersection occured and a penetration vector that can be used to push the colliders apart.</returns>
        public abstract CollisionInfo WillIntersect(Vector2 velocity, RectCollider otherRectCollider);

        /// <summary>
        /// Checks whether or not this <see cref="Collider"/> is on the same layer(s) as the given Collider.
        /// </summary>
        /// <param name="other">The other collider to compare layers with.</param>
        /// <returns>True if at least one layer matches, false otherwise. Always returns false if one or both colliders
        /// are not assigned to any layer at all.</returns>
        public bool SharesLayerWith(Collider other) => (Layers & other.Layers) != 0;

        /// <summary>
        /// Checks whether this <see cref="Collider"/> is on the given collision layer(s).
        /// </summary>
        /// <param name="layers">The layer(s) to check against.</param>
        /// <returns>True if at least one layer this collider is on matches with the given layers.</returns>
        public bool IsOnLayer(CollisionLayers layers) => (this.Layers & layers) != 0;

        #endregion

    }

}
