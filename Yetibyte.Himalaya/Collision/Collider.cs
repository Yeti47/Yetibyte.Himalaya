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
        public Vector2 Position => (GameEntity != null ? GameEntity.Transform.Position : Vector2.Zero) + Offset;

        // Constructor


        // Methods

    }

}
