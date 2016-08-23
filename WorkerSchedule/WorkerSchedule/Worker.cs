using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerSchedule
{
   public class Worker
    {

        Random randomWorker = new Random();

        public Worker(string name, int workerNumber)
        {
            
            Name = name;
            CurrentStateProp = currentState;
            vacationDaysLeft = 21;
            WorkerNumber = workerNumber;
            ShiftDaysLeft = 3;
        }

        public string Name { get; }
        private CurrentState currentState;
        public CurrentState CurrentStateProp { get { return currentState; } set { currentState = value; } }
        public int DaysWorked { get; set; }
        private int vacationDaysLeft;
        public int VacationDaysLeft { get { return vacationDaysLeft; } private set { vacationDaysLeft = value; } }
        private int currentVacationsDays;
        public int CurrentVacationDays { get { return currentVacationsDays; } set { currentVacationsDays = value; } }
        public int WorkerNumber { get; }
        public int ShiftDaysLeft { get; set; }



        public bool VacationAvaliable(Worker[]workers)
        {
            int counter = 0;
            foreach (Worker worker in workers)
            {
                if (worker.CurrentStateProp == CurrentState.vacation) 
                counter++;
            }
            if (counter > 1)
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
        private bool GoOnVacation(Worker[] workers)
        {
             

            int possibility = randomWorker.Next(20);
            if (possibility > 7)
                return false;
            else if (VacationAvaliable(workers) == false)
                return false;
            else
                if (VacationDaysLeft > 0)
            {
                CurrentStateProp = CurrentState.vacation;
                CurrentVacationDays = randomWorker.Next(2, 5);
                if (VacationDaysLeft - CurrentVacationDays < 1)
                    VacationDaysLeft = 0;
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
            free,
            work,
            vacation
        }
    }
}
