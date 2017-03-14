using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Framework;

namespace Yetibyte.Himalaya.GameElements {

    /// <summary>
    /// A class that can be used as a component for any object that can be positioned, scaled and rotated.
    /// </summary>
    public class Transform {

        // Properties

        public Transform Parent { get; set; }

        /// <summary>
        /// The position relative to the parent Transform.
        /// </summary>
        public Vector2 LocalPosition {

            get
            {
                Vector2 relation = Parent != null ? Parent.LocalPosition : Vector2.Zero;
                return Position - relation;
            }

            set
            {
                Vector2 relation = Parent != null ? Parent.Position : Vector2.Zero;
                Position = value + relation;
            }

        }

        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        // Methods

        public Vector2 Translate(Vector2 offset) {

            Position += offset;
            return Position;

        }

        public Vector2 TranslateLocally(Vector2 offset) {

            LocalPosition += offset;
            return LocalPosition;

        }

    }

}
