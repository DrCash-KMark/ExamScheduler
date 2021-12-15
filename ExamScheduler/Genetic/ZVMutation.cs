using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamScheduler.Genetic
{
    class ZVMutation : MutationBase
    {
        Context ctx;

        public ZVMutation(Context context)
        {
            ctx = context;
        }

        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            ZVChromosome ch = (ZVChromosome)chromosome;

            if (RandomizationProvider.Current.GetDouble() <= probability * 10)
            {
                var indexes = RandomizationProvider.Current.GetUniqueInts(2, 0, chromosome.Length);
                var firstIndex = indexes[0];
                var secondIndex = indexes[1];
                var firstGene = chromosome.GetGene(firstIndex);
                var secondGene = chromosome.GetGene(secondIndex);

                chromosome.ReplaceGene(firstIndex, secondGene);
                chromosome.ReplaceGene(secondIndex, firstGene);
            }

            if (RandomizationProvider.Current.GetDouble() <= probability / 2)
            {
                for (int i = 0; i < ctx.availableHours; i += 5)
                {
                    Gene[] genes = new Gene[5];
                    int[] geneIndexes = new int[5];
                    Exam[] exams = new Exam[5];
                    Teacher[] presidents = new Teacher[5];
                    Teacher[] secretaries = new Teacher[5];
                    for (int j = 0; j < 5; j++)
                    {
                        if (ch.Schedule.availableHourUses[i + j])
                        {
                            Gene g = ch.GetGenes().Where(g =>
                            {
                                Exam e = (Exam)g.Value;
                                return e.timeSlot == i + j;
                            }).SingleOrDefault();
                            genes[(i + j) % 5] = g;
                            geneIndexes[(i + j) % 5] = ch.GetGeneIdx(g);

                            Exam e = (Exam)g.Value;
                            exams[(i + j) % 5] = e;
                            presidents[(i + j) % 5] = e.president;
                            secretaries[(i + j) % 5] = e.secretary;
                        }
                    }

                    var mostPresident = presidents.GroupBy(k => k).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

                    var mostSecretary = secretaries.GroupBy(k => k).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

                    for (int l = 0; l < 5; l++)
                    {
                        if (exams[l] != null && mostPresident != null && exams[l].president != mostPresident && RandomizationProvider.Current.GetDouble() <= probability / 2)
                        {

                            ((Exam)genes[l].Value).president = mostPresident;
                            chromosome.ReplaceGene(geneIndexes[l], genes[l]);

                        }

                        if (exams[l] != null && mostSecretary != null && exams[l].secretary != mostSecretary && RandomizationProvider.Current.GetDouble() <= probability / 2)
                        {
                            ((Exam)genes[l].Value).secretary = mostSecretary;
                            chromosome.ReplaceGene(geneIndexes[l], genes[l]);
                        }
                    }

                }
            }

            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                for (int i = 0; i < ctx.examCount; i++)
                {
                    Gene gene = chromosome.GetGene(i);
                    Exam exam = (Exam)gene.Value;

                    if (exam.consultant.president && exam.consultant != exam.president && RandomizationProvider.Current.GetDouble() <= probability)
                    {
                        ((Exam)gene.Value).president = exam.consultant;
                        chromosome.ReplaceGene(i, gene);

                    }
                }
            }

            if (RandomizationProvider.Current.GetDouble() <= probability)
            {
                for (int i = 0; i < ctx.examCount; i++)
                {
                    Gene gene = chromosome.GetGene(i);
                    Exam exam = (Exam)gene.Value;

                    if (exam.consultant.secretary && exam.consultant != exam.secretary && RandomizationProvider.Current.GetDouble() <= probability)
                    {
                        ((Exam)gene.Value).secretary = exam.consultant;
                        chromosome.ReplaceGene(i, gene);
                    }
                }
            }

            if (RandomizationProvider.Current.GetDouble() <= probability / 4)
            {
                for (int i = 0; i < ctx.examCount; i++)
                {
                    Gene gene = chromosome.GetGene(i);
                    Exam exam = (Exam)gene.Value;

                    if (exam.president.avability[exam.timeSlot] == false)
                    {
                        ((Exam)gene.Value).president = ctx.presidents[ctx.rnd.Next(0, ctx.presidents.Count)];
                        chromosome.ReplaceGene(i, gene);
                    }

                    if (exam.secretary.avability[exam.timeSlot] == false)
                    {
                        ((Exam)gene.Value).secretary = ctx.secretaries[ctx.rnd.Next(0, ctx.secretaries.Count)];
                        chromosome.ReplaceGene(i, gene);
                    }
                }

            }

        }
    }
}
