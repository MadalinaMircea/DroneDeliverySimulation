using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneDeliverySystem.Utils
{
    class PackageRequest
    {
        public int ProducerID { get; set; }
        public int PackageID { get; set; }
        public Position position { get; set; }

        public PackageRequest(int producerID, int packageID, Position position)
        {
            ProducerID = producerID;
            PackageID = packageID;
            this.position = position;
        }

        public override bool Equals(object obj)
        {
            PackageRequest p = (PackageRequest)obj;
            return p.PackageID == PackageID && p.ProducerID == ProducerID;
        }

        public override int GetHashCode()
        {
            int hashCode = -894796678;
            hashCode = hashCode * -1521134295 + ProducerID.GetHashCode();
            hashCode = hashCode * -1521134295 + PackageID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Position>.Default.GetHashCode(position);
            return hashCode;
        }
    }
}
