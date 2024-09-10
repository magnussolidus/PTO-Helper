using System;

namespace PTO_Calculator;

public class PtoHelper(int year, int month)
{
    private decimal MonthlyIncome { get; set; }
    private int DailyWorkingHours { get; set; }
    public Month CurrentMonth { get; } = new(year, month);
    private int PtoDays { get; set; }
    private int PtoMonthlyHours { get; set; }
    private string[] ValidatedText { get; } = new string[sizeof(FieldEnum)];
    private string _errorMessage = string.Empty;

    public bool InsertText(string text, FieldEnum textSource)
    {
        return SaveSanitizedText(text, textSource);
    }

    public void ResetErrorMessage()
    {
        _errorMessage = string.Empty;
    }

    public string GetErrorMessage()
    {
        _errorMessage = $"{_errorMessage.Trim()}";
        return _errorMessage;
    }
    
    private bool SaveSanitizedText(string input, FieldEnum textField)
    {
        input = input.Trim().Replace("_", "");
        if (string.IsNullOrEmpty(input))
        {
            _errorMessage = $"{_errorMessage}{Enum.GetName(textField)} | ";
            return false;
        }
        
        ValidatedText[(int)textField] = input; 
        return true;
    }

    public bool InitializeFieldsFromSavedTexts()
    {
        var isIncomeParsed = decimal.TryParse(ValidatedText[(int)FieldEnum.Income], out var income);
        var isDailyHoursParsed = int.TryParse(ValidatedText[(int)FieldEnum.DailyHours], out var dailyHours);
        var isPtoDaysParsed = int.TryParse(ValidatedText[(int)FieldEnum.PtoDays], out var ptoDays);
        var isPtoMonthlyParsed = int.TryParse(ValidatedText[(int)FieldEnum.PtoMonthlyHours], out var ptoMonthly);

        if (!isIncomeParsed || !isDailyHoursParsed || !isPtoDaysParsed || !isPtoMonthlyParsed)
        {
            _errorMessage = "Failed to parse the input data. Please try again.";
            return false;
        }
        
        if (ptoMonthly <= 0)
        {
            _errorMessage = "The amount of PTO Monthly Hours must be greater than zero!";
            return false;
        }


        MonthlyIncome = income;
        DailyWorkingHours = dailyHours;
        PtoDays = ptoDays;
        PtoMonthlyHours = ptoMonthly;

        CurrentMonth.WorkingDays = CurrentMonth.BusinessDays - PtoDays;
        CurrentMonth.MonthlyWorkingHours = DailyWorkingHours * CurrentMonth.WorkingDays;

        return true;
    }

    public decimal CalculatePtoHourValue()
    {
        return MonthlyIncome / PtoMonthlyHours;
    }

    public decimal CalculateHourlyIncome()
    {
        return MonthlyIncome / (Convert.ToDecimal(DailyWorkingHours) * CurrentMonth.BusinessDays);
    }

    public decimal CalculateExpectedMonthlyIncome()
    {
        var regularIncome = Convert.ToDecimal(DailyWorkingHours) * CurrentMonth.WorkingDays * CalculateHourlyIncome();
        var ptoIncome = Convert.ToDecimal(DailyWorkingHours) * PtoDays * CalculatePtoHourValue();
        return regularIncome + ptoIncome;
    }
}