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

        private ControlSettings controlSettings;
        private ControlsManager controlManager;

        public float Speed { get; set; } = 150f;

        // Constructor

        public Player(Scene scene, string name, Vector2 position, Sprite sprite, int renderLayer = 1, SpriteAnimator animator = null) : base(scene, name, position, sprite, renderLayer, animator) {
            
        }

        // Methods

        public override void Initialize() {
            base.Initialize();

            controlSettings = new ControlSettings();
            controlSettings.ControlMap.Add("Left", new GameControl { Key = Keys.Left, AlternativeKey = Keys.A } );
            controlSettings.ControlMap.Add("Right", new GameControl { Key = Keys.Right, AlternativeKey = Keys.D });
            controlSettings.ControlMap.Add("Up", new GameControl { Key = Keys.Up, AlternativeKey = Keys.W });
            controlSettings.ControlMap.Add("Down", new GameControl { Key = Keys.Down, AlternativeKey = Keys.S });

            controlManager = new ControlsManager(PlayerIndex.One, controlSettings);

        }

        public override void Update(GameTime gameTime, float globalTimeScale) {
            base.Update(gameTime, globalTimeScale);

            controlManager.Update(gameTime);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(controlManager.GetButtonDown("Left")) {

                Transform.Position -= new Vector2(Speed * deltaTime, 0f);

            }
            else if(controlManager.GetButtonDown("Right")) {

                Transform.Position += new Vector2(Speed * deltaTime, 0f);

            }

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            base.Draw(spriteBatch, gameTime);
            
        }

    }

}
