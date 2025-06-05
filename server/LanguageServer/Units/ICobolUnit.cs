using Antlr4.Runtime;

public interface ICobolUnit
{
    string Uri { get; }
    ParserRuleContext? Tree { get; }
}