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

        private bool cycleMode;

        private Random rnd = new Random();

        public ZVCycleCrossover(float mixProbability)
            : base(2, 2)
        {
            IsOrdered = true;
            MixProbability = mixProbability;
        }

        public float MixProbability { get; set; }

        protected IList<IChromosome> PerformCycleCross(IList<IChromosome> parents)
        {
            geneIndex = 0;
            cycleMode = true;

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
                if (!cycleMode) break;
            }
            if (cycleMode)
            {
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
            else
            {
                return PerformCross(parents);
            }
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
                if (parent2Exam == null) cycleMode = false;
                if (!cycleMode) return;

                cycle.Add(timeSlot);
                Exam newExam = parent1Exams.First(e => e.student.id == parent2Exam.student.id);

                if (timeSlot != newExam.timeSlot)
                {
                    CreateCycle(parent1Exams, parent2Exams, newExam.timeSlot, cycle);
                }
            }
        }

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
                    bool done = false;
                    do
                    {
                        deleteFromListBasedOnTimeSlot(firstParentExams, e.timeSlot);

                        deleteFromListBasedOnStudentId(secondParentExams, e.student.id);

                        firstChild.ReplaceGene(i, new Gene(e));

                        Exam tmp = getExamBasedOnTimeSlot(secondParentExams, e.timeSlot);

                        if (tmp != null)
                        {
                            e = getExamBasedOnStudentId(firstParentExams, tmp.student.id);

                            deleteFromListBasedOnTimeSlot(secondParentExams, tmp.timeSlot);
                            i++;
                        }

                        else
                        {
                            done = true;
                        }
                    } while (!done);
                }


                else
                {
                    Exam e = secondParentExams[rnd.Next(0, secondParentExams.Count)];
                    bool done = false;
                    do
                    {
                        deleteFromListBasedOnTimeSlot(secondParentExams, e.timeSlot);

                        deleteFromListBasedOnStudentId(firstParentExams, e.student.id);

                        firstChild.ReplaceGene(i, new Gene(e));

                        Exam tmp = getExamBasedOnTimeSlot(firstParentExams, e.timeSlot);

                        if (tmp != null)
                        {
                            e = getExamBasedOnStudentId(secondParentExams, tmp.student.id);

                            deleteFromListBasedOnTimeSlot(firstParentExams, tmp.timeSlot);
                            i++;
                        }

                        else
                        {
                            done = true;
                        }
                    } while (!done);
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
                    bool done = false;
                    do
                    {
                        deleteFromListBasedOnTimeSlot(firstParentExams, e.timeSlot);

                        deleteFromListBasedOnStudentId(secondParentExams, e.student.id);

                        secondChild.ReplaceGene(i, new Gene(e));

                        Exam tmp = getExamBasedOnTimeSlot(secondParentExams, e.timeSlot);

                        if (tmp != null)
                        {
                            e = getExamBasedOnStudentId(firstParentExams, tmp.student.id);

                            deleteFromListBasedOnTimeSlot(secondParentExams, tmp.timeSlot);
                            i++;
                        }

                        else
                        {
                            done = true;
                        }
                    } while (!done);
                }


                else
                {
                    Exam e = secondParentExams[rnd.Next(0, secondParentExams.Count)];
                    bool done = false;
                    do
                    {
                        deleteFromListBasedOnTimeSlot(secondParentExams, e.timeSlot);

                        deleteFromListBasedOnStudentId(firstParentExams, e.student.id);

                        secondChild.ReplaceGene(i, new Gene(e));

                        Exam tmp = getExamBasedOnTimeSlot(firstParentExams, e.timeSlot);

                        if (tmp != null)
                        {
                            e = getExamBasedOnStudentId(secondParentExams, tmp.student.id);

                            deleteFromListBasedOnTimeSlot(firstParentExams, tmp.timeSlot);
                            i++;
                        }

                        else
                        {
                            done = true;
                        }
                    } while (!done);
                }
            }

            return new List<IChromosome> { firstChild, secondChild };
        }






        private void fillExamList(List<Exam> list, IChromosome parent)
        {
            for (int i = 0; i < parent.Length; i++)
            {
                Exam e = (Exam)parent.GetGene(i).Value;
                list.Add(e);
            }
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

        private void deleteFromListBasedOnStudentId(List<Exam> list, int id)
        {
            foreach (var l in list)
            {
                if (l.student.id == id)
                {
                    list.Remove(l);
                    break;
                }
            }
        }

        private Exam getExamBasedOnTimeSlot(List<Exam> list, int timeSlot)
        {
            foreach (var l in list)
            {
                if (l.timeSlot == timeSlot)
                {
                    return l;
                }
            }
            return null;
        }

        private Exam getExamBasedOnStudentId(List<Exam> list, int id)
        {
            foreach (var l in list)
            {
                if (l.student.id == id)
                {
                    return l;
                }
            }
            return null;
        }
    }
}
