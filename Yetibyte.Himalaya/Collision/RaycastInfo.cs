using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Collision {

    public struct RaycastInfo {

        #region Fields

        public bool Impact;
        public Vector2 ImpactPoint;
        public Collider Collider;
        public float Distance;
        public LineSegment LineSegment;

        #endregion

        #region Properties

        public static RaycastInfo Default => new RaycastInfo(false, Vector2.Zero, null, float.PositiveInfinity, new LineSegment(Vector2.Zero, Vector2.Zero));

        public GameEntity GameEntity => Collider?.GameEntity;
        public Transform Transform => GameEntity?.Transform;

        #endregion

        #region Constructors

        public RaycastInfo(bool impact, Vector2 impactPoint, Collider collider, float distance, LineSegment lineSegment) {

            Impact = impact;
            ImpactPoint = impactPoint;
            Collider = collider;
            Distance = distance;
            LineSegment = lineSegment;

        }

        #endregion


    }
}
