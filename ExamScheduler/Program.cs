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
            InputReader inputReader = new InputReader(folder+ "Instructors1.csv",folder+ "Courses1.csv",folder+ "Students1.csv");
            inputReader.ReadInput();
            Context ctx = inputReader.GetContext();
            ZVScheduler scheduler = new ZVScheduler(ctx);

            var task = scheduler.RunAsync().ContinueWith(scheduleTask =>
            {
                Schedule resultSchedule = scheduleTask.Result;

                /*         ZVFitness evaluator = new ZVFitness(context);
                         double penaltyScore = evaluator.EvaluateAll(resultSchedule);
                         Console.WriteLine("Penalty score: " + penaltyScore);*/

            });


            Console.ReadKey();
        }
    }
}
