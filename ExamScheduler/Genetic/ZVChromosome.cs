using GeneticSharp.Domain.Chromosomes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler.Genetic
{
    class ZVChromosome : ChromosomeBase
    {
        private Context ctx;

        public List<int> freeTimeSlots = new List<int>();

    //    public List<int> owo;
        public List<int> uwu = new List<int>();

        public ZVChromosome(Context _ctx) : base(_ctx.examCount)
        {
            ctx = _ctx;

            for(int i = 0; i < ctx.availableHours; i++)
            {
                freeTimeSlots.Add(i);
            }

            for (int i = 0; i < ctx.examCount; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
            var g = GetGenes();
        }

        public Schedule Schedule
        {
            get
            {
                List<int> owo = new List<int>();
                Schedule schedule = new Schedule(ctx.examCount, ctx.availableHours);
                for (int i = 0; i < ctx.examCount; i++)
                {
                    Exam e = (Exam)GetGene(i).Value;
                    schedule.exams[i] = e;
                    owo.Add(e.timeSlot);
                    schedule.availableHourUses[e.timeSlot] = true;
                }
                return schedule;
            }
        }

        public int GetGeneIdx(Gene g)
        {
            for(int i = 0; i < ctx.examCount; i++)
            {
                if (GetGene(i) == g)
                    return i;
            }
            return -1;
        }

        public override Gene GenerateGene(int geneIndex)
        {
            Exam e = new Exam();

            int idx = ctx.rnd.Next(0, freeTimeSlots.Count);
            int timeSlot = freeTimeSlots[idx];
            freeTimeSlots.RemoveAt(idx);

            e.timeSlot = timeSlot;

            uwu.Add(timeSlot);

            e.student = ctx.rndStudents[geneIndex];
            e.consultant = e.student.consultant;
            e.president = ctx.presidents[ctx.rnd.Next(0, ctx.presidents.Count)];
            e.secretary = ctx.secretaries[ctx.rnd.Next(0, ctx.secretaries.Count)];
            e.member = ctx.members[ctx.rnd.Next(0, ctx.members.Count)];
            e.examiner = e.student.course.teachers[ctx.rnd.Next(0, e.student.course.teachers.Length)];

            return new Gene(e);
        }

        public override IChromosome CreateNew()
        {
            return new ZVChromosome(ctx);
        }
    }
}
