using System;

namespace PTO_Calculator;

public class Month(int year, int month)
{
    public int BusinessDays { get; set; } = CalculateNumberOfBusinessDaysInMonth(year, month);
    public DateTime MonthDateTime {get; set;} = new(year, month, 1);
    public int WorkingDays { get; set; }
    public int MonthlyWorkingHours { get; set; }

    private static int CalculateNumberOfBusinessDaysInMonth(int year, int month)
    {
        var monthDateTime = new DateTime(year, month, 1);
        var businessDays = 0;
        for (var i=1; i <= DateTime.DaysInMonth(year, month); i++)
        {
            var isWeekend = monthDateTime.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
            monthDateTime = monthDateTime.AddDays(1);
            if (isWeekend)
            {
                continue;
            }
            businessDays++;
        }
        
        return businessDays;
    }
}