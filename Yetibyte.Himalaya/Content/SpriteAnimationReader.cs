using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Yetibyte.Himalaya.Graphics;

namespace Yetibyte.Himalaya.Content {

    public class SpriteAnimationReader : ContentTypeReader<SpriteAnimation> {

        protected override SpriteAnimation Read(ContentReader input, SpriteAnimation existingInstance) {

            string name = input.ReadString();
            bool isLooping = input.ReadBoolean();
            float frameDuration = input.ReadSingle();
            int frameCount = input.ReadInt32();

            SpriteAnimation animation = new SpriteAnimation(name, frameDuration, isLooping);

            for (int i = 0; i < frameCount; i++) {

                animation.SpriteIndices[i] = input.ReadVector2().ToPoint();

            }

            return animation;

        }

    }

}
