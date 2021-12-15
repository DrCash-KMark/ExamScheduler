 using System;
using System.Collections.Generic;
using System.Text;

namespace ExamScheduler
{
    static class Scores
    {
        //Hard
        public const double StudentDuplicated = 10000;

        public const double PresidentNotAvailable = 1000;
        public const double SecretaryNotAvailable = 1000;
        public const double ExaminerNotAvailable = 1000;


        public const double PresidentChange = 1000;
        public const double SecretaryChange = 1000;

        public const double NoLunch = 1000;

        //Soft
        public const double MemberNotAvailable = 5;
        public const double ConsultantNotAvailable = 5;

        public const double PresidentWorkloadWorst = 30;
        public const double PresidentWorkloadWorse = 20;
        public const double PresidentWorkloadBad = 10;

        public const double SecretaryWorkloadWorst = 30;
        public const double SecretaryWorkloadWorse = 20;
        public const double SecretaryWorkloadBad = 10;

        public const double MemberWorkloadWorst = 30;
        public const double MemberWorkloadWorse = 20;
        public const double MemberWorkloadBad = 10;

        public const double ExaminerWorkloadWorst = 30;
        public const double ExaminerWorkloadWorse = 20;
        public const double ExaminerWorkloadBad = 10;

        public const double ConsultantNotPresident = 2;
        public const double ConsultantNotSecretary = 1;
        public const double ExaminerNotPresident = 1;

        public const double LunchStartsSoon = 40;
        public const double LunchEndsLate = 40;
        public const double LunchNotOptimalLenght = 0.5;

        public const double LeakySchedule = 5;

        public const double ExamsStartsTooSoon = 10;
        public const double ExamsEndsTooLate = 10;
    }
}
