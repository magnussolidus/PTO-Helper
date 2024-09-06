# PTO Helper

This project is about a simple software to help you calculate if taking a PTO on a particular month will increase/decrease 
your income for said month.

![PTO Helper software screenshot](/resources/software.png)

## Why?

Some companies use a fixed amount of hours to calculate your PTO, while your monthly working hours may vary.
This variation happens due to the number of business days changing each month, while your income is fixed.

> Example:
> 
> With a monthly income of \$500, it means that on August 2024 you had to work 22 business days, on a \$2.84 hourly rate.
> 
> While on May 2024 you had to work 23 business days for the same \$500, on a \$2.72 hourly rate.
 
This variation in the hourly rate is what can cause the total income difference, when the PTO hours don't use the same 
value.

## How

You need to fill in the following fields for this software to properly calculate things:

1. Your Monthly Income 
   1. Your monthly income in USD
2. Daily Working Hours: 
   1. The amount of hours you are expected to work on a daily basis.
   2. Default Value is 8.
3. PTO Days Count
   1. The amount of days you intend to take as PTO for that month.
   2. Default Value is 0.
4. Pick your month
   1. Select the desired month for the calculations to happen
5. Monthly Hours considered for PTO
   1. The amount of hours that are considered when calculating your PTO. 
   2. Usually this information is provided by your company and tends to be an annual medium of the monthly working hours.

The following fields will be automatically filled by the software:

1. Monthly Working Days:
   1. The software already calculates the amount of business days based on the month you picked.
   2. Holidays are NOT included in this calculation. 
      1. i.e. if you have a Holiday on a Wednesday, it will still count as a business day
   3. It automatically adjusts the number of working days by subtracting your PTO days
      1. i.e. if you will take 2 PTO days, it will display the **total of working days for you**, not the amount of business days that month.
2. Your Hourly Rate
   1. Will display the value of your hourly rate for that particular month
3. Your PTO Hourly rate
   1. Will display the value of your hourly rate while taking a PTO.
   2. This calculation is based on the amount of daily working hours and how many monthly hours are considered for the PTO calculation.
4. Total Working Hours in `month` 
   1. Will display the total of working hours you are expected to do on said month.
   2. It adjusts accordingly to your PTO days count.
5. The Expected income text
   1. Will display the expected income for said `month`. 
   2. It calculates the sum of your regular working hours with the pto hours.

---

# Requirements

* [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [AvaloniaUI](https://avaloniaui.net/)

# License

This software is distributed under the GNU GPL v3. A copy of this license is available here.

# Future Improvements

- [ ] Improve UI
- [ ] Cleanup the Code
- [ ] Provide binaries for Windows
- [ ] Provide binaries for Linux

