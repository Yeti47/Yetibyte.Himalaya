using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Collision {

    public struct RaycastInfo {

        #region Readonly Fields

        public readonly bool Impact;
        public readonly Vector2 ImpactPoint;
        public readonly Collider Collider;
        public readonly float Distance;
        public readonly LineSegment Ray;

        #endregion

        #region Properties

        public static RaycastInfo Default => new RaycastInfo(false, Vector2.Zero, null, float.PositiveInfinity, new LineSegment(Vector2.Zero, Vector2.Zero));

        public GameEntity GameEntity => Collider?.GameEntity;
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
