using Haondt.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Odyssey.Client.Core.Services
{
    public interface IStandaloneModelBinder
    {
        Task<DetailedResult<TModel, ModelStateDictionary>> BindAndValidateFormAsync<TModel>(HttpContext context);
        Task<TModel> BindAndValidateFormAsync<TModel, TValidationComponent>(HttpContext context, Optional<string> hxSwapId = default);
        Task<DetailedResult<TModel, ModelStateDictionary>> BindAndValidateQueryAsync<TModel>(HttpContext context);
        Task<TModel> BindAndValidateQueryAsync<TModel, TValidationComponent>(HttpContext context, Optional<string> hxSwapId = default);
    }
}
