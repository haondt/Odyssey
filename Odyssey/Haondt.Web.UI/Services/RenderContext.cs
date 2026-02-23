namespace Haondt.Web.UI.Services
{
    public class RenderContext : IRenderContextAccessor, IRenderContextMutator
    {
        public bool IsReset { get; set; }
    }
    public interface IRenderContextAccessor
    {
        bool IsReset { get; }
    }

    public interface IRenderContextMutator
    {
        bool IsReset { get; set; }
    }
}
