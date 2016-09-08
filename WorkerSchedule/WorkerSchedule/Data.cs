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
        private static int _workingDay = 0;
        private static int _weekend = 1;
        private static int _holiday = 2;

        private static int _maxCalendarValue = 728;
        private static int _firstDayOfTheCalendar = 3;
        public static int FirstDayOfSecondYear = 364;




        public void YearsInitialize(List<int> calendar) // invokes monthInitialize method depend on how many days certain month is possesing//
        {

            for (int month = 1; month < 25; month++)
            {



                if (month % 2 == 0 && month != 2 && month != 14)

                    MonthsInitialize(30, calendar, month, _workingDay, _weekend);

                else if (month == 2)

                    MonthsInitialize(28, calendar, month, _workingDay, _weekend);

                else if (month == 14)

                    MonthsInitialize(29, calendar, month, _workingDay, _weekend);
                else
                    MonthsInitialize(31, calendar, month, _workingDay, _weekend);
            }
        }

        private void MonthsInitialize(int monthDaysCount, List<int> calendar, int month, int workingDay, int weekend) //often months ends in the middle on the week or at the weekend next month must start from the next day//

        {
            int workingDaysCounter = 0;
            int weekendDaysCounter = 0;// these counters are used in weekInitializer method in "for" loops to decrese first loop for as many days as needed.//

            if (month == 1)
            {
                workingDaysCounter = _firstDayOfTheCalendar; // calendar in this case must starts with thursday//
            }


            if (WorkingDaysLeft > 0)
            {
                workingDaysCounter = WorkingDaysLeft;
                WeekInitialize(calendar, monthDaysCount, workingDaysCounter, weekendDaysCounter);
            }
            else if (WeekendDaysLeft > 0)
            {
                weekendDaysCounter = WeekendDaysLeft;
                RevWeekInitialize(calendar, monthDaysCount, workingDaysCounter, weekendDaysCounter);
            }
            else
                WeekInitialize(calendar, monthDaysCount, workingDaysCounter, weekendDaysCounter);
        }
        private void WeekInitialize(List<int> calendar, int monthDaysCount, int workingDaysCounter, int weekendDaysCounter)// method assigns proper value to the certain day od calendar.//
        {
            int day = 0;

            while (day < monthDaysCount)
            {

                for (; workingDaysCounter < 5; workingDaysCounter++)
                {
                    if (day < monthDaysCount)
                    {
                        calendar.Add(_workingDay);
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
                            calendar.Add(_weekend);
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
        private void RevWeekInitialize(List<int> calendar, int monthDaysCount, int workingDaysCounter, int weekendDaysCounter)// reversed version of weekInitializer is used in case new month starts at weekend.//
        {
            int day = 0;

            while (day < monthDaysCount)
            {

                for (; weekendDaysCounter < 2; weekendDaysCounter++)
                {
                    if (day < monthDaysCount)
                    {
                        calendar.Add(_weekend);
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
                            calendar.Add(_workingDay);
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

                calendar[random.Next(0, calendar.Count)] = _holiday;
                totalHolidays++;
            }

        }
        public int CheckAvailableWorker(Worker[] workers)  //checking witch worker has a free status to do the next shift//
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

        public void CheckWorkStatus(Worker[] workers)
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
        public void AssignWork(List<int> calendar, Worker[] workers)
        {
            int worker = 0;
            worker = CheckAvailableWorker(workers);

            for (int day = 0; day < 363;)
            {
                for (int shiftDays = 0; shiftDays < 3; shiftDays++)
                {
                    if (CheckCalendar(calendar, day))
                    {

                        calendar[day] = worker;
                        day++;
                        CurrentVacationDaysDecrement(workers);
                    }
                    else
                    {
                        day++;
                        shiftDays--;

                    }

                }
                worker = CheckAvailableWorker(workers);
            }
        }

        public void AssignWork(List<int> calendar, Worker[] workers, WorkersComparer comparer)
        {
            foreach (Worker worker in workers)  //resets count of vacation days for the next year//
                worker.ResetVacationsDays();

            int Worker = 0;
            Worker = CheckAvailableWorker(workers);    // assign an available worker to do the next shift, checkAvailable Worker method returns a specific worker number//

            for (int day = 363; day < _maxCalendarValue;)   //sorting the workers every 10 days to distribute the work evenly//
            {
                if (day % 10 == 0)
                {
                    WorkerSort(workers, comparer, calendar);
                }

                for (int shiftDays = 0; shiftDays < 3; shiftDays++)
                {
                    if (day < 729)
                    {
                        if (CheckCalendar(calendar, day)) //checking calendar if it is a working day (int = 0)// 
                        {

                            calendar[day] = Worker;// worker number of selected worker is assigned to the calendar array//
                            day++;
                            CurrentVacationDaysDecrement(workers);
                        }
                        else //shifts must last 3 days so in case of checking holidays and weekends shiftDays count must stay const//
                        {
                            day++;
                            shiftDays--;

                        }
                    }
                }
                Worker = CheckAvailableWorker(workers);
            }
        }
        private bool CheckCalendar(List<int> calendar, int day)
        {



            if (calendar[day] == 0)
                return true;
            else
                return false;
        }
        private void CurrentVacationDaysDecrement(Worker[] workers)
        {
            foreach (Worker worker in workers)
            {
                if (worker.CurrentVacationDays > 0)
                    worker.CurrentVacationDays--;
                if (worker.CurrentVacationDays == 0 && worker.CurrentStateProp == Worker.CurrentState.Vacation)
                    worker.CurrentStateProp = Worker.CurrentState.Free;
            }
        }

        public void WorkersEfficiency(List<int> calendar, Worker[] workers) // method counts the numer of day worked for each worker// 
        {
            foreach (Worker worker in workers)
                worker.DaysWorked = 0;
            for (int day = 0; day < calendar.Count; day++)
            {
                foreach (Worker worker in workers)
                    if (day == worker.WorkerNumber)
                        worker.DaysWorked++;

            }
        }

        public void WorkerSort(Worker[] workers, WorkersComparer comparer, List<int> calendar) 
        {
            WorkersEfficiency(calendar, workers);
            Array.Sort(workers, comparer);
        }

    }
}