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