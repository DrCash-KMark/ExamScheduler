using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamScheduler.Genetic
{
    class ZVFitness : IFitness
    {
        private Context ctx;

        public readonly List<Func<Schedule, double>> CostFunctions;


        public ZVFitness(Context context)
        {
            ctx = context;
            CostFunctions = new List<Func<Schedule, double>>()
            {
                GetStudentDuplicatedScore,

                GetPresidentNotAvailableScore,
                GetSecretaryNotAvailableScore,
                GetExaminerNotAvailableScore,
                GetMemberNotAvailableScore,
                GetConsultantNotAvailableScore,

                GetPresidentWorkloadScore,
                GetSecretaryWorkloadScore,
                GetMemberWorkloadScore,
                GetExaminerWorkloadScore,

                GetPresidentChangeScore,
                GetSecretaryChangeScore,

                GetConsultantNotPresidentScore,
                GetConsultantNotSecretaryScore,
                GetExaminerNotPresidentScore,

                GetLunchScore,
                GetLeakinessScore,

                GetExamsStartsTooSoonScore,
                GetExamsEndsTooLateScore
           };
        }

        public double Evaluate(IChromosome ch)
        {
            Schedule sch = new Schedule(ctx.examCount, ctx.availableHours);
            for (int i = 0; i < ctx.examCount; i++)
            {
                sch.exams[i] = (Exam)ch.GetGene(i).Value;
                sch.availableHourUses[sch.exams[i].timeSlot] = true;
            }
            double score = 0;
            var tasks = CostFunctions.Select(cf => Task.Run(() => cf(sch))).ToArray();
            Task.WaitAll(tasks);
            foreach (var task in tasks)
            {
                score -= (int)task.Result;
            }

            return score;
        }

        public double GetStudentDuplicatedScore(Schedule sch)
        {
            double score = 0;
            int[] count = new int[ctx.examCount];
            foreach (var e in sch.exams)
            {
                count[e.student.id]++;
            }
            for (int i = 0; i < ctx.examCount; i++)
            {
                if (count[i] > 1)
                {
                    score += (count[i] - 1) * Scores.StudentDuplicated;

                }

            }
            return score;
        }

        public double GetPresidentNotAvailableScore(Schedule sch)
        {
            double score = 0;
            foreach (var e in sch.exams)
            {
                if (e.president.avability[e.timeSlot] == false)
                    score += Scores.PresidentNotAvailable;
            }
            return score;
        }

        public double GetSecretaryNotAvailableScore(Schedule sch)
        {
            double score = 0;
            foreach (var e in sch.exams)
            {
                if (e.secretary.avability[e.timeSlot] == false)
                    score += Scores.SecretaryNotAvailable;
            }
            return score;
        }

        public double GetExaminerNotAvailableScore(Schedule sch)
        {
            double score = 0;
            foreach (var e in sch.exams)
            {
                if (e.examiner.avability[e.timeSlot] == false)
                    score += Scores.ExaminerNotAvailable;
            }
            return score;
        }

        public double GetMemberNotAvailableScore(Schedule sch)
        {
            double score = 0;
            foreach (var e in sch.exams)
            {
                if (e.member.avability[e.timeSlot] == false)
                    score += Scores.MemberNotAvailable;
            }
            return score;
        }

        public double GetConsultantNotAvailableScore(Schedule sch)
        {
            double score = 0;
            foreach (var e in sch.exams)
            {
                if (e.consultant.avability[e.timeSlot] == false)
                    score += Scores.ConsultantNotAvailable;
            }
            return score;
        }

        public double GetPresidentWorkloadScore(Schedule sch)
        {
            double score = 0;
            int[] workloads = new int[ctx.presidents.Count];
            foreach (var e in sch.exams)
            {
                workloads[ctx.presidents.IndexOf(e.president)]++;
            }
            double optimum = ctx.examCount / ctx.presidents.Count;
            foreach (var p in ctx.presidents)
            {
                if (workloads[ctx.presidents.IndexOf(p)] < optimum * 0.5 || workloads[ctx.presidents.IndexOf(p)] > optimum * 1.5)
                    score += Scores.PresidentWorkloadWorst;

                if (workloads[ctx.presidents.IndexOf(p)] > optimum * 0.5 && workloads[ctx.presidents.IndexOf(p)] < optimum * 0.7)
                    score += Scores.PresidentWorkloadWorse;
                if (workloads[ctx.presidents.IndexOf(p)] > optimum * 1.3 && workloads[ctx.presidents.IndexOf(p)] < optimum * 1.5)
                    score += Scores.PresidentWorkloadWorse;

                if (workloads[ctx.presidents.IndexOf(p)] > optimum * 0.7 && workloads[ctx.presidents.IndexOf(p)] < optimum * 0.9)
                    score += Scores.PresidentWorkloadBad;
                if (workloads[ctx.presidents.IndexOf(p)] > optimum * 1.1 && workloads[ctx.presidents.IndexOf(p)] < optimum * 1.3)
                    score += Scores.PresidentWorkloadBad;
            }
            return score;
        }


        public double GetSecretaryWorkloadScore(Schedule sch)
        {
            double score = 0;
            int[] workloads = new int[ctx.secretaries.Count];
            foreach (var e in sch.exams)
            {
                workloads[ctx.secretaries.IndexOf(e.secretary)]++;
            }
            double optimum = ctx.examCount / ctx.secretaries.Count;
            foreach (var p in ctx.secretaries)
            {
                if (workloads[ctx.secretaries.IndexOf(p)] < optimum * 0.5 || workloads[ctx.secretaries.IndexOf(p)] > optimum * 1.5)
                    score += Scores.SecretaryWorkloadWorst;

                if (workloads[ctx.secretaries.IndexOf(p)] > optimum * 0.5 && workloads[ctx.secretaries.IndexOf(p)] < optimum * 0.7)
                    score += Scores.SecretaryWorkloadWorse;
                if (workloads[ctx.secretaries.IndexOf(p)] > optimum * 1.3 && workloads[ctx.secretaries.IndexOf(p)] < optimum * 1.5)
                    score += Scores.SecretaryWorkloadWorse;

                if (workloads[ctx.secretaries.IndexOf(p)] > optimum * 0.7 && workloads[ctx.secretaries.IndexOf(p)] < optimum * 0.9)
                    score += Scores.SecretaryWorkloadBad;
                if (workloads[ctx.secretaries.IndexOf(p)] > optimum * 1.1 && workloads[ctx.secretaries.IndexOf(p)] < optimum * 1.3)
                    score += Scores.SecretaryWorkloadBad;
            }
            return score;
        }

        public double GetMemberWorkloadScore(Schedule sch)
        {
            double score = 0;
            int[] workloads = new int[ctx.members.Count];
            foreach (var e in sch.exams)
            {
                workloads[ctx.members.IndexOf(e.member)]++;
            }
            double optimum = ctx.examCount / ctx.members.Count;
            foreach (var p in ctx.members)
            {
                if (workloads[ctx.members.IndexOf(p)] < optimum * 0.5 || workloads[ctx.members.IndexOf(p)] > optimum * 1.5)
                    score += Scores.MemberWorkloadWorst;

                if (workloads[ctx.members.IndexOf(p)] > optimum * 0.5 && workloads[ctx.members.IndexOf(p)] < optimum * 0.7)
                    score += Scores.MemberWorkloadWorse;
                if (workloads[ctx.members.IndexOf(p)] > optimum * 1.3 && workloads[ctx.members.IndexOf(p)] < optimum * 1.5)
                    score += Scores.MemberWorkloadWorse;

                if (workloads[ctx.members.IndexOf(p)] > optimum * 0.7 && workloads[ctx.members.IndexOf(p)] < optimum * 0.9)
                    score += Scores.MemberWorkloadBad;
                if (workloads[ctx.members.IndexOf(p)] > optimum * 1.1 && workloads[ctx.members.IndexOf(p)] < optimum * 1.3)
                    score += Scores.MemberWorkloadBad;
            }
            return score;
        }

        public double GetExaminerWorkloadScore(Schedule sch)
        {
            double score = 0;

            foreach (var c in ctx.courses)
            {

                int[] workloads = new int[c.teachers.Length];
                foreach (var e in sch.exams)
                {
                    if (e.student.course == c)
                        workloads[Array.IndexOf(c.teachers, e.examiner)]++;
                }
                double optimum = ctx.examCountPerCourse.GetValueOrDefault(c) / c.teachers.Length;
                foreach (var p in c.teachers)
                {
                    if (workloads[Array.IndexOf(c.teachers, p)] < optimum * 0.5 || workloads[Array.IndexOf(c.teachers, p)] > optimum * 1.5)
                        score += Scores.ExaminerWorkloadWorst;

                    if (workloads[Array.IndexOf(c.teachers, p)] > optimum * 0.5 && workloads[Array.IndexOf(c.teachers, p)] < optimum * 0.7)
                        score += Scores.ExaminerWorkloadWorse;
                    if (workloads[Array.IndexOf(c.teachers, p)] > optimum * 1.3 && workloads[Array.IndexOf(c.teachers, p)] < optimum * 1.5)
                        score += Scores.ExaminerWorkloadWorse;

                    if (workloads[Array.IndexOf(c.teachers, p)] > optimum * 0.7 && workloads[Array.IndexOf(c.teachers, p)] < optimum * 0.9)
                        score += Scores.ExaminerWorkloadBad;
                    if (workloads[Array.IndexOf(c.teachers, p)] > optimum * 1.1 && workloads[Array.IndexOf(c.teachers, p)] < optimum * 1.3)
                        score += Scores.ExaminerWorkloadBad;
                }
            }
            return score;
        }

        public double GetPresidentChangeScore(Schedule sch)
        {
            double score = 0;

            for (int i = 0; i < sch.availableHourUses.Length; i += 5)
            {
                if (sch.availableHourUses[i])
                {
                    if (sch.availableHourUses[i + 1])
                        if (sch.exams.Where(e => e.timeSlot == i).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 1).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                    if (sch.availableHourUses[i + 2])
                        if (sch.exams.Where(e => e.timeSlot == i).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 2).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                    if (sch.availableHourUses[i + 3])
                        if (sch.exams.Where(e => e.timeSlot == i).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 3).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                    if (sch.availableHourUses[i + 4])
                        if (sch.exams.Where(e => e.timeSlot == i).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 4).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                }
                if(sch.availableHourUses[i + 1])
                {
                    if (sch.availableHourUses[i + 2])
                        if (sch.exams.Where(e => e.timeSlot == i + 1).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 2).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                    if (sch.availableHourUses[i + 3])
                        if (sch.exams.Where(e => e.timeSlot == i + 1).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 3).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                    if (sch.availableHourUses[i + 4])
                        if (sch.exams.Where(e => e.timeSlot == i + 1).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 4).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                }
                if (sch.availableHourUses[i + 2])
                {
                    if (sch.availableHourUses[i + 3])
                        if (sch.exams.Where(e => e.timeSlot == i + 2).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 3).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                    if (sch.availableHourUses[i + 4])
                        if (sch.exams.Where(e => e.timeSlot == i + 2).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 4).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                }
                if (sch.availableHourUses[i + 3])
                {
                    if (sch.availableHourUses[i + 4])
                        if (sch.exams.Where(e => e.timeSlot == i + 3).SingleOrDefault().president != sch.exams.Where(e => e.timeSlot == i + 4).SingleOrDefault().president)
                            score += Scores.PresidentChange;
                }

            }

            return score;
        }

        public double GetSecretaryChangeScore(Schedule sch)
        {
            double score = 0;

            for (int i = 0; i < sch.availableHourUses.Length; i += 5)
            {
                if (sch.availableHourUses[i])
                {
                    if (sch.availableHourUses[i + 1])
                        if (sch.exams.Where(e => e.timeSlot == i).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 1).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                    if (sch.availableHourUses[i + 2])
                        if (sch.exams.Where(e => e.timeSlot == i).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 2).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                    if (sch.availableHourUses[i + 3])
                        if (sch.exams.Where(e => e.timeSlot == i).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 3).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                    if (sch.availableHourUses[i + 4])
                        if (sch.exams.Where(e => e.timeSlot == i).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 4).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                }
                if (sch.availableHourUses[i + 1])
                {
                    if (sch.availableHourUses[i + 2])
                        if (sch.exams.Where(e => e.timeSlot == i + 1).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 2).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                    if (sch.availableHourUses[i + 3])
                        if (sch.exams.Where(e => e.timeSlot == i + 1).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 3).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                    if (sch.availableHourUses[i + 4])
                        if (sch.exams.Where(e => e.timeSlot == i + 1).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 4).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                }
                if (sch.availableHourUses[i + 2])
                {
                    if (sch.availableHourUses[i + 3])
                        if (sch.exams.Where(e => e.timeSlot == i + 2).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 3).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                    if (sch.availableHourUses[i + 4])
                        if (sch.exams.Where(e => e.timeSlot == i + 2).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 4).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                }
                if (sch.availableHourUses[i + 3])
                {
                    if (sch.availableHourUses[i + 4])
                        if (sch.exams.Where(e => e.timeSlot == i + 3).SingleOrDefault().secretary != sch.exams.Where(e => e.timeSlot == i + 4).SingleOrDefault().secretary)
                            score += Scores.SecretaryChange;
                }

            }

            return score;
        }

        public double GetConsultantNotPresidentScore(Schedule sch)
        {
            double score = 0;
            foreach (var e in sch.exams)
            {
                if (e.consultant.president && e.consultant != e.president)
                    score += Scores.ConsultantNotPresident;
            }
            return score;
        }

        public double GetConsultantNotSecretaryScore(Schedule sch)
        {
            double score = 0;
            foreach (var e in sch.exams)
            {
                if (e.consultant.secretary && e.consultant != e.secretary)
                    score += Scores.ConsultantNotSecretary;
            }
            return score;
        }

        public double GetExaminerNotPresidentScore(Schedule sch)
        {
            double score = 0;
            foreach (var e in sch.exams)
            {
                if (e.examiner.president && e.examiner != e.president)
                    score += Scores.ExaminerNotPresident;
            }
            return score;
        }

        public double GetLunchScore(Schedule sch)
        {
            double score = 0;
            for (int i = 4; i < sch.availableHourUses.Length; i += 10)
            {
                if (sch.availableHourUses[i - 1] && sch.availableHourUses[i] && sch.availableHourUses[i + 1])
                    score += Scores.NoLunch;

                if (!sch.availableHourUses[i - 1] && sch.availableHourUses[i] && sch.availableHourUses[i + 1])
                {
                    score += Scores.LunchStartsSoon;
                }

                if (sch.availableHourUses[i - 1] && sch.availableHourUses[i] && !sch.availableHourUses[i + 1])
                {
                    score += Scores.LunchEndsLate;
                }

                if (!sch.availableHourUses[i - 1] && !sch.availableHourUses[i] && sch.availableHourUses[i + 1])
                {
                    score += Scores.LunchStartsSoon;
                    score += Scores.LunchNotOptimalLenght;
                }

                if (sch.availableHourUses[i - 1] && !sch.availableHourUses[i] && !sch.availableHourUses[i + 1])
                {
                    score += Scores.LunchEndsLate;
                    score += Scores.LunchNotOptimalLenght;
                }

                if (!sch.availableHourUses[i - 1] && sch.availableHourUses[i] && !sch.availableHourUses[i + 1])
                {
                    score += Scores.LunchStartsSoon;
                }

                if (!sch.availableHourUses[i - 1] && !sch.availableHourUses[i] && !sch.availableHourUses[i + 1])
                {
                    score += Scores.LunchStartsSoon;
                    score += Scores.LunchEndsLate;
                    score += Scores.LunchNotOptimalLenght;
                }
            }
            return score;
        }

        public double GetLeakinessScore(Schedule sch)
        {
            double score = 0;
            bool previous = false;
            int multiplicator = 1;
            for (int i = 0; i < sch.availableHourUses.Length; i++)
            {

                if (!sch.availableHourUses[i] && previous)
                {
                    multiplicator++;
                }
                else if (sch.availableHourUses[i] && previous)
                {
                    score += multiplicator * Scores.LeakySchedule;
                    multiplicator = 1;
                }


                if (!sch.availableHourUses[i])
                {
                    previous = true;
                }
                else
                {
                    previous = false;
                }

            }
            return score;
        }

        public double GetExamsStartsTooSoonScore(Schedule sch)
        {
            double score = 0;
            for (int i = 0; i < sch.availableHourUses.Length; i += 10)
            {
                if (sch.availableHourUses[i])
                    score += Scores.ExamsStartsTooSoon;
            }
            return score;
        }

        public double GetExamsEndsTooLateScore(Schedule sch)
        {
            double score = 0;
            for (int i = 9; i < sch.availableHourUses.Length; i += 10)
            {
                if (sch.availableHourUses[i])
                    score += Scores.ExamsEndsTooLate;
            }
            return score;
        }
    }
}
