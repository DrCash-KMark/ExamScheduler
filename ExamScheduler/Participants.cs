using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler
{
    class Participants
    {
        public Student Examinee { get; set; }
        public Teacher President { get; set; }
        public Teacher Secretary { get; set; }
        public Teacher Member { get; set; }
        public Teacher Examiner { get; set; }
        public Teacher Consultant { get; set; }
    }
}
