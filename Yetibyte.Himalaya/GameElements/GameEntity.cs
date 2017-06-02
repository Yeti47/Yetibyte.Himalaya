using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Yetibyte.Himalaya.Collision;

namespace Yetibyte.Himalaya.GameElements {
	
	public abstract class GameEntity : IUpdate, ITimeScale, IDraw {

        #region Fields

        private bool _isAwake;
        private bool _isActive = true;
        private List<GameEntity> _childEntities = new List<GameEntity>();
        private GameEntity _parentEntity;
        private List<EntityComponent> _components = new List<EntityComponent>();

        #endregion

        #region Events

        public event EventHandler<ComponentEventArgs> ComponentAdded;
        public event EventHandler<ComponentEventArgs> ComponentRemoved;

        #endregion

        #region Properties

        /// <summary>
        /// A <see cref="GameEntity"/> that is inactive will be ignored by the <see cref="Yetibyte.Himalaya.GameElements.Scene"/> and will
        /// therefore not be updated. This also takes into account the active state of the parent GameEntity. If the parent is not active, this will return false 
        /// regardless of this GameEntity's local active state.
        /// </summary>
        /// <seealso cref="IsActiveSelf"/>
        public bool IsActive {

            get => this.IsActiveSelf && (!this.HasParent || _parentEntity.IsActive);
            set => this.IsActiveSelf = value;

        }

        /// <summary>
        /// The local active state of this <see cref="GameEntity"/>. This ignores the active state of the parent. Pleae note that, even if this is set to true, the GameEntity 
        /// may still be considered inactive by the <see cref="Yetibyte.Himalaya.GameElements.Scene"/> because the parent is not active.
        /// </summary>
        /// <seealso cref="IsActive"/>
        public bool IsActiveSelf {

            get => _isActive;
            set => _isActive = value;

        }

        public String Name { get; protected set; } = "unnamed";

        /// <summary>
        /// The <see cref="Yetibyte.Himalaya.GameElements.Scene"/> this GameEntity lives in.
        /// </summary>
	    public Scene Scene { get; set; }

