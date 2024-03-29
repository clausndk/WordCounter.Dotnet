using AutoFixture.Xunit2;
using Moq;

namespace DotnetApp.Tests;

public class WordCounterTests
{
    [Theory]
    [AutoMoqData]
    public void CanInstantiate(WordCounter sut)
    {
        Assert.NotNull(sut);
    }

    [Theory]
    [AutoMoqData]
    public void CountWordsInFiles_NoInput_EmptyResult(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        WordCounter sut)
    {
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns((IWordFile)null!);

        var result = sut.CountWords();

        Assert.Empty(result.Words);
        Assert.Equal(0, result.TotalWords);
    }

    [Theory]
    [AutoMoqData]
    public void CountWordsInFiles_OneWordContent_OneWordResult(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        Mock<IWordFile> file,
        WordCounter sut)
    {
        file.Setup(f => f.Content).Returns("Test");
        file.Setup(f => f.Filename).Returns("something.txt");
        var wordList = new List<IWordFile> { file.Object };
        wordsSourceFactory.Setup(x => x.GetWordSources()).Returns(wordList);
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns((IWordFile)null!);

        var result = sut.CountWords();

        Assert.Single(result.Words);
        Assert.Equal(1, result.Words.First().Count);
    }

    [Theory]
    [AutoMoqData]
    public void CountWordsInFiles_TwoWordContent_TwoWordResult(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        Mock<IWordFile> file,
        WordCounter sut)
    {
        file.Setup(f => f.Content).Returns("Test peach");
        file.Setup(f => f.Filename).Returns("something.txt");
        var wordList = new List<IWordFile> { file.Object };
        wordsSourceFactory.Setup(x => x.GetWordSources()).Returns(wordList);
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns((IWordFile)null!);

        var result = sut.CountWords();

        Assert.Equal(2, result.TotalWords);
        Assert.Equal(1, result.Words.First().Count);
        Assert.Equal(1, result.Words.Last().Count);
    }

    [Theory]
    [AutoMoqData]
    public void CountWordsInFiles_FourWordContent_FourWordResultWithTwoCount(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        Mock<IWordFile> file,
        WordCounter sut)
    {
        file.Setup(f => f.Content).Returns("Test peach test Peach");
        file.Setup(f => f.Filename).Returns("something.txt");
        var wordList = new List<IWordFile> { file.Object };
        wordsSourceFactory.Setup(x => x.GetWordSources()).Returns(wordList);
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns((IWordFile)null!);

        var result = sut.CountWords();

        Assert.Equal(4, result.TotalWords);
        Assert.Equal(2, result.Words.First().Count);
        Assert.Equal(2, result.Words.Last().Count);
    }

    [Theory]
    [AutoMoqData]
    public void CountWordsInFiles_FourWordContentInTwoFiles_EightWordResultWithFourCount(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        Mock<IWordFile> file1,
        Mock<IWordFile> file2,
        WordCounter sut)
    {
        file1.Setup(f => f.Content).Returns("Test peach test Peach");
        file1.Setup(f => f.Filename).Returns("something.txt");
        file2.Setup(f => f.Content).Returns("Test peach test Peach");
        file2.Setup(f => f.Filename).Returns("somethingelse.txt");
        var wordList = new List<IWordFile> { file1.Object, file2.Object };
        wordsSourceFactory.Setup(x => x.GetWordSources()).Returns(wordList);
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns((IWordFile)null!);

        var result = sut.CountWords();

        Assert.Equal(8, result.TotalWords);
        Assert.Equal(4, result.Words.First().Count);
        Assert.Equal(4, result.Words.Last().Count);
    }

    [Theory]
    [AutoMoqData]
    public void CountWordsInFiles_LongContent_BigResultWith100Count(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        Mock<IWordFile> file,
        WordCounter sut)
    {
        file.Setup(f => f.Content).Returns(LongText);
        file.Setup(f => f.Filename).Returns("something.txt");
        var wordList = new List<IWordFile> { file.Object };
        wordsSourceFactory.Setup(x => x.GetWordSources()).Returns(wordList);
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns((IWordFile)null!);

        var result = sut.CountWords();

        Assert.Equal(100, result.TotalWords);
        Assert.All(result.Words, entry => Assert.Equal(1, entry.Count));
    }

