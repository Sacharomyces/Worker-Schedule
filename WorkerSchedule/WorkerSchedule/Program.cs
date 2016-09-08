using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerSchedule
{

    class Program
    {

        static void Main(string[] args)
        {
            DateTime dateTime = new DateTime(2016, 01, 01);
            WorkersComparer comparer = new WorkersComparer();
            Data data = new Data();
            List<int> calendar = new List<int>();
            Worker[] workers = new Worker[5];
            workers[0] = new Worker("Józef", 3);
            workers[1] = new Worker("Wiktor", 4);
            workers[2] = new Worker("Maria", 5);
            workers[3] = new Worker("Bartosz", 6);
            workers[4] = new Worker("Marcin", 7);
            data.YearsInitialize(calendar);
            data.HolidaysInitialize(calendar);
            data.AssignWork(calendar, workers);
            data.WorkersEfficiency(calendar, workers);
            data.WorkerSort(workers, comparer, calendar);// I use this method here to check the sorted workers by their efficiency after assigning them without sorting method.//
            data.AssignWork(calendar, workers, comparer);// overloaded method with sorting method to evenly distribute shifts//
            data.WorkersEfficiency(calendar, workers);
            Output(calendar, dateTime);


        }



        private static void Output(List<int> calendar, DateTime dateTime)
        {
            string workerList = "";
            string date = "";
            for (int day = Data.FirstDayOfSecondYear; day <= calendar.Count - 1; day++)
            {
                if (day == Data.FirstDayOfSecondYear)
                {
                    date = dateTime.ToShortDateString();
                }
                else
                {
                    dateTime = dateTime.AddDays(+1);
                    date = dateTime.ToShortDateString();
                }

                switch (calendar[day])
                {
                    case 1:
                        workerList += date + " Weekend" + Environment.NewLine;
                        break;
                    case 2:
                        workerList += date + " Święto" + Environment.NewLine;
                        break;

                    case 3:
                        workerList += date + " Józef" + Environment.NewLine;
                        break;

                    case 4:
                        workerList += date + " Wiktor" + Environment.NewLine;
                        break;
                    case 5:
                        workerList += date + " Maria" + Environment.NewLine;
                        break;
                    case 6:
                        workerList += date + " Bartosz" + Environment.NewLine;
                        break;
                    case 7:
                        workerList += date + " Marcin" + Environment.NewLine;
                        break;
                }
            }

            Console.WriteLine(workerList);
            Console.ReadKey();
        }
    }
}







