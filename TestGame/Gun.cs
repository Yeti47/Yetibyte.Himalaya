using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.Graphics;
using Yetibyte.Himalaya.Controls;
using Yetibyte.Himalaya.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame {

    public class Gun : Actor {

        // Constructor

        public Gun(string name, Vector2 position, Sprite sprite, int renderLayer = 1, SpriteAnimator animator = null) : base(name, position, sprite, renderLayer, animator) {

        }
        
    }

}
