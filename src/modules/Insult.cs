
/**
 *  @project >> Internet Relay Chat: Chatbot
 *  @authors >> Adib
 *  @version >> 02.00.00
 *  @release >> 06.06.16
 *  @licence >> MIT
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace IRC
{
    public static class Insult
    {

        #region

        static HttpWebRequest request;

        /**
         *  This event is called whenever someone, either in a private query
         *  or in a channel, calls up the bot, with any command that matches
         *  this modules synonyms. Any remaining part of the message is then
         *  delivered here along with the room to reply to, and the user who
         *  called it. In order to send out a message back out, then use the
         *  syntax of 'Anxious.Send(<target>, <message>)'. If the message is
         *  supposed to be send in private then insert the user parameter in
         *  the target, else insert the rooms parameter. In the message part
         *  please keep the replies short to avoid sending too much flooding
         */
        public static async void OnMessage(string user, string room, string text)
        {
            try
            {

                request = (HttpWebRequest)WebRequest.Create("http://quandyfactory.com/insult/json");
                request.AllowAutoRedirect = false;

                request.BeginGetResponse(new AsyncCallback(AsyncResponse), null);


            }
            catch
            {

            }
        }

        public static void AsyncResponse(IAsyncResult asynchronousResult)
        {
            HttpWebRequest myHttpWebRequest = request;
            var response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);


            var headers = response.Headers;

            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                var responseText = reader.ReadToEnd();
                var responseObject = serializer.Deserialize<Dictionary<string, string>>(responseText);

                var outputText = responseObject["insult"];
                Anxious.Send(room, outputText + ", " + Title.GetUser(text));
            }
            response.Close();
        }
        #endregion
    }
}
