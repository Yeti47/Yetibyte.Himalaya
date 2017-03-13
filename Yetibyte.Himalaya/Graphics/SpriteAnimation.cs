using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.GameElements;
using MonoGame.Framework;

namespace Yetibyte.Himalaya.Graphics {

    public class SpriteAnimation {

        // Properties

        public string Name { get; private set; }

        public float FrameDuration { get; private set; }
        public List<Point> SpriteIndices { get; set; }

        public bool IsLooping { get; set; }

        public int FrameCount {

            get { return SpriteIndices != null ? SpriteIndices.Count : 0; }

        }
        
        // Constructor

        public SpriteAnimation(string name, float frameDuration, bool isLooping) {
            
        }

    }
}
