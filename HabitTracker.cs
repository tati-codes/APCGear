using System;
using System.Collections.Generic;

public class HabitTracker
{
    public Dictionary<string, HabitTrackerMonth> months = new Dictionary<string, HabitTrackerMonth>();
    public string currentMonth { get { return DateTime.Now.ToString("MM-yyyy"); } }
    public void addMonth() {
        if (months.ContainsKey(currentMonth)) { } 
        else {
            months.Add(currentMonth, new HabitTrackerMonth());
        }
    }
}

