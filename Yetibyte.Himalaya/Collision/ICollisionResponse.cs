using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.Collision {

    internal interface ICollisionResponse {

        void OnTriggerEnter(Collider ownCollider, Collider otherCollider);
        void OnTrigger(Collider ownCollider, Collider otherCollider);
        void OnTriggerLeave(Collider ownCollider, Collider otherCollider);

    }

}
