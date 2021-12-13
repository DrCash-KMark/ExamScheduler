using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler
{
    class Context
    {
        public List<Student> students;
        public List<Teacher> teachers;
        public List<Course> courses;

        public List<Teacher> presidents;
        public List<Teacher> secretaries;
        public List<Teacher> members;
        public List<Teacher> examiners;

        public Context(List<Student> _students, List<Teacher> _teachers, List<Course> _courses)
        {
            students = _students;
            teachers = _teachers;
            courses = _courses;

            presidents = new List<Teacher>();
            secretaries = new List<Teacher>();
            members = new List<Teacher>();
            
            foreach(Teacher t in teachers)
            {
                if (t.president)
                    presidents.Add(t);
                if (t.secretary)
                    secretaries.Add(t);
                if (t.member)
                    members.Add(t);
            }
        }

    }
}
