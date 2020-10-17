using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
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
using Newtonsoft.Json;

namespace FaceUnlockVocalNode.Resources
{
    class MyCognitive
    {
        
        static string key = "73185574f3d74f51aebe5262d6f31445";
        // Gets the analysis of the specified image by using the Face REST API.
       public static string Detect(string imageFilePath)
        {


            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_03&returnRecognitionModel=false&detectionModel=detection_01");
            // Request body. Posts a locally stored JPEG image.


            byte[] byteData = GetImageAsByteArray(imageFilePath);


            request.Method = "POST";
            request.ContentType = "application/octet-stream";
            request.ContentLength = byteData.Length;
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Host = "provaFaccia.cognitiveservices.azure.com";

            using (var stream = request.GetRequestStream())
            {
                stream.Write(byteData, 0, byteData.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine("Risultati: " + responseString);
            dynamic json = JsonConvert.DeserializeObject(responseString);

            var id = json[0].faceId;
            return id ;
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


        public static string identify(String img)
        {

            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/identify?recognitionModel=recognition_03");

            var postData = "{\"PersonGroupId\": \"2\",\"faceIds\":[\"" + img + "\"], \"maxNumOfCandidatesReturned\": 1, \"confidenceThreshold\": 0.5}";
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

            dynamic json = JsonConvert.DeserializeObject(responseString);            
            string b = String.Join(" ", json[0].candidates);
         
            if (b != "")
            {
               
                int c = (int)json[0].candidates[0].confidence;
                if (c > 0.75)
                {
                    return json[0].candidates[0].personId;
                }
                else
                {
                    return "";
                }
            } else {
                return "";
            }
        }


        public static string addPerson(string nome, string userData)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/persongroups/2/persons?recognitionModel=recognition_03");

            var postData = "{\"name\": \"" + nome + "\",\"userData\":\"" + userData + "\"}";
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
            Console.WriteLine("Risultati: " + responseString);
            dynamic json = JsonConvert.DeserializeObject(responseString);

            return json.personId;

        }


        //aggiungere faccia al persongroup person
        public static void addFace(string personId, string pathImage)
        {

            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/persongroups/2/persons/" + personId + "/persistedFaces?detectionModel=detection_01&recognitionModel=recognition_03");
            // Request body. Posts a locally stored JPEG image.


            byte[] byteData = GetImageAsByteArray(pathImage);


            request.Method = "POST";
            request.ContentType = "application/octet-stream";
            request.ContentLength = byteData.Length;
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Host = "provaFaccia.cognitiveservices.azure.com";

            using (var stream = request.GetRequestStream())
            {
                stream.Write(byteData, 0, byteData.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine("Risultati: " + responseString);


        }

        //crea person group, in input da utente id del personGroup
        public static void createPersonGroup(string personGroup)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/persongroups/" + personGroup + "?recognitionModel=recognition_03");

            var postData = "{\"name\": \"nome\",\"userData\":\"gruppo\",\"recognitionModel\":\"recognition_03\"}";
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
            dynamic json = JsonConvert.DeserializeObject(responseString);

          

        }

        public static void trainPersonGroup(string personGroupId)
        {

            var request = (HttpWebRequest)WebRequest.Create("https://provaFaccia.cognitiveservices.azure.com/face/v1.0/persongroups/" + personGroupId + "/train?recognitionModel=recognition_03");

            var postData = "";
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

        }


    }
}