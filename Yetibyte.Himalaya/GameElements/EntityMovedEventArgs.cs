using System;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.GameElements {

    public class EntityMovedEventArgs : EventArgs {

        public GameEntity GameEntity { get; }
        public Vector2 OriginalPosition { get; }
        public Vector2 Offset { get; }
        public Vector2 NewPosition => GameEntity != null ? GameEntity.Transform.Position : Vector2.Zero;

        public EntityMovedEventArgs(GameEntity gameEntity, Vector2 originalPosition, Vector2 offset) {

            this.GameEntity = gameEntity;
            this.OriginalPosition = originalPosition;
            this.Offset = offset;

        }

    }

}
