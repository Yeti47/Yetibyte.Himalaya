using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Framework;

namespace Yetibyte.Himalaya.GameElements {

    public abstract class Transformable {

        // Properties

        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        // Methods

        public Vector2 Translate(Vector2 offset) {

            Position += offset;
            return Position;

        }

    }

}
