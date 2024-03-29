namespace DotnetApp.Tests;

public class WordTests
{
    [Fact]
    public void Cunstructor_RequriesNonNullWord_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Word(null!));
    }

    [Fact]
    public void Cunstructor_RequriesNonEmptyWord_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Word(string.Empty));
    }

    [Fact]
    public void Cunstructor_RequriesNonEmptySpaceWord_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Word(" "));
    }

    [Theory, AutoMoqData]
    public void ToString_ReturnsWord(string wordToUse)
    {
        var sut = new Word(wordToUse);
        Assert.Equal(wordToUse, sut.ToString());
    }

    [Theory, AutoMoqData]
    public void GetHashCode_ReturnsWordLowercasedsHashCode(string wordToUse)
    {
        var sut = new Word(wordToUse);
        Assert.Equal(wordToUse.ToLowerInvariant().GetHashCode(), sut.GetHashCode());
    }

    [Theory, AutoMoqData]
    public void Increment_IncrementsCount_ReturnsOne(string wordToUse)
    {
        var sut = new Word(wordToUse);
        Assert.Equal(1, sut.Count);
    }
}