    [Theory]
    [AutoMoqData]
    public void CountWordsInFiles_FourWordContentWithExclutions_TwoWordResultWithTwoCount(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        Mock<IWordFile> file,
        Mock<IWordFile> exclutionFile,
        WordCounter sut)
    {
        file.Setup(f => f.Content).Returns("Test peach test Peach");
        file.Setup(f => f.Filename).Returns("something.txt");
        exclutionFile.Setup(f => f.Content).Returns("Test");
        exclutionFile.Setup(f => f.Filename).Returns("exclude.txt");
        var wordList = new List<IWordFile> { file.Object };
        wordsSourceFactory.Setup(x => x.GetWordSources()).Returns(wordList);
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns(exclutionFile.Object);

        var result = sut.CountWords();

        Assert.Equal(2, result.TotalWords);
        Assert.Equal(2, result.TotalWordsSkipped);
        Assert.Equal(2, result.Words.First().Count);
        Assert.Equal(2, result.Words.Last().Count);
    }

    [Theory]
    [AutoMoqData]
    public void CountWords_FourWordContentWithExclutions_LetterIndexWords(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        Mock<IWordFile> file,
        WordCounter sut)
    {
        file.Setup(f => f.Content).Returns("Test peach test Peach");
        file.Setup(f => f.Filename).Returns("something.txt");
        var wordList = new List<IWordFile> { file.Object };
        wordsSourceFactory.Setup(x => x.GetWordSources()).Returns(wordList);
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns((IWordFile)null!);

        var result = sut.CountWords();

        Assert.Equal(4, result.TotalWords);
        Assert.Equal(0, result.TotalWordsSkipped);
        Assert.Equal(2, result.Words.First().Count);
        Assert.Equal(2, result.Words.Last().Count);
        Assert.Equal(2, result.LetterIndexedWords.Count);
        Assert.All(result.LetterIndexedWords, entry => Assert.Single(entry.Value));
    }

    [Theory]
    [AutoMoqData]
    public void CountWords_FourWordContentWithExclutions_LetterIndexWords2(
        [Frozen] Mock<IWordFilesSourceFactory> wordsSourceFactory,
        Mock<IWordFile> file,
        WordCounter sut)
    {
        file.Setup(f => f.Content).Returns("Test peach test Peach tempo Tempo Hello hello");
        file.Setup(f => f.Filename).Returns("something.txt");
        var wordList = new List<IWordFile> { file.Object };
        wordsSourceFactory.Setup(x => x.GetWordSources()).Returns(wordList);
        wordsSourceFactory.Setup(x => x.GetExcludeFile()).Returns((IWordFile)null!);

        var result = sut.CountWords();

        Assert.Equal(8, result.TotalWords);
        Assert.Equal(0, result.TotalWordsSkipped);
        Assert.Equal(2, result.Words.First().Count);
        Assert.Equal(2, result.Words.Last().Count);
        Assert.Equal(3, result.LetterIndexedWords.Count);
        Assert.Single(result.LetterIndexedWords["p"]);
        Assert.Single(result.LetterIndexedWords["h"]);
        Assert.Equal(2, result.LetterIndexedWords["t"].Count);
    }

    private const string LongText = @"tree g knight measurements swimming museum ga valued takes signatures adapter deadly printable fuel disco relevance friends garage medium consensus runner refused te zero cakes energy meals electrical notebooks than religions hebrew cingular dinner laundry tips leaving impossible cliff bit criminal letting cardiac middle land across five reply sweet planning seriously rep decision socket lee contributions society senate civilian clusters throwing attempts greensboro delivery stretch rom radical eh auditor finest cage falls canada sarah foundations sign bird generator holmes difficulties ii declared thick sealed convertible achievement discover bryan building horny front essay undergraduate totally modular injection spread war swingers relocation";
}