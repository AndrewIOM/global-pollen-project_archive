using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.App.Validation;
using static GlobalPollenProject.App.Validation.AppServiceResultBase;

namespace GlobalPollenProject.WebUI.Extensions
{
    public static class ModelValidationExtensionMethods
    {
        public static ModelStateDictionary AddServiceErrors(this ModelStateDictionary state, List<AppServiceMessage> messages)
        {
            foreach (var error in messages.Where(m => m.MessageType == AppServiceMessageType.Error))
            {
                state.AddModelError(error.Key, error.Message);
            }
            return state;
        }
    }
}