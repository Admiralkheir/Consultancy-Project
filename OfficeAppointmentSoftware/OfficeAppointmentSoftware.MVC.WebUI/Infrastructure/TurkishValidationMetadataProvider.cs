using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace OfficeAppointmentSoftware.MVC.WebUI.Infrastructure
{
    // Gives every validation attribute without an explicit message a Turkish default,
    // so entities and view models only need [Display(Name = ...)].
    public class TurkishValidationMetadataProvider : IValidationMetadataProvider
    {
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var attribute in context.ValidationMetadata.ValidatorMetadata.OfType<ValidationAttribute>())
            {
                if (HasCustomMessage(attribute))
                {
                    continue;
                }

                var message = attribute switch
                {
                    RequiredAttribute => "{0} alanı zorunludur.",
                    StringLengthAttribute { MinimumLength: > 0 } => "{0} en az {2}, en fazla {1} karakter olmalıdır.",
                    StringLengthAttribute => "{0} en fazla {1} karakter olabilir.",
                    MaxLengthAttribute => "{0} en fazla {1} karakter olabilir.",
                    MinLengthAttribute => "{0} en az {1} karakter olmalıdır.",
                    EmailAddressAttribute => "Geçerli bir e-posta adresi giriniz.",
                    CompareAttribute => "{0} ile {1} eşleşmiyor.",
                    RangeAttribute => "{0} {1} ile {2} arasında olmalıdır.",
                    RegularExpressionAttribute => "{0} geçerli bir biçimde değil.",
                    _ => null
                };
                if (message != null)
                {
                    attribute.ErrorMessage = message;
                }
            }
        }

        private static bool HasCustomMessage(ValidationAttribute attribute)
        {
            if (attribute.ErrorMessageResourceType != null)
            {
                return true;
            }
            if (string.IsNullOrEmpty(attribute.ErrorMessage))
            {
                return false;
            }
            // Some attributes (e.g. EmailAddress) report their built-in English text through ErrorMessage,
            // so compare with a fresh instance to tell a default from a message set in code.
            var type = attribute.GetType();
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                return true;
            }
            var fresh = (ValidationAttribute)Activator.CreateInstance(type);
            return attribute.ErrorMessage != fresh.ErrorMessage;
        }

        public static void Configure(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(new TurkishValidationMetadataProvider());

            var messages = options.ModelBindingMessageProvider;
            messages.SetValueMustNotBeNullAccessor(_ => "Bu alan zorunludur.");
            messages.SetMissingBindRequiredValueAccessor(field => $"{field} alanı zorunludur.");
            messages.SetAttemptedValueIsInvalidAccessor((value, field) => $"'{value}' değeri {field} için geçerli değil.");
            messages.SetNonPropertyAttemptedValueIsInvalidAccessor(value => $"'{value}' değeri geçerli değil.");
            messages.SetUnknownValueIsInvalidAccessor(field => $"{field} için girilen değer geçerli değil.");
            messages.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Girilen değer geçerli değil.");
            messages.SetValueIsInvalidAccessor(value => $"'{value}' değeri geçerli değil.");
            messages.SetValueMustBeANumberAccessor(field => $"{field} bir sayı olmalıdır.");
            messages.SetNonPropertyValueMustBeANumberAccessor(() => "Bu alan bir sayı olmalıdır.");
        }
    }
}
