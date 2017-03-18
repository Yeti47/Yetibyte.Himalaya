using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class Scene : ITimeScale {

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

        public List<Actor> Actors => GameEntities.Where(e => e is Actor).Cast<Actor>().ToList();

        /// <summary>
        /// Returns a list of all GameEntities in this scene that are currently active and not destroyed.
        /// </summary>
        public List<GameEntity> LiveGameEntities => GameEntities.Where(e => e.IsActive && !e.IsDestroyed).ToList();

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
									
			while(GameEntitiesToRemove.Count > 0) {
				
				GameEntities.Remove(GameEntitiesToRemove.Dequeue());
				
			}	
			
			foreach(GameEntity gameEntity in GameEntities) {
				
				if(gameEntity.IsActive && !gameEntity.IsDestroyed)
					gameEntity.Update(gameTime, TimeScale);
				
			}

            while (GameEntitiesToAdd.Count > 0) {

                GameEntity newEntity = GameEntitiesToAdd.Dequeue();

                if (newEntity.IsActive && !newEntity.IsDestroyed)
                    newEntity.Update(gameTime, TimeScale);

                GameEntities.Add(newEntity);

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
					gameEntity.Draw(spriteBatch, gameTime);
				
			}				
			
		}
		/// <summary>
        /// Adds the given GameEntity to this Scene. Processing of newly added Entity will start after all existing Entities have been processed.
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

        /// <summary>
        /// Returns a list of all <see cref="GameEntity"/> objects of the given Type that currently live in this scene.
        /// </summary>
        /// <typeparam name="T">The type of the GameEntities to filter out.</typeparam>
        public List<T> GetGameEntitiesOfType<T>() where T : GameEntity {

            return GameEntities.Where(e => e is T).Cast<T>().ToList();

        }

        /// <summary>
        /// Returns the <see cref="Microsoft.Xna.Framework.Game"/> this Scene belongs to casted to
        /// the derived class defined by T.
        /// </summary>
        /// <typeparam name="T">A subclass of <see cref="Microsoft.Xna.Framework.Game"/>.</typeparam>
        public T GetGame<T> () where T : Game {

            return (T)Game;

        }
		
	}
	
}
