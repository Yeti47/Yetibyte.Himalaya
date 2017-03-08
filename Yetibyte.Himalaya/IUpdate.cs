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

        void Update(GameTime gameTime, float timeScale);

    }
}
