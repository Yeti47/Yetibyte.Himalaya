using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Utilities;

namespace Yetibyte.Himalaya.Gui {

    /// <summary>
    /// The base of every element in a GUI. Provides all the basic functionality that is shared among all GUI components.
    /// A GuiElement needs to be attached to a <see cref="GuiCanvas"/> via the appropriate method in order to be processed.
    /// Please be aware, that a <see cref="GuiElement"/> cannot be assigned to multiple canvases at once.
    /// </summary>
    public class GuiElement : ParentChildHierarchy<GuiElement>, IDraw, IUpdate, IBounds {

        #region Fields

        private bool _isVisible = true;
        private bool _isActive = true;

        #endregion

        #region Properties

        public string Name { get; set; } = "unnamed";
        public string HierarchyPath => HasParent ? Parent.HierarchyPath + "/" + Name : Name;

        /// <summary>
        /// The GuiCanvas this GuiElement is assigned to.
        /// </summary>
        public GuiCanvas Canvas { get; set; }

        /// <summary>
        /// Determines the unit in which the local positioning and scaling values are measured.
        /// </summary>
        public GuiScalingUnit ScalingUnit { get; set; } = GuiScalingUnit.Pixels;

        public GuiScalingUnit PositioningUnit { get; set; } = GuiScalingUnit.Pixels;

        /// <summary>
        /// The orientation point of this element's position.
        /// </summary>
        public GuiAnchorPoint AnchorPoint { get; set; } = GuiAnchorPoint.TopLeft;

        /// <summary>
        /// The local position of this <see cref="GuiElement"/>. It is relative to the <see cref="GuiAnchorPoint"/> and can be in either pixels or percent
        /// depending on the <see cref="GuiScalingUnit"/>. To get the actual position of this element on the screen, use <see cref="AbsolutePosition"/>.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The absolute position of this <see cref="GuiElement"/> on the screen in pixels. Setting this value will recalculate the local position.
        /// </summary>
        public Vector2 AbsolutePosition {

            get {

                Vector2 localPosition = (PositioningUnit == GuiScalingUnit.Pixels) ? Position : new Vector2(Position.X /100f * ReferenceSize.X, Position.Y / 100f * ReferenceSize.Y);
                return ReferencePosition + localPosition;

            }

            set {

                Vector2 localPositionPixels = value - AbsolutePosition;

                if (PositioningUnit == GuiScalingUnit.Pixels)
                    Position = localPositionPixels;
                else {

                    Vector2 referenceSize = ReferenceSize;
                    Position = new Vector2(100f / referenceSize.X * localPositionPixels.X, 100f / referenceSize.Y * localPositionPixels.Y);

                }
                

            }

        }

        /// <summary>
        /// Gets the size in pixels of the object in the GUI's hierarchy that determines this <see cref="GuiElement"/>'s relative size.
        /// </summary>
        public Vector2 ReferenceSize {

            get => HasParent ? Parent.AbsoluteSize : (HasCanvas ? Canvas.ScreenSize : Vector2.Zero);

        }

        /// <summary>
        /// The origin point from where this <see cref="GuiElement"/>'s absolute position is determined. This takes into account the
        /// position of the parent element (if there is one) as well as the <see cref="GuiAnchorPoint"/> of this element.
        /// </summary>
        public Vector2 ReferencePosition {

            get {

                Vector2 normalizedAnchorCoords = GetNormalizedAnchorChoordinates();

                Vector2 anchorCoords = HasParent ?
                    (Parent.AbsolutePosition + new Vector2(normalizedAnchorCoords.X * Parent.AbsoluteSize.X, normalizedAnchorCoords.Y * Parent.AbsoluteSize.Y)) :
                    (new Vector2(normalizedAnchorCoords.X * Canvas.ScreenSize.X, normalizedAnchorCoords.Y * Canvas.ScreenSize.Y));

                return anchorCoords;
            }

        }

        /// <summary>
        /// The unprocessed size of this <see cref="GuiElement"/>. This can be in either percent or pixels depending on the <see cref="GuiScalingUnit"/>.
        /// To retrieve the actual size on the screen in pixels, use <see cref="AbsoluteSize"/>.
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// The effective size of this <see cref="GuiElement"/> on the screen in pixels. Setting this value will calculate the new value 
        /// for <see cref="Size"/> if the <see cref="GuiScalingUnit"/> is set to "percent".
        /// </summary>
        public Vector2 AbsoluteSize {

            get {

                if (ScalingUnit == GuiScalingUnit.Pixels)
                    return Size;

                Vector2 reference = ReferenceSize;

                return new Vector2(reference.X * Size.X / 100f, reference.Y * Size.Y / 100f);

            }

            set {

                if (ScalingUnit == GuiScalingUnit.Pixels)
                    Size = value;
                else {

                    Vector2 reference = ReferenceSize;

                    Size = new Vector2(value.X * 100f / reference.X, value.Y * 100f / reference.Y);

                }

            }

        }

