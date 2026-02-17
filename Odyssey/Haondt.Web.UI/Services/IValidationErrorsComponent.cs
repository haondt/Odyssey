namespace Haondt.Web.UI.Services
{
    public interface IValidationErrorsComponent : IValidationComponent
    {
        Dictionary<string, string> ValidationErrors { get; set; }
    }
}
