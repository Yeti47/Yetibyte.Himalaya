using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Graphics {

    public class SpriteAnimator : IUpdate, ITimeScale {

        // Nested Enum

        public enum PlayingState { Stopped, Playing, Paused }

        // Properties

        public Sprite Sprite { get; private set; }
        public SpriteAnimation Animation { get; private set; }
        public PlayingState State { get; set; }
        public float PlaybackTime { get; private set; }
        public int CurrentFrameIndex { get; private set; } = 0;

        public float TimeScale { get; set; } = 1f;

        // Constructors

        /// <summary>
        /// Creates a new <see cref="SpriteAnimator"/> for the given <see cref="Yetibyte.Himalaya.Graphics.Sprite"/>.
        /// </summary>
        /// <param name="sprite">The sprite to animate.</param>
        /// <param name="animation">The animation to use.</param>
        public SpriteAnimator(Sprite sprite, SpriteAnimation animation) {
                        
            if(animation.FrameCount > 0) {

                UpdateSprite();

            }
            
            this.State = PlayingState.Stopped;

        }

        // Methods

        /// <summary>
        /// Updates the animation logic.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current game timing values.</param>
        /// <param name="globalTimeScale">Scaling value for elapsed time.</param>
        public void Update(GameTime gameTime, float globalTimeScale) {

            if (Animation.FrameCount <= 0)
                return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * globalTimeScale;
            
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
                        UpdateSprite();

                    }

                }

            }
            
        }

        /// <summary>
        /// Applies all changes determined by the animation logic to the <see cref="Yetibyte.Himalaya.Graphics.Sprite"/>.
        /// </summary>
        private void UpdateSprite() {

            Sprite.Index = Animation.SpriteIndices[CurrentFrameIndex];

        }

        /// <summary>
        /// Plays the animation.
        /// </summary>
        public void Play() {

            State = PlayingState.Playing;

        }

        /// <summary>
        /// Pauses the animation. When <see cref="Play()"/> is called again, the state of the animation will be resumed.
        /// </summary>
        public void Pause() {

            State = PlayingState.Paused;

        }

        /// <summary>
        /// Stops the animation. When <see cref="Play()"/> is called again, the animation will restart from the first frame.
        /// </summary>
        public void Stop() {

            State = PlayingState.Stopped;
            PlaybackTime = 0f;
            CurrentFrameIndex = 0;
            UpdateSprite();

        }

        /// <summary>
        /// Stops the animation and restarts it from the first frame.
        /// </summary>
        public void Restart() {

            Stop();
            Play();

        }


    }

}
