﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yetibyte.Himalaya.Collision;
using Yetibyte.Himalaya.Graphics;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class Scene : ITimeScale {

        #region Fields

        private SpriteBatch _spriteBatch;

        private List<GuiCanvas> _guiCanvases = new List<GuiCanvas>();

        #endregion

        #region Properties

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
        public Queue<GameEntity> GameEntitiesToRemove { get; protected set; }

        public Game Game { get; protected set; }

        public IEnumerable<Actor> Actors => GameEntities.OfType<Actor>();

        public Physics Physics { get; protected set; }
        public Camera Camera { get; private set; }

        /// <summary>
        /// Returns a list of all GameEntities in this scene that are currently active and not destroyed.
        /// </summary>
        public IEnumerable<GameEntity> LiveGameEntities => GameEntities.Where(e => e.IsActive && !e.IsDestroyed);

        #endregion

        #region Constructors

        protected Scene(Game game) {

            this.Game = game;
            this.GameEntities = new List<GameEntity>();
            this.GameEntitiesToAdd = new Queue<GameEntity>();
            this.GameEntitiesToRemove = new Queue<GameEntity>();
            this.Physics = new Physics(this);
            this.Camera = new Camera(new Vector2(game.GraphicsDevice.Viewport.Width/2, game.GraphicsDevice.Viewport.Height/2));
            this.Camera.Scene = this;

        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Responsible for loading content that is used in this Scene.
        /// </summary>
        public virtual void LoadContent() {

            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        }

        /// <summary>
        /// Calls the Initialize method of each GameEntity within this Scene.
        /// </summary>
        public virtual void Initialize() {

            foreach (GameEntity gameEntity in GameEntities) {

                gameEntity.Initialize();

            }

        }

        /// <summary>
        /// Calls the Update method of each GameEntity within this Scene.
        /// </summary>
        /// <param name="gameTime">Provides snapshot of current timing values.</param>
        public virtual void Update(GameTime gameTime) {

            Physics.BuildCollisionTree();

            while (GameEntitiesToRemove.Count > 0) {

                GameEntities.Remove(GameEntitiesToRemove.Dequeue());

            }

            foreach (GameEntity gameEntity in GameEntities) {

                if (gameEntity.IsActive && !gameEntity.IsDestroyed)
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
        /// <param name="gameTime">Provides snapshot of current timing values.</param>
        public virtual void Draw(GameTime gameTime) {

            foreach (GuiCanvas guiCanvas in _guiCanvases.Where(c => c.IsVisible).OrderBy(c => c.DrawOrder)) {

                guiCanvas.Draw(gameTime);

            }

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetViewMatrix());

            foreach (GameEntity gameEntity in GameEntities.OrderBy(e => e.DrawOrder)) {

                if (gameEntity.IsActive)
                    gameEntity.Draw(_spriteBatch, gameTime);

            }

            _spriteBatch.End();

        }
        /// <summary>
        /// Adds the given GameEntity to this Scene. Processing of newly added Entity will start after all existing Entities have been processed.
        /// </summary>
        /// <param name="gameEntity">The GameEntity to add to the Scene.</param>
        public void AddGameEntity(GameEntity gameEntity) {

            // Cancel if the Game Entity was already added to the Scene.
            if (GameEntities.Contains(gameEntity) || GameEntitiesToAdd.Contains(gameEntity))
                return;

            gameEntity.Scene = this;
            GameEntitiesToAdd.Enqueue(gameEntity);
            gameEntity.Initialize();

        }

        /// <summary>
        /// Removes the given GameEntity from this Scene. Please note that GameEntities will effectively be removed at the start of the next
        /// Update interval.
        /// </summary>
        /// <param name="gameEntity">The GameEntity to remove from this Scene.</param>
        public void RemoveGameEntity(GameEntity gameEntity) {

            // Cancel if the Game Entity is not in this Scene or was already enqueued to be removed.
            if (!GameEntities.Contains(gameEntity) || GameEntitiesToRemove.Contains(gameEntity))
                return;

            GameEntitiesToRemove.Enqueue(gameEntity);

        }

        /// <summary>
        /// Returns a list of all <see cref="GameEntity"/> objects of the given Type that currently live in this scene.
        /// </summary>
        /// <typeparam name="T">The type of the GameEntities to filter out.</typeparam>
        public IEnumerable<T> GetGameEntitiesOfType<T>() where T : GameEntity => GameEntities.OfType<T>();

        /// <summary>
        /// Returns the <see cref="Microsoft.Xna.Framework.Game"/> this Scene belongs to cast to
        /// the derived class defined by T.
        /// </summary>
        /// <typeparam name="T">A subclass of <see cref="Microsoft.Xna.Framework.Game"/>.</typeparam>
        public T GetGame<T>() where T : Game => (T)Game;

        /// <summary>
        /// Searches this Scene for the first <see cref="GameEntity"/> with the given name (that is not destroyed) and returns it.
        /// </summary>
        /// <typeparam name="T">The type of GameEntity to search for.</typeparam>
        /// <param name="name">The name to search for.</param>
        /// <returns>The first <see cref="GameEntity"/> of type T in this Scene that is not destroyed and has the given name.
        /// Null if no Entity was found.</returns>
        public T FindGameEntity<T>(string name) where T : GameEntity {

            foreach (GameEntity gameEntity in GameEntities) {

                if (gameEntity.Name == name && !gameEntity.IsDestroyed && gameEntity is T)
                    return (T)gameEntity;

            }

            return null;

        }

        /// <summary>
        /// Adds the given <see cref="GuiCanvas"/> to this Scene.
        /// </summary>
        /// <param name="guiCanvas">The GuiCanvas to add to this Scene.</param>
        public void AddGuiCanvas(GuiCanvas guiCanvas) {

            // Cancel if the GUI Canvas was already added to the Scene.
            if (_guiCanvases.Contains(guiCanvas))
                return;

            _guiCanvases.Add(guiCanvas);

        }

        /// <summary>
        /// Removes the given <see cref="GuiCanvas"/> from this Scene.
        /// </summary>
        /// <param name="guiCanvas">The GuiCanvas to remove.</param>
        public void RemoveGuiCanvas(GuiCanvas guiCanvas) {

            // If the GUI Canvas is not included in the list of canvases, cancel
            if (!_guiCanvases.Contains(guiCanvas))
                return;

            _guiCanvases.Remove(guiCanvas);

        }

        #endregion

    }
	
}
