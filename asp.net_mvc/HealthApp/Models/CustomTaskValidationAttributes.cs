using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace healthApp.Models
{
    //A Validation attribute that checks if one DateTime is greater than another
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        string otherPropertyName;

        public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                // Using reflection we can get a reference to the other date property, in this example the project start date
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
                // Let's check that otherProperty is of type DateTime as we expect it to be
                if (otherPropertyInfo.PropertyType.Equals(new DateTime().GetType()))
                {
                    DateTime toValidate = (DateTime)value;
                    DateTime referenceProperty = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                    // if the end date is lower than the start date, than the validationResult will be set to false and return
                    // a properly formatted error message
                    if (toValidate.CompareTo(referenceProperty) < 1)
                    {
                        validationResult = new ValidationResult(ErrorMessageString);
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }
    }
    


    //A ValidationAttribute that checks for the valid types of frequency's in a task's iCal implementation
    //TODO: take in valid types as arguments
    public class ValidFrequencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = new ValidationResult("\"" + value.ToString() + "\"" + ErrorMessageString);
            try
            {
                if (value is string)
                {
                    string frequencyString = (string)value;
                    if (frequencyString.Equals("hourly"))
                        validationResult = ValidationResult.Success;
                    if (frequencyString.Equals("daily"))
                        validationResult = ValidationResult.Success;
                    if (frequencyString.Equals("weekly"))
                        validationResult = ValidationResult.Success;
                    if (frequencyString.Equals("monthly"))
                        validationResult = ValidationResult.Success;
                    if (frequencyString.Equals("yearly"))
                        validationResult = ValidationResult.Success;
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type String");
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }
    }
}