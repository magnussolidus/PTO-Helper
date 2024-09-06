using System;
using System.Diagnostics;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PTO_Calculator;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        SetCulture();
    }

    private void SetCulture()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
    }

    public void Plem(object sender, RoutedEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(Income.Text))
        {
            Console.WriteLine("No Income data");
            return;
        }

        if (string.IsNullOrWhiteSpace(DailyHours.Text))
        {
            Console.WriteLine("No Daily Hours data");
            return;
        }
        
        var monthlyIncome = int.Parse(SanitizeMaskedInput(Income.Text));
        var dailyHours = int.Parse(SanitizeMaskedInput(DailyHours.Text));
        
        
        Debug.WriteLine($"Plem! Income: {Income.Text}\nDaily Hours: {DailyHours.Text}");
    }

    public void UpdateMonthWorkingHoursLabel(object sender, DatePickerSelectedValueChangedEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(Income.Text))
        {
            Console.WriteLine("No Income data");
            return;
        }

        if (string.IsNullOrWhiteSpace(DailyHours.Text))
        {
            Console.WriteLine("No Daily Hours data");
            return;
        }

        if (string.IsNullOrWhiteSpace(PtoDays.Text))
        {
            Console.WriteLine("No PTO Day data");
            return;
        }

        if (string.IsNullOrWhiteSpace(PtoMonthlyHours.Text))
        {
            Console.WriteLine("No PTO Monthly Hours data");
            return;
        }
        
        var monthlyIncome = int.Parse(SanitizeMaskedInput(Income.Text));
        var dailyHours = int.Parse(SanitizeMaskedInput(DailyHours.Text));
        var ptoDays = int.Parse(SanitizeMaskedInput(PtoDays.Text));
        var ptoMonthlyHours = int.Parse(SanitizeMaskedInput(PtoMonthlyHours.Text));
        
        var month = DateTimeOffsetParser(args.NewDate);
        // var dateTimeMonth = new DateTime(month.Year, month.Month, 1);
        
        var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
        var businessDays = CalculateNumberOfBusinessDaysInMonth(month.Year, month.Month);
        var hourlyIncome = (double) monthlyIncome / (dailyHours * businessDays);
        var ptoHourValue = (double) monthlyIncome / ptoMonthlyHours;
        
        HourlyRateTextBox.Text = hourlyIncome.ToString("C");
        MonthlyWorkingHoursLabel.Content = $"Total Working Hours in {month.ToString("MMMM")}";
        MonthlyWorkingHoursResult.Text = (businessDays * dailyHours).ToString("D");
        MonthlyWorkingDaysResult.Text = (businessDays - ptoDays).ToString("D");
        PtoHourlyRateTextBox.Text = ptoHourValue.ToString("C");
        // UpdateSelectedMonthLabel(month.ToString("MMMM"));
    }

    private void UpdateSelectedMonthLabel(string month)
    {
        MonthlyWorkingHoursLabel.Content = $"{month} Working Hours:";
    }

    public int CalculateMonthWorkingHours(int month)
    {
        
        return 0;
    }

    private DateTimeOffset DateTimeOffsetParser(DateTimeOffset? dateTime)
    {
        if (dateTime == null)
        {
            // TODO - Error Handling
            return DateTimeOffset.Now;
        }
        
        return dateTime.Value;
    }

    private string SanitizeMaskedInput(string input)
    {
        return input.Trim().Replace("_", "");
    }

    private int CalculateNumberOfBusinessDaysInMonth(int year, int month)
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