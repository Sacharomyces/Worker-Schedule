using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerSchedule
{
   public class WorkersComparer:IComparer<Worker>

    {
        public int Compare (Worker x, Worker y)
        {
            if (x.DaysWorked > y.DaysWorked)
                return 1;
            else if (x.DaysWorked < y.DaysWorked)
                return -1;
            else
                return 0;

                
        }
    }
}
