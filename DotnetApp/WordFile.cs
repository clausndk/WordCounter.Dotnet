public class WordFile : IWordFile
{
    public string PathAndFilename { get; }
    public string Filename { get; }
    public string Content { get; }

    public WordFile(string pathAndFilename)
    {
        PathAndFilename = pathAndFilename;
        Filename = Path.GetFileName(PathAndFilename);
        Content = File.ReadAllText(pathAndFilename)
            .ReplaceLineEndings(" ");
    }
}