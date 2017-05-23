using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.Graphics {

    public enum GuiScalingUnit { Pixels, Percent }

    public enum GuiAnchorPoint { TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, Center }

    public enum GuiTextureScalingMode { ScaleToFit, Tile }

    public class GuiWindow : IDraw {

        #region Fields

        private bool _isVisible = true;

        #endregion

        #region Properties

        public string Name { get; set; } = "unnamed";

        public GuiCanvas Canvas { get; private set; }

        public GuiScalingUnit ScalingUnit { get; set; } = GuiScalingUnit.Pixels;
        public GuiAnchorPoint AnchorPoint { get; set; } = GuiAnchorPoint.TopLeft;
        public GuiTextureScalingMode TextureScalingMode { get; set; } = GuiTextureScalingMode.ScaleToFit;

        public Texture2D Texture { get; private set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public Vector2 Size {

            get => new Vector2(Width, Height);

            set {

                Width = value.X;
                Height = value.Y;

            }

        }

        public Vector2 Position { get; set; }

        public float X {

            get => Position.X;
            set => Position = new Vector2(value, Position.Y);

        }

        public float Y {

            get => Position.Y;
            set => Position = new Vector2(Position.X, value);

        }

        public Vector2 Origin { get; set; }

        public Color TintColor { get; set; } = Color.White;

        public bool IsVisible {

            get => HasCanvas ? (Canvas.IsVisible && _isVisible) : false;
            set => _isVisible = value;

        }

        public bool HasCanvas => Canvas != null;

        public int DrawOrder { get; set; }

        #endregion

        #region Constructors

        public GuiWindow(GuiCanvas canvas, Texture2D texture, string name) {

            this.Canvas = canvas;
            this.Texture = texture;
            this.Name = name;

        }

        public GuiWindow(GuiCanvas canvas, Color color, string name) {

            this.Canvas = canvas;
            this.Texture = new Texture2D(canvas.GraphicsDevice, 1, 1);
            this.Texture.SetData(new Color[] { Color.White });
            this.TintColor = color;
            this.Name = name;

        }

        #endregion

        #region Methods

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {

            spriteBatch.Draw(Texture, Position, null, null, Origin, 0f, Vector2.One, TintColor);

        }

        #endregion

    }

}
