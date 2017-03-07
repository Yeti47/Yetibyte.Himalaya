using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Content.Pipeline.SpriteAnimation {

    public struct SpriteAnimationFrameData {

        // Fields

        public Point SpriteIndex;
        public float EndTimeStamp;

        // Constructor

        public SpriteAnimationFrameData(Point spriteIndex, float endTImeStamp) {

            this.SpriteIndex = spriteIndex;
            this.EndTimeStamp = endTImeStamp;

        } 

    }

}
