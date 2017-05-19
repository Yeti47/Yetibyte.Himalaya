using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Collision {

    /// <summary>
    /// An interface for components that respond to collision/trigger events.
    /// </summary>
    public interface ICollisionResponse {

        void OnTriggerEnter(Collider ownCollider, Collider otherCollider);
        void OnTrigger(Collider ownCollider, Collider otherCollider);
        void OnTriggerLeave(Collider ownCollider, Collider otherCollider);

    }

}
