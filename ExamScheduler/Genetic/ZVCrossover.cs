using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler.Genetic
{
    class ZVCrossover : CrossoverBase
    {
        public ZVCrossover(float mixProbability)
            : base(2, 2)
        {
            MixProbability = mixProbability;
        }

        public ZVCrossover() : this(0.5f)
        {
        }

        private Random rnd = new Random();

        public float MixProbability { get; set; }

        protected override IList<IChromosome> PerformCross(IList<IChromosome> parents)
        {
            var firstParent = parents[0];
            var secondParent = parents[1];
            var firstChild = firstParent.CreateNew();
            var secondChild = secondParent.CreateNew();

            List<Exam> firstParentExams = new List<Exam>();
            List<Exam> secondParentExams = new List<Exam>();

            fillExamList(firstParentExams, firstParent);
            fillExamList(secondParentExams, secondParent);


            for (int i = 0; i < firstChild.Length; i++)
            {
                if (RandomizationProvider.Current.GetDouble() < MixProbability)
                {
                    Exam e = firstParentExams[rnd.Next(0, firstParentExams.Count)];
                    deleteFromListBasedOnTimeSlot(firstParentExams, e.timeSlot);
                    deleteFromListBasedOnTimeSlot(secondParentExams, e.timeSlot);


                    firstChild.ReplaceGene(i, new Gene(e));
                }
                else
                {
                    Exam e = secondParentExams[rnd.Next(0, secondParentExams.Count)];
                    deleteFromListBasedOnTimeSlot(firstParentExams, e.timeSlot);
                    deleteFromListBasedOnTimeSlot(secondParentExams, e.timeSlot);


                    firstChild.ReplaceGene(i, new Gene(e));
                }
            }

            firstParentExams = new List<Exam>();
            secondParentExams = new List<Exam>();
            fillExamList(firstParentExams, firstParent);
            fillExamList(secondParentExams, secondParent);

            for (int i = 0; i < secondChild.Length; i++)
            {
                if (RandomizationProvider.Current.GetDouble() < MixProbability)
                {
                    Exam e = firstParentExams[rnd.Next(0, firstParentExams.Count)];
                    deleteFromListBasedOnTimeSlot(firstParentExams, e.timeSlot);
                    deleteFromListBasedOnTimeSlot(secondParentExams, e.timeSlot);


                    secondChild.ReplaceGene(i, new Gene(e));
                }
                else
                {
                    Exam e = secondParentExams[rnd.Next(0, secondParentExams.Count)];
                    deleteFromListBasedOnTimeSlot(firstParentExams, e.timeSlot);
                    deleteFromListBasedOnTimeSlot(secondParentExams, e.timeSlot);


                    secondChild.ReplaceGene(i, new Gene(e));
                }
            }

            return new List<IChromosome> { firstChild, secondChild };
        }

        private void deleteFromListBasedOnTimeSlot(List<Exam> list, int timeSlot)
        {
            foreach (var l in list)
            {
                if (l.timeSlot == timeSlot)
                {
                    list.Remove(l);
                    break;
                }
            }
        }

        private void fillExamList(List<Exam> list, IChromosome parent)
        {
            for (int i = 0; i < parent.Length; i++)
            {
                Exam e = (Exam) parent.GetGene(i).Value;
                list.Add(e);
            }
        }
    }
}