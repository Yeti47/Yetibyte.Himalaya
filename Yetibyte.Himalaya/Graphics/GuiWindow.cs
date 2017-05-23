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

    public class GuiWindow {

        #region Fields

        #endregion

        #region Properties

        public GuiCanvas Canvas { get; private set; }

        public GuiScalingUnit ScalingUnit { get; set; } = GuiScalingUnit.Pixels;
        public GuiAnchorPoint AnchorPoint { get; set; } = GuiAnchorPoint.TopLeft;

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

        #endregion

        #region Constructors

        public GuiWindow(GuiCanvas canvas, Texture2D texture) {

            this.Canvas = canvas;
            this.Texture = texture;

        }

        public GuiWindow(GuiCanvas canvas, Color color) {

            this.Canvas = canvas;
            this.Texture = new Texture2D(canvas.GraphicsDevice, 1, 1);
            this.Texture.SetData(new Color[] { color });

        }

        #endregion

        #region Methods

        #endregion

    }

}
