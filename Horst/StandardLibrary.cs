using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Horst.Parser;

namespace Horst
{
    public class StandardLibrary
    {
        public Environment Environment { get; set; }

        public StandardLibrary()
        {
            Environment = new Environment(null);
            
            
            //#######################################
            //CONSTANTS
            //#######################################
            
            Environment.Define("PI", Math.PI);
            Environment.Define("E", Math.E);
            Environment.Define("TAU", Math.Tau);
            
            //#######################################
            //FUNCTIONS
            //#######################################


            #region Console

            Func<dynamic, dynamic> print = text =>
            {
                Console.WriteLine(text[0]);
                return false;
            };
            
            Func<dynamic[], dynamic> read = (s) =>
            {
                if (s.Length > 0)
                {
                    Console.Write(s[0]);
                }

                return Console.ReadLine();
            };
            
            Func<dynamic, dynamic> error = text =>
            {
                throw new Exception((string)text[0]);
            };
            
            Func<dynamic, dynamic> clear = text =>
            {
                Console.Clear();
                return false;
            };
            
            Environment.Define("print", print);
            Environment.Define("read", read);
            Environment.Define("error", error);
            Environment.Define("clear", clear);
            
            #endregion

            #region FileSystem

            Func<dynamic, dynamic> readFile = path =>
            {
                return File.ReadAllText(path[0]);
            };
            
            Func<dynamic, dynamic> writeFile = args =>
            {
                File.WriteAllText(args[0], args[1]);
                return false;
            };
            
            Func<dynamic, dynamic> deleteFile = path =>
            {
                File.Delete(path[0]);
                return false;
            };
            
            Func<dynamic, dynamic> isFile = path =>
            {
                return File.Exists(path[0]);
            };
            
            Func<dynamic, dynamic> isDir = path =>
            {
                return Directory.Exists(path[0]);
            };
            
            Func<dynamic, dynamic> createDir = path =>
            {
                Directory.CreateDirectory(path[0]);
                return false;
            };
            
            Environment.Define("readFile", readFile);
            Environment.Define("writeFile", writeFile);
            Environment.Define("deleteFile", deleteFile);
            Environment.Define("isFile", isFile);
            Environment.Define("isDir", isDir);
            Environment.Define("createDir", createDir);

            #endregion

            #region Network
            
            Func<dynamic, dynamic> httpGet = args =>
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create (args[0]);

                // Set some reasonable limits on resources used by this request
                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                // Set credentials to use for this request.
                request.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse ();

                //Console.WriteLine ("Content length is {0}", response.ContentLength);
                //Console.WriteLine ("Content type is {0}", response.ContentType);

                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream ();

                // Pipes the stream to a higher level stream reader with the required encoding format.
                StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);

                //Console.WriteLine ("Response stream received.");
                string res =  (readStream.ReadToEnd ());
                response.Close ();
                readStream.Close ();
                return res;
            };

            Func<dynamic, dynamic> httpPost = args =>
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

                string ret = "";

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    ret = responseFromServer;
                }

                // Close the response.
                response.Close();
                return ret;
            };
            
            Environment.Define("httpGet", httpGet);
            Environment.Define("httpPost", httpPost);

            #endregion
            
            //More
            
            Func<dynamic, dynamic> eval = code =>
            {
                return Interpreter.Evaluate(new Parser.Parser(new TokenStream(new InputStream(code[0]))).Parse(),
                    Environment);
            };

            Func<dynamic[], dynamic> exec = args =>
            {

                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
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
            };
            
            Environment.Define("eval", eval);
            Environment.Define("exec", exec);
        }
    }
}