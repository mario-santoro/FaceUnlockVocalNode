using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Net;
using System.Text; 
using Newtonsoft.Json;
namespace face_quickstart
{
    class Program
    {
        static string id=null;
        static string key=  "XXXXXXXXXXXXXXXXX";      
       
        static void Main(string[] args)
        {

                 //Console.WriteLine("Add person group person:");
              //  var personId= addPerson("Orlando Bloom", "Orlando");

              /*
                Console.WriteLine("Add face to person group person:");
                
                 addFace( "64bb5eab-5bed-42c0-9459-67512d8f6255",  @"C:\Users\mario\Desktop\DInfUniSa\MagistraleMatricola_866\1anno\1semestre\CloudComputing\face-quickstart\img\ob1.jpg").Wait();
               */       
                      
                        // Get the path and filename to process from the user.
                
                Console.WriteLine("Detect faces:");
                Console.Write(
                    "Enter the path to an image with faces that you wish to analyze: ");
                string imageFilePath = Console.ReadLine();

                if (File.Exists(imageFilePath))
                {
                    try
                    {
                        Console.WriteLine("\nWait a moment for the results to appear.\n");
                        Detect(imageFilePath).Wait();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\n" + e.Message + "\n");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid file path.\n");
                }
                  Console.WriteLine(id);
                  identify(id);
                  
                  
          
        }
        // Gets the analysis of the specified image by using the Face REST API.
        static async Task Detect(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
             
            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_03&returnRecognitionModel=false&detectionModel=detection_01";

            // Assemble the URI for the REST API Call.
            string uri ="https://provaFaccia.cognitiveservices.azure.com/face/v1.0/detect?" + requestParameters;
            //Console.WriteLine(uri);
            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n");
                dynamic json  = JsonConvert.DeserializeObject(contentString);
		   
                 id= json[0].faceId;
              //  Console.WriteLine(JsonPrettyPrint(contentString));
            }
        }


            // Returns the contents of the specified file as a byte array.
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }


        // Formats the given JSON string by adding line breaks and indents.
        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }
  

/*
        //detect for image on the web
        public  static String Detect()
	    {

            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_03&returnRecognitionModel=false&detectionModel=detection_01");
                        
            var postData = "{\"url\":\"https://i3.ytimg.com/vi/VOgFZfRVaww/mqdefault.jpg\"}";
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Headers.Add("Ocp-Apim-Subscription-Key", "73185574f3d74f51aebe5262d6f31445");
            request.Host = "provaFaccia.cognitiveservices.azure.com";
            
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
                
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
             Console.WriteLine(responseString);
            dynamic json  = JsonConvert.DeserializeObject(responseString);
		   
            return json[0].faceId;
            
	    }



*/  
        public static void identify(String img)
	    {

            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/identify?recognitionModel=recognition_03");
                        
            var postData =  "{\"PersonGroupId\": \"2\",\"faceIds\":[\""+img+"\"], \"maxNumOfCandidatesReturned\": 1, \"confidenceThreshold\": 0.5}";
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Host = "provaFaccia.cognitiveservices.azure.com";
            
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine("Risultati: "+responseString);
        }

        public static string  addPerson(string nome, string userData){
            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/persongroups/2/persons?recognitionModel=recognition_03");
                        
            var postData =  "{\"name\": \""+nome+"\",\"userData\":\""+userData+"\"}";
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Host = "provaFaccia.cognitiveservices.azure.com";
            
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine("Risultati: "+responseString);
             dynamic json  = JsonConvert.DeserializeObject(responseString);
		    
            return json.personId;

        }


//aggiungere faccia al persongroup person
            static async Task addFace(string personId, string pathImage){
                HttpClient client = new HttpClient();
               // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);                        
            
            // Request parameters. A third optional parameter is "details".
            string requestParameters = "detectionModel=detection_01&recognitionModel=recognition_03";

            // Assemble the URI for the REST API Call.
            string uri ="https://provaFaccia.cognitiveservices.azure.com/face/v1.0/persongroups/2/persons/"+personId+"/persistedFaces?" + requestParameters;
            //Console.WriteLine(uri);
            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(pathImage);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    // Execute the REST API call.
                    response = await client.PostAsync(uri, content);

                    // Get the JSON response.
                    string contentString = await response.Content.ReadAsStringAsync();

                    // Display the JSON response.
                    Console.WriteLine("\nResponse:\n");
                
                    Console.WriteLine(JsonPrettyPrint(contentString));
                }
       
        }
   
        //crea person group, in input da utente id del personGroup
        public static void createPersonGroup(string personGroup){
            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/persongroups/"+personGroup+"?recognitionModel=recognition_03");
            
            var postData =  "{\"name\": \"nome\",\"userData\":\"gruppo\",\"recognitionModel\":\"recognition_03\"}";
            var data = Encoding.UTF8.GetBytes(postData);


            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Host = "provaFaccia.cognitiveservices.azure.com";

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            dynamic json  = JsonConvert.DeserializeObject(responseString);
		    
            Console.WriteLine("GEISON: "+json);            

        }

        public static void trainPersonGroup(string personGroupId){
        
            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/persongroups/"+personGroupId+"/train?recognitionModel=recognition_03");
            request.Method = "POST";
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Host = "provaFaccia.cognitiveservices.azure.com";

            var response = (HttpWebResponse)request.GetResponse();
        }

     
    }
    
}
