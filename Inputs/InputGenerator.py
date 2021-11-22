import random

for i in range(3):
    fCourses = open("Courses"+(i+1)+".csv", "w")
    courseNumber = random.randint(12,16)

    fInstructors = open("Instructors"+(i+1)+".csv", "w")


    fStudents = open("Students"+(i+1)+".csv", "w")

    f1.write("Woops! I have deleted the content!")
    f1.close()

print("Generating is done")