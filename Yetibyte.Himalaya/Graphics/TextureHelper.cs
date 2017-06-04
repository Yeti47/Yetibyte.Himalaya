using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Graphics {

    public static class TextureHelper {

        /// <summary>
        /// Creates a simple single-colored texture with the given dimensions.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to create the new texture on.</param>
        /// <param name="color">The color of the texture to create.</param>
        /// <param name="width">The width of the texture to create.</param>
        /// <param name="height">The height of the texture to create.</param>
        /// <returns>A simple single-colored texture with the given dimensions.</returns>
        public static Texture2D CreateColoredBox(GraphicsDevice graphicsDevice, Color color, int width, int height) {

            width = Math.Max(1, width);
            height = Math.Max(1, height);

            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            int size = width * height;

            Color[] data = new Color[size];

            for (int i = 0; i < size; i++)
                data[i] = color;

            return texture;

        }

        /// <summary>
        /// Creates a simple single-colored texture with a border.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to create the new texture on.</param>
        /// <param name="boxColor">The color of the inner area.</param>
        /// <param name="width">The width of inner area. Note: This does not include the border size.</param>
        /// <param name="height">The height of inner area. Note: This does not include the border size.</param>
        /// <param name="borderColor">The color of the border.</param>
        /// <param name="borderSize">The size of the border.</param>
        /// <returns>A simple single-colored texture with a border.</returns>
        public static Texture2D CreateBorderedColoredBox(GraphicsDevice graphicsDevice, Color boxColor, int width, int height, Color borderColor, int borderSize) {

            width = Math.Max(1, width);
            height = Math.Max(1, height);
            borderSize = Math.Max(0, borderSize);

            int totalWidth = width + borderSize * 2;
            int totalHeight = height + borderSize * 2;

            Texture2D texture = new Texture2D(graphicsDevice, totalWidth, totalHeight);

            int size = totalWidth * totalHeight;

            Color[] data = new Color[size];

            for (int i = 0; i < size; i++) {

                int x = i % totalWidth;
                int y = i / totalHeight;

                if (x < borderSize || x >= (totalWidth - borderSize) || y < borderSize || y >= (totalHeight - borderSize))
                    data[i] = borderColor;
                else
                    data[i] = boxColor;

            }

            texture.SetData(data);

            return texture;

        }

    }

}
