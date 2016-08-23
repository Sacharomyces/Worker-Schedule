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
            WorkersComparer comparer = new WorkersComparer();
            Data data = new Data();
            int[] calendar = new int[733];
            Worker[] workers = new Worker[5];
            workers[0] = new Worker("Józef", 3);
            workers[1] = new Worker("Wiktor", 4);
            workers[2] = new Worker("Maria", 5);
            workers[3] = new Worker("Bartosz", 6);
            workers[4] = new Worker("Marcin", 7);
            calendarInitialize(calendar, workers);
            data.assignWork(calendar, workers);
            data.workersEfficiency(calendar,workers);
            data.WorkerSort(workers, comparer,calendar);
            data.assignWork(calendar, workers, comparer);
            data.workersEfficiency(calendar, workers);
            output(calendar);
            Console.ReadKey();
           

        }


        private static void calendarInitialize(int[] calendar, Worker[] workers)
        {
            int index = 0;
            while (index < 728)
            {
                for (int workingDay = 0; workingDay < 5; workingDay++)
                {
                    calendar[index] = 0;
                    index++;
                }

                for (int weekEnd = 0; weekEnd < 2; weekEnd++)
                {
                    calendar[index] = 1;
                    index++;
                }
            }
            Random random = new Random();
            int holidays = 0;
            while (holidays < 40)
            {

                calendar[random.Next(0, 729)] = 2;
                holidays++;
            }

        }
        private static void output(int[] calendar)
        {
            for (int day = 365;day<=729;day++)
                switch(calendar[day])
                {
                    case 1: Console.WriteLine("Weekend");
                        break;
                    case 2: Console.WriteLine("Święto");
                        break;
                    case 3: Console.WriteLine("Józef");
                        break;
                    case 4: Console.WriteLine("Wiktor");
                        break;
                    case 5: Console.WriteLine("Maria");
                        break;
                    case 6: Console.WriteLine("Bartosz");
                        break;
                    case 7: Console.WriteLine("Marcin");
                        break; 
                }
                   
        } 
    }
}






                     


