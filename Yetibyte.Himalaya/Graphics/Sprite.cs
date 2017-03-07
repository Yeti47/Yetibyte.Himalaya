using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Graphics {

    public class Sprite : Transformable {

        // Fields

        private Rectangle _sourceRect;

        // Properties

        public Texture2D Texture { get; set; }
        
        public Rectangle SourceRect {

            get { return _sourceRect; }
            set { _sourceRect = value; }

        }
        
        public int Width { get { return SourceRect.Width; } }
        public int Height { get { return SourceRect.Height; } }

        public Point Index {

            get { return _sourceRect.Location; }
            set { _sourceRect.Location = value; }

        }

        public Color TintColor { get; set; }

        // Constructor

        public Sprite(Texture2D texture) {

            this.Texture = texture;
            this.SourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            
        }

        public Sprite(Texture2D texture, Rectangle sourceRect) {

            this.Texture = texture;
            this.SourceRect = sourceRect;

        }

        public Sprite(Texture2D texture, int sourceX, int sourceY, int width, int height) {

            this.Texture = texture;
            this.SourceRect = new Rectangle(sourceX, sourceY, width, height);

        }

        // Methods

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            
            

        }

    }

}
