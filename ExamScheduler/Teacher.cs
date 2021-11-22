using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler
{
    class Teacher
    {
        public String Name { get; set; }

        public bool President { get; set; }
        public bool Secretary { get; set; }
        public bool Member { get; set; }

        public List<String> courses;

        public Dictionary<String, bool[]> avability;

        public Teacher()
        {
            courses = new List<String>();
            avability = new Dictionary<String, bool[]>();
        }
    }
}
