using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.Collision;

namespace Yetibyte.Himalaya.Debugging {

    public static class DebugUtility {

        #region Fields

        private static Texture2D _dummyTexture;

        #endregion

        #region Methods

        private static void CreateTexture(GraphicsDevice graphicsDevice) {

            _dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            Color[] data = { Color.White };
            _dummyTexture.SetData(data);

        }

        public static void DrawLine(SpriteBatch spritebatch, Vector2 start, Vector2 end, Color color, int thickness = 1) {

            float angle = (float)Math.Atan2(start.Y - end.Y, start.X - end.X);
            float length = Vector2.Distance(start, end);

            DrawLine(spritebatch, start, angle, length, color, thickness);

        }

        public static void DrawLine(SpriteBatch spritebatch, Vector2 start, float angle, float length, Color color, int thickness = 1) {

            if (_dummyTexture == null)
                CreateTexture(spritebatch.GraphicsDevice);

            spritebatch.Draw(_dummyTexture, start, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);

        }

        public static void DrawRectangle(SpriteBatch spritebatch, Rectangle rectangle, Color color, int lineThickness = 1) {

            DrawRectangle(spritebatch, new RectangleF(rectangle), color, lineThickness);

        }

        public static void DrawRectangle(SpriteBatch spritebatch, RectangleF rectangle, Color color, int lineThickness = 1) {

            Vector2[] points = {

                rectangle.Location,
                new Vector2(rectangle.Right, rectangle.Top),
                new Vector2(rectangle.Right, rectangle.Bottom),
                new Vector2(rectangle.Left, rectangle.Bottom)

            };

            for (int i = 0; i < points.Length; i++)
                DrawLine(spritebatch, points[i], (i < points.Length - 1 ? points[i + 1] : points[0]), color, lineThickness);

        }

        public static void DrawFilledRectangle(SpriteBatch spritebatch, Rectangle rectangle, Color color) {

            DrawFilledRectangle(spritebatch, new RectangleF(rectangle), color);

        }

        public static void DrawFilledRectangle(SpriteBatch spritebatch, RectangleF rectangle, Color color) {

            if (_dummyTexture == null)
                CreateTexture(spritebatch.GraphicsDevice);

            spritebatch.Draw(_dummyTexture, rectangle.Location, null, color, 0, Vector2.Zero, rectangle.Size, SpriteEffects.None, 0);

        }

        /// <summary>
        /// Draws the outlines of all <see cref="Collider"/>s that currently live in the given <see cref="Scene"/> on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="scene">The scene the colliders live in.</param>
        /// <param name="color">The color of the outlines drawn.</param>
        public static void VisualizeColliders(SpriteBatch spritebatch, Scene scene, Color color) {

            if (scene == null || scene.Physics == null)
                return;

            foreach (Collider collider in scene.Physics.GetActiveColliders()) {

                switch (collider) {

                    case RectCollider r:
                        DrawRectangle(spritebatch, r.Bounds, color);
                        break;

                    default:
                        break;
                }

            }

        }

        #endregion

    }

}
