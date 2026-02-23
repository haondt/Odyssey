using Haondt.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing;
using Odyssey.Client.Core.Exceptions;

namespace Odyssey.Client.Core.Services
{
    public class StandaloneModelBinder(
        IModelBinderFactory modelBinderFactory,
        IModelMetadataProvider metadataProvider,
        IObjectModelValidator objectValidator
        ) : IStandaloneModelBinder
    {

        private BindingSourceValueProvider GetValueProvider(IFormCollection form) => new FormValueProvider(
                BindingSource.Form,
                form,
                System.Globalization.CultureInfo.CurrentCulture);

        private BindingSourceValueProvider GetValueProvider(IQueryCollection query) => new QueryStringValueProvider(
                BindingSource.Query,
                query,
                System.Globalization.CultureInfo.CurrentCulture);

        public Task<DetailedResult<TModel, ModelStateDictionary>> BindAndValidateQueryAsync<TModel>(HttpContext context)
        {
            return BindAndValidateAsync<TModel>(GetValueProvider(context.Request.Query), context);
        }

        public async Task<TModel> BindAndValidateQueryAsync<TModel, TValidationComponent>(HttpContext context, Optional<string> hxSwapId = default)
        {
            var result = await BindAndValidateAsync<TModel>(GetValueProvider(context.Request.Query), context);
            if (!result.IsSuccessful)
                throw new StandaloneModelBinderValidationException(typeof(TValidationComponent), result.Reason, hxSwapId);
            return result.Value;
        }

        public Task<DetailedResult<TModel, ModelStateDictionary>> BindAndValidateFormAsync<TModel>(HttpContext context)
        {
            return BindAndValidateAsync<TModel>(GetValueProvider(context.Request.Form), context);
        }

        public async Task<TModel> BindAndValidateFormAsync<TModel, TValidationComponent>(HttpContext context, Optional<string> hxSwapId = default)
        {
            var result = await BindAndValidateAsync<TModel>(GetValueProvider(context.Request.Form), context);
            if (!result.IsSuccessful)
                throw new StandaloneModelBinderValidationException(typeof(TValidationComponent), result.Reason, hxSwapId);
            return result.Value;
        }

        private async Task<DetailedResult<TModel, ModelStateDictionary>> BindAndValidateAsync<TModel>(BindingSourceValueProvider values, HttpContext context)
        {
            var modelState = new ModelStateDictionary();

            var meta = metadataProvider.GetMetadataForType(typeof(TModel));

            var actionContext = new ActionContext
            {
                HttpContext = context,
                RouteData = context.GetRouteData(),
                ActionDescriptor = new()
            };

            var binder = modelBinderFactory.CreateBinder(new()
            {
                Metadata = meta,
                BindingInfo = new()
            });

            var bindingContext = DefaultModelBindingContext.CreateBindingContext(
                actionContext,
                values,
                meta,
                bindingInfo: new(),
                modelName: "");

            await binder.BindModelAsync(bindingContext);

            TModel model;
            if (!bindingContext.Result.IsModelSet)
                model = Activator.CreateInstance<TModel>();
            else
                model = (TModel)bindingContext.Result.Model!;

            objectValidator.Validate(
                actionContext,
                validationState: null,
                prefix: "",
                model: model);

            if (actionContext.ModelState.IsValid)
                return new(model);
            return new(actionContext.ModelState);
        }
    }
}
