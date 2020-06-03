using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Utils.Containers
{
    class IncreasingPriorityQueue<T> : PriorityQueue<T>
    {
        public override int Add(PriorityPair<T> t)
        {
            int i = base.Add(t);

            for (int j = i; j < elements.Count; j++)
            {
                elements[i].Priority++;
            }

            return i;
        }
    }
}
