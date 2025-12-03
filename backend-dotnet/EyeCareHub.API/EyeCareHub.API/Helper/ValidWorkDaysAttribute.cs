using EyeCareHub.DAL.Entities.Identity;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Helper
{
    public class ValidWorkDaysAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var days = value as List<string>;
            if (days == null)
                return new ValidationResult("Invalid input");

            foreach (var day in days)
            {
                if (!Enum.TryParse(typeof(WorkDays), day, ignoreCase: true, out _))
                {
                    return new ValidationResult($"Invalid work day: {day}");
                }
            }

            return ValidationResult.Success;
        }
    }
}
