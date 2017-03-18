using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya {

    public interface IDraw {

        int DrawOrder { get; set; }

        void Draw(SpriteBatch spriteBatch, GameTime gameTime);

    }

}
