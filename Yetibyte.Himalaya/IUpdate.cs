using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya {

    /// <summary>
    /// Interface for objects that can be updated.
    /// </summary>
    public interface IUpdate {

        /// <summary>
        /// Updates the logic. 
        /// </summary>
        /// <param name="gameTime">A snapshot of the current game timing values.</param>
        /// <param name="globalTimeScale">Scaling value for elapsed time.</param>
        void Update(GameTime gameTime, float globalTimeScale);

    }
}
