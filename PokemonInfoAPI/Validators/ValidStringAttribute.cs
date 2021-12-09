using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonInfoAPI.Validators
{
	public class ValidStringAttribute : ValidationAttribute
    {
        
        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value as string))
            {
                return false;
            }

            if (!(value is string stringValue)) return false;

            double dobleValue;
            var hasNumber = double.TryParse(stringValue, out dobleValue);

            return !hasNumber;

            
        }

    }

}
