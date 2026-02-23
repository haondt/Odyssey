namespace Haondt.Web.UI.Attributes
{
    /// <summary>
    /// Sets <see cref="Haondt.Web.UI.Services.RenderContext.IsReset"/> to <see cref="true"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ResetRenderContextAttribute() : Attribute
    {
    }
}
