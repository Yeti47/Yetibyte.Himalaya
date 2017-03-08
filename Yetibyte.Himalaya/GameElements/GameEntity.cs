using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class GameEntity : Transformable {
		
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

        public GameEntity ParentEntity { get; protected set; }

		public List<GameEntity> ChildEntities {
			
			get { return _childEntities; }
			set { _childEntities = value; }
			
		}
		
		// Constructor
				
		protected GameEntity(Scene scene, string name, Vector2 position) {
			
			this.Scene = scene;
			this.Name = name;
			this.Position = position;
						
		}

        protected GameEntity(Scene scene, string name, Vector2 position, GameEntity parentEntitiy) : this(scene, name, position) {

            this.ParentEntity = parentEntitiy;

        }
		
		// Methods
		
		public virtual void Initialize() {
						
		}
		
		public virtual void Update(GameTime gameTime) {
			
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
			
			Scene.RemoveGameEntity(this);
			
		}
		
	}
	
}
