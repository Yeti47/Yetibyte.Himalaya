using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Extensions {

    public static class GameTimeExtensions {

        /// <summary>
        /// Returns the time in seconds elapsed between the previous frame and the current frame.
        /// Shorthand for "(float)GameTime.ElapsedGameTime.TotalSeconds".
        /// </summary>
        /// <param name="gameTime">An instance of GameTime.</param>
        public static float DeltaTime(this GameTime gameTime) => (float)gameTime.ElapsedGameTime.TotalSeconds;

    }

}
