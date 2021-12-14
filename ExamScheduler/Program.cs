using System;
using GeneticSharp;
using ExamScheduler.InputReading;

namespace ExamScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = @"Inputs\";
            InputReader inputReader = new InputReader(folder+ "Instructors1.csv",folder+ "Courses1.csv",folder+ "Students1.csv");
            inputReader.ReadInput();
            Console.ReadKey();
        }
    }
}
