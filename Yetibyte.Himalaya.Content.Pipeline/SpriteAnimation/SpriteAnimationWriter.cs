using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework;
using MonoGame.Framework.Content.Pipeline.Builder;
using Yetibyte.Himalaya.Content;

namespace Yetibyte.Himalaya.Content.Pipeline.SpriteAnimation {

    [ContentTypeWriter]
    public class SpriteAnimationWriter : ContentTypeWriter<SpriteAnimationData> {

        public override string GetRuntimeReader(TargetPlatform targetPlatform) {

            return typeof(SpriteAnimationReader).AssemblyQualifiedName;

        }

        
        public override string GetRuntimeType(TargetPlatform targetPlatform) {

            return typeof(Graphics.SpriteAnimation).AssemblyQualifiedName;

        }
        
        protected override void Write(ContentWriter output, SpriteAnimationData value) {

            output.Write(value.Name);
            output.Write(value.IsLooping);
            output.Write(value.FrameDuration);
            output.Write(value.FrameCount);
            
            for (int i = 0; i < value.FrameCount; i++) {

                output.Write(value.SpriteIndices[i]);

            }
            
        }

    }

}
