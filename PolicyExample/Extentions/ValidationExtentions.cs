
using FluentValidation.Results;

namespace PolicyExample.Extentions
{
    public static class ValidationExtentions
    {

        public static List<string> CustomValidationErrorList(this ValidationResult validationResult)
        {
            var tempList = new List<string>();


            validationResult.Errors.ForEach(x => tempList.Add(x.ToString()));


            return tempList;
        }
    }
}
