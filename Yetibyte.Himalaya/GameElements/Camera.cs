using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.GameElements {

    public sealed class Camera {

        #region Properties

        public Vector2 Position { get; set; }
        
        public Scene Scene { get; internal set; }
        public Game Game => Scene?.Game;

        #endregion

        #region Constructors

        public Camera(Vector2 position) {

            this.Position = position;

        }

        #endregion

        #region Methods

        public Matrix GetViewMatrix() {
            
            Vector2 screenCenter = new Vector2((float)Game.GraphicsDevice.Viewport.Width / 2f, (float)Game.GraphicsDevice.Viewport.Height / 2f);
            Matrix matrix = Matrix.CreateTranslation(new Vector3(screenCenter - Position, 0)) * Matrix.CreateTranslation(new Vector3(screenCenter, 0));
            return matrix;

        }

        #endregion

    }

}
