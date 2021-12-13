from math import fabs
import random


#generating 3 inputs
for i in range(3):
    #generating the 3 input files    
    courseNumber = random.randint(12,16) 
    teacherNumber = random.randint(48,65)
    studentNumber = random.randint(50,200)
    #Generating courses file
    fCourses = open("Courses"+str(i+1)+".csv", "w")
    for j in range(courseNumber):
        teacherToCourse = random.randint(1,10)
        if(teacherToCourse>5):
            teacherToCourse=1
        courseLine="CourseID"+str(j)+";"+"CourseName"+str(j)
        InstructorNames= [-1]*teacherToCourse
        for k in range(teacherToCourse):
            notNewName= True
            while(notNewName):
                temp=random.randint(0,teacherNumber)
                isItNew=True
                for x in InstructorNames:
                    if x==temp:
                        isItNew=False
                if isItNew:
                    InstructorNames[k]=temp
                    notNewName=False
            courseLine+=";InstructorName"+str(InstructorNames[k])
        fCourses.write(courseLine+"\n")
    fCourses.close()
    #generating teachers file
    fInstructors = open("Instructors"+str(i+1)+".csv", "w")
    daysNum=random.randint(10,16)   
    for j in range(teacherNumber):
        Line="InstructorName"+str(j)
        role=random.randint(0,10)
        if(role==0):
            Line+=";1;0;0;"
        elif(role==1):
            Line+=";0;1;0"
        elif(role==2):
            Line+=";0;0;1"
        else:
            Line+=";0;0;0"
        
        for k in range(daysNum):
            Line+=";2020.01."+str(k+1)+"."
            for x in range(10):
                avalability= random.randint(0,5)
                if(avalability==0):
                    #not avalable
                    Line+=";0"
                else:
                    Line+=";1"
        fInstructors.write(Line+"\n")
    fInstructors.close()
    #Generating Students file
    fStudents = open("Students"+str(i+1)+".csv", "w")
    for j in range(studentNumber):
        randSupirvesor=random.randint(0,teacherNumber)
        randCourse=random.randint(0,courseNumber)
        Line="StudentName"+str(j)+";Neptun"+str(j)+";InstructorName"+str(randSupirvesor)+";CourseName"+str(randCourse)+";CourseID"+str(randCourse)+"\n"
        fStudents.write(Line)
    fStudents.close()
print("Generating is done")