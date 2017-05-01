using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Collision {

    /// <summary>
    /// Describes the result of a raycast by providing information like the closest <see cref="Collision.Collider"/> detected and the intersection point.
    /// </summary>
    public struct RaycastInfo {

        #region Readonly Fields

        private static readonly RaycastInfo _default = new RaycastInfo(false, Vector2.Zero, null, float.PositiveInfinity, new LineSegment(Vector2.Zero, Vector2.Zero));

        /// <summary>
        /// Whether or not the raycast detected a <see cref="Collision.Collider"/>.
        /// </summary>
        public readonly bool Impact;

        /// <summary>
        /// The point where the ray intersected with the collider.
        /// </summary>
        public readonly Vector2 ImpactPoint;

        /// <summary>
        /// The collider detected by the ray.
        /// </summary>
        public readonly Collider Collider;

        /// <summary>
        /// The distance betwenn the ray's origin and the impact point.
        /// </summary>
        public readonly float Distance;

        /// <summary>
        /// A representation of the ray as a line segment.
        /// </summary>
        public readonly LineSegment Ray;

        #endregion

        #region Properties

        /// <summary>
        /// A static instance of RaycastInfo that is used to represent the result of a raycast that did not detect anything.
        /// </summary>
        public static RaycastInfo Default => _default;

        /// <summary>
        /// The <see cref="GameElements.GameEntity"/> the detected <see cref="Collision.Collider"/> is attached to.
        /// </summary>
        public GameEntity GameEntity => Collider?.GameEntity;

        /// <summary>
        /// The <see cref="GameElements.Transform"/> of the <see cref="GameElements.GameEntity"/> the detected <see cref="Collision.Collider"/> is attached to.
        /// </summary>
        public Transform Transform => GameEntity?.Transform;

        #endregion

        #region Constructors

        private RaycastInfo(bool impact, Vector2 impactPoint, Collider collider, float distance, LineSegment ray) {

            Impact = impact;
            ImpactPoint = impactPoint;
            Collider = collider;
            Distance = distance;
            Ray = ray;

        }

        public RaycastInfo(bool impact, Vector2 impactPoint, Collider collider, Vector2 rayOrigin)
            : this(impact, impactPoint, collider, Vector2.Distance(rayOrigin, impactPoint), new LineSegment(rayOrigin, impactPoint)) { }

        #endregion


    }
}
