using GeneticSharp.Domain;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler.Genetic
{
    class ZVTermination : ITermination
    {
        ITermination fitnessStagnation = new FitnessStagnationTermination(Parameters.StagnationTermination);
        public bool ShouldTerminate { get; set; } = false;
        public bool HasReached(IGeneticAlgorithm geneticAlgorithm)
        {
            if (ShouldTerminate) return true;

            return fitnessStagnation.HasReached(geneticAlgorithm);
        }
    }
}
