﻿using System;
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


        static void WriteFile(string input)
        {
            StreamWriter newFile = new StreamWriter(@"C:\Users\ellio\Documents\programming\program-data\output.txt");
            newFile.Write(input);
            newFile.Close();
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
            int rot = 31; 
            List<Int32> charVal = new List<Int32>();
            try
            {
                //pass the file path and file name to the StreamReader constructor
                Console.WriteLine("enter the file path of the file you'd like to encrypt");
                string path = Console.ReadLine();
                StreamReader sr = new StreamReader(@path); //@path seems to have parsed the input as a system.io.path value
                inStream = ReadFile(sr); //generate input stream of raw data from filepath (with newlines)
           
   
                //encrypt
                charVal = MakeCharVals(inStream); //turns the input stream into a list of decimals converted from it's chars
                outStream = Encrypt(charVal, rot);
                WriteFile(outStream);

                Console.WriteLine("input: "+ inStream + "\noutput: " + outStream);

                //decrypt 
                inStream = outStream;
                charVal = MakeCharVals(outStream);
                outStream = Encrypt(charVal, -rot);

                Console.WriteLine("input: " + inStream + "\noutput: " + outStream);

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nerror: " + e.Message);
            }
            finally
            {
                Console.WriteLine("finshed reading");
            }
        }
    }
}