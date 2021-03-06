﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.Gui {

    /// <summary>
    /// A <see cref="GuiElement"/> that renders a texture on the screen.
    /// </summary>
    public class GuiBox : GuiElement {

        #region Properties

        public Texture2D Texture { get; set; }
        public Color TintColor { get; set; } = Color.White;
        public GuiTextureScalingMode TextureScalingMode { get; set; }
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        public float RenderLayerDepth { get; set; } = 0f;

        #endregion

        #region Constructors

        public GuiBox(string name, Texture2D texture = null) : base(name) {

            this.Texture = texture;

        }

        #endregion

        #region Methods

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            base.Draw(spriteBatch, gameTime);

            if (Texture == null)
                return;

            Vector2 scale = new Vector2(AbsoluteSize.X / Texture.Width, AbsoluteSize.Y / Texture.Height);

            spriteBatch.Draw(Texture, AbsolutePosition, null, null, Origin, Rotation, scale, TintColor, SpriteEffect, RenderLayerDepth);

        }

        #endregion

    }

}
