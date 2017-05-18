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

        public bool IsAwake { get; internal set; }
        
        #endregion

        #region Constructors

        #endregion

        #region Methods

        public abstract void Awake();
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
