using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Collision {

    public sealed class RectCollider : Collider {

        #region Properties

        public float Width { get; set; }
        public float Height { get; set; }

        public Vector2 Size {

            get => new Vector2(this.Width, this.Height);

            set {

                this.Width = value.X;
                this.Height = value.Y;

            }

        }

        public override RectangleF Bounds => new RectangleF(Position.X - Width / 2, Position.Y - Height / 2, Width, Height);

        #endregion

    }

}
