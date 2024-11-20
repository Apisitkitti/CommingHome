using System;

namespace Gley.TrafficSystem.Internal
{
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
}