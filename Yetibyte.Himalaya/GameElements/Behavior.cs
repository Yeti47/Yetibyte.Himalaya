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

        #region Methods

        public abstract void Awake();
        public abstract void Update(GameTime gameTime, float globalTimeScale);
        public abstract void OnTrigger(Collider ownCollider, Collider otherCollider);
        public abstract void OnTriggerEnter(Collider ownCollider, Collider otherCollider);
        public abstract void OnTriggerLeave(Collider ownCollider, Collider otherCollider);
        
        #endregion

    }

}
