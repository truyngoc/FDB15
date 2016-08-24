using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FDB.Common
{
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
        //Using:
        //[DisplayName("Start date")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime StartDate { get; set; }

        //[DisplayName("Estimated end date")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[DateGreaterThan("StartDate", "End Date end date must not exceed start date")]
        //public DateTime EndDate { get; set; }
    }

    public class EndDateValidationAttribute : ValidationAttribute
    {
        public object CheckDate { get; set; }


        public override bool IsValid(object value)
        {
            // This is not a required field validator, so if the value equals null return true.
            if (value == null) return true;


            // Unbox incoming value parameter.
            DateTime EndDate = (DateTime)value;


            // Unbox CheckDate (wasn't able to use DateTime).
            if (EndDate < (DateTime)CheckDate) return false;


            return true;
        }
    }
    //Using:
    //[EndDateValidation(CheckDate=(object)StartDate, ErrorMessage="End Date must be after Start Date")]
}
