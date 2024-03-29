using DotnetApp;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to a Word counter.");
        Console.WriteLine("A source directory must be supplied, where '.txt' files must be located. These test files");
        Console.WriteLine("must include minimum 200 words of content, which this program will count. Good luck.");

        if (args == null || args.Length == 0)
        {
            Console.WriteLine("No source directory supplied, exiting...");
            return;
        }

        var sourceDirectory = args[0];
        if (!Path.Exists(sourceDirectory))
        {
            Console.WriteLine($"Source directory '{sourceDirectory}' does not exist, exiting...");
        }
        Console.WriteLine($"Source directory supplied: '{sourceDirectory}'.");

        var outputFolder = Path.Combine(sourceDirectory, "Output");
        if (Path.Exists(outputFolder))
        {
            Console.WriteLine("Cleaning output folder...");
            Directory.Delete(outputFolder, true);
            Console.WriteLine("Output folder cleaned.");
        }
        Directory.CreateDirectory(outputFolder);
        Console.WriteLine("Outputfolder created.");
        Console.WriteLine();

        Console.WriteLine("Counting words...");

        var wordFiles = Directory.GetFiles(sourceDirectory, "*.txt");
        var wordFileSourcesFactory = new WordFilesSourceFactory(wordFiles);
        var wordCounter = new WordCounter(wordFileSourcesFactory);
        var wordCountResult = wordCounter.CountWords();
        Console.WriteLine("Word count is:");
        foreach (var word in wordCountResult.Words)
        {
            Console.WriteLine($"  '{word}': {word.Count}");
        }
        Console.WriteLine($"Total words: {wordCountResult.TotalWords}");
        Console.WriteLine($"Total words skipped: {wordCountResult.TotalWordsSkipped}");

        var excludedCountResultFile = Path.Combine(outputFolder, "ExcludedCount.txt");
        Console.WriteLine($"Writing number of excluded words to file: {excludedCountResultFile}");
        File.WriteAllText(excludedCountResultFile, $"Excluded word count: {wordCountResult.TotalWordsSkipped}");

        Console.WriteLine();
        Console.WriteLine("Writing letter index files to output folder...");
        foreach (var letterIndexWord in wordCountResult.LetterIndexedWords)
        {
            var letterIndexWordFile = Path.Combine(outputFolder, $"File_{letterIndexWord.Key}.txt");
            var letterIndexWordContent = string.Join(Environment.NewLine, letterIndexWord.Value.Select(x => $"{x} {x.Count}"));
            File.WriteAllText(letterIndexWordFile, letterIndexWordContent);
            Console.WriteLine($"  Wrote file {letterIndexWordFile}");
        }
        Console.WriteLine("Done writing letter files.");

        Console.WriteLine();
        Console.WriteLine("Have a nice day!");
    }
}
