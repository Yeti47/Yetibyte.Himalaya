using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.Graphics;

namespace Yetibyte.Himalaya.GameElements {

    [Obsolete("The newly introduced component system makes this class obsolete. It can no longer be used and will most likely be removed shortly.")]
    public abstract class Actor : GameEntity {
		
		// Fields
		
		// Properties

        // Constructor

        protected Actor(string name, Vector2 position, Sprite sprite, SpriteAnimator animator = null) : base(name, position) {

            //this.Sprite = sprite;
            //this.Animator = animator;
            
        }
        
        // Methods

        public override void Update(GameTime gameTime, float globalTimeScale) {

            base.Update(gameTime, globalTimeScale);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {

            base.Draw(spriteBatch, gameTime);

        }

    }
	
}
