﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.Graphics;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class Actor : GameEntity {
		
		// Fields
		
		// Properties
		
		public Sprite Sprite { get; set; }
        public SpriteAnimator Animator { get; set; }
        public float RenderLayerDepth { get; set; }

        /// <summary>
        /// If set to true, the Update method of <see cref="Animator"/> will not be called automatically.
        /// </summary>
        public bool IgnoreAnimator { get; set; } = false;

        // Constructor

        protected Actor(Scene scene, string name, Vector2 position, Sprite sprite, float renderLayerDepth = 0, SpriteAnimator animator = null) : base(scene, name, position) {

            this.Sprite = sprite;
            this.Animator = animator;
            this.RenderLayerDepth = renderLayerDepth;
            
        }
        
        // Methods

        public override void Update(GameTime gameTime, float globalTimeScale) {

            base.Update(gameTime, globalTimeScale);

            if(!IgnoreAnimator)
                Animator?.Update(gameTime, globalTimeScale);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {

            Sprite?.Draw(spriteBatch, Transform.Position, Transform.Rotation, Transform.Scale, RenderLayerDepth);

        }

    }
	
}
