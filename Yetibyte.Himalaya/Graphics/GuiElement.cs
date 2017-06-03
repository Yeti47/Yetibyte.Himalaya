using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Utilities;

namespace Yetibyte.Himalaya.Graphics {

    public abstract class GuiElement : ParentChildHierarchy<GuiElement>, IDraw, IUpdate {

        #region Fields

        private bool _isVisible = true;
        private bool _isActive = true;

        #endregion

        #region Properties

        public string Name { get; set; } = "unnamed";

        public GuiCanvas Canvas { get; set; }

        public GuiScalingUnit ScalingUnit { get; set; } = GuiScalingUnit.Pixels;
        public GuiAnchorPoint AnchorPoint { get; set; } = GuiAnchorPoint.TopLeft;

        public Vector2 Position { get; set; }

        public Vector2 AbsolutePosition {

            get;
            set;

        }

        /// <summary>
        /// Gets the size in pixels of the object in the GUI's hierarchy that determines this <see cref="GuiElement"/>'s relative size.
        /// </summary>
        public Vector2 ReferenceSize {

            get => HasParent ? Parent.AbsoluteSize : (HasCanvas ? Canvas.ScreenSize : Vector2.One);

        }

        /// <summary>
        /// The unprocessed scaling values. This can be either in procent or in pixels depending on the <see cref="GuiScalingUnit"/>.
        /// For retrieving the actual size on the screen in pixels, use <see cref="AbsoluteSize"/>.
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

                Vector2 reference = ReferenceSize;

                Size = new Vector2(value.X * 100f / reference.X, value.Y * 100f / reference.Y);

            }

        }

        public Vector2 Origin { get; set; }

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

        #endregion

        #region Constructors

        public GuiElement(string name) {

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

        private Vector2 GetAnchorCoordinates() {

            throw new NotImplementedException("TODO!");

        }

        #endregion

    }

}
