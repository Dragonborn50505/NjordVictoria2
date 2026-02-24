using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        string sourceFolder = @"E:\SteamLibrary\steamapps\common\Victoria 2\mod\NjordVictoria2\history\provinces";
        string outputFolder = @"C:\Users\isake\Pictures\megacampaign\provincesmodding2";

        // Stores modified file content in memory
        Dictionary<string, string> modifiedFiles = new Dictionary<string, string>();

        Console.WriteLine("=== Victoria 2 Province Tag Editor ===");
        Console.WriteLine();

        string continueEditing = "";

        while (continueEditing == "")
        {
            Console.WriteLine("Choose tag to change FROM:");
            string tagFrom = Console.ReadLine();

            Console.WriteLine("Choose tag to change TO:");
            string tagTo = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("Enter province ID (write 0 to finish this tag swap):");
                string input = Console.ReadLine();

                if (input == "0")
                    break;

                string prefix1 = input + " -";
                string prefix2 = input + "-";

                string[] matches = Directory.GetFiles(
                    sourceFolder,
                    prefix1 + "*.txt",
                    SearchOption.AllDirectories);

                if (matches.Length == 0)
                {
                    matches = Directory.GetFiles(
                        sourceFolder,
                        prefix2 + "*.txt",
                        SearchOption.AllDirectories);
                }

                if (matches.Length == 0)
                {
                    Console.WriteLine("No province file found for ID: " + input);
                    continue;
                }

                string filePath = matches[0];
                string filename = Path.GetFileName(filePath);

                string originalText;

                // If already modified in memory, use that version
                if (modifiedFiles.ContainsKey(filePath))
                {
                    originalText = modifiedFiles[filePath];
                }
                else
                {
                    originalText = File.ReadAllText(filePath);
                }

                string modifiedText = originalText.Replace(tagFrom, tagTo);

                modifiedFiles[filePath] = modifiedText;

                // Optional backup output folder
                Directory.CreateDirectory(outputFolder);
                string outputPath = Path.Combine(outputFolder, filename);
                File.WriteAllText(outputPath, modifiedText);

                Console.WriteLine("Queued modification for: " + filename);
            }

            Console.WriteLine("Press ENTER to do another tag swap, or type anything to finish:");
            continueEditing = Console.ReadLine();
        }

        Console.WriteLine();
        Console.WriteLine("=========================================");
        Console.WriteLine("All modifications queued in memory.");
        Console.WriteLine("Close Victoria 2 now, then press ENTER to apply changes.");
        Console.ReadLine();

        foreach (var entry in modifiedFiles)
        {
            try
            {
                File.SetAttributes(entry.Key, FileAttributes.Normal);
                File.WriteAllText(entry.Key, entry.Value);
                Console.WriteLine("Overwritten: " + entry.Key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to overwrite: " + entry.Key);
                Console.WriteLine(ex.Message);
            }
        }

        Console.WriteLine();
        Console.WriteLine("All files saved successfully.");
        Console.WriteLine("Press ENTER to exit.");
        Console.ReadLine();
    }
}