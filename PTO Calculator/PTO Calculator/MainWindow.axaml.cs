using System;
using System.Diagnostics;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PTO_Calculator;

public partial class MainWindow : Window
{
    private PtoHelper _helper;
    private bool _hasDate;
    public MainWindow()
    {
        InitializeComponent();
        SetCulture();
    }

    private static void SetCulture()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
    }

    private bool SaveInputData()
    {
        var monthlyIncome = _helper.InsertText(Income.Text, TextValidationEnum.Income);
        var dailyHours = _helper.InsertText(DailyHours.Text, TextValidationEnum.DailyHours);
        var ptoDays = _helper.InsertText(PtoDays.Text, TextValidationEnum.PtoDays);
        var monthlyPtoHours = _helper.InsertText(PtoMonthlyHours.Text, TextValidationEnum.PtoMonthlyHours);
        
        return monthlyPtoHours && dailyHours && ptoDays && monthlyIncome;
    }
    
    public void Calculate(object sender, RoutedEventArgs args)
    {
        if (!_hasDate)
        {
            // TODO - Error handling with dialog
            Console.WriteLine("Need Data to initialize _helper");
            return;
        }
        
        var hasInputData = SaveInputData();
        if (!hasInputData)
        {
            // TODO - Error handling with dialog
            Console.WriteLine("Missing required data!");
            return;
        }

        var isDataInitialized = _helper.InitializeFieldsFromSavedTexts();
        if (!isDataInitialized)
        {
            // TODO - Error handling with dialog
            Console.WriteLine("Required Data was not properly initialized!");
            return;
        }
        
        var businessDays = _helper.CurrentMonth.BusinessDays;
        var hourlyIncome = _helper.CalculateHourlyIncome();
        var ptoHourValue = _helper.CalculatePtoHourValue();
        var expectedIncome = _helper.CalculateExpectedMonthlyIncome();
        
        // TODO - Update UI with Results
        HourlyRateTextBox.Text = hourlyIncome.ToString("C");
        MonthlyWorkingHoursLabel.Content = $"Total Working Hours in {_helper.CurrentMonth.MonthDateTime:MMMM}";
        MonthlyWorkingHoursResult.Text = _helper.CurrentMonth.MonthlyWorkingHours.ToString("D");
        MonthlyWorkingDaysResult.Text = _helper.CurrentMonth.WorkingDays.ToString("D");
        PtoHourlyRateTextBox.Text = ptoHourValue.ToString("C");
        ExpectedIncomeLabel.Content = $"The expected income for {_helper.CurrentMonth.MonthDateTime:MMMM} is {expectedIncome:C}";
    }
    
    public void UpdateMonthWorkingHoursLabel(object sender, DatePickerSelectedValueChangedEventArgs args)
    {
        var parsedDateTime = DateTimeOffsetParser(args.NewDate);
        _helper = new PtoHelper(parsedDateTime.Year, parsedDateTime.Month);
        _hasDate = true;

        // var businessDays = helper.CurrentMonth.BusinessDays; 
        //Month.CalculateNumberOfBusinessDaysInMonth(parsedDateTime.Year, parsedDateTime.Month);
        // var hourlyIncome = (double) monthlyIncome / (dailyHours * businessDays);
        // var ptoHourValue = (double) monthlyIncome / ptoMonthlyHours;

    }

    private static DateTimeOffset DateTimeOffsetParser(DateTimeOffset? dateTime)
    {
        if (dateTime == null)
        {
            // TODO - Error Handling with Dialog
            return DateTimeOffset.Now;
        }
        
        return dateTime.Value;
    }

    private string SanitizeMaskedInput(string input)
    {
        return input.Trim().Replace("_", "");
    }
}