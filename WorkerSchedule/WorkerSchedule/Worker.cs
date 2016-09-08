using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerSchedule
{
    public class Worker
    {
        Random _randomWorker = new Random();


        public Worker(string name, int workerNumber)
        {

            Name = name;
            CurrentStateProp = _currentState;
            _vacationDaysLeft = 21; // a number of vacation days for a year//
            WorkerNumber = workerNumber;
            ShiftDaysLeft = 3; // shift has always 3 days//
        }
        private static int _vacationPossibilityParameter = 7;
        public string Name { get; }
        private CurrentState _currentState;
        public CurrentState CurrentStateProp { get { return _currentState; } set { _currentState = value; } }
        public int DaysWorked { get; set; }
        private int _vacationDaysLeft;
        public int VacationDaysLeft { get { return _vacationDaysLeft; } private set { _vacationDaysLeft = value; } } // total vacation days per year//
        private int _currentVacationsDays;
        public int CurrentVacationDays { get { return _currentVacationsDays; } set { _currentVacationsDays = value; } }
        public int WorkerNumber { get; } //value witch will be assigned to proper day during the fulfilling the calendar//
        public int ShiftDaysLeft { get; set; }


        public void ResetVacationsDays()
        {
            VacationDaysLeft = 21;
        }
        public bool VacationAvaliable(Worker[] workers)
        {
            int workersOnVacation = 0;
            foreach (Worker worker in workers)
            {
                if (worker.CurrentStateProp == CurrentState.Vacation)
                    workersOnVacation++;
            }
            if (workersOnVacation > 2)
                return false;
            else
                return true;
        }
        public bool DoThisJob(Worker[] workers)
        {
            if (GoOnVacation(workers))
                return false;
            else
                return true;
        }
        private bool GoOnVacation(Worker[] workers) //check if selected worker with free status will go on vacation and more important if he can//
        {


            int possibility = _randomWorker.Next(20);
            if (possibility > _vacationPossibilityParameter) // there is a changeable possibility that workers can go on vacation//
                return false;
            else if (VacationAvaliable(workers) == false)// if more than two worker is on the vacation at the moment, selected worker must have next shift//
                return false;
            else
                if (VacationDaysLeft > 0)
            {
                CurrentStateProp = CurrentState.Vacation;
                CurrentVacationDays = _randomWorker.Next(2, 5);  // random number of days of one vacation// 
                if (VacationDaysLeft - CurrentVacationDays < 0) 
                {
                    CurrentVacationDays = VacationDaysLeft;
                    VacationDaysLeft = 0;
                }

                else
                    VacationDaysLeft -= CurrentVacationDays;
            }
            else
            {
                return false;
            }


            return true;
        }



        public enum CurrentState
        {
            Free,
            Work,
            Vacation
        }
    }
}
