
namespace CobolTranspiler;

public class SymbolTable
{
    private static readonly Lazy<SymbolTable> _instance = new Lazy<SymbolTable>(() => new SymbolTable());

    public readonly Dictionary<string, CobolDataVariable> dataNodes = new();

    private SymbolTable() { }

    public static SymbolTable Instance => _instance.Value;

    public bool AddDataNode(CobolDataVariable node)
    {
        if (dataNodes.TryAdd(node.Name, node))
        {
            dataNodes[node.Name] = node;
            return true;
        }

        return false;

    }

    public void AddQualifiedDataNode(CobolDataVariable node, string qualifedName)
    {
        if (dataNodes.TryAdd(qualifedName, node))
        {
            dataNodes[qualifedName] = node;

        }

    }
    
    public CobolDataVariable GetDataNode(string name)
    {
        return dataNodes.TryGetValue(name, out var node) ? node : null;
    }

    public object GetValue(string name)
    {
        var node = GetDataNode(name);
        return node?.Value;
    }

    public void SetValue(string name, object value)
    {
        var node = GetDataNode(name);
        if (node != null)
        {
            node.Value = value;
        }
    }

}