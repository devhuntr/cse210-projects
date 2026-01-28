using System;
public class Job
{
    public string _company;
    public string _jobtitle;
    public int _startyear;
    public int _endyear;

    public void Display()
    {
        Console.WriteLine($"{_jobtitle}, {_company} ({_startyear} - {_endyear})");
    }
};