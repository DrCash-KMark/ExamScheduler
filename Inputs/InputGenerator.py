import random

for i in range(3):
    f1 = open("Courses"+(i+1)+".txt", "w")
    f1.write("Woops! I have deleted the content!")
    f1.close()

print("Generating is done")