using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Graphics {

    public struct SpriteAnimationFrame {

        // Properties

        public Point SpriteIndex { get; set; }
        public float EndTimeStamp { get; set; }

        // Constructor

        public SpriteAnimationFrame(Point spriteIndex, float endTimeStamp) {

            this.SpriteIndex = spriteIndex;
            this.EndTimeStamp = endTimeStamp;

        }

    }
}
