using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Utils.Containers
{
    class PriorityQueue<T>
    {
        List<PriorityPair<T>> elements;

        public PriorityQueue()
        {
            this.elements = new List<PriorityPair<T>>();
        }

        public PriorityQueue(int limit)
        {
        }

        public void Add(PriorityPair<T> t)
        {
            int i = 0;
            while (i < elements.Count && elements[i].Priority + 1 >= t.Priority)
                i++;

            for (int j = i; j < elements.Count; j++)
            {
                elements[i].Priority++;
            }

            elements.Insert(i, t);
        }

        public int Count()
        {
            return elements.Count;
        }

        public PriorityPair<T> Peek()
        {
            return elements[0];
        }


        public PriorityPair<T> PopFront()
        {
            PriorityPair<T> p = Peek();
            elements.RemoveAt(0);
            return p;
        }

        public void Remove(PriorityPair<T> t)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Element.Equals(t.Element))
                {
                    elements.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
