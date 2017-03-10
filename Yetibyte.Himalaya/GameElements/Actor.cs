using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.Graphics;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class Actor : GameEntity, IDraw {
		
		// Fields
		
		// Properties
		
		public Sprite Sprite { get; set; }
        public SpriteAnimator Animator { get; set; }
        public int RenderLayer { get; set; }

        // Constructor

        protected Actor(Scene scene, string name, Vector2 position, Sprite sprite, SpriteAnimator animator = null, GameEntity parentEntity = null) : base(scene, name, position, parentEntity) {

        }
        
        // Methods

        public override void Update(GameTime gameTime, float globalTimeScale) {

            base.Update(gameTime, globalTimeScale);

            Animator.Update(gameTime, globalTimeScale);

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {

            Sprite.Draw(spriteBatch, Position, Rotation, RenderLayer);

        }

    }
	
}
