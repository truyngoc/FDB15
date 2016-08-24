using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;
using System.Reflection;


namespace FDB.Common
{

      
       public class AtLeastOnePropertyAttribute : ValidationAttribute, IClientValidatable
        {
            
                               public override bool IsValid(object value)
                      {
                        //  Need to use reflection to get properties of "value"...
                        var typeInfo = value.GetType();

                        var propertyInfo = typeInfo.GetProperties();

                        foreach (var property in propertyInfo)
                        {
                          if (null != property.GetValue(value, null))
                          
                          {
                            // We've found a property with a value
                            return true;
                          }
                        }

                        // All properties were null.
                        return false;
                      }

                               public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
                            ModelMetadata metadata,
                            ControllerContext context)
                               {
                                   yield return new ModelClientValidationRule
                                   {
                                       ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                                       ValidationType = "atleastonecheckrequired"
                                   };
                               }
            }

        


        }

      
     
    

