#nullable enable
namespace ShapezShifter.Hijack
{
    public interface IConsoleRewirer : IRewirer
    {
        void RegisterCommands(IDebugConsole console);
    }
}
