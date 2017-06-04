using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.GameElements {

    /// <summary>
    /// The camera is used to determine what part of the game world should be displayed in the game window.
    /// </summary>
    public sealed class Camera : IUpdate {

        #region Constants

        /// <summary>
        /// The minimum zoom factor. <see cref="ZoomFactor"/> is constrained to never be lower than this value.
        /// </summary>
        public const int MIN_ZOOM_FACTOR = 1;

        /// <summary>
        /// The maximum zoom factor. <see cref="ZoomFactor"/> is constrained to never be greater than this value.
        /// </summary>
        public const int MAX_ZOOM_FACTOR = 5000;

        #endregion

        #region Fields

        private float _zoomFactor = 100f;

        #endregion

        #region Properties

        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Rotation { get; set; }
        
        public Scene Scene { get; internal set; }
        public Game Game => Scene?.Game;

        public Viewport Viewport { get; set; }

        /// <summary>
        /// The center coordinates of this <see cref="Camera"/>'s viewport.
        /// </summary>
        public Vector2 ViewportCenter => new Vector2(Viewport.Width / 2f, Viewport.Height / 2f);

        /// <summary>
        /// The camera's zoom factor in percent. 
        /// This value is constrained to lie between <see cref="MIN_ZOOM_FACTOR"/> and <see cref="MAX_ZOOM_FACTOR"/>.
        /// </summary>
        public float ZoomFactor {

            get => _zoomFactor;
            set => _zoomFactor = MathHelper.Clamp(_zoomFactor, MIN_ZOOM_FACTOR, MAX_ZOOM_FACTOR);

        }

        /// <summary>
        /// The translation <see cref="Matrix"/> of this camera.
        /// </summary>
        public Matrix ViewMatrix {

            get {

                float normalizedZoom = ZoomFactor / 100f;

                Matrix viewMatrix = Matrix.CreateTranslation(-Position.X, -Position.Y, 0f) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(normalizedZoom, normalizedZoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));

                return viewMatrix;

            }

        }

        /// <summary>
        /// The Target can be used to make the <see cref="Camera"/> follow a specific GameEntity, so that it is always in the center
        /// of the viewport. If the Target is set to 'null', the camera will stop following any GameEntity.
        /// </summary>
        public GameEntity Target { get; set; }

        /// <summary>
        /// The camera's bounds in world space.
        /// </summary>
        public RectangleF WorldBounds {

            get {

                Vector2 location = ViewportToWorldPoint(Vector2.Zero);
                Vector2 size = ViewportToWorldPoint(new Vector2(Viewport.Width, Viewport.Height)) - location;

                return new RectangleF(location, size);

            }

        }

        #endregion

        #region Constructors

        public Camera(Scene scene, Viewport viewport) {

            this.Scene = scene;
            this.Viewport = viewport;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Moves the <see cref="Camera"/> so that a specific <see cref="GameEntity"/> is in the center of the viewport.
        /// </summary>
        /// <param name="gameEntity">The GameEntity to move the camera to.</param>
        public void LookAt(GameEntity gameEntity) {

            Position = gameEntity.Transform.Position;

        }

        public void Update(GameTime gameTime, float globalTimeScale) {

            if (Target != null)
                LookAt(Target);

        }

        /// <summary>
        /// Converts world coordinates to viewport coordinates.
        /// </summary>
        /// <param name="worldPoint">The coordinates in the game world to convert to viewport coordinates.</param>
        /// <returns>The given world coordinates converted into viewport coordinates.</returns>
        public Vector2 WorldToViewportPoint(Vector2 worldPoint) => Vector2.Transform(worldPoint, ViewMatrix);

        /// <summary>
        /// Converts viewport coordinates to world coordinates.
        /// </summary>
        /// <param name="viewportPoint">The viewport coordinates to convert to world coordinates.</param>
        /// <returns>The given viewport coordinates converted into world coordinates.</returns>
        public Vector2 ViewportToWorldPoint(Vector2 viewportPoint) => Vector2.Transform(viewportPoint, Matrix.Invert(ViewMatrix));

        #endregion

    }

}
