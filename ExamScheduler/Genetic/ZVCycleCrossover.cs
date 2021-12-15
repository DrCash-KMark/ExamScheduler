using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamScheduler.Genetic
{
    class ZVCycleCrossover : CrossoverBase
    {

        private static int geneIndex;

        public ZVCycleCrossover()
            : base(2, 2)
        {
            IsOrdered = true;
        }

        protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
        {
            geneIndex = 0;

            var parent1 = parents[0];
            var parent2 = parents[1];

            if (parents.AnyHasRepeatedGene())
            {
                throw new CrossoverException(this, "The Cycle Crossover (CX) can be only used with ordered chromosomes. The specified chromosome has repeated genes.");
            }

            var cycles = new List<List<int>>();
            var offspring1 = parent1.CreateNew();
            var offspring2 = parent2.CreateNew();

            var parent1Exams = new List<Exam>();
            var parent2Exams = new List<Exam>();
            fillExamList(parent1Exams, parent1);
            fillExamList(parent2Exams, parent2);

            // Search for the cycles.
            for (int i = 0; i < parent1Exams.Count; i++)
            {
                if (!cycles.SelectMany(p => p).Contains(parent1Exams[i].timeSlot))
                {
                    var cycle = new List<int>();
                    CreateCycle(parent1Exams, parent2Exams, parent1Exams[i].timeSlot, cycle);
                    cycles.Add(cycle);
                }
            }

            // Copy the cycles to the offpring.
            for (int i = 0; i < cycles.Count; i++)
            {
                var cycle = cycles[i];

                if (i % 2 == 0)
                {
                    // Copy cycle index pair: values from Parent 1 and copied to Child 1, and values from Parent 2 will be copied to Child 2.
                    CopyCycleIndexPair(cycle, parent1Exams, offspring1, parent2Exams, offspring2);
                }
                else
                {
                    // Copy cycle index odd: values from Parent 1 will be copied to Child 2, and values from Parent 1 will be copied to Child 1.
                    CopyCycleIndexPair(cycle, parent1Exams, offspring2, parent2Exams, offspring1);
                }
            }

            return new List<IChromosome>() { offspring1, offspring2 };

        }

        private static void CopyCycleIndexPair(IList<int> cycle, List<Exam> fromParent1Exams, IChromosome toOffspring1, List<Exam> fromParent2Exams, IChromosome toOffspring2)
        {
            int geneTimeSlot = 0;

            for (int j = 0; j < cycle.Count; j++)
            {
                geneTimeSlot = cycle[j];

                Exam e1 = fromParent1Exams.Where(e => e.timeSlot == geneTimeSlot).SingleOrDefault();

                toOffspring1.ReplaceGene(geneIndex, new Gene(e1));

                Exam e2 = fromParent2Exams.Where(e => e.timeSlot == geneTimeSlot).SingleOrDefault();

                toOffspring2.ReplaceGene(geneIndex, new Gene(e2));

                geneIndex++;
            }
        }

        private void CreateCycle(List<Exam> parent1Exams, List<Exam> parent2Exams, int timeSlot, List<int> cycle)
        {
            if (!cycle.Contains(timeSlot))
            {
                Exam parent2Exam = parent2Exams.Where(e => e.timeSlot == timeSlot).SingleOrDefault();

                cycle.Add(timeSlot);
                Exam newExam = parent1Exams.First(e => e.student.id == parent2Exam.student.id);

                if (timeSlot != newExam.timeSlot)
                {
                    CreateCycle(parent1Exams, parent2Exams, newExam.timeSlot, cycle);
                }
            }
        }

        private void fillExamList(List<Exam> list, IChromosome parent)
        {
            for (int i = 0; i < parent.Length; i++)
            {
                Exam e = (Exam)parent.GetGene(i).Value;
                list.Add(e);
            }
        }
    }
}
