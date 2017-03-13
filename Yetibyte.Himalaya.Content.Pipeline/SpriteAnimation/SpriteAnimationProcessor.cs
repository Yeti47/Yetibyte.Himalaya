using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

using TInput = System.String;
using TOutput = Yetibyte.Himalaya.Content.Pipeline.SpriteAnimation.SpriteAnimationData;

namespace Yetibyte.Himalaya.Content.Pipeline.SpriteAnimation {
    
    [ContentProcessor(DisplayName = "Himalaya Sprite Animation Processor")]
    public class SpriteAnimationProcessor : ContentProcessor<TInput, TOutput> {

        public override TOutput Process(TInput input, ContentProcessorContext context) {

            Script luaScript = new Script();
            DynValue dv = luaScript.DoString(input);

            Table animationTable = luaScript.Globals.Get("animation").Table;
            string name = animationTable.Get("name").String;
            float frameDuration = (float)animationTable.Get("frameDuration").Number;
            bool isLooping = animationTable.Get("isLooping").Boolean;
            Table frames = animationTable.Get("frames").Table;

            TOutput animationData = new TOutput {

                Name = name,
                IsLooping = isLooping,
                FrameDuration = frameDuration,
                FrameCount = frames.Length

            };

            animationData.SpriteIndices = new Vector2[frames.Length];

            for (int i = 1; i < frames.Length + 1; i++) {

                Table currentFrame = frames.Get(i).Table;
                Table spriteIndex = currentFrame.Get("spriteIndex").Table;
                int spriteIndexX = (int)spriteIndex.Get("x").Number;
                int spriteIndexY = (int)spriteIndex.Get("y").Number;

                animationData.SpriteIndices[i - 1] = new Vector2(spriteIndexX, spriteIndexY);

            }

            return animationData;

        }

    }

}