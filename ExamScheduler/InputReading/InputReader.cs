using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler.InputReading
{
    class InputReader
    {
        private String filePathTeachers;
        private String filePathCourses;
        private String filePathStudents;
        private List<Teacher> teachers;
        private List<Student> students;
        private List<Course> courses;

        public InputReader(String teachersInputFilePath, String coursesInputFilePath, String studentsInputFilePath)
        {
            this.filePathTeachers = teachersInputFilePath;
            this.filePathCourses = coursesInputFilePath;
            this.filePathStudents = studentsInputFilePath;
        }

        public List<Teacher> GetTeachers()
        {
            return teachers;
        }
        public List<Course> GetCourses()
        {
            return courses;
        }

        public List<Student> GetStudents()
        {
            return students;
        }

        public Context GetContext()
        {
            return new Context(students, teachers, courses);
        }

        public void ReadInput()
        {
            ReadFileTeacher();
            ReadFileCourses();
            ReadFileStudents();
        }

        private void ReadFileTeacher()
        {
            string[] lines = System.IO.File.ReadAllLines(filePathTeachers);
            teachers = new List<Teacher>();

            foreach (String line in lines)
            {
                string[] data = line.Split(';');
                Teacher newTeacher = new Teacher
                {
                    name = data[0],
                    president = data[1] == "1",
                    secretary = data[2] == "1",
                    member = data[3] == "1"
                };
                int avalibilityDays = ((data.Length - 4) / 11);
                int numberDayIsDivedInto = 10;//this is a magic constant for now
                newTeacher.avability = new bool[avalibilityDays * numberDayIsDivedInto];
                for (int i = 0; i < avalibilityDays; i++)
                {
                    for (int j = 0; j < numberDayIsDivedInto; j++)
                    {
                        //+5 is name+roles+date 1+3+1
                        newTeacher.avability[i * numberDayIsDivedInto + j] = data[i * numberDayIsDivedInto + j + 5] == "1";
                    }
                }
                teachers.Add(newTeacher);
            }
        }

        private void ReadFileCourses()
        {
            string[] lines = System.IO.File.ReadAllLines(filePathCourses);
            courses = new List<Course>();

            foreach (string line in lines)
            {
                string[] date = line.Split(';');
                Course newCourse = new Course
                {
                    courseCode = date[0],
                    name = date[1]
                };
                Teacher[] coursTeachers = new Teacher[date.Length - 2];
                for (int i = 0; i < coursTeachers.Length; i++)
                {
                    coursTeachers[i] = teachers.Find(o => o.name == date[i + 2]);
                }
                newCourse.teachers = coursTeachers;

                courses.Add(newCourse);
            }
        }


        private void ReadFileStudents()
        {
            string[] lines = System.IO.File.ReadAllLines(filePathStudents);
            students = new List<Student>(); // previously stored data will be lost

            //procesing data
            foreach (String line in lines)
            {
                string[] data = line.Split(';');
                Student newStudent = new Student
                {
                    name = data[0],
                    neptun = data[1],
                    consultant = teachers.Find(o => o.name == data[2]),
                    course = courses.Find(o => o.courseCode == data[4])
                };
                students.Add(newStudent);
            }
        }

    }
}
