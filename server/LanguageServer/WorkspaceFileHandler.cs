
public class WorkspaceFileHandler
{
    private readonly Dictionary<string, List<ICobolUnit>> _workspaceFiles = new();

    private readonly Dictionary<string, string> _includeFiles = new();

    public T? GetUnit<T>(string uri) where T : class, ICobolUnit
    {
        if (_workspaceFiles.TryGetValue(uri, out var units))
        {
            return units.OfType<T>().FirstOrDefault();
        }
        return null;
    }

    public void AddUnits(string uri, List<ICobolUnit> units) 
    {
        _workspaceFiles.Add(uri, units);

    }

    public void RemoveDocument(string uri)
    {
        if (_workspaceFiles.ContainsKey(uri))
        {
            _workspaceFiles.Remove(uri);
        }
    }



    public void AddIncludeFile(string copybookName, string uri)
    {
        if (!_includeFiles.ContainsKey(copybookName))
        {
            _includeFiles[copybookName] = uri;
        }
    }
    public string? GetIncludeFileUri(string copybookName)
    {
        if (_includeFiles.TryGetValue(copybookName.ToUpper(), out var uri))
        {
            return uri;
        }
        return null;
    }
    public void RemoveIncludeFile(string uri)
    {
        if (_includeFiles.ContainsKey(uri))
        {
            _includeFiles.Remove(uri);
        }
    }

    public string[] GetFileAllLines(string uri)
    {
        return File.ReadAllLines(uri);
    }
}
    