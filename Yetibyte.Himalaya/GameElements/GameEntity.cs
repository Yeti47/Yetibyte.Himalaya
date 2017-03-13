using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class GameEntity : Transformable, IUpdate, ITimeScale {
		
		// Fields
		
		private bool _isActive = true;
		private List<GameEntity> _childEntities = new List<GameEntity>();
		
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

        public GameEntity ParentEntity { get; set; }

		public List<GameEntity> ChildEntities {
			
			get { return _childEntities; }
			protected set { _childEntities = value; }
			
		}

        public float TimeScale { get; set; } = 1f;

        public bool IsDestroyed { get; protected set; }

        // Constructor

        protected GameEntity(Scene scene, string name, Vector2 position) {
			
			this.Scene = scene;
			this.Name = name;
			this.Position = position;
						
		}
        		
		// Methods
		
		public virtual void Initialize() {
						
		}
		
		public virtual void Update(GameTime gameTime, float globalTimeScale) {
			
			foreach(GameEntity childEntity in ChildEntities) {
				
				childEntity.Position = Position;
				childEntity.Origin = Origin;
				childEntity.Rotation = Rotation;
				childEntity.Scale = Scale;
								
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
