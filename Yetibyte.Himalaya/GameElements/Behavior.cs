using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.Collision;

namespace Yetibyte.Himalaya.GameElements {

    public abstract class Behavior : EntityComponent, IUpdate, ICollisionResponse {

        #region Properties

        /// <summary>
        /// Whether or not the Awake method was already called.
        /// </summary>
        public bool IsAwake { get; internal set; }
        
        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// This method is called once at the start of the very first update iteration. Use this for initialization.
        /// </summary>
        public abstract void Awake();

        /// <summary>
        /// This method is called every frame. Use this for updating your logic.
        /// </summary>
        /// <param name="gameTime">A snapshot of current timing values.</param>
        /// <param name="globalTimeScale">The time scale of the <see cref="Scene"/>.</param>
        public abstract void Update(GameTime gameTime, float globalTimeScale);

        protected abstract void OnTrigger(Collider ownCollider, Collider otherCollider);
        protected abstract void OnTriggerEnter(Collider ownCollider, Collider otherCollider);
        protected abstract void OnTriggerLeave(Collider ownCollider, Collider otherCollider);

        void ICollisionResponse.OnTrigger(Collider ownCollider, Collider otherCollider) => this.OnTrigger(ownCollider, otherCollider);
        void ICollisionResponse.OnTriggerEnter(Collider ownCollider, Collider otherCollider) => this.OnTriggerEnter(ownCollider, otherCollider);
        void ICollisionResponse.OnTriggerLeave(Collider ownCollider, Collider otherCollider) => this.OnTriggerLeave(ownCollider, otherCollider);

        #endregion

    }

}
