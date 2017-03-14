using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class GameEntity : IUpdate, ITimeScale {
		
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

                _parentEntity = value;
                this.Transform.Parent = value?.Transform;

            }

        }

		public List<GameEntity> ChildEntities {
			
			get { return _childEntities; }
			protected set { _childEntities = value; }
			
		}

        public float TimeScale { get; set; } = 1f;

        public bool IsDestroyed { get; protected set; }

        public Transform Transform { get; set; } = new Transform();

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
			
			foreach(GameEntity childEntity in ChildEntities) {
				
				childEntity.Transform.Position = Transform.Position;
				childEntity.Transform.Origin = Transform.Origin;
				childEntity.Transform.Rotation = Transform.Rotation;
				childEntity.Transform.Scale = Transform.Scale;
								
			}
					
		}
		
		public virtual void Draw(GameTime gameTime) {
									
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

            childEntity.ParentEntity = this;
            ChildEntities.Add(childEntity);

        }
		
        public void RemoveChildEntity(GameEntity childEntity) {

            childEntity.ParentEntity = null;
            ChildEntities.Remove(childEntity);

        }

	}
	
}
