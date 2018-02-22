using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.Gui {

    /// <summary>
    /// A <see cref="GuiElement"/> responsible for rendering text on the screen.
    /// </summary>
    public class GuiText : GuiElement {

        #region Properties

        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public Color TextColor { get; set; } = Color.White;
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        public float RenderLayerDepth { get; set; } = 0f;

        #endregion

        #region Constructors

        public GuiText(string name, SpriteFont font, string text = "") : base(name) {

            this.Font = font;
            this.Text = text;

        }

        #endregion

        #region Methods

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            base.Draw(spriteBatch, gameTime);

            if (Font == null)
                return;

            spriteBatch.DrawString(Font, Text, AbsolutePosition, TextColor, Rotation, Origin, Vector2.One, SpriteEffect, RenderLayerDepth);

        }

        #endregion

    }

}
