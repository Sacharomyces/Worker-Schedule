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

        

        public  int checkWorker(Worker[] workers)
        {
            Random random = new Random();
            int workerNumber = 0;

            next:
            checkWorkStatus(workers);
            foreach (Worker worker in workers)
            {
                if (worker.CurrentStateProp == Worker.CurrentState.free)
                {
                    if (worker.DoThisJob(workers))
                    {
                        workerNumber = worker.WorkerNumber;
                        worker.CurrentStateProp = Worker.CurrentState.work;
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

        public  void checkWorkStatus(Worker[] workers)
        {
            int WorkStatusCounter = 0;
            foreach (Worker worker in workers)
            {
                if (worker.CurrentStateProp != Worker.CurrentState.free)
                    WorkStatusCounter++;
            }
            if (WorkStatusCounter == 5)
            {
                foreach (Worker worker in workers)
                    if (worker.CurrentStateProp == Worker.CurrentState.work)
                        worker.CurrentStateProp = Worker.CurrentState.free;

            }
        }
        public void assignWork(int[] calendar, Worker[] workers)
        {
            int Worker = 0;
            Worker = checkWorker(workers);

            for (int day = 0; day < 363;)
            {
                for (int shiftDays = 0; shiftDays < 3; shiftDays++)
                {
                    if (checkCalendar(calendar, day))
                    {

                        calendar[day] = Worker;
                        day++;
                        vacationDaysDecrement(workers);
                    }
                    else
                    {
                        day++;
                        shiftDays--;
                        
                    }

                }
                Worker = checkWorker(workers);
            }
        }

        public void assignWork(int[] calendar, Worker[] workers,WorkersComparer comparer)
        {
            int Worker = 0;
            Worker = checkWorker(workers);

            for (int day = 363; day < 729;)
            {
                if (day % 10 == 0)
                {
                    WorkerSort(workers, comparer,calendar);
                }
                
                for (int shiftDays = 0; shiftDays < 3; shiftDays++)
                {
                    
                    if (checkCalendar(calendar, day))
                    {

                        calendar[day] = Worker;
                        day++;
                        vacationDaysDecrement(workers);
                    }
                    else
                    {
                        day++;
                        shiftDays--;

                    }

                }
                Worker = checkWorker(workers);
            }
        }
        private  bool checkCalendar(int[] calendar, int day)
        {
            

                
            if (calendar[day] == 0)
                return true;
            else
                return false;
        }
        private void vacationDaysDecrement(Worker[] workers)
        {
            foreach (Worker worker in workers)
            {
                if (worker.CurrentVacationDays > 0)
                    worker.CurrentVacationDays--;
                if (worker.CurrentVacationDays == 0 && worker.CurrentStateProp == Worker.CurrentState.vacation)
                    worker.CurrentStateProp = Worker.CurrentState.free;
            }
        }

        public void workersEfficiency(int[] calendar,Worker[] workers)
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
            workersEfficiency(calendar,workers);
            Array.Sort(workers, comparer);
        }

    }
}
   