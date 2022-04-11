using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;

//began at 2:45 PM
//dotnet 5.0 console project
//github 
//test data
//starting with https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-query-the-contents-of-files-in-a-folder-lin
//read into scalability concerns & wildcard possibilities of this impl
//3:15 wanted to avoid regex for performance reasons, but that won't be possible so...
// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/how-to-combine-linq-queries-with-regular-expressions 
//4:15 added classes & Regex for SSN & CCN 
//Assumptions - have admin file/folder access. searching local folders/files, not over the network.
//4:30 refactoring query & file loops. instead of per query per file, run per file per query using LINQ
//5:00 LINQ/Regex issue getting the matched text back
//5:15 commenting code, cleaning up
//have not tested on 100k files, even so my first performance improvement would be to limit/batch the folders returned by getDirectories

namespace GalacticDemo
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //add a GUI folder selection
            string startFolder = @"C:\Users\nsc31\Desktop\DummyData";
            IEnumerable<System.IO.FileInfo> fileList = getDirectories(startFolder);
            
            var queryTerms = new List<ISupportedQuery>() { new SocialSecurityNumber(), new CreditCardNumber() };
            var regexTerms = new List<System.Text.RegularExpressions.Regex>();
            foreach (var query in queryTerms)
            {
                // Create the regular expression
                regexTerms.Add(new System.Text.RegularExpressions.Regex(query.getQueryTerm()));
            }
            //add a GUI to select queries
            
            // Search the contents of each file  
            var regexQueryMatchingFiles =
                from file in fileList //for each file
                let fileText = System.IO.File.ReadAllText(file.FullName)//get file text
                from term in regexTerms//for each regex
                let matches = term.Matches(fileText)//get matches
                where matches.Count > 0
                let matchedValue = from match in matches select match.Value//issue here getting the regex matched Value
                select new { name = file.FullName, value = matchedValue};

                foreach (var query in queryTerms)
                {
                    Console.WriteLine("Searched for \"{0}\"", query.getName());
                }

            if (regexQueryMatchingFiles.Any())
            {
                foreach (var matchedfile in regexQueryMatchingFiles)
                {
                    Console.WriteLine("The file \"{0}\" contained, \"{1}\". ", matchedfile.name, matchedfile.value.Count());//so here we show the number of matches instead
                }
            }
            else
            {
                Console.WriteLine("No Matches Found");
            }            

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            // write to report file
            
        }

        static IEnumerable<System.IO.FileInfo> getDirectories(string path)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);

            //will need to get all files for now
            //add file type select GUI
            //inspect & warn users of folder/file perm issues
            //System.IO.SearchOption.AllDirectories gets all sub dirs as well
            return dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
        }

        //expand this method for handling specialized file types
        //expand this for ignoring files/types
        //this impl leans heavily on .NET's built-in file reading, which is effective but naive
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
