﻿using System;
using System.Collections.Generic;

namespace Kalavarda.Primitives.Skills
{
    public interface IChildItemsOwner
    {
        IChildItemsContainer ChildItemsContainer { get; }
    }

    public interface IChildItemsOwnerExt
    {
        IChildItemsContainerExt ChildItemsContainer { get; }
    }

    public interface IChildItemsContainer
    {
        //IReadOnlyCollection<IChildItem> ChildItems { get; }

        event Action<IChildItemsContainer, IChildItem> ChildItemAdded;

        event Action<IChildItemsContainer, IChildItem> ChildItemRemoved;
    }

    public interface IChildItemsContainerExt: IChildItemsContainer
    {
        void Add(IChildItem item);

        void Remove(IChildItem item);
    }

    public interface IChildItem
    {
        IChildItemsContainer Container { get; }
    }

    public class ChildItemsContainer: IChildItemsContainerExt
    {
        private readonly ICollection<IChildItem> _childItems = new List<IChildItem>();

        public event Action<IChildItemsContainer, IChildItem> ChildItemAdded;
        public event Action<IChildItemsContainer, IChildItem> ChildItemRemoved;

        public void Add(IChildItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            lock (_childItems)
                _childItems.Add(item);
            ChildItemAdded?.Invoke(this, item);
        }

        public void Remove(IChildItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            lock (_childItems)
                _childItems.Remove(item);
            ChildItemRemoved?.Invoke(this, item);
        }
    }

    public class ChildItemsAggregator: IChildItemsContainer
    {
        private ICollection<IChildItemsContainer> _containers = new List<IChildItemsContainer>();

        public void Add(IChildItemsContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            container.ChildItemAdded += Container_ChildItemAdded;
            container.ChildItemRemoved += Container_ChildItemRemoved;
            _containers.Add(container);
        }

        public void Remove(IChildItemsContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            container.ChildItemAdded -= Container_ChildItemAdded;
            container.ChildItemRemoved -= Container_ChildItemRemoved;
            _containers.Remove(container);
        }

        public void Add(IChildItemsOwner owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            Add(owner.ChildItemsContainer);
        }

        public void Remove(IChildItemsOwner owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            Remove(owner.ChildItemsContainer);
        }

        private void Container_ChildItemAdded(IChildItemsContainer container, IChildItem childItem)
        {
            ChildItemAdded?.Invoke(container, childItem);
        }

        private void Container_ChildItemRemoved(IChildItemsContainer container, IChildItem childItem)
        {
            ChildItemRemoved?.Invoke(container, childItem);
        }

        public event Action<IChildItemsContainer, IChildItem> ChildItemAdded;
        public event Action<IChildItemsContainer, IChildItem> ChildItemRemoved;
    }
}
