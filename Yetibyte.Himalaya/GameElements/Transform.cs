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

        public Transform Parent { get; private set; }
        public List<Transform> Children { get; private set; }

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

        /// <summary>
        /// The global scale of this Transform (read-only). Use <see cref="LocalScale"/> to manipulate the scaling value.
        /// </summary>
        public Vector2 Scale
        {

            get
            {
                Vector2 relation = Parent != null ? Parent.Scale : Vector2.One;
                return LocalScale * relation;
            }

        }

        /// <summary>
        /// The global rotation of this Transform in radians (read-only). Use <see cref="LocalRotation"/> to manipulate the rotation value.
        /// </summary>
        public float Rotation
        {

            get
            {
                float relation = Parent != null ? Parent.Rotation : 0f;
                return relation + LocalRotation;
            }

        }
        
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 LocalScale { get; set; }
        public float LocalRotation { get; set; }

        // Methods

        public Vector2 Translate(Vector2 offset) {

            Position += offset;
            return Position;

        }

        public Vector2 TranslateLocally(Vector2 offset) {

            LocalPosition += offset;
            return LocalPosition;

        }

        public void AddChild(Transform childTransform) {

            Children.Add(childTransform);
            childTransform.Parent = this;

        }

        public void RemoveChild(Transform childTransform) {

            Children.Remove(childTransform);
            childTransform.Parent = null;

        }

    }

}
