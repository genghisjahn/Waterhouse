using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Net;
namespace Waterhouse2
{
    public class Utilities
    {
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);

        [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);

        private const int VK_CAPITAL = 0x14;
        private const int VK_NUMLOCK = 0x90;
        private const int VK_SCROLL = 0x91;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;

        private const int KEYEVENTF_KEYUP = 0x2;
        private string _Text;
        private string _File;
        private int _DashLength = 600;  //Length of  dash.
        //Dash /3
        private int _DotLength = 200;   //Length of a dot.
        //Dash /3
        private int _InTRALetter = 200;  //Space between parts of a letter.
        //Dash
        private int _InTERLetter = 600;  //Space between letters in a word.
        //Dot * 7
        private int _InTERWord = 1400;  //Space between words.
        private enum eDataType
        {
            StringText = 1,
            LocalFile = 2,
            UNCFilePath = 3,
            HTTPWEB = 4
        }
        private eDataType _DataType = eDataType.LocalFile;
        public Utilities(string[] args)
        {
            bool tempcmdOK = false;
            bool tempTextOK = false;
            TurnNumLockOff();
            string[] tempcmdLine = args;
            tempcmdOK = ProcessCommandLine(tempcmdLine);
            if (!tempcmdOK)
                System.Environment.Exit(0);
            try
            {
                tempTextOK = SetText();
                if (tempTextOK)
                {
                    ProcessMorseCode();
                }
                else
                {
                    _File = "";
                    throw new Exception("Error processing text.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error processing file {0}. {1}", _File.ToUpper(), ex.Message.ToString()));
                Console.ReadLine();
            }
        }
        private void ToggleLight()
        {
            keybd_event(VK_NUMLOCK, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
            keybd_event(VK_NUMLOCK, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        private void ProcessMorseCode()
        {
            string tempCode = "";
            foreach (char c in _Text)
            {
                if (Morse.GetMorse(c) != "!")
                {
                    if (c == ' ')
                    {
                        Thread.Sleep(_InTERWord);
                    }
                    else
                    {
                        tempCode = Morse.GetMorse(c);
                        bool tempFirst = true;
                        foreach (char p in tempCode)
                        {
                            if (!tempFirst)
                            {
                                Thread.Sleep(_InTRALetter);
                            }
                            ToggleLight();
                            switch (p)
                            {
                                case '.':
                                    Thread.Sleep(_DotLength);
                                    break;
                                case '_':
                                    Thread.Sleep(_DashLength);
                                    break;
                            }
                            ToggleLight();
                            tempFirst = false;
                        }
                        tempFirst = false;
                        Thread.Sleep(_InTERLetter);
                    }
                }
            }
        }
        private void ShowHelp()
        {
            List<string> tempHelp = new List<string>();
            {
                tempHelp.Add("Waterhouse.exe Help".ToUpper());
                tempHelp.Add("-------------------");
                tempHelp.Add("This application takes various text inputs and");
                tempHelp.Add("shows them in Morse Code by toggling the NumLock key.");
                tempHelp.Add("Parameters switches:");
                tempHelp.Add("");
                tempHelp.Add("/T:\"SOME TEXT DATA\".");
                tempHelp.Add("Use this if you just want to pass some text.");
                tempHelp.Add("Must be surrounded by \".");
                tempHelp.Add("");
                tempHelp.Add("/F:<FilePath>  This can be local file path, a UNC path");
                tempHelp.Add("or a web resource provided you include the HTTP:// prefix.");
                tempHelp.Add("");
                tempHelp.Add("/I:<Interval Integer Default=600 ms>");
                tempHelp.Add("     The DASH interval measured in milliseconds");
                tempHelp.Add("     that will be used to produce the code.");
                tempHelp.Add("     DASH length will be equal to the value given for /I:");
                tempHelp.Add("     DOT length will be equal to 1/3 the DASH length.");
                tempHelp.Add("     Time between parts of the same letter is equal 1 DOT length.");
                tempHelp.Add("     Time between letters is equal to 3 DOT lengths.");
                tempHelp.Add("     Time between two words is equal to 7 DOT lengths.");
                tempHelp.Add("     Non alpha-numeric characters are skipped.");
                tempHelp.Add("");
                tempHelp.Add("---------------------------------------");
                tempHelp.Add("-----------End of Line-----------------");
            }
            foreach (string h in tempHelp)
            {
                Console.WriteLine(h);
            }
            Console.ReadKey();
        }
        private bool ProcessCommandLine(string[] paramCMD)
        {
            bool tempResult = true;

            try
            {
                foreach (string c in paramCMD)
                {
                    string tempPrefix = null;
                    if (c == @"/?" | c == "/HELP")
                    {
                        tempResult = false;
                        ShowHelp();
                    }
                    else
                    {
                        tempPrefix = c.Substring(0, 3);
                        switch (tempPrefix.ToUpper())
                        {
                            case "/F:":
                                _File = c.Substring(3);
                                if (_File.StartsWith("@http://"))
                                {
                                    _DataType = eDataType.HTTPWEB;
                                }
                                if (_File.StartsWith(@"\\"))
                                {
                                    _DataType = eDataType.UNCFilePath;
                                }
                                break;
                            case "/T:":
                                _DataType = eDataType.StringText;
                                _Text = c.Substring(3);
                                if (!(_Text.StartsWith("") & _Text.EndsWith("")))
                                {
                                    Console.WriteLine(@"/T: must start and end with \"".  Example \""Sample Text\"".");
                                }
                                _Text = _Text.TrimEnd();  //TODO remove double quotes.
                                _Text = _Text.TrimStart(); //TODO remove double quotes.
                                break;
                            case "/I:":
                                int intvresult;
                                if (int.TryParse(c.Substring(3),out intvresult) == true)
                                {
                                    _DashLength = Convert.ToInt32(c.Substring(3));
                                }
                                else
                                {
                                    Console.WriteLine("/D: must be an integer.  Default value of 500 ms used instead.");
                                }
                                _DotLength = _DashLength / 3;
                                _InTRALetter = _DotLength;
                                _InTERLetter = _DashLength;
                                _InTERWord = _DotLength * 7;

                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing command line.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace.ToString());
                tempResult = false;
            }
            return tempResult;
        }
        private bool SetText()
        {
            bool result = true;
            try
            {
                switch (_DataType)
                {
                    case eDataType.StringText:
                        break;
                    //Nothing to do, the Text is already set.
                    case eDataType.HTTPWEB:
                        ProcessWeb();
                        break;
                    case eDataType.LocalFile:
                    case eDataType.UNCFilePath:
                        ProcessFile();
                        break;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        private void ProcessWeb()
        {
            string URL = _File;
            WebClient client = new WebClient();
            try
            {
                Stream data = client.OpenRead(URL);
                StreamReader reader = new StreamReader(data);
                _Text = reader.ReadToEnd().ToString().ToUpper();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Processing Web Request.  " + ex.Message);
            }
        }
        private void ProcessFile()
        {
            string tempPath = "";
            StreamReader tempStream = default(StreamReader);
            bool tempPathExists = false;
            if (_File.Contains(":\\") | _File.StartsWith("\\\\"))
            {
                if (File.Exists(_File))
                {
                    tempPath = _File;
                    tempPathExists = true;
                }
            }
            else
            {
                if (File.Exists(string.Format("{0}\\{1}", Environment.CurrentDirectory, _File)))
                {
                    tempPath = string.Format("{0}\\{1}", Environment.CurrentDirectory, _File);
                    tempPathExists = true;
                }
            }
            if (tempPathExists)
            {
                tempStream = File.OpenText(tempPath);
                _Text = tempStream.ReadToEnd().ToString().ToUpper();
            }
            else
            {
                throw new Exception(string.Format("File {0} not found.", _File.ToUpper().ToString()));
            }
        }
        private void TurnNumLockOff()
        {
            if (GetKeyState(VK_NUMLOCK)>0)
            {
                ToggleLight();
                Thread.Sleep(3000);
            }
        }

        
    }
}
