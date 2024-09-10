using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace PTO_Calculator;

public partial class MainWindow : Window
{
    private PtoHelper _helper;
    private bool _hasDate;
    
    private readonly Color _defaultColor = Colors.Black;
    private readonly Color _errorColor = Colors.Red;
    
    private const string MissingDateMessage = "You must select a month and year before you can calculate it...";
    private const string MissingRequiredFieldsMessage = "You must fill all of the required fields!\nMissing fields: {0}";
    private const string FailedToInitializeDataMessage = "Required Data was not properly initialized!\nReason: {0}";

    public MainWindow()
    {
        InitializeComponent();
        SetCulture();
        HideErrorMessage();
    }

    private static void SetCulture()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
    }

    private bool SaveInputData()
    {
        _helper.ResetErrorMessage();
        var monthlyIncome = _helper.InsertText(Income.Text, FieldEnum.Income);
        var dailyHours = _helper.InsertText(DailyHours.Text, FieldEnum.DailyHours);
        var ptoDays = _helper.InsertText(PtoDays.Text, FieldEnum.PtoDays);
        var monthlyPtoHours = _helper.InsertText(PtoMonthlyHours.Text, FieldEnum.PtoMonthlyHours);
        
        UpdateRequiredFieldsVisuals(monthlyIncome, dailyHours, ptoDays, monthlyPtoHours);

        return monthlyPtoHours && dailyHours && ptoDays && monthlyIncome;
    }

    private void UpdateRequiredFieldsVisuals(bool monthlyIncome, bool dailyHours, bool ptoDays, bool monthlyPtoHours)
    {
        UpdateBorderColorsOnField(FieldEnum.Income, monthlyIncome ? _defaultColor : _errorColor);
        UpdateBorderColorsOnField(FieldEnum.DailyHours, dailyHours ?  _defaultColor : _errorColor);
        UpdateBorderColorsOnField(FieldEnum.PtoDays, ptoDays ? _defaultColor : _errorColor);
        UpdateBorderColorsOnField(FieldEnum.PtoMonthlyHours, monthlyPtoHours ? _defaultColor : _errorColor);
    }

    public void Calculate(object sender, RoutedEventArgs args)
    {
        if (!ValidateRequiredFields())
        {
            return;
        }

        var isDataInitialized = _helper.InitializeFieldsFromSavedTexts();
        if (!isDataInitialized)
        {
            SetErrorMessage(string.Format(FailedToInitializeDataMessage, _helper.GetErrorMessage())); // TODO - update this log
            return;
        }
        
        var hourlyIncome = _helper.CalculateHourlyIncome();
        var ptoHourValue = _helper.CalculatePtoHourValue();
        var expectedIncome = _helper.CalculateExpectedMonthlyIncome();
        
        UpdateUiTextWithResults(hourlyIncome, ptoHourValue, expectedIncome);
    }

    private void UpdateUiTextWithResults(decimal hourlyIncome, decimal ptoHourValue, decimal expectedIncome)
    {
        HourlyRateTextBox.Text = hourlyIncome.ToString("C");
        MonthlyWorkingHoursLabel.Content = $"Total Working Hours in {_helper.CurrentMonth.MonthDateTime:MMMM}";
        MonthlyWorkingHoursResult.Text = _helper.CurrentMonth.MonthlyWorkingHours.ToString("D");
        MonthlyWorkingDaysResult.Text = _helper.CurrentMonth.WorkingDays.ToString("D");
        PtoHourlyRateTextBox.Text = ptoHourValue.ToString("C");
        ExpectedIncomeLabel.Content = $"The expected income for {_helper.CurrentMonth.MonthDateTime:MMMM} is {expectedIncome:C}";
    }

    private bool ValidateRequiredFields()
    {
        if (!_hasDate)
        {
            UpdateBorderColorsOnField(FieldEnum.DatePicker, _errorColor);
            SetErrorMessage(MissingDateMessage);
            return false;
        }
        
        UpdateBorderColorsOnField(FieldEnum.DatePicker, _defaultColor);
        var hasInputData = SaveInputData();
        
        if (!hasInputData)
        {
            SetErrorMessage(string.Format(MissingRequiredFieldsMessage, _helper.GetErrorMessage()));
            return false;
        }
        
        HideErrorMessage();
        return true;
    }

    public void UpdateMonthWorkingHoursLabel(object sender, DatePickerSelectedValueChangedEventArgs args)
    {
        var parsedDateTime = DateTimeOffsetParser(args.NewDate);
        _helper = new PtoHelper(parsedDateTime.Year, parsedDateTime.Month);
        _hasDate = true;
    }
    
    private void UpdateBorderColorsOnField(FieldEnum field, Color color)
    {
        switch (field)
        {
            case FieldEnum.Income:
                Income.BorderBrush = new SolidColorBrush(color);
                break;
            case FieldEnum.DailyHours:
                DailyHours.BorderBrush = new SolidColorBrush(color);
                break;
            case FieldEnum.PtoDays:
                PtoDays.BorderBrush = new SolidColorBrush(color);
                break;
            case FieldEnum.PtoMonthlyHours:
                PtoMonthlyHours.BorderBrush = new SolidColorBrush(color);
                break;
            case FieldEnum.DatePicker:
                DatePicker.BorderBrush = new SolidColorBrush(color);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(field), field, null);
        }
    }

    private void SetErrorMessage(string text)
    {
        ErrorLabel.Content = text;
        ErrorLabel.IsVisible = true;
    }

    private void HideErrorMessage()
    {
        ErrorLabel.IsVisible = false;
    }

    private DateTimeOffset DateTimeOffsetParser(DateTimeOffset? dateTime)
    {
        return dateTime ?? DateTimeOffset.Now;
    }
}