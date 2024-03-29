using System.Collections;

namespace DotnetApp;

public class WordCountResult
{
    private readonly HashSet<IWord> _words;
    private readonly HashSet<IWord> _wordsToSkip;
    private int _totalWords;
    private int _totalWordsSkipped;

    public int TotalWords => _totalWords;
    public int TotalWordsSkipped => _totalWordsSkipped;
    public IReadOnlyCollection<IWord> Words => _words;
    public IReadOnlyCollection<IWord> WordsSkipped => _wordsToSkip;
    public IReadOnlyDictionary<string, IReadOnlyList<IWord>> LetterIndexedWords =>
        _words.GroupBy(x => x.LetterIndex)
            .ToDictionary(x => x.Key, x => (IReadOnlyList<IWord>)x.OrderBy(x => x.ToString()).ToList());

    public WordCountResult(HashSet<IWord> wordsToSkip)
    {
        _words = new HashSet<IWord>(new WordComparer());
        _wordsToSkip = wordsToSkip;
    }

    public void Count(IWord word)
    {
        if (_wordsToSkip.TryGetValue(word, out var existingWordToSkip))
        {
            existingWordToSkip.IncrementCount();
            _totalWordsSkipped++;
            return;
        }

        if (_words.TryGetValue(word, out var existingWord))
        {
            existingWord.IncrementCount();
        }
        else
        {
            _words.Add(word);
        }
        _totalWords++;
    }
}
