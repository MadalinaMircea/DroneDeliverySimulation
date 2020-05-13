using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
