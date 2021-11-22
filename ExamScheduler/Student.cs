using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler
{
    class Student
    {
        public String Name { get; set; }
        public String Neptun { get; set; }
        public Teacher Supervisor { get; set; }

        public String CourseId { get; set; }
    }
}
