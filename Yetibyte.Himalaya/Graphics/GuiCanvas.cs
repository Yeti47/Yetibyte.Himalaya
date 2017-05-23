using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.Graphics {

    public class GuiCanvas {

        #region Fields

        private SpriteBatch _spriteBatch;

        #endregion

        #region Properties

        public string Name { get; set; }

        public int DrawOrder { get; set; }

        public GraphicsDevice GraphicsDevice => _spriteBatch.GraphicsDevice;

        #endregion

        #region Constructors

        public GuiCanvas(GraphicsDevice graphicsDevice) {

            _spriteBatch = new SpriteBatch(graphicsDevice);

        }

        #endregion

        #region Methods

        public void Draw(GameTime gameTime) {

        }

        #endregion

    }

}
