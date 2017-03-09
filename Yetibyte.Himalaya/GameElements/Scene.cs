using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class Scene {

        // Properties

        /// <summary>
        /// A global scaling value for time based operations to simulate the effect of slowing down or speeding up time. This value is passed
        /// to the Update method of all objects that implement the IUpdate interface.
        /// </summary>
        public float TimeScale { get; set; } = 1f;

        /// <summary>
        /// A collection of all the GameEntities that currently live in this Scene.
        /// </summary>
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
		
        /// <summary>
        /// Responsible for loading content that is used in this Scene.
        /// </summary>
		public virtual void LoadContent() {
						
		}
		
        /// <summary>
        /// Calls the Initialize method of each GameEntity within this Scene.
        /// </summary>
		public virtual void Initialize() {
			
			foreach(GameEntity gameEntity in GameEntities) {
				
				gameEntity.Initialize();
				
			}
			
		}
		
        /// <summary>
        /// Calls the Update method of each GameEntity within this Scene.
        /// </summary>
        /// <param name="gameTime">Provides snapshot of current timing values.</param>
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

        /// <summary>
        /// Calls the Draw method of each GameEntity within this Scene.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use for rendering.</param>
        /// <param name="gameTime">Provides snapshot of current timing values.</param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			
			foreach(GameEntity gameEntity in GameEntities) {
				
				if(gameEntity.IsActive)
					gameEntity.Draw(gameTime);
				
			}				
			
		}
		/// <summary>
        /// Adds the given GameEntity to this Scene. Please note that the processing of newly added GameEntities will start in the next
        /// Update interval.
        /// </summary>
        /// <param name="gameEntity">The GameEntity to add to the Scene.</param>
		public void AddGameEntity(GameEntity gameEntity) {
			
			GameEntitiesToAdd.Enqueue(gameEntity);
			gameEntity.Initialize();
			
		}
		
        /// <summary>
        /// Removes the given GameEntity from this Scene. Please note that GameEntities will effectively be removed at the start of the next
        /// Update interval.
        /// </summary>
        /// <param name="gameEntity">The GameEntity to remove from this Scene.</param>
		public void RemoveGameEntity(GameEntity gameEntity) {
			
			GameEntitiesToRemove.Enqueue(gameEntity);
			
		}
		
	}
	
}
