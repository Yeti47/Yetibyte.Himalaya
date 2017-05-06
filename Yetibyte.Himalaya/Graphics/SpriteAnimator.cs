using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Graphics {

    public class SpriteAnimator : EntityComponent, IUpdate, ITimeScale {

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
        /// Creates a new <see cref="SpriteAnimator"/> component.
        /// </summary>
        /// <param name="animation">The animation to use.</param>
        public SpriteAnimator(SpriteAnimation animation) {

            this.Animation = animation;
            this.State = PlayingState.Stopped;

        }

        // Methods

        /// <summary>
        /// Updates the animation logic.
        /// </summary>
        /// <param name="gameTime">A snapshot of the current game timing values.</param>
        /// <param name="globalTimeScale">Scaling value for elapsed time.</param>
        public void Update(GameTime gameTime, float globalTimeScale) {

            if (Animation == null || Animation.FrameCount <= 0)
                return;

            Sprite = FindSprite();

            if (Sprite == null)
                return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * globalTimeScale * TimeScale;
            
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
        /// Makes sure this <see cref="SpriteAnimator"/> uses the correct instance of <see cref="Graphics.Sprite"/>. 
        /// 
        /// <para>If the Sprite of this animator is null or was attached to a different <see cref="GameEntity"/>, this method 
        /// looks for the first Sprite component in the <see cref="GameEntity"/> this animator is attached to and returns it. 
        /// Otherwise the currently used Sprite is returned.
        /// </para>
        /// </summary>
        /// <returns>The <see cref="Graphics.Sprite"/> this animator should animate. Null if no Sprite could be found.</returns>
        private Sprite FindSprite() {

            if (!IsAttached)
                return null;

            if(Sprite == null || Sprite.GameEntity != GameEntity) {

                Sprite resultSprite = GameEntity.GetComponent<Sprite>();

                if(resultSprite != null)
                    UpdateSprite();

                return resultSprite;

            }

            return Sprite;

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
