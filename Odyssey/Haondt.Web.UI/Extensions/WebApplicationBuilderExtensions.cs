using System.ComponentModel.DataAnnotations;
using Haondt.Core.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Components.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Haondt.Web.UI.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication AddHaondtWebUIEndpoints(this WebApplication app)
        {
            app.MapGet("/haondt-ui/fragments/confirmation-dialog", async (HttpContext context, IComponentFactory componentFactory, [AsParameters] ConfirmationDialogModel model) =>
            {
                var ctx = new ValidationContext(model);
                if (!Validator.TryValidateObject(model, ctx, [], true))
                    return TypedResults.BadRequest("The request was not valid");

                return await componentFactory.RenderComponentAsync(new ConfirmationDialog
                {
                    Message = model.Message,
                    Intent = model.Intent ?? ConfirmationDialogIntent.Inert,
                    Title = model.Title.AsOptional().Reject(string.IsNullOrWhiteSpace),
                    ConfirmText = model.ConfirmText.AsOptional().Reject(string.IsNullOrWhiteSpace),
                    CancelText = model.CancelText.AsOptional().Reject(string.IsNullOrWhiteSpace)
                });
            });

            app.MapGet("/haondt-ui/fragments/chip", async (HttpContext context, IComponentFactory componentFactory, [AsParameters] ChipModel model) =>
            {
                var ctx = new ValidationContext(model);
                if (!Validator.TryValidateObject(model, ctx, [], true))
                    return TypedResults.BadRequest("The request was not valid");

                ChipInputData? inputData = null;
                var inputName = model.InputName.AsOptional().Reject(string.IsNullOrEmpty);
                var inputValue = model.InputValue.AsOptional().Reject(string.IsNullOrEmpty);
                if (inputName.HasValue || inputValue.HasValue)
                    inputData = new(Name: inputName.Unwrap(), Value: inputValue.Unwrap());

                return await componentFactory.RenderComponentAsync(new Chip
                {
                    Text = model.Text,
                    Label = model.Label.AsOptional().Reject(string.IsNullOrWhiteSpace),
                    Deletable = model.Deletable ?? false,
                    EmitEvents = model.EmitEvents ?? false,
                    Input = inputData
                });
            });

            return app;
        }
    }
}
