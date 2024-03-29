using DotnetApp;

public class WordCounter
{
    private readonly IWordFilesSourceFactory _wordFilesSourceFactory;

    public WordCounter(IWordFilesSourceFactory wordFilesSourceFactory)
    {
        _wordFilesSourceFactory = wordFilesSourceFactory;
    }

    public WordCountResult CountWords()
    {
        var wordSources = _wordFilesSourceFactory.GetWordSources();
        var excludeFile = _wordFilesSourceFactory.GetExcludeFile();
        var wordsToSkip = new HashSet<IWord>(new WordComparer());
        if (excludeFile != null)
        {
            wordsToSkip = excludeFile.Content
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(w => new Word(w) as IWord)
                .ToHashSet(new WordComparer());
        }

        var result = new WordCountResult(wordsToSkip);

        foreach (var wordSource in wordSources)
        {
            var words = wordSource.Content.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                var w = new Word(word);
                result.Count(w);
            }
        }

        return result;
    }
}
