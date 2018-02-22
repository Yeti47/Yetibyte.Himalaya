using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Yetibyte.Himalaya.Controls {

    public interface IClickable {

        void OnClicked(MouseState mouseState);
        void OnMouseReleased(MouseState mouseState);

    }

}
