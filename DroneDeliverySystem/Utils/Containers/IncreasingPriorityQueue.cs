namespace DroneDeliverySystem.Utils.Containers
{
    class IncreasingPriorityQueue<T> : PriorityQueue<T>
    {
        public override int Add(PriorityPair<T> t)
        {
            int i = base.Add(t);

            for (int j = i + 1; j < elements.Count; j++)
            {
                elements[j].Priority++;
            }

            return i;
        }
    }
}
