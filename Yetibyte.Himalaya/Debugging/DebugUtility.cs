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

        /// <summary>
        /// Creates the 1x1 <see cref="Texture2D"/> used for drawing objects on the screen.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use for the texture.</param>
        private static void CreateTexture(GraphicsDevice graphicsDevice) {

            _dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            Color[] data = { Color.White };
            _dummyTexture.SetData(data);

        }

        /// <summary>
        /// Draws a line segment on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="start">The coordinates of the line's start point.</param>
        /// <param name="end">The coordinates of the line's end point.</param>
        /// <param name="color">The color to draw the line.</param>
        /// <param name="thickness">The thickness of the line to draw.</param>
        public static void DrawLine(SpriteBatch spritebatch, Vector2 start, Vector2 end, Color color, int thickness = 1) {

            float angle = (float)Math.Atan2(start.Y - end.Y, start.X - end.X);
            float length = Vector2.Distance(start, end);

            DrawLine(spritebatch, start, angle, length, color, thickness);

        }

        /// <summary>
        /// Draws a line segment on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="start">The coordinates of the line's start point.</param>
        /// <param name="angle">The angle of the line in radians.</param>
        /// <param name="length">The length of the line.</param>
        /// <param name="color">The color to draw the line.</param>
        /// <param name="thickness">The thickness of the line to draw.</param>
        public static void DrawLine(SpriteBatch spritebatch, Vector2 start, float angle, float length, Color color, int thickness = 1) {

            if (_dummyTexture == null)
                CreateTexture(spritebatch.GraphicsDevice);

            spritebatch.Draw(_dummyTexture, start, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);

        }

        /// <summary>
        /// Draws a line segment on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="line">The line segment to draw.</param>
        /// <param name="color">The color to draw the line.</param>
        /// <param name="thickness">The thickness of the line to draw.</param>
        public static void DrawLine(SpriteBatch spritebatch, LineSegment line, Color color, int thickness = 1) {

            DrawLine(spritebatch, line.Start, line.End, color, thickness);

        }

        /// <summary>
        /// Draws the outlines of the given <see cref="Rectangle"/> on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="rectangle">The rectangle to draw the outlines of.</param>
        /// <param name="color">The color to draw the outlines.</param>
        /// <param name="lineThickness">The thickness of the lines to draw.</param>
        public static void DrawRectangle(SpriteBatch spritebatch, Rectangle rectangle, Color color, int lineThickness = 1) {

            DrawRectangle(spritebatch, new RectangleF(rectangle), color, lineThickness);

        }

        /// <summary>
        /// Draws the outlines of the given <see cref="RectangleF"/> on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="rectangle">The rectangle to draw the outlines of.</param>
        /// <param name="color">The color to draw the outlines.</param>
        /// <param name="lineThickness">The thickness of the lines to draw.</param>
        public static void DrawRectangle(SpriteBatch spritebatch, RectangleF rectangle, Color color, int lineThickness = 1) {

            foreach (LineSegment line in rectangle.GetEdges())
                DrawLine(spritebatch, line, color, lineThickness);

        }

        /// <summary>
        /// Draws a filled <see cref="Rectangle"/> on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The color to draw the rectangle.</param>
        public static void DrawFilledRectangle(SpriteBatch spritebatch, Rectangle rectangle, Color color) {

            DrawFilledRectangle(spritebatch, new RectangleF(rectangle), color);

        }

        /// <summary>
        /// Draws a filled <see cref="RectangleF"/> on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The color to draw the rectangle.</param>
        public static void DrawFilledRectangle(SpriteBatch spritebatch, RectangleF rectangle, Color color) {

            if (_dummyTexture == null)
                CreateTexture(spritebatch.GraphicsDevice);

            spritebatch.Draw(_dummyTexture, rectangle.Location, null, color, 0, Vector2.Zero, rectangle.Size, SpriteEffects.None, 0);

        }

        /// <summary>
        /// Draws the edges of the given <see cref="IEdges"/> on the screen.
        /// </summary>
        /// <param name="spritebatch">The spritebatch to use for rendering.</param>
        /// <param name="edges">The instance of <see cref="IEdges"/> that holds the line segments to draw.</param>
        /// <param name="color">The color to draw the lines.</param>
        /// <param name="thickness">The thickness of the lines to draw..</param>
        public static void DrawEdges(SpriteBatch spritebatch, IEdges edges, Color color, int thickness = 1) {

            foreach (LineSegment line in edges.GetEdges())
                DrawLine(spritebatch, line, color, thickness);

        }

        /// <summary>
        /// Draws the outlines of all active <see cref="Collider"/>s that currently live in the given <see cref="Scene"/> on the screen.
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
