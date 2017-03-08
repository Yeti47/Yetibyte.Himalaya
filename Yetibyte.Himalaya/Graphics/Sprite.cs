using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.GameElements;
using MonoGame.Framework;

namespace Yetibyte.Himalaya.Graphics {

    public class Sprite {

        // Nested Enum

        public enum OriginPoint { TopLeft, TopRight, Center, BottomLeft, BottomRight }

        // Fields

        private Rectangle _sourceRect;

        // Properties

        public Texture2D Texture { get; set; }

        public Rectangle SourceRect {

            get { return _sourceRect; }
            set { _sourceRect = value; }

        }

        public int Width { get { return _sourceRect.Width; } set { _sourceRect.Width = value; } }
        public int Height { get { return _sourceRect.Height; } set { _sourceRect.Height = value; } }

        public Point Index {

            get { return _sourceRect.Location; }
            set { _sourceRect.Location = value; }

        }

        public Color TintColor { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        // Constructors

        public Sprite(Texture2D texture) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height)) {
                       
        }
        
        public Sprite(Texture2D texture, int sourceX, int sourceY, int width, int height) : this(texture, new Rectangle(sourceX, sourceY, width, height)) {

        }

        public Sprite(Texture2D texture, Rectangle sourceRect) {

            this.Texture = texture;
            this.SourceRect = sourceRect;

        }

        // Methods

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale, float layerDepth) {

            spriteBatch.Draw(Texture, position, SourceRect, TintColor, rotation, Origin, scale, SpriteEffect, layerDepth);

        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float layerDepth) {

            Draw(spriteBatch, position, 0f, scale, layerDepth);

        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation , float layerDepth) {

            Draw(spriteBatch, position, rotation, Vector2.Zero, layerDepth);

        }


    }

}
