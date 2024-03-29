namespace DotnetApp;

public class WordFilesSourceFactory : IWordFilesSourceFactory
{
    private const string ExcludeFileName = "exclude.txt";
    private readonly IReadOnlyList<string> _sourceFiles;

    public WordFilesSourceFactory(IReadOnlyList<string> sourceFiles)
    {
        _sourceFiles = sourceFiles;
    }

    public IReadOnlyList<IWordFile> GetWordSources()
    {
        return _sourceFiles
            .Where(f => !Path.GetFileName(f).Equals(ExcludeFileName))
            .Select(f => new WordFile(f))
            .ToList();
    }

    public IWordFile GetExcludeFile()
    {
        return _sourceFiles
            .Where(f => Path.GetFileName(f).Equals(ExcludeFileName))
            .Select(f => new WordFile(f))
            .FirstOrDefault();
    }
}
