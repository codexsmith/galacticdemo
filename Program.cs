using System;
using System.Collections.Generic;
using System.Linq;

//began at 2:45 PM
//dotnet 5.0 console project
//github 
//starting with https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-query-the-contents-of-files-in-a-folder-lin

namespace GalacticDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            


            //add command prompt
            //add GUI folder selection
            string startFolder = @"C:\Users\nsc31\Desktop\DummyData";

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            
            string searchTerm = @"RLS";

            // Search the contents of each file.  
            // A regular expression created with the RegEx class  
            // could be used instead of the Contains method.  
            // queryMatchingFiles is an IEnumerable<string>.  
            var queryMatchingFiles =
                from file in fileList
                where file.Extension == ".pdf"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains(searchTerm)
                select file.FullName;

            // Execute the query.  
            Console.WriteLine("The term \"{0}\" was found in:", searchTerm);
            foreach (string filename in queryMatchingFiles)
            {
                Console.WriteLine(filename);
            }

            // Keep the console window open in debug mode.  
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        // Read the contents of the file.  
        static string GetFileText(string name)
        {
            string fileContents = String.Empty;

            // If the file has been deleted since we took
            // the snapshot, ignore it and return the empty string.  
            if (System.IO.File.Exists(name))
            {
                fileContents = System.IO.File.ReadAllText(name);
            }
            return fileContents;
        }
    }
}
