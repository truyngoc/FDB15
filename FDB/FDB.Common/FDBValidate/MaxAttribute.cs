using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace FDB.Common
{
    public class MaxAttribute : ValidationAttribute
    {
    }




    // FDB_DateGreaterThan
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FDB_DateGreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        string otherPropertyName;

        public FDB_DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
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
                // get value of reference object
                object referenceObj = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                if (otherPropertyInfo.PropertyType == typeof(DateTime?))
                { 
                    var d = value as DateTime?;
                    // gia tri can so sanh ko null
                    if (d.HasValue)
                    {
                        if (referenceObj != null)
                        {
                            DateTime toValidate = (DateTime)value;
                            DateTime referenceProperty = (DateTime)referenceObj;

                            // if the end date is lower than the start date, than the validationResult will be set to false and return
                            // a properly formatted error message
                            if (toValidate.CompareTo(referenceProperty) < 1)
                            {
                                validationResult = new ValidationResult(ErrorMessageString);
                            }
                        }                        
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
            dateGreaterThanRule.ErrorMessage = errorMessage;
            dateGreaterThanRule.ValidationType = "dategreaterthan"; // This is the name the jQuery adapter will use
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            dateGreaterThanRule.ValidationParameters.Add("otherpropertyname", otherPropertyName);

            yield return dateGreaterThanRule;
        }
    }


    // FDB_DateGreaterThanOrEqualTo
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FDB_DateGreaterThanOrEqualToAttribute : ValidationAttribute, IClientValidatable
    {
        string otherPropertyName;

        public FDB_DateGreaterThanOrEqualToAttribute(string otherPropertyName, string errorMessage)
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
                // get value of reference object
                object referenceObj = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                if (otherPropertyInfo.PropertyType == typeof(DateTime?))
                {
                    var d = value as DateTime?;
                    // gia tri can so sanh ko null
                    if (d.HasValue)
                    {
                        if (referenceObj != null)
                        {
                            DateTime toValidate = (DateTime)value;
                            DateTime referenceProperty = (DateTime)referenceObj;

                            // if the end date is lower than the start date, than the validationResult will be set to false and return
                            // a properly formatted error message
                            if (toValidate.CompareTo(referenceProperty) < 0)
                            {
                                validationResult = new ValidationResult(ErrorMessageString);
                            }
                        }
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
            dateGreaterThanRule.ErrorMessage = errorMessage;
            dateGreaterThanRule.ValidationType = "dategreaterthanorequalto"; // This is the name the jQuery adapter will use
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            dateGreaterThanRule.ValidationParameters.Add("otherpropertyname", otherPropertyName);

            yield return dateGreaterThanRule;
        }
    }

    // FDB_DateLessThanOrEqualTo
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FDB_DateLessThanOrEqualToAttribute : ValidationAttribute, IClientValidatable
    {
        string otherPropertyName;

        public FDB_DateLessThanOrEqualToAttribute(string otherPropertyName, string errorMessage)
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
                // get value of reference object
                object referenceObj = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                if (otherPropertyInfo.PropertyType == typeof(DateTime?))
                {
                    var d = value as DateTime?;
                    // gia tri can so sanh ko null
                    if (d.HasValue)
                    {
                        if (referenceObj != null)
                        {
                            DateTime toValidate = (DateTime)value;
                            DateTime referenceProperty = (DateTime)referenceObj;

                            // if the end date is lower than the start date, than the validationResult will be set to false and return
                            // a properly formatted error message
                            if (toValidate.CompareTo(referenceProperty) > 0)
                            {
                                validationResult = new ValidationResult(ErrorMessageString);
                            }
                        }
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
            dateGreaterThanRule.ErrorMessage = errorMessage;
            dateGreaterThanRule.ValidationType = "datelessthanorequalto"; // This is the name the jQuery adapter will use
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            dateGreaterThanRule.ValidationParameters.Add("otherpropertyname", otherPropertyName);

            yield return dateGreaterThanRule;
        }
    }


    // FDB_RequiredIfEqualTo
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FDB_RequiredIfEqualToAttribute : ValidationAttribute, IClientValidatable
    {
        string otherPropertyName;
        object otherPropertyValueEqualTo;

        public FDB_RequiredIfEqualToAttribute(string otherPropertyName, object otherPropertyValueEqualTo, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
            this.otherPropertyValueEqualTo = otherPropertyValueEqualTo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                // Using reflection we can get a reference to the other date property, in this example the project start date
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
                // get value of reference object
                object referenceObj = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                
                        if (referenceObj != null)
                        {
                            int toValidate = (int) otherPropertyValueEqualTo;
                            int referenceProperty = (int) referenceObj;                            

                            // if the end date is lower than the start date, than the validationResult will be set to false and return
                            // a properly formatted error message
                            if ((toValidate == referenceProperty) && (value != null))
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
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule requiredifequaltoRule = new ModelClientValidationRule();
            requiredifequaltoRule.ErrorMessage = errorMessage;
            requiredifequaltoRule.ValidationType = "requiredifequalto"; // This is the name the jQuery adapter will use
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            requiredifequaltoRule.ValidationParameters.Add("otherpropertyname", otherPropertyName);

            yield return requiredifequaltoRule;
        }
    }
}
