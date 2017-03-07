using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.Graphics;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class Actor : GameEntity {
		
		// Fields
		
		// Properties
		
		public Sprite Sprite { get; set; }

        // Constructor

        protected Actor(Scene scene, string name, Vector2 position) : base(scene, name, position) {

        }

        protected Actor(Scene scene, string name, Vector2 position, GameEntity parentEntity) : base(scene, name, position, parentEntity) {

        }

    }
	
}
