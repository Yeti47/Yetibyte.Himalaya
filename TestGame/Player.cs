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
using Yetibyte.Himalaya.Extensions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Yetibyte.Himalaya.Debugging;
using Yetibyte.Himalaya.Collision;

namespace TestGame {

    public class Player : GameEntity {

        // Constructor

        public Player(string name, Vector2 position) : base(name, position) {
        }

        // Methods

        public override void Initialize() {
            base.Initialize();

        }

        protected override void Awake() {
            base.Awake();

        }

        public override void Update(GameTime gameTime, float globalTimeScale) {
            base.Update(gameTime, globalTimeScale);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            base.Draw(spriteBatch, gameTime);
            
        }

    }

}
