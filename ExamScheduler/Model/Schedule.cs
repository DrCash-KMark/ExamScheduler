using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler
{
    class Schedule
    {
        public Exam[] exams;

        public Schedule(int examCount)
        {
            exams = new Exam[examCount];
        }
    }
}
