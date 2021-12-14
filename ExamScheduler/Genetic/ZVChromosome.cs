using GeneticSharp.Domain.Chromosomes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler.Genetic
{
    class ZVChromosome : ChromosomeBase
    {
        private Context ctx;
        public Schedule schedule;

        public List<int> freeTimeSlots = new List<int>();

        public ZVChromosome(Context _ctx) : base(_ctx.examCount)
        {
            ctx = _ctx;
            schedule = new Schedule(ctx.examCount, ctx.availableHours);

            for(int i = 0; i < ctx.availableHours; i++)
            {
                freeTimeSlots.Add(i);
            }

            for (int i = 0; i < ctx.examCount; i++)
            {
                ReplaceGene(i, GenerateGene(i));
                schedule.exams[i] = (Exam)GetGene(i).Value;
            }
        }

        public override Gene GenerateGene(int geneIndex)
        {
            Exam e = new Exam();


            int timeSlot = freeTimeSlots[ctx.rnd.Next(0, freeTimeSlots.Count)];
            schedule.availableHourUses[timeSlot] = true;
            freeTimeSlots.Remove(timeSlot);

            e.timeSlot = timeSlot;
            

            e.student = ctx.rndStudents[geneIndex];
            e.consultant = e.student.consultant;
            e.president = ctx.presidents[ctx.rnd.Next(0, ctx.presidents.Count)];
            e.secretary = ctx.secretaries[ctx.rnd.Next(0, ctx.secretaries.Count)];
            e.member = ctx.members[ctx.rnd.Next(0, ctx.secretaries.Count)];
            e.examiner = e.student.course.teachers[ctx.rnd.Next(0, e.student.course.teachers.Length)];

            return new Gene(e);
        }

        public override IChromosome CreateNew()
        {
            return new ZVChromosome(ctx);
        }
    }
}
