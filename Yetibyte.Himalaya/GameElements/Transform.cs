using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.GameElements {

    /// <summary>
    /// A class that can be used as a component for any object that can be positioned, scaled and rotated.
    /// </summary>
    public class Transform : EntityComponent {

        #region Fields

        private Vector2 _position = Vector2.Zero;

        #endregion

        #region Properties

        public override bool IsRemovable => false;

        public Transform Parent => GameEntity?.ParentEntity?.Transform;

        public IEnumerable<Transform> Children => IsAttached ? GameEntity.GetComponentsInChildren<Transform>(false, true) : new Transform[0];

        /// <summary>
        /// The position relative to the parent Transform.
        /// </summary>
        public Vector2 LocalPosition {

            get {
                Vector2 relation = Parent != null ? Parent.LocalPosition : Vector2.Zero;
                return Position - relation;
            }

            set {
                Vector2 relation = Parent != null ? Parent.Position : Vector2.Zero;
                Position = value + relation;
            }

        }

        /// <summary>
        /// The global scale of this Transform (read-only). Use <see cref="LocalScale"/> to manipulate the scaling value.
        /// </summary>
        public Vector2 Scale {

            get {
                Vector2 relation = Parent != null ? Parent.Scale : Vector2.One;
                return LocalScale * relation;
            }

        }

        /// <summary>
        /// The global rotation of this Transform in radians (read-only). Use <see cref="LocalRotation"/> to manipulate the rotation value.
        /// </summary>
        public float Rotation {

            get {
                float relation = Parent != null ? Parent.Rotation : 0f;
                return relation + LocalRotation;
            }

        }

        /// <summary>
        /// The global position. Setting this value will also manipulate the position of all child Transforms.
        /// </summary>
        public Vector2 Position {

            get { return _position; }

            set {

                Vector2 delta = value - _position;

                foreach (Transform child in Children)
                    child.Position += delta;

                _position = value;

            }

        }

        /// <summary>
        /// The x-coordinate of this Transform's global position.
        /// </summary>
        public float X {

            get => _position.X;
            set => this.Position = new Vector2(value, _position.Y);

        }

        /// <summary>
        /// The y-coordinate of this Transform's global position.
        /// </summary>
        public float Y {

            get => _position.Y;
            set => this.Position = new Vector2(_position.X, value);

        }

        public Vector2 Origin { get; set; }
        public Vector2 LocalScale { get; set; } = Vector2.One;
        public float LocalRotation { get; set; }

        #endregion

        #region Methods

        public Vector2 Translate(Vector2 offset) {

            Position += offset;
            return Position;

        }

        public Vector2 TranslateLocally(Vector2 offset) {

            LocalPosition += offset;
            return LocalPosition;

        }

        #endregion

    }

}
