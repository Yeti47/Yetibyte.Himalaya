﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class GameEntity : IUpdate, ITimeScale, IDraw {

        // Fields

        private bool _isActive = true;
		private List<GameEntity> _childEntities = new List<GameEntity>();
        private GameEntity _parentEntity;
		
		// Properties

        /// <summary>
        /// A <see cref="GameEntity"/> that is inactive will be ignored by the <see cref="Yetibyte.Himalaya.GameElements.Scene"/> and will
        /// therefore not be updated.
        /// </summary>
        public bool IsActive {

            get { return _isActive; }
			set { _isActive = value;}

		}
		
		public String Name { get; protected set; }

        /// <summary>
        /// The <see cref="Yetibyte.Himalaya.GameElements.Scene"/> this GameEntity lives in.
        /// </summary>
	    public Scene Scene { get; protected set; }
		
		public Game Game {
			
			get { 
			
				if(Scene == null)
					return null;
				
				return Scene.Game;
				
			}
			
		}

        /// <summary>
        /// Sets the parent <see cref="GameEntity"/> of this <see cref="GameEntity"/>. Will automatically call AddChildEntity and RemoveChildEntity methods where needed.
        /// That means: When the parent entity changes, this GameEntity will be added to the child entity list of the future parent and removed from the orignal parent's list of child entities
        /// (unless the original parent was null.
        /// </summary>
        public GameEntity ParentEntity {

            get { return _parentEntity; }

            set {

                GameEntity futureParent = value;

                if(this.HasParent) {

                    _parentEntity.RemoveChildEntity(this);

                }

                futureParent?.AddChildEntity(this);

                _parentEntity = futureParent;

            }

        }

        /// <summary>
        /// A list of Game Entities that are children of this <see cref="GameEntity"/>.
        /// </summary>
		public List<GameEntity> ChildEntities {
			
			get { return _childEntities; }
			protected set { _childEntities = value; }
			
		}

        public float TimeScale { get; set; } = 1f;

        public bool IsDestroyed { get; protected set; }

        public Transform Transform { get; set; } = new Transform();

        public bool HasParent => _parentEntity != null;

        // Constructor

        protected GameEntity(Scene scene, string name, Vector2 position) {
			
			this.Scene = scene;
			this.Name = name;
			this.Transform.Position = position;

		}
        
        // Methods

		public virtual void Initialize() {
						
		}
		
		public virtual void Update(GameTime gameTime, float globalTimeScale) {
			
			
					
		}
		
		public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
									
		}
		
        /// <summary>
        /// Destroys this <see cref="GameEntity"/>. It will be removed from the <see cref="Yetibyte.Himalaya.GameElements.Scene"/> it lived in.
        /// </summary>
		public void DestroyEntity() {
			
			foreach(GameEntity childEntity in ChildEntities)
				childEntity.DestroyEntity();

            if(!IsDestroyed) {

                IsDestroyed = true;
                Scene.RemoveGameEntity(this);

            }
            			
		}

        /// <summary>
        /// Adds the given <see cref="GameEntity"/> to the list of child entities. Also sets the parent entity respectively.
        /// </summary>
        /// <param name="childEntity">The child entity to add.</param>
        public void AddChildEntity(GameEntity childEntity) {

            if (!HasChild(childEntity)) {

                ChildEntities.Add(childEntity);
                Transform.AddChild(childEntity.Transform);
                childEntity.ParentEntity = this;

            }
            
        }

        /// <summary>
        /// Removes the given  <see cref="GameEntity"/> from the list of child entities. Also sets the parent of the given entity to null.
        /// </summary>
        /// <param name="childEntity">The child entity to remove.</param>
        public void RemoveChildEntity(GameEntity childEntity) {

            if (HasChild(childEntity)) {

                ChildEntities.Remove(childEntity);
                Transform.RemoveChild(childEntity.Transform);
                childEntity.ParentEntity = null;

            }
            
        }

        /// <summary>
        /// Checks whether the given <see cref="GameEntity"/> is included in the list of child entities fo this <see cref="GameEntity"/>.
        /// </summary>
        /// <param name="childEntity">The child game entity.</param>
        /// <returns>True if the given entity is a child of this GameEntity.</returns>
        public bool HasChild(GameEntity childEntity) {

            return ChildEntities.Contains(childEntity);

        }

	}
	
}
