using System;
using GeneticSharp;
using ExamScheduler.InputReading;
using ExamScheduler.Genetic;

namespace ExamScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = @"Inputs\";
            InputReader inputReader = new InputReader(folder+ "InstructorsCyclic.csv",folder+ "CoursesCyclic.csv",folder+ "StudentsCyclic.csv");
            inputReader.ReadInput();
            Context ctx = inputReader.GetContext();
            ZVScheduler scheduler = new ZVScheduler(ctx);

            var task = scheduler.RunAsync().ContinueWith(scheduleTask =>
            {
                Schedule resultSchedule = scheduleTask.Result;
            });


            Console.ReadKey();
        }
    }
}
