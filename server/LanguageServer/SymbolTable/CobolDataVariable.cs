namespace CobolTranspiler;

public class CobolDataVariable
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string Type { get; set; } // Could be 'X' for alphanumeric, '9' for numeric, etc.
    public int Length { get; set; } // Based on PIC clause
    public bool IsArray { get; set; }
    public int OccursCount { get; set; } // If OCCURS is present
    public string Usage { get; set; } // USAGE clause
    public CobolDataVariable Parent { get;  set; } // Reference to the parent node

    public CobolDataVariable RedefinedVar {get; set; }

    public List<CobolDataVariable> Children { get; set; } = new List<CobolDataVariable>();
    public object Value { get; set; }

    public void AddChild(CobolDataVariable child)
    {
        child.Parent = this;
        Children.Add(child);
    }
}


public interface ICobolNode
{
    string Name { get; }
    void Execute();
}

public class CobolDataNode : ICobolNode
{
    public string Name { get; set; }
    public List<CobolDataVariable> Variables { get; set; } = new List<CobolDataVariable>();

    public void Execute() { /* Execution logic for data division nodes */ }
}

public class CobolProcedureNode : ICobolNode
{
    public string Name { get; set; }
    public List<string> Instructions { get; set; } = new List<string>();

    public void Execute() { /* Execution logic for procedure division nodes */ }
}