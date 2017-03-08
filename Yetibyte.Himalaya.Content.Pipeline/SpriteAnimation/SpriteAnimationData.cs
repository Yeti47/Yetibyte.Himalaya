using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Content.Pipeline.SpriteAnimation {

    public class SpriteAnimationData {

        // Properties

        public string Name { get; set; }
        public bool IsLooping { get; set; }
        public int FramesPerSecond { get; set; }
        public int FrameCount { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
        public Point[] SpriteIndices { get; set; }


    }

}