        public Game Game {

            get {

                if (Scene == null)
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

                if (this.HasParent) {

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

        public Transform Transform { get; private set; }

        public bool HasParent => _parentEntity != null;

        public int DrawOrder { get; set; }

        internal bool IsAwake => _isAwake;

        #endregion

        #region Constructors

        protected GameEntity(string name, Vector2 position) {

            this.Name = name;
            
            this.Transform = new Transform();
            this.AddComponent(Transform);
            this.Transform.Position = position;

        }

        #endregion

        #region Methods

        public virtual void Initialize() {

        }

        protected virtual void Awake() {
            
        }

        public virtual void Update(GameTime gameTime, float globalTimeScale) {

            if(!_isAwake) {

                _isAwake = true;
                Awake();

            }

            foreach (Behavior behavior in GetComponents<Behavior>().Where(b => b.IsActive && !b.IsAwake).OrderByDescending(b => b.Priority)) {

                if (IsDestroyed)
                    break;

                behavior.Awake();
                behavior.IsAwake = true;

            }

            foreach (IUpdate updateableComponent in _components.Where(c => c.IsActive).OrderByDescending(c => c.Priority).OfType<IUpdate>()) {

                if (IsDestroyed)
                    break;

                updateableComponent.Update(gameTime, globalTimeScale);

            }

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) {

            foreach (IDraw drawableComponent in _components.Where(c => c.IsActive).OfType<IDraw>().OrderBy(d => d.DrawOrder)) {

                drawableComponent.Draw(spriteBatch, gameTime);

            }

        }

        /// <summary>
        /// Destroys this <see cref="GameEntity"/>. It will be removed from the <see cref="Yetibyte.Himalaya.GameElements.Scene"/> it lived in.
        /// </summary>
        public void DestroyEntity() {

            foreach (GameEntity childEntity in ChildEntities)
                childEntity.DestroyEntity();

            if (!IsDestroyed) {

                IsDestroyed = true;
                Scene.RemoveGameEntity(this);

            }

        }

        /// <summary>
        /// Adds the given <see cref="GameEntity"/> to the list of child entities. Also sets the parent entity respectively. The child
        /// Game Entity will also optionally be added to the Scene the parent lives in (if it wasn't already).
        /// </summary>
        /// <param name="childEntity">The child entity to add.</param>
        /// <param name="doAddToScene">Should the child Game Entity be added to the Scene as well? True by default.</param>
        public void AddChildEntity(GameEntity childEntity, bool doAddToScene = true) {

            if (!IsParentOf(childEntity)) {

                ChildEntities.Add(childEntity);
                childEntity.ParentEntity = this;

                if (doAddToScene)
                    Scene.AddGameEntity(childEntity);

            }

        }

        /// <summary>
        /// Removes the given  <see cref="GameEntity"/> from the list of child entities. Also sets the parent of the given entity to null.
        /// The child Game Entity will also optionally be removed from the Scene the parent lives in (if it wasn't already).
        /// </summary>
        /// <param name="childEntity">The child entity to remove.</param>
        /// <param name="doRemoveFromScene">Should the child Game Entity be removed from the Scene as well? False by default.</param>
        public void RemoveChildEntity(GameEntity childEntity, bool doRemoveFromScene = false) {

            if (IsParentOf(childEntity)) {

                ChildEntities.Remove(childEntity);
                childEntity.ParentEntity = null;

                if (doRemoveFromScene)
                    Scene.RemoveGameEntity(childEntity);

            }

        }

        /// <summary>
        /// Checks whether the given <see cref="GameEntity"/> is included in the list of child entities of this <see cref="GameEntity"/>.
        /// </summary>
        /// <param name="childEntity">The child game entity.</param>
        /// <returns>True if the given entity is a child of this GameEntity.</returns>
        public bool IsParentOf(GameEntity childEntity) => ChildEntities.Contains(childEntity);

        public void AddComponent(EntityComponent component) {

            if (component == null || HasComponent(component))
                return;

            Type componentType = component.GetType();

            if (!component.AllowMultiple && HasComponentOfType(componentType))
                return;

            component.GameEntity?.RemoveComponent(component);
            _components.Add(component);
            component.SetGameEntityDirectly(this);

            component.OnAdded();
            OnRaiseComponentAdded(new ComponentEventArgs(component));

        }

        public void RemoveComponent(EntityComponent component) {

            if (component == null || !HasComponent(component) || !component.IsRemovable)
                return;

            _components.Remove(component);
            component.SetGameEntityDirectly(null);

            component.OnRemoved(this);
            OnRaiseComponentRemoved(new ComponentEventArgs(component));

        }

        /// <summary>
        /// Returns a collection of all <see cref="EntityComponent"/>s attached to this <see cref="GameEntity"/> that are of or derive from the given Type.
        /// </summary>
        /// <typeparam name="T">The type to filter the components by.</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetComponents<T>() where T : EntityComponent => _components.OfType<T>();

        public IEnumerable<T> GetActiveComponents<T>() where T : EntityComponent => _components.OfType<T>().Where(c => c.IsActive);

        /* TODO 
         * Maybe split this into two separate methods, where one method includes the root components and one doesn't, 
         * since "GetComponentsInChildren" does not intuitively imply that including the root components is an option.
        */
        /// <summary>
        /// Returns a collection of all <see cref="EntityComponent"/>s attached to this <see cref="GameEntity"/>'s children that match the given Type or derive from it.
        /// </summary>
        /// <typeparam name="T">The Type to filter the components by.</typeparam>
        /// <param name="includeSelf">Should the root <see cref="GameEntity"/>'s components be included in the collection?</param>
        /// <param name="recurse">Should the components attached to grandchildren, great-grandchildren etc. also be included in the collection?</param>
        /// <returns>A collection of all components that meet the specified conditions.</returns>
        public IEnumerable<T> GetComponentsInChildren<T>(bool includeSelf, bool recurse) where T : EntityComponent {

            IEnumerable<T> ownComponents = includeSelf ? GetComponents<T>() : new T[0];

            if (_childEntities == null) // Just a safety precaution
                return ownComponents.Concat(new T[0]);

            if (!recurse)
                return ownComponents.Concat(_childEntities.SelectMany(e => e.GetComponents<T>()));

            return ownComponents.Concat(_childEntities.SelectMany(e => e.GetComponentsInChildren<T>(true, true)));

        }

        /// <summary>
        /// Returns the first <see cref="EntityComponent"/> attached to this <see cref="GameEntity"/> that matches the given Type or derives from it.
        /// </summary>
        /// <typeparam name="T">The type of the component to look for.</typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : EntityComponent => GetComponents<T>().FirstOrDefault();

        public bool HasComponent(EntityComponent component) => _components.Contains(component);

        /// <summary>
        /// Checks whether this GameEntity has a <see cref="EntityComponent"/> of the given type. This will only look for the given type explicitly, not derived types.
        /// </summary>
        /// <param name="componentType">The type of component to look for.</param>
        /// <returns>True if there is a component attached to this <see cref="GameEntity"/> that matches the given type.</returns>
        public bool HasComponentOfType(Type componentType) {

            foreach (EntityComponent component in _components) {

                if (component.GetType() == componentType)
                    return true;

            }

            return false;

        }

        /// <summary>
        /// Checks whether this GameEntity has a <see cref="EntityComponent"/> of the given base type. 
        /// </summary>
        /// <param name="componentType">The type of component to look for.</param>
        /// <returns>True if there is a component attached to this <see cref="GameEntity"/> that matches the given type.</returns>
        public bool HasComponentOfDerivedType(Type componentType) {

            foreach (EntityComponent component in _components) {

                if (componentType.IsAssignableFrom(component.GetType()))
                    return true;

            }

            return false;

        }

        /// <summary>
        /// Invokes the ComponentAdded event handler.
        /// </summary>
        /// <param name="e">The EventArgs to send.</param>
        private void OnRaiseComponentAdded(ComponentEventArgs e) {

            EventHandler<ComponentEventArgs> componentAddedHandler = ComponentAdded;
            componentAddedHandler?.Invoke(this, e);

        }

        /// <summary>
        /// Invokes the ComponentRemoved event handler.
        /// </summary>
        /// <param name="e">The EventArgs to send.</param>
        private void OnRaiseComponentRemoved(ComponentEventArgs e) {

            EventHandler<ComponentEventArgs> componentRemovedHandler = ComponentRemoved;
            componentRemovedHandler?.Invoke(this, e);

        }

        /// <summary>
        /// Gets the <see cref="GameEntity"/> that is on top of the entity hierarchy.
        /// </summary>
        /// <returns>The GameEntity that is on top of the entity hierarchy.</returns>
        public GameEntity GetAncestor() => !HasParent ? this : ParentEntity.GetAncestor();

        internal void OnTrigger(Collider ownCollider, Collider otherCollider) {

            foreach (ICollisionResponse collisionResponsiveComponent in _components.Where(c => c.IsActive).OrderByDescending(c => c.Priority).OfType<ICollisionResponse>()) {

                if (IsDestroyed)
                    return;

                collisionResponsiveComponent.OnTrigger(ownCollider, otherCollider);

            }

        }

        internal void OnTriggerEnter(Collider ownCollider, Collider otherCollider) {

            foreach (ICollisionResponse collisionResponsiveComponent in _components.Where(c => c.IsActive).OrderByDescending(c => c.Priority).OfType<ICollisionResponse>()) {

                if (IsDestroyed)
                    return;

                collisionResponsiveComponent.OnTriggerEnter(ownCollider, otherCollider);

            }
            
        }

        internal void OnTriggerLeave(Collider ownCollider, Collider otherCollider) {

            foreach (ICollisionResponse collisionResponsiveComponent in _components.Where(c => c.IsActive).OrderByDescending(c => c.Priority).OfType<ICollisionResponse>()) {

                if (IsDestroyed)
                    return;

                collisionResponsiveComponent.OnTriggerLeave(ownCollider, otherCollider);

            }

        }

        #endregion

    }
	
}
