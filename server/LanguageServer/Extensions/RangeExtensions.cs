using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace LanguageServer.Extensions
{
    public static class RangeExtensions
    {
        public static bool Contains(this Microsoft.VisualStudio.LanguageServer.Protocol.Range range, Position position)
        {
            if (position.Line < range.Start.Line || position.Line > range.End.Line)
                return false;

            if (position.Line == range.Start.Line && position.Character < range.Start.Character)
                return false;

            if (position.Line == range.End.Line && position.Character > range.End.Character)
                return false;

            return true;
        }
    }
}