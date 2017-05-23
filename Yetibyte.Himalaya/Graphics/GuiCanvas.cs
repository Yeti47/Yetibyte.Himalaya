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

        private List<GuiWindow> _windows = new List<GuiWindow>();

        #endregion

        #region Properties

        public string Name { get; set; }

        public int DrawOrder { get; set; }

        public GraphicsDevice GraphicsDevice => _spriteBatch.GraphicsDevice;

        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Deferred;

        #endregion

        #region Constructors

        public GuiCanvas(GraphicsDevice graphicsDevice) {

            _spriteBatch = new SpriteBatch(graphicsDevice);

        }

        #endregion

        #region Methods

        public void Draw(GameTime gameTime) {

            _spriteBatch.Begin(SpriteSortMode);

            foreach (GuiWindow window in _windows.OrderBy(w => w.DrawOrder)) {

                window.Draw(_spriteBatch, gameTime);

            }

            _spriteBatch.End();

        }

        /// <summary>
        /// Returns the first <see cref="GuiWindow"/> on this <see cref="GuiCanvas"/> with the given name.
        /// </summary>
        /// <param name="name">The name of the GuiWindow to look for.</param>
        /// <returns>The first <see cref="GuiWindow"/> on this <see cref="GuiCanvas"/> with the given name.</returns>
        public GuiWindow FindWindow(string name) {

            return _windows.Where(w => w.Name == name).FirstOrDefault();

        }

        #endregion

    }

}
