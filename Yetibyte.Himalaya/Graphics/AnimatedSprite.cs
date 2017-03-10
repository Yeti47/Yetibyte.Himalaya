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

    public class AnimatedSprite : Sprite, IUpdate {

        // Nested Enum

        public enum PlayingState { Stopped, Playing, Paused }

        // Properties

        public SpriteAnimation Animation { get; private set; }
        public PlayingState State { get; set; }
        public float PlaybackTime { get; private set; }
        public int CurrentFrameIndex { get; private set; }

        // Constructors

        public AnimatedSprite(Texture2D texture, SpriteAnimation animation) : base(texture) {
                        
            if(animation.FrameCount > 0) {

                this.Index = animation.SpriteIndices[0];
                
            }

            this.Width = animation.SpriteWidth;
            this.Height = animation.SpriteHeight;
            this.State = PlayingState.Stopped;

        }

        // Methods

        /// <summary>
        /// Updated the animation logic.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current game timing values.</param>
        /// <param name="timeScale">Scaling value for elapsed time.</param>
        public override void Update(GameTime gameTime, float timeScale) {

            if (Animation.FrameCount <= 0)
                return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * timeScale;
            
            if (State == PlayingState.Playing) {

                PlaybackTime += deltaTime;
                float frameEndingTime = Animation.FrameDuration * (CurrentFrameIndex + 1);

                if (PlaybackTime >= frameEndingTime) {

                    if (CurrentFrameIndex >= Animation.FrameCount - 1) {

                        if (Animation.IsLooping)
                            Restart();
                        else
                            Pause();

                    }
                    else {

                        CurrentFrameIndex++;

                    }

                }

            }
            
        }

        public void Play() {

            State = PlayingState.Playing;

        }

        public void Pause() {

            State = PlayingState.Paused;

        }

        public void Stop() {

            State = PlayingState.Stopped;
            PlaybackTime = 0f;
            CurrentFrameIndex = 0;

        }

        public void Restart() {

            Stop();
            Play();

        }


    }

}
