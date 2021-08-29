using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Text;
using Horst.Parser;

namespace Horst
{
    public class StandardLibrary
    {
        public Environment Environment { get; }
        private readonly string _file;

        public StandardLibrary(string file)
        {
            this._file = file;
            Environment = new Environment(null);
            
            
            //#######################################
            //CONSTANTS
            //#######################################
            
            Environment.Define("PI", Math.PI);
            Environment.Define("E", Math.E);
            Environment.Define("TAU", Math.Tau);
            Environment.Define("PATH", file);
            
            //#######################################
            //FUNCTIONS
            //#######################################


            #region Console

            dynamic Print(dynamic args)
            {
                if (args.Length > 1)
                {
                    if (args[1] == "black")
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "darkblue")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "darkgreen")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "darkcyan")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "darkred")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "darkmagenta")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "darkyellow")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "gray")
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "darkgray")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "blue")
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "green")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "cyan")
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "red")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "magenta")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "yellow")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                    else if (args[1] == "white")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(args[0]);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine(args[0]);   
                }
                return false;
            }

            dynamic Read(dynamic[] s)
            {
                if (s.Length > 0)
                {
                    Console.Write(s[0]);
                }

                return Console.ReadLine();
            }

            dynamic Error(dynamic text)
            {
                throw new Exception((string) text[0]);
            }

            dynamic Clear(dynamic text)
            {
                Console.Clear();
                return false;
            }

            Environment.Define("print", (Func<dynamic, dynamic>) Print);
            Environment.Define("read", (Func<dynamic[], dynamic>) Read);
            Environment.Define("error", (Func<dynamic, dynamic>) Error);
            Environment.Define("clear", (Func<dynamic, dynamic>) Clear);
            
            #endregion

            #region FileSystem

            dynamic ReadFile(dynamic path)
            {
                return File.ReadAllText(Path.Combine(file, path[0]));
            }

            dynamic WriteFile(dynamic args)
            {
                File.WriteAllText(Path.Combine(file, args[0]), args[1]);
                return false;
            }

            dynamic DeleteFile(dynamic path)
            {
                File.Delete(Path.Combine(file, path[0]));
                return false;
            }

            dynamic IsFile(dynamic path)
            {
                return File.Exists(Path.Combine(file, path[0]));
            }

            dynamic IsDir(dynamic path)
            {
                return Directory.Exists(Path.Combine(file, path[0]));
            }

            dynamic CreateDir(dynamic path)
            {
                Directory.CreateDirectory(Path.Combine(file, path[0]));
                return false;
            }

            Environment.Define("readFile", (Func<dynamic, dynamic>) ReadFile);
            Environment.Define("writeFile", (Func<dynamic, dynamic>) WriteFile);
            Environment.Define("deleteFile", (Func<dynamic, dynamic>) DeleteFile);
            Environment.Define("isFile", (Func<dynamic, dynamic>) IsFile);
            Environment.Define("isDir", (Func<dynamic, dynamic>) IsDir);
            Environment.Define("createDir", (Func<dynamic, dynamic>) CreateDir);

            #endregion

            #region Network

            dynamic HttpGet(dynamic args)
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(args[0]);

                // Set some reasonable limits on resources used by this request
                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                // Set credentials to use for this request.
                request.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                //Console.WriteLine ("Content length is {0}", response.ContentLength);
                //Console.WriteLine ("Content type is {0}", response.ContentType);

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format.
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                //Console.WriteLine ("Response stream received.");
                string res = (readStream.ReadToEnd());
                response.Close();
                readStream.Close();
                return res;
            }

            dynamic HttpPost(dynamic args)
            {
                // Create a request using a URL that can receive a post.
                WebRequest request = WebRequest.Create(args[0]);
                // Set the Method property of the request to POST.
                request.Method = "POST";

                // Create POST data and convert it to a byte array.
                string postData = args[1];
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;

                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse) response).StatusDescription);

                string ret;

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream ?? throw new InvalidOperationException());
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    ret = responseFromServer;
                }

                // Close the response.
                response.Close();
                return ret;
            }

            Environment.Define("httpGet", (Func<dynamic, dynamic>) HttpGet);
            Environment.Define("httpPost", (Func<dynamic, dynamic>) HttpPost);

            #endregion

            #region Math

            dynamic Cos(dynamic args)
            {
                return Math.Cos(args[0]);
            }
            dynamic Sin(dynamic args)
            {
                return Math.Sin(args[0]);
            }
            dynamic Sqrt(dynamic args)
            {
                return Math.Sqrt(args[0]);
            }
            dynamic Exp(dynamic args)
            {
                return Math.Exp(args[0]);
            }
            dynamic Log(dynamic args)
            {
                return Math.Log(args[0], args[1]);
            }
            dynamic Pow(dynamic args)
            {
                return Math.Pow(args[0], args[1]);
            }
            dynamic Max(dynamic args)
            {
                return Math.Max(args[0], args[1]);
            }
            dynamic Min(dynamic args)
            {
                return Math.Min(args[0], args[1]);
            }
            dynamic Round(dynamic args)
            {
                return Math.Round(args[0]);
            }
            
            
            
            Environment.Define("cos",(Func<dynamic, dynamic>) Cos);
            Environment.Define("sin",(Func<dynamic, dynamic>) Sin);
            Environment.Define("exp",(Func<dynamic, dynamic>) Exp);
            Environment.Define("sqrt",(Func<dynamic, dynamic>) Sqrt);
            Environment.Define("log",(Func<dynamic, dynamic>) Log);
            Environment.Define("pow",(Func<dynamic, dynamic>) Pow);
            Environment.Define("max",(Func<dynamic, dynamic>) Max);
            Environment.Define("min",(Func<dynamic, dynamic>) Min);
            Environment.Define("round",(Func<dynamic, dynamic>) Round);

            #endregion
            
            //More

            dynamic Eval(dynamic code) => Interpreter.Evaluate(new Parser.Parser(new TokenStream(new InputStream(code[0], _file))).Parse(), Environment);

            dynamic Exec(dynamic[] args)
            {
                Process cmd = new Process {StartInfo = {FileName = "cmd.exe", RedirectStandardInput = true}};
                if (args.Length > 1)
                {
                    cmd.StartInfo.RedirectStandardOutput = args[1];
                }

                cmd.StartInfo.RedirectStandardOutput = false;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine(args[0]);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                return false;
            }

            Environment.Define("eval", (Func<dynamic, dynamic>) Eval);
            Environment.Define("exec", (Func<dynamic[], dynamic>) Exec);
        }
    }
}