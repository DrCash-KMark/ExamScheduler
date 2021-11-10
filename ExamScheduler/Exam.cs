using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler
{
    class Exam
    {
        /*
         * This class is like an individual in genetic programing
         */
        private string exeminee { get; set; }
        private string consultant { get; set; }
        private string chairman { get; set; }//can't be secretary
        private string secretary { get; set; }//can't be chairman
        private string insider { get; set; }
        private string outsider { get; set; }
        private string examer01 { get; set; }
        private string examer02 { get; set; }
        private float Fitness { get; set; }

        public Exam ()
        {

        }
        public float CalculateFitness()
        {
            return 0;
        }


    }
}
