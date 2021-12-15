using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamScheduler
{
    class Context
    {
        public List<Student> students;
        public List<Teacher> teachers;
        public List<Course> courses;

        public int examCount;
        public Dictionary<Course, int> examCountPerCourse = new Dictionary<Course, int>();
        public int availableHours;

        public List<Teacher> presidents;
        public List<Teacher> secretaries;
        public List<Teacher> members;
    //    public Dictionary<Course, int> examinersPerCourse = new Dictionary<Course, int>();

        public Random rnd = new Random();

        public List<Student> rndStudents;

        public Context(List<Student> _students, List<Teacher> _teachers, List<Course> _courses)
        {
            students = _students;
            teachers = _teachers;
            courses = _courses;

            FillStudentIds();

            examCount = _students.Count;
            availableHours = teachers[0].availability.Length;

            presidents = new List<Teacher>();
            secretaries = new List<Teacher>();
            members = new List<Teacher>();
            
            foreach(Teacher t in teachers)
            {
                if (t.isPresident)
                    presidents.Add(t);
                if (t.isSecretary)
                    secretaries.Add(t);
                if (t.isMember)
                    members.Add(t);
            }

            rndStudents = new List<Student>();
            rndStudents = students.OrderBy(a => rnd.Next()).ToList();

            foreach(var c in courses)
            {
             /*   int examinerPerC = 0;
                foreach(var t in c.teachers)
                {
                    examinerPerC++;
                }
                examinersPerCourse.Add(c, examinerPerC);*/

                int examPerC = 0;
                foreach(var s in students)
                {
                    if (s.course.Equals(c))
                        examPerC++;
                }
                examCountPerCourse.Add(c, examPerC);
            }
        }
        
        public void FillStudentIds()
        {
            for (int i = 0; i < students.Count; i++)
                students[i].id = i;
        }

    }
}
