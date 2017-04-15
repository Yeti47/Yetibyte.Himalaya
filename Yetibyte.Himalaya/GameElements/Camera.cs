using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.GameElements {

    public sealed class Camera : GameEntity {

        #region Constants

        public const string DEFAULT_CAMERA_NAME = "_CAMERA_";

        #endregion

        #region Constructors

        public Camera(Vector2 position) : base(DEFAULT_CAMERA_NAME, position) {

        }

        #endregion

        #region Methods

        public Matrix GetViewMatrix() {
            
            Vector2 screenCenter = new Vector2((float)Game.GraphicsDevice.Viewport.Width / 2f, (float)Game.GraphicsDevice.Viewport.Height / 2f);
            Matrix matrix = Matrix.CreateTranslation(new Vector3(screenCenter - Transform.Position, 0)) * Matrix.CreateTranslation(new Vector3(screenCenter, 0));
            return matrix;

        }

        #endregion

    }

}