        public Vector2 Origin { get; set; }

        /// <summary>
        /// The local rotation of this <see cref="GuiElement"/> in radians.
        /// </summary>
        public float LocalRotation { get; set; }

        /// <summary>
        /// The global rotation of this <see cref="GuiElement"/> i radians.
        /// </summary>
        public float Rotation => (HasParent ? Parent.Rotation : 0f) + LocalRotation;

        /// <summary>
        /// The visibility state of this <see cref="GuiElement"/>. Elements that are set to invisible will not be rendered on the screen.
        /// </summary>
        public bool IsVisible {

            get => IsVisibleSelf && (!HasParent || Parent.IsVisible);
            set => IsVisibleSelf = value;

        }

        /// <summary>
        /// The active state of this <see cref="GuiElement"/>. Elements that are inactive will be ignored by the <see cref="GuiCanvas"/>.
        /// </summary>
        public bool IsActive {

            get => IsActiveSelf && (!HasParent || Parent.IsActive);
            set => IsActiveSelf = value;

        }

        /// <summary>
        /// The local visibility state of this <see cref="GuiElement"/>. This ignores the visibility state of the parent. Please note that, even if this is
        /// set to true, the GuiElement may still be invisible because its parent is invisible.
        /// </summary>
        public bool IsVisibleSelf {

            get => _isVisible;
            set => _isVisible = value;

        }

        /// <summary>
        /// The local active state of this <see cref="GuiElement"/>. This ignores the active state of the parent. Please note that, even if this is set to true, the GuiElement 
        /// may still be considered inactive by the <see cref="GuiCanvas"/> because the parent is not active.
        /// </summary>
        /// <seealso cref="IsActive"/>
        public bool IsActiveSelf {

            get => _isActive;
            set => _isActive = value;

        }

        /// <summary>
        /// Whether or not this <see cref="GuiElement"/> was assigned to a <see cref="GuiCanvas"/>.
        /// </summary>
        public bool HasCanvas => Canvas != null;

        public int DrawOrder { get; set; }

        /// <summary>
        /// The bounding box of this <see cref="GuiElement"/>.
        /// </summary>
        public RectangleF Bounds => new RectangleF(AbsolutePosition, AbsoluteSize);

        #endregion

        #region Constructors

        protected GuiElement(string name) {

            this.Name = name;

        }

        #endregion

        #region Methods

        public virtual void Awake() {

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {

        }

        public virtual void Update(GameTime gameTime, float globalTimeScale) {
            
        }

        /// <summary>
        /// Gets the normalized coordinates of the <see cref="AnchorPoint"/>, meaning that its components will range from 0 to 1. For example,
        /// if the anchor point is set to <see cref="GuiAnchorPoint.Top"/>, this method returns {X: 0.5 | Y: 0}.
        /// </summary>
        /// <returns>The normalized coordinates of this elements anchor point.</returns>
        public Vector2 GetNormalizedAnchorChoordinates() {

            Vector2 anchor = Vector2.Zero;

            switch (AnchorPoint) {

                case GuiAnchorPoint.TopLeft:
                    anchor = Vector2.Zero;
                    break;
                case GuiAnchorPoint.Top:
                    anchor = new Vector2(0.5f, 0f);
                    break;
                case GuiAnchorPoint.TopRight:
                    anchor = Vector2.UnitX;
                    break;
                case GuiAnchorPoint.Right:
                    anchor = new Vector2(1f, 0.5f);
                    break;
                case GuiAnchorPoint.BottomRight:
                    anchor = Vector2.One;
                    break;
                case GuiAnchorPoint.Bottom:
                    anchor = new Vector2(0.5f, 1f);
                    break;
                case GuiAnchorPoint.BottomLeft:
                    anchor = Vector2.UnitY;
                    break;
                case GuiAnchorPoint.Left:
                    anchor = new Vector2(0f, 0.5f);
                    break;
                case GuiAnchorPoint.Center:
                    anchor = new Vector2(0.5f, 0.5f);
                    break;
                default:
                    break;

            }

            return anchor;

        }

        #endregion

    }

}
