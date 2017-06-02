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

        public GuiCanvas Canvas { get; private set; }

        public GuiScalingUnit ScalingUnit { get; set; } = GuiScalingUnit.Pixels;
        public GuiAnchorPoint AnchorPoint { get; set; } = GuiAnchorPoint.TopLeft;

        public Vector2 Position { get; set; }

        public Vector2 Origin { get; set; }

        public bool IsVisible {

            get => IsVisibleSelf && (!HasParent || Parent.IsVisible);
            set => IsVisibleSelf = value;

        }

        public bool IsActive {

            get => IsActiveSelf && (!HasParent || Parent.IsActive);
            set => IsActiveSelf = value;

        }

        public bool IsVisibleSelf {

            get => _isVisible;
            set => _isVisible = value;

        }

        public bool IsActiveSelf {

            get => _isActive;
            set => _isActive = value;

        }

        public bool HasCanvas => Canvas != null;

        public int DrawOrder { get; set; }

        #endregion

        #region Constructors

        public GuiElement(GuiCanvas canvas, string name) {

            this.Canvas = canvas;
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

        #endregion

    }

}
