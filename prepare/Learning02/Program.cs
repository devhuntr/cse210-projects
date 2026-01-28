using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Job job1 = new Job();
        job1._company = "Microsoft";
        job1._jobtitle = "Software Engineer";
        job1._startyear = 2015;
        job1._endyear = 2020;

        Job job2 = new Job();
        job2._company = "AWS";
        job2._jobtitle = "Solutions Architect";
        job2._startyear = 2020;
        job2._endyear = 2024;

        Resume myresume = new Resume();
        myresume._name = "Hunter Anderson";

        myresume._jobs.Add(job1);
        myresume._jobs.Add(job2);

        myresume.Display();

    }
}