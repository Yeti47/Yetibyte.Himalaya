using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.GameElements {

    public abstract class EntityComponent {

        // Fields

        protected bool _isActive = true;
        protected GameEntity _gameEntity;

        // Properties

        public virtual bool AllowMultiple => false;

        public GameEntity GameEntity {

            get => _gameEntity;

            set {

                GameEntity futureGameEntity = value;

                if (IsAttached)
                    _gameEntity.RemoveComponent(this);

                futureGameEntity?.AddComponent(this);

                _gameEntity = futureGameEntity;

            }

        }

        public bool IsAttached => _gameEntity != null;


        /// <summary>
        /// Determines whether or not this component is currently active. This also takes into account the active state of
        /// the <see cref="Yetibyte.Himalaya.GameElements.GameEntity"/> this component is attached to. If the GameEntity is not active, this will return false
        /// regardless of the local active state of this component. A component that is not attached to any GameEntity is always considered inactive.
        /// </summary>
        /// <seealso cref="IsActiveSelf"/>
        public bool IsActive {

            get => _gameEntity != null && _gameEntity.IsActive && this.IsActiveSelf;
            set => this.IsActiveSelf = value;

        }

        /// <summary>
        /// The local active state of this component. This ignores the active state of the <see cref="Yetibyte.Himalaya.GameElements.GameEntity"/> this component is attached to. 
        /// Pleae note that, even if this is set to true, the component may still be considered inactive by the <see cref="Scene"/> because the GameEntity is not active or this
        /// component has not been attached to a GameEntity at all.
        /// </summary>
        /// /// <seealso cref="IsActive"/>
        public bool IsActiveSelf { get => _isActive; set => _isActive = value; }

        // Constructor

        // Methods

        /// <summary>
        /// Sets the <see cref="Yetibyte.Himalaya.GameElements.GameEntity"/> this component is attached to without
        /// the overhead of recalculating relations. Only use this if you know what you're doing!
        /// </summary>
        /// <param name="gameEntity">The new GameEntity.</param>
        internal void SetGameEntityDirectly(GameEntity gameEntity) => _gameEntity = gameEntity;


    }
}
