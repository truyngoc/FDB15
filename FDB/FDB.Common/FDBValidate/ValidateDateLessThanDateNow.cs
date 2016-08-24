using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FDB.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateDateLessThanDateNow : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value!=null)
            {
                string svalue = Convert.ToDateTime(value).ToString("dd/MM/yyyy");
                string snow = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime dvalue = Convert.ToDateTime(svalue);
                DateTime dnow = Convert.ToDateTime(snow);
                dvalue = DateTime.ParseExact(svalue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dnow = DateTime.ParseExact(snow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (dvalue > dnow)
                {
                    return new ValidationResult("Ngày không được lớn hơn ngày hiện tại.");
                }

            }
          
            return ValidationResult.Success;
        }


        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    ModelClientValidationRule mvr = new ModelClientValidationRule();
        //    mvr.ErrorMessage = "Ngày không được lớn hơn ngày hiện tại.";
        //    mvr.ValidationType = "validatedatelessthandatenow";
        //    return new[] { mvr };
        //}
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
            dateGreaterThanRule.ErrorMessage = "Ngày không được lớn hơn ngày hiện tại.";
            dateGreaterThanRule.ValidationType = "validatedatelessthandatenow"; // This is the name the jQuery adapter will use
            yield return dateGreaterThanRule;
            
        }

        //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        //public sealed class ValidBirthDate : ValidationAttribute, IClientValidatable
        //{
        //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //    {
        //        if (value != null)
        //        {
        //            DateTime _birthJoin = Convert.ToDateTime(value);
        //            if (_birthJoin > DateTime.Now)
        //            {
        //                return new ValidationResult("Birth date can not be greater than current date.");
        //            }
        //        }
        //        return ValidationResult.Success;
        //    }

        //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //    {
        //        ModelClientValidationRule mvr = new ModelClientValidationRule();
        //        mvr.ErrorMessage = "Birth Date can not be greater than current date";
        //        mvr.ValidationType = "validbirthdate";
        //        return new[] { mvr };
        //    }
        //}
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateQuyLessThanQuyNow : ValidationAttribute, IClientValidatable
    {
        string otherPropertyName;
        public ValidateQuyLessThanQuyNow(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int _quyNow = 1;
                switch(DateTime.Now.Month)
                {
                    case 1:
                    case 2:
                    case 3:
                        _quyNow = 1;
                        break;
                    case 4:
                    case 5:
                    case 6:
                        _quyNow = 2;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        _quyNow = 3;
                        break;
                    case 10:
                    case 11:
                    case 12:
                        _quyNow = 4;
                        break;
                    default:
                        _quyNow = 1;
                        break;
                        
                }
                var otherProperty = validationContext.ObjectType.GetProperty(this.otherPropertyName);
                // get value NAM
                object otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);
                if (otherPropertyValue!=null)
                {
                    if (Convert.ToInt32(value) > _quyNow && Convert.ToInt32(otherPropertyValue)==DateTime.Now.Year)
                    {
                        return new ValidationResult("Quý không được lớn hơn quý hiện tại.");
                    }

                }
              
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            
            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
            dateGreaterThanRule.ErrorMessage = "Quý không được lớn hơn quý hiện tại.";
            dateGreaterThanRule.ValidationType = "validatequylessthanquynow"; // This is the name the jQuery adapter will use
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            dateGreaterThanRule.ValidationParameters.Add("otherpropertyname", otherPropertyName);

            yield return dateGreaterThanRule;
          
        }

      
    }
}
