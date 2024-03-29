public interface IWordFilesSourceFactory
{
    IReadOnlyList<IWordFile> GetWordSources();
    IWordFile GetExcludeFile();
}