public interface IWord
{
    public int Count { get; }
    public string LetterIndex { get; }
    public void IncrementCount();
}