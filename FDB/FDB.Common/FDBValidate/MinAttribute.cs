using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FDB.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MinAttribute : ValidationAttribute, IClientValidatable
    {
        public int MinValue { get; set; }
        public MinAttribute(int minValue, string errorMessage)
            : base(errorMessage)
        {
            this.MinValue = minValue;
        }
 
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            
            if (value != null)
            {
                int toValidate = Convert.ToInt32(value);

                if (toValidate < MinValue)
                {
                    validationResult = new ValidationResult(ErrorMessageString);
                }
            }

            return validationResult;
        }



        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            var minRule = new ModelClientValidationRule() { 
                ValidationType = "min",
                ErrorMessage = errorMessage // This is the name the jQuery adapter will use
            };

            //"minvalue" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            minRule.ValidationParameters.Add("minvalue", MinValue.ToString());

            yield return minRule;
        }
    }
}
