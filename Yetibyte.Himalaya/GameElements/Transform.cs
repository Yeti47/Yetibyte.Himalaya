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
    public class Transform {

        #region Fields

        private Transform _parent;
        private Vector2 _position = Vector2.Zero;

        #endregion

        #region Properties

        public Transform Parent {

            get { return _parent; }

            set {

                Transform futureParent = value;

                if (this.HasParent) {

                    _parent.RemoveChild(this);

                }

                futureParent?.AddChild(this);

                _parent = futureParent;

            }

        }

        public List<Transform> Children { get; private set; } = new List<Transform>();

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

        public bool HasParent => _parent != null;

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

        public void AddChild(Transform childTransform) {

            if (IsParentOf(childTransform))
                return;

            Children.Add(childTransform);
            childTransform.Parent = this;

        }

        public void RemoveChild(Transform childTransform) {

            if (!IsParentOf(childTransform))
                return;

            Children.Remove(childTransform);
            childTransform.Parent = null;

        }

        /// <summary>
        /// Checks whether the given <see cref="Transform"/> is included in the list of child entities of this <see cref="Transform"/>.
        /// </summary>
        /// <param name="childTransform">The child transform.</param>
        /// <returns>True if the given Transform is a child of this Transform.</returns>
        public bool IsParentOf(Transform childTransform) => Children.Contains(childTransform);

        #endregion

    }

}
