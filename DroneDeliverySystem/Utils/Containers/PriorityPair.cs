using System;

namespace DroneDeliverySystem.Utils.Containers
{
    class PriorityPair<T> : IComparable
    {
        public T Element { get; set; }
        public int Priority { get; set; }

        public PriorityPair(T element, int priority)
        {
            Element = element;
            Priority = priority;
        }

        public int CompareTo(object o)
        {
            PriorityPair<T> p = (PriorityPair<T>)o;
            return Priority.CompareTo(p.Priority);
        }

    }
}
