using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerSchedule
{
   public class Data
    {
        public Data ()
        {
            
        }

        public int Worker0Counter { get; private set; }
        public int Worker1Counter { get; private set; }
       public int Worker2Counter { get; private set; }
       public int Worker3Counter { get; private set; }
       public int Worker4Counter { get; private set; }

        

        public  int CheckWorker(Worker[] workers)
        {
            Random random = new Random();
            int workerNumber = 0;

            next:
            CheckWorkStatus(workers);
            foreach (Worker worker in workers)
            {
                if (worker.CurrentStateProp == Worker.CurrentState.Free)
                {
                    if (worker.DoThisJob(workers))
                    {
                        workerNumber = worker.WorkerNumber;
                        worker.CurrentStateProp = Worker.CurrentState.Work;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
                else
                {
                    continue;
                }
                
            }
            if (workerNumber == 0)
                goto next;
            else
            return workerNumber;

        }

        public  void CheckWorkStatus(Worker[] workers)
        {
            int workStatusCounter = 0;
            foreach (Worker worker in workers)
            {
                if (worker.CurrentStateProp != Worker.CurrentState.Free)
                    workStatusCounter++;
            }
            if (workStatusCounter == 5)
            {
                foreach (Worker worker in workers)
                    if (worker.CurrentStateProp == Worker.CurrentState.Work)
                        worker.CurrentStateProp = Worker.CurrentState.Free;

            }
        }
        public void AssignWork(int[] calendar, Worker[] workers)
        {
            int worker = 0;
            worker = CheckWorker(workers);

            for (int day = 0; day < 363;)
            {
                for (int shiftDays = 0; shiftDays < 3; shiftDays++)
                {
                    if (CheckCalendar(calendar, day))
                    {

                        calendar[day] = worker;
                        day++;
                        VacationDaysDecrement(workers);
                    }
                    else
                    {
                        day++;
                        shiftDays--;
                        
                    }

                }
                worker = CheckWorker(workers);
            }
        }

        public void AssignWork(int[] calendar, Worker[] workers,WorkersComparer comparer)
        {
            int worker = 0;
            worker = CheckWorker(workers);

            for (int day = 363; day < 729;)
            {
                if (day % 10 == 0)
                {
                    WorkerSort(workers, comparer,calendar);
                }
                
                for (int shiftDays = 0; shiftDays < 3; shiftDays++)
                {
                    
                    if (CheckCalendar(calendar, day))
                    {

                        calendar[day] = worker;
                        day++;
                        VacationDaysDecrement(workers);
                    }
                    else
                    {
                        day++;
                        shiftDays--;

                    }

                }
                worker = CheckWorker(workers);
            }
        }
        private  bool CheckCalendar(int[] calendar, int day)
        {
            

                
            if (calendar[day] == 0)
                return true;
            else
                return false;
        }
        private void VacationDaysDecrement(Worker[] workers)
        {
            foreach (Worker worker in workers)
            {
                if (worker.CurrentVacationDays > 0)
                    worker.CurrentVacationDays--;
                if (worker.CurrentVacationDays == 0 && worker.CurrentStateProp == Worker.CurrentState.Vacation)
                    worker.CurrentStateProp = Worker.CurrentState.Free;
            }
        }

        public void WorkersEfficiency(int[] calendar,Worker[] workers)
        {
            foreach (Worker worker in workers)
                worker.DaysWorked = 0;
            for (int day = 0; day<calendar.Length;day++)
            {
                foreach (Worker worker in workers)
                    if (calendar[day] == worker.WorkerNumber)
                        worker.DaysWorked++;

            }
        }

        public void WorkerSort (Worker[] workers,WorkersComparer comparer,int[] calendar)
        {
            WorkersEfficiency(calendar,workers);
            Array.Sort(workers, comparer);
        }

    }
}
   