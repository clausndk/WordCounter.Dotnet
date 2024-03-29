using Moq;

namespace DotnetApp.Tests;

public class WordCountResultTests
{
    [Theory, AutoMoqData]
    public void CanInstantiate(WordCountResult sut)
    {
        Assert.NotNull(sut);
    }

    [Theory, AutoMoqData]
    public void Count_Word_OnlyOneExists(
        Mock<IWord> word,
        WordCountResult sut)
    {
        sut.Count(word.Object);

        var elementInList = Assert.Single(sut.Words);
        Assert.Equal(word.Object, elementInList);
        word.Verify(x => x.IncrementCount(), Times.Never);
    }

    [Theory, AutoMoqData]
    public void Count_Word_IncrementForSameWord(
        Mock<IWord> word,
        WordCountResult sut)
    {
        sut.Count(word.Object);
        sut.Count(word.Object);

        var elementInList = Assert.Single(sut.Words);
        Assert.Equal(word.Object, elementInList);
        word.Verify(x => x.IncrementCount(), Times.Once);
    }

    [Theory, AutoMoqData]
    public void Count_Word_LetterIndexPopulated(
        Mock<IWord> word,
        string actualWord,
        WordCountResult sut)
    {
        var letterIndex = actualWord.ToLowerInvariant()[..1];
        word.Setup(x => x.LetterIndex).Returns(letterIndex);
        sut.Count(word.Object);

        var elementInList = Assert.Single(sut.Words);
        Assert.Equal(word.Object, elementInList);
        word.Verify(x => x.IncrementCount(), Times.Never);
        var letterIndexEntry = Assert.Single(sut.LetterIndexedWords);
        Assert.Equal(letterIndex, letterIndexEntry.Key);
    }
}
