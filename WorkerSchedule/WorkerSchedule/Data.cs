using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerSchedule
{
   public class Data
    {
       

       public int Worker0Counter { get; private set; } //Counts worked days//
        public int Worker1Counter { get; private set; }
        public int Worker2Counter { get; private set; }
        public int Worker3Counter { get; private set; }
        public int Worker4Counter { get; private set; }

        public int WorkingDaysLeft { get; private set; }
        public int WeekendDaysLeft { get; private set; }


        // integers used to fullfill the calendar with proper value//
        private static int workingDay = 0;
        private static int weekend = 1;
        private static int holiday = 2;

        private static int maxCalendarValue = 728;
        private static int firstDayOfTheCalendar = 3;
        public static int firstDayOfSecondYear = 364;




        public void YearsInitialize(List<int> calendar) // invokes monthInitialize method depend on how many days certain month is possesing//
        {

            for (int month = 1; month < 25; month++)
            {



                if (month % 2 == 0 && month != 2 && month != 14)

                    monthsInitialize(30, calendar, month, workingDay, weekend);

                else if (month == 2)

                    monthsInitialize(28, calendar, month, workingDay, weekend);

                else if (month == 14)

                    monthsInitialize(29, calendar, month, workingDay, weekend);
                else
                    monthsInitialize(31, calendar, month, workingDay, weekend);
            }
        }

        private void monthsInitialize(int monthDaysCount, List<int> calendar, int month, int workingDay, int weekend) //often months ends in the middle on the week or at the weekend next month must start from the next day//

        {
            int workingDaysCounter = 0;
            int weekendDaysCounter = 0;// these counters are used in weekInitializer method in "for" loops to decrese first loop for as many days as needed.//

            if (month == 1)
            {
                workingDaysCounter = firstDayOfTheCalendar; // calendar in this case must starts with thursday//
            }


            if (WorkingDaysLeft > 0)
            {
                workingDaysCounter = WorkingDaysLeft;
                weekInitialize(calendar, monthDaysCount, workingDaysCounter, weekendDaysCounter);
            }
            else if (WeekendDaysLeft > 0)
            {
                weekendDaysCounter = WeekendDaysLeft;
                revWeekInitialize(calendar, monthDaysCount, workingDaysCounter, weekendDaysCounter);
            }
            else
                weekInitialize(calendar, monthDaysCount, workingDaysCounter, weekendDaysCounter);
        }
        private void weekInitialize(List<int> calendar, int monthDaysCount, int workingDaysCounter, int weekendDaysCounter)// method assigns proper value to the certain day od calendar.//
        {
            int day = 0;

            while (day < monthDaysCount)
            {

                for (; workingDaysCounter < 5; workingDaysCounter++)
                {
                    if (day < monthDaysCount)
                    {
                        calendar.Add(workingDay);
                        day++;
                    }
                    else
                    {
                        WorkingDaysLeft = workingDaysCounter;
                        WeekendDaysLeft = 0;
                        return;
                    }
                }

                if (day != 0)
                {
                    for (; weekendDaysCounter < 2; weekendDaysCounter++)
                    {
                        if (day < monthDaysCount)// ends loop at the end of the month//
                        {
                            calendar.Add(weekend);
                            day++;
                        }
                        else
                        {
                            WeekendDaysLeft = weekendDaysCounter;// remaining days are passing to WeekDaysLeft property to decrese next month first loop//.
                            WorkingDaysLeft = 0;
                            return;
                        }
                    }
                }
                else if (workingDaysCounter == 5 && weekendDaysCounter == 0)
                {
                    WorkingDaysLeft = workingDaysCounter;
                }
                workingDaysCounter = 0;
                weekendDaysCounter = 0;
            }
        }
        private void revWeekInitialize(List<int> calendar, int monthDaysCount, int workingDaysCounter, int weekendDaysCounter)// reversed version of weekInitializer is used in case new month starts at weekend.//
        {
            int day = 0;

            while (day < monthDaysCount)
            {

                for (; weekendDaysCounter < 2; weekendDaysCounter++)
                {
                    if (day < monthDaysCount)
                    {
                        calendar.Add(weekend);
                        day++;
                    }
                    else
                    {
                        WeekendDaysLeft = weekendDaysCounter;
                        WorkingDaysLeft = 0;
                        return;
                    }
                }

                if (day != 0)
                {
                    for (; workingDaysCounter < 5; workingDaysCounter++)
                    {
                        if (day < monthDaysCount)
                        {
                            calendar.Add(workingDay);
                            day++;
                        }
                        else
                        {
                            WorkingDaysLeft = workingDaysCounter;
                            WeekendDaysLeft = 0;
                            return;
                        }
                    }
                }
                else if (workingDaysCounter == 5 && weekendDaysCounter == 0)
                {
                    WorkingDaysLeft = workingDaysCounter;
                }
                workingDaysCounter = 0;
                weekendDaysCounter = 0;
            }
        }

        public void HolidaysInitialize(List<int> calendar) //holidays are spread randomly// 
        {
            Random random = new Random();
            int totalHolidays = 0;
            while (totalHolidays < 40)
            {

                calendar[random.Next(0, calendar.Count)] = holiday;
                totalHolidays++;
            }

        }
        public int checkAvailableWorker(Worker[] workers)  //checking witch worker has a free status to do the next shift//
        {
            Random random = new Random();
            int workerNumber = 0;

            next:
            checkWorkStatus(workers);
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

        public void checkWorkStatus(Worker[] workers)
        {
            int WorkStatusCounter = 0;
            foreach (Worker worker in workers)
            {
                if (worker.CurrentStateProp != Worker.CurrentState.Free)
                    WorkStatusCounter++;
            }
            if (WorkStatusCounter == 5)
            {
                foreach (Worker worker in workers)
                    if (worker.CurrentStateProp == Worker.CurrentState.Work)
                        worker.CurrentStateProp = Worker.CurrentState.Free;

            }
        }
        public void assignWork(List<int> calendar, Worker[] workers)
        {
            int Worker = 0;
            Worker = checkAvailableWorker(workers);

            for (int day = 0; day < 363;)
            {
                for (int shiftDays = 0; shiftDays < 3; shiftDays++)
                {
                    if (checkCalendar(calendar, day))
                    {

                        calendar[day] = Worker;
                        day++;
                        currentVacationDaysDecrement(workers);
                    }
                    else
                    {
                        day++;
                        shiftDays--;

                    }

                }
                Worker = checkAvailableWorker(workers);
            }
        }

        public void assignWork(List<int> calendar, Worker[] workers, WorkersComparer comparer)
        {
            foreach (Worker worker in workers)  //resets count of vacation days for the next year//
                worker.ResetVacationsDays();

            int Worker = 0;
            Worker = checkAvailableWorker(workers);    // assign an available worker to do the next shift, checkAvailable Worker method returns a specific worker number//

            for (int day = 363; day < maxCalendarValue;)   //sorting the workers every 10 days to distribute the work evenly//
            {
                if (day % 10 == 0)
                {
                    WorkerSort(workers, comparer, calendar);
                }

                for (int shiftDays = 0; shiftDays < 3; shiftDays++)
                {
                    if (day < 729)
                    {
                        if (checkCalendar(calendar, day)) //checking calendar if it is a working day (int = 0)// 
                        {

                            calendar[day] = Worker;// worker number of selected worker is assigned to the calendar array//
                            day++;
                            currentVacationDaysDecrement(workers);
                        }
                        else //shifts must last 3 days so in case of checking holidays and weekends shiftDays count must stay const//
                        {
                            day++;
                            shiftDays--;

                        }
                    }
                }
                Worker = checkAvailableWorker(workers);
            }
        }
        private bool checkCalendar(List<int> calendar, int day)
        {



            if (calendar[day] == 0)
                return true;
            else
                return false;
        }
        private void currentVacationDaysDecrement(Worker[] workers)
        {
            foreach (Worker worker in workers)
            {
                if (worker.CurrentVacationDays > 0)
                    worker.CurrentVacationDays--;
                if (worker.CurrentVacationDays == 0 && worker.CurrentStateProp == Worker.CurrentState.Vacation)
                    worker.CurrentStateProp = Worker.CurrentState.Free;
            }
        }

        public void workersEfficiency(List<int> calendar, Worker[] workers)
        {
            foreach (Worker worker in workers)
                worker.DaysWorked = 0;
            for (int day = 0; day < calendar.Count; day++)
            {
                foreach (Worker worker in workers)
                    if (calendar[day] == worker.WorkerNumber)
                        worker.DaysWorked++;

            }
        }

        public void WorkerSort(Worker[] workers, WorkersComparer comparer, List<int> calendar)
        {
            workersEfficiency(calendar, workers);
            Array.Sort(workers, comparer);
        }

    }
}