
using System.Text.RegularExpressions;

public class Word : IWord
{
    private static readonly Regex _trimmer = new Regex(@"[^\u0000-\u007F]+", RegexOptions.Compiled);
    private readonly string _word;
    private readonly string _wordLowercased;
    private int _count;

    public int Count => _count;

    public string LetterIndex => _wordLowercased[..1];

    public Word(string word)
    {
        if (string.IsNullOrEmpty(word))
            throw new ArgumentNullException("word");
        _word = word.Replace(",", string.Empty).Replace(".", string.Empty).Replace(" ", string.Empty);
        if (string.IsNullOrEmpty(_word))
            throw new ArgumentNullException("word");
        _wordLowercased = _word.ToLowerInvariant();
        _count = 1;
    }

    public override string ToString()
    {
        return _word;
    }

    public override int GetHashCode()
    {
        return _wordLowercased.GetHashCode();
    }

    public void IncrementCount()
    {
        _count++;
    }
}