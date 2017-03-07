using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Content.Pipeline.SpriteAnimation {

    public class SpriteAnimationData {

        // Properties

        public string Name { get; set; }
        public bool IsLooping { get; set; }
        public int FrameCount { get; set; }
        public List<SpriteAnimationFrameData> Frames { get; set; }


    }

}
