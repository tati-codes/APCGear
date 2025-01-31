using System;
using System.Linq;

public struct HabitTrackerMonth 
{
    int maxHabit;
    string[] habitNames;
    int[] days;

    public HabitTrackerMonth(string[] _habitNames, int _days)
    {
        if (_habitNames.Length > 3)
        {
            throw new ArgumentOutOfRangeException("habitNames", "should only have 3 maximum elements.");
        }
        maxHabit = _habitNames.Length;
        this.habitNames = _habitNames;
        days = Enumerable.Range(0, _days).ToArray();
    }

    public void increase_day(int day) {
        if (days.Length <= day && days[day] < 3)
        {
            days[day]++;
        }
    }

    public void decrease_day(int day)
    {
        if (days.Length <= day && days[day] > 0) {
            days[day]--;
        }
    }
}
