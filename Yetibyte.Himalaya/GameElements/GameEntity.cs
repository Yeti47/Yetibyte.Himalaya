using System;
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
		
        public bool IsActive {

            get { return _isActive; }
			set { _isActive = value;}

		}
		
		public String Name { get; protected set; }
	    public Scene Scene { get; protected set; }
		
		public Game Game {
			
			get { 
			
				if(Scene == null)
					return null;
				
				return Scene.Game;
				
			}
			
		}

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
		
		public void DestroyEntity() {
			
			foreach(GameEntity childEntity in ChildEntities)
				childEntity.DestroyEntity();

            if(!IsDestroyed) {

                IsDestroyed = true;
                Scene.RemoveGameEntity(this);

            }
            			
		}

        public void AddChildEntity(GameEntity childEntity) {

            if (!HasChild(childEntity)) {

                ChildEntities.Add(childEntity);
                Transform.AddChild(childEntity.Transform);
                childEntity.ParentEntity = this;

            }
            
        }
		
        public void RemoveChildEntity(GameEntity childEntity) {

            if (HasChild(childEntity)) {

                ChildEntities.Remove(childEntity);
                Transform.RemoveChild(childEntity.Transform);
                childEntity.ParentEntity = null;

            }
            
        }

        public bool HasChild(GameEntity childEntity) {

            return ChildEntities.Contains(childEntity);

        }

	}
	
}
