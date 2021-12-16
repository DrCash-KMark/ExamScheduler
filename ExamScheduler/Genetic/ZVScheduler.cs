using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExamScheduler.Genetic
{
    class ZVScheduler
    {
        private Context ctx;
        public readonly Dictionary<int, double> GenerationFitness = new Dictionary<int, double>();
        private GeneticAlgorithm geneticAlgorithm;
        private ZVTermination termination;


        public ZVFitness Fitness { get; private set; }


        public ZVScheduler(Context context)
        {
            this.ctx = context;

        }

        public Task<Schedule> RunAsync()
        {
            //var selection = new EliteSelection();
            //var selection = new TournamentSelection();
            var selection = new RouletteWheelSelection();

       //   var crossover = new ZVCycleCrossover();
            var crossover = new ZVCrossover(0.5f);
            var mutation = new ZVMutation(ctx);


            var chromosome = new ZVChromosome(ctx);
            Fitness = new ZVFitness(ctx);


            var population = new Population(Parameters.MinPopulationSize, Parameters.MaxPopulationSize, chromosome);

            termination = new ZVTermination();

            geneticAlgorithm = new GeneticAlgorithm(population, Fitness, selection, crossover, mutation);
            geneticAlgorithm.Termination = termination;
            geneticAlgorithm.GenerationRan += GenerationRan;
            geneticAlgorithm.MutationProbability = 0.05f;


            return Task.Run<Schedule>(
                () =>
                {
                    Console.WriteLine("GA running...");
                    geneticAlgorithm.Start();

                    Console.WriteLine("Best solution found has {0} fitness.", geneticAlgorithm.BestChromosome.Fitness);
                    var bestChromosome = geneticAlgorithm.BestChromosome as ZVChromosome;
                    var best = bestChromosome.Schedule;
                    return best;

                });

        }

        internal void Cancel()
        {
            termination.ShouldTerminate = true;
        }

        void GenerationRan(object sender, EventArgs e)
        {
            var bestChromosome = geneticAlgorithm.BestChromosome as ZVChromosome;
            var bestFitness = bestChromosome.Fitness.Value;

            GenerationFitness.Add(geneticAlgorithm.GenerationsNumber, bestFitness);
            Console.WriteLine("Generation {0}: {1:N0}", geneticAlgorithm.GenerationsNumber, bestFitness);
        }


    }
}
