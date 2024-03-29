public class WordComparer : IEqualityComparer<IWord>
{
    public bool Equals(IWord? x, IWord? y)
    {
        if (x == null && y == null) return true;
        if (x == null) return false;
        if (y == null) return false;
        return x.GetHashCode() == y.GetHashCode();
    }

    public int GetHashCode(IWord obj)
    {
        return obj.GetHashCode();
    }
}
