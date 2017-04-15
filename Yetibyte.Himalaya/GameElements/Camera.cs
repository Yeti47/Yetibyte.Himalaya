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

        public Matrix GetMatrix() {

            Matrix matrix = Matrix.CreateTranslation(new Vector3(Transform.Position, 0));
            return matrix;

        }

        #endregion

    }

}
