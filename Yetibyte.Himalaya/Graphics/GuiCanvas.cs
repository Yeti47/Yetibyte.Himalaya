using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.Graphics {

    /// <summary>
    /// <para>A GuiCanvas is responsible for managing and rendering <see cref="GuiElement"/>s. All GuiElements must be attached to a canvas in order
    /// to be processed.</para>
    /// <para>GuiCanvases can be added to or removed from the <see cref="Yetibyte.Himalaya.GameElements.Scene"/> via the appropriate methods. There can
    /// be multiple canvases in a scene. However, each <see cref="GuiElement"/> can only be attached to one GuiCanvas at a time.</para> 
    /// </summary>
    public class GuiCanvas : IUpdate {

        #region Fields

        private SpriteBatch _spriteBatch;

        private List<GuiElement> _elements = new List<GuiElement>();

        #endregion

        #region Properties

        /// <summary>
        /// The name used to identify this canvas.
        /// </summary>
        public string Name { get; set; } = "unnamed";

        /// <summary>
        /// Determines the rendering order. GuiCanvases are drawn in ascending order.
        /// </summary>
        public int DrawOrder { get; set; }

        /// <summary>
        /// The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> this <see cref="GuiCanvas"/> lives on.
        /// </summary>
        public GraphicsDevice GraphicsDevice => _spriteBatch.GraphicsDevice;

        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Deferred;

        /// <summary>
        /// A GuiCanvas that is set to invisible will not be rendered on the screen. However, it will still be updated.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// A GuiCanvas that is set to inactive will be ignored by the <see cref="Yetibyte.Himalaya.GameElements.Scene"/> it is assigned to
        /// and therefore won't be processed.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Determines the order in which <see cref="GuiCanvas"/>es are processed. The <see cref="Yetibyte.Himalaya.GameElements.Scene"/> processes
        /// GuiCanvases in ascending order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Provides quick access to the screen dimensions of the graphics device.
        /// </summary>
        public Vector2 ScreenSize => new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        #endregion

        #region Constructors

        public GuiCanvas(GraphicsDevice graphicsDevice, string name) {

            _spriteBatch = new SpriteBatch(graphicsDevice);
            this.Name = name;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Renders this <see cref="GuiCanvas"/> and all of its <see cref="GuiElement"/>s on the screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime) {

            _spriteBatch.Begin(SpriteSortMode);

            foreach (GuiElement element in _elements.Where(w => w.IsVisible).OrderBy(w => w.DrawOrder)) {

                element.Draw(_spriteBatch, gameTime);

            }

            _spriteBatch.End();

        }

        /// <summary>
        /// Returns the first <see cref="GuiElement"/> on this <see cref="GuiCanvas"/> with the given name.
        /// </summary>
        /// <param name="name">The name of the GuiWindow to look for.</param>
        /// <returns>The first <see cref="GuiElement"/> on this <see cref="GuiCanvas"/> with the given name.</returns>
        public GuiElement FindElement(string name) {

            return _elements.Where(w => w.Name == name).FirstOrDefault();

        }

        /// <summary>
        /// Adds the given <see cref="GuiElement"/> to this <see cref="GuiCanvas"/>.
        /// </summary>
        /// <param name="element">The GuiElement to add.</param>
        public void AddGuiElement(GuiElement element) {

            if (HasGuiElement(element))
                return;

            _elements.Add(element);
            element.Canvas = this;

        }

        /// <summary>
        /// Removes the given <see cref="GuiElement"/> from this <see cref="GuiCanvas"/>.
        /// </summary>
        /// <param name="element">The GuiElement to remove.</param>
        public void RemoveGuiElement(GuiElement element) {

            if (!HasGuiElement(element))
                return;

            element.Canvas = null;
            _elements.Remove(element);

        }

        /// <summary>
        /// Checks wether the given <see cref="GuiElement"/> is attached to this <see cref="GuiCanvas"/>.
        /// </summary>
        /// <param name="element">The GuiElement to look for.</param>
        /// <returns>True if the given element is attached to this canvas, false otherwise.</returns>
        public bool HasGuiElement(GuiElement element) => _elements.Contains(element);

        /// <summary>
        /// Updates every <see cref="GuiElement"/> on this <see cref="GuiCanvas"/> that implements <see cref="IUpdate"/>.
        /// </summary>
        /// <param name="gameTime">A snapshot of current timing values.</param>
        /// <param name="globalTimeScale">The global time scaling value.</param>
        public void Update(GameTime gameTime, float globalTimeScale) {

            foreach (IUpdate updatableElement in _elements.OfType<IUpdate>()) {

                updatableElement.Update(gameTime, globalTimeScale);

            }

        }

        private void HandleMouseInput() {



        }

        #endregion

    }

}
