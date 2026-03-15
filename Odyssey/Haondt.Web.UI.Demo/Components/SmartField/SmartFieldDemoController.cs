using Haondt.Web.Core.Controllers;
using Haondt.Web.UI.Attributes;
using Haondt.Web.UI.Components.Element;
using Haondt.Web.UI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Haondt.Web.UI.Demo.Components.SmartField
{
    [Route("/demo/smart-field/")]
    [ServiceFilter(typeof(ModelStateValidationFilter))]
    public class SmartFieldDemoController : UIController
    {
        [HttpGet]
        public Task<IResult> Get() => ComponentFactory.RenderComponentAsync<SmartFieldDemo>();

        [HttpPost]
        [ValidationState(typeof(FieldInvalidator))]
        public Task<IResult> Post([FromForm] SmartFieldData data) => ComponentFactory.RenderComponentAsync(new SmartFieldDataPostResult
        {
            Data = data
        });
    }
}
