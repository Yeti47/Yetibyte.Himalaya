using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class Scene {

        // Properties

        public float TimeScale { get; set } = 1f;

		public List<GameEntity> GameEntities { get; protected set; }
		public Queue<GameEntity> GameEntitiesToAdd { get; protected set; }
		public Queue<GameEntity> GameEntitiesToRemove { get; protected set;}
		
		public Game Game { get; protected set; }
				
		// Constructor
				
		protected Scene(Game game) {
			
			this.Game = game;
			this.GameEntities = new List<GameEntity>();
			this.GameEntitiesToAdd = new Queue<GameEntity>();
			this.GameEntitiesToRemove = new Queue<GameEntity>();
			
		}
		
		// Methods
		
		public virtual void LoadContent() {
						
		}
		
		public virtual void Initialize() {
			
			foreach(GameEntity gameEntity in GameEntities) {
				
				gameEntity.Initialize();
				
			}
			
		}
		
		public virtual void Update(GameTime gameTime) {
			
			while(GameEntitiesToAdd.Count > 0) {
				
				GameEntities.Add(GameEntitiesToAdd.Dequeue());
				
			}
			
			while(GameEntitiesToRemove.Count > 0) {
				
				GameEntities.Remove(GameEntitiesToRemove.Dequeue());
				
			}	
			
			foreach(GameEntity gameEntity in GameEntities) {
				
				if(gameEntity.IsActive)
					gameEntity.Update(gameTime, TimeScale);
				
			}
						
		}
		
		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			
			foreach(GameEntity gameEntity in GameEntities) {
				
				if(gameEntity.IsActive)
					gameEntity.Draw(gameTime);
				
			}				
			
		}
		
		public void AddGameEntity(GameEntity gameEntity) {
			
			GameEntitiesToAdd.Enqueue(gameEntity);
			gameEntity.Initialize();
			
		}
		
		public void RemoveGameEntity(GameEntity gameEntity) {
			
			GameEntitiesToRemove.Enqueue(gameEntity);
			
		}
		
	}
	
}
