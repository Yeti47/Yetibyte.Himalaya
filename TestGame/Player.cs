using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.Graphics;
using Yetibyte.Himalaya.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace TestGame {

    public class Player : Actor {
        
        public float Speed { get; set; } = 150f;

        // Constructor

        public Player(Scene scene, string name, Vector2 position, Sprite sprite, int renderLayer = 1, SpriteAnimator animator = null) : base(scene, name, position, sprite, renderLayer, animator) {
            
        }

        // Methods

        public override void Initialize() {
            base.Initialize();

        }

        public override void Update(GameTime gameTime, float globalTimeScale) {
            base.Update(gameTime, globalTimeScale);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ControlListener controlListener = Scene.GetGame<Game1>().ControlListenerPlayer1;

            if(controlListener.GetButtonDown("Left")) {

                Transform.Position -= new Vector2(Speed * deltaTime, 0f);

            }
            else if(controlListener.GetButtonDown("Right")) {

                Transform.Position += new Vector2(Speed * deltaTime, 0f);

            }

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            base.Draw(spriteBatch, gameTime);
            
        }

    }

}
