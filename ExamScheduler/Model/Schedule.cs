using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler
{
    class Schedule
    {
        public Exam[] exams;
        public bool[] availableHourUses;

        public Schedule(int examCount, int _availableHours)
        {
            exams = new Exam[examCount];
            availableHourUses = new bool[_availableHours];
            for (int i = 0; i < _availableHours; i++)
            {
                availableHourUses[i] = false;
            }
        }
    }
}
