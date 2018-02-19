using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Extensions
{
    static class ModelStateDictionaryExtensions
    {
        public static List<string> ErrorsToList(this ModelStateDictionary modelState)
        {
            return (from modelStateEntry in modelState.Values from error in modelStateEntry.Errors select error.ErrorMessage).ToList();
        }
    }
}
