using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace file_reading_test
{
    class Program
    {


        static string Encrypt(List<int> charVal, int rot) //type returned specified in keyword after static
        {
            Console.WriteLine("encrypting...");
            string outStream = "";
            for (var i = 0; i < charVal.Count; i++)
            {
                charVal[i] += rot; //does not offset newlines (code: 10) or carriage returns (code: 13)
                outStream += Convert.ToString(Convert.ToChar(charVal[i]));
            }

            return outStream;
        }


        static List<int> MakeCharVals(string inStream)
        {
            Console.WriteLine("populating list...");
            string pattern = @"\r|\n|."; //carriage return OR newline OR any character
            List<int> charVal = new List<int>();
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(inStream); //finds first match
         
            while (match.Success) //generates a list of all characters in the input stream
            {
                charVal.Add(Char.Parse(match.Value)); //match gets implicity converted to an int and stored in the list 
                match = match.NextMatch(); //finds next match
            }
            return charVal;
        }


        static void WriteFile(string input, string path)
        {
            StreamWriter newFile = new StreamWriter(@path);
            newFile.Write(input);
            newFile.Close();
            Console.WriteLine("now encrypted at {0}", path);
        }


        static string ReadFile(StreamReader sr)
        {
            string line;
            string inStream;
            line = inStream = sr.ReadLine(); //read the first line of text
            inStream += "\n"; //adds newline char
            int i = 1;
            while (line != null) //continue reading until file end
            {
                //read the next line
                Console.Write("\rread {0} lines", i); //writing carriage return first allows the line to be rewritten 
                line = sr.ReadLine();
                if (line != null) { inStream += line + "\n"; }
                i++;
            }
            inStream = inStream.Trim(new Char[] { '\n' }); //gets rid of the single excess newline to preserve the exact original data
                                                           //close the file
            sr.Close();
            return inStream;
        }


        static void Main(string[] args)
        {
            string inStream;
            string outStream;
            int rot; 
            List<Int32> charVal = new List<Int32>();
            try
            {
                //pass the file path and file name to the StreamReader constructor
                Console.WriteLine("this progam will overwrite the file you select with the encrypted text file\nto decrypt the file, just enter the negative of the initial rot (remember it - else you'll have to use trial and error!)\nenter the file path of the file you'd like to encrypt or drag and drop file into this window");
                string path = Console.ReadLine();
                StreamReader sr = new StreamReader(@path); //@path seems to have parsed the input as a system.io.path value
                inStream = ReadFile(sr); //generate input stream of raw data from filepath (with newlines)

                Console.WriteLine("\nenter the rot value you'd like to encrypt with");
                rot = Int16.Parse(Console.ReadLine());

                //encrypt
                charVal = MakeCharVals(inStream); //turns the input stream into a list of decimals converted from it's char 
                outStream = Encrypt(charVal, rot);

                Console.WriteLine("input:{0}\noutput(rot {1}):{2}", inStream, rot, outStream);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("overwrite file with encrypted version? (Y/N)");
                Console.ResetColor();
                string response = Console.ReadLine();
                Console.Clear();

                if (response.ToLower() == "y") { WriteFile(outStream, path); } else { Console.WriteLine("no file overwritten"); }
                

                //WriteFile(outStream, path);

                //decrypt 
                //inStream = outStream;
                //charVal = MakeCharVals(outStream);
                //outStream = Encrypt(charVal, -rot);

                //Console.WriteLine("input:{0}\noutput(rot -{1}):{2}", inStream, rot, outStream);

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nerror: " + e.Message);
                Console.ReadLine();
            }
            finally
            {
                Console.WriteLine("exiting...");
            }
        }
    }
}
