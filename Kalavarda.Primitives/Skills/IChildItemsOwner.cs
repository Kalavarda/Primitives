using System;

namespace Kalavarda.Primitives.Skills
{
    public interface IChildItemsOwner
    {
        //IReadOnlyCollection<IChildItem> ChildItems { get; }

        event Action<IChildItemsOwner, IChildItem> ChildItemAdded;

        event Action<IChildItemsOwner, IChildItem> ChildItemRemoved;
    }

    public interface IChildItemsOwnerExt: IChildItemsOwner
    {
        void Add(IChildItem item);

        void Remove(IChildItem item);
    }

    public interface IChildItem
    {
        IChildItemsOwner Owner { get; }
    }
}
