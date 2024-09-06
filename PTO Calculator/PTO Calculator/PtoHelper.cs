using System;

namespace PTO_Calculator;

public class PtoHelper(int year, int month)
{
    public decimal MonthlyIncome { get; set; }
    public int DailyWorkingHours { get; set; }
    public Month CurrentMonth { get; set; } = new(year, month);
    public int PtoDays { get; set; }
    public int PtoMonthlyHours { get; set; }
    private string[] ValidatedText { get; set; } = new string[sizeof(TextValidationEnum)];

    public bool InsertText(string text, TextValidationEnum textSource)
    {
        return SaveSanitizedText(text, textSource);
    }

    public string ReturnValidatedText(TextValidationEnum textSource) =>  textSource switch
    {
        TextValidationEnum.Income => ValidatedText[0],
        TextValidationEnum.DailyHours => ValidatedText[1],
        TextValidationEnum.PtoDays => ValidatedText[2],
        TextValidationEnum.PtoMonthlyHours => ValidatedText[3],
        _ => throw new ArgumentOutOfRangeException(nameof(textSource), textSource, null)
    };

    private bool SaveSanitizedText(string input, TextValidationEnum textField)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }
        
        ValidatedText[(int)textField] = input.Trim().Replace("_", ""); 
        return true;
    }

    public bool InitializeFieldsFromSavedTexts()
    {
        var isIncomeParsed = decimal.TryParse(ValidatedText[(int)TextValidationEnum.Income], out var income);
        var isDailyHoursParsed = int.TryParse(ValidatedText[(int)TextValidationEnum.DailyHours], out var dailyHours);
        var isPtoDaysParsed = int.TryParse(ValidatedText[(int)TextValidationEnum.PtoDays], out var ptoDays);
        var isPtoMonthlyParsed = int.TryParse(ValidatedText[(int)TextValidationEnum.PtoMonthlyHours], out var ptoMonthly);

        if (!isIncomeParsed || !isDailyHoursParsed || !isPtoDaysParsed || !isPtoMonthlyParsed)
        {
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

    private static bool IsTextFieldValid(string field)
    {
        return !string.IsNullOrWhiteSpace(field);
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