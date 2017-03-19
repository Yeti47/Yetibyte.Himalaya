using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Utilities {

    public abstract class ParentChildHierarchy<T> where T : ParentChildHierarchy<T> {

        // Fields

        protected T _parent;

        // Properties

        public T Parent
        {

            get { return _parent; }

            set
            {

                T futureParent = value;

                if (this.HasParent) {

                    _parent.RemoveChild((T)this);

                }

                futureParent?.AddChild((T)this);

                _parent = futureParent;

            }

        }

        public List<T> Children { get; protected set; } = new List<T>();
        public bool HasParent => _parent != null;

        // Methods

        public bool AddChild(T child) {

            if (IsParentOf(child))
                return false;

            Children.Add((T)child);
            child.Parent = (T)this;

            return true;

        }

        public bool RemoveChild(T child) {

            if (!IsParentOf(child))
                return false;

            Children.Remove((T)child);
            child.Parent = null;

            return true;

        }

        public bool IsParentOf(T child) => Children.Contains(child);

        public T GetAncestor() {

            ParentChildHierarchy<T> currentChild = this;

            while (currentChild.Parent != null) {

                currentChild = currentChild.Parent;

            }

            return (T)currentChild;

        }

    }

}
