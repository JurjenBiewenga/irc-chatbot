
/**
 *  @project >> Internet Relay Chat: Chatbot
 *  @authors >> Adib, SaraDR
 *  @version >> 02.01.00
 *  @updated >> 22.06.16
 *  @licence >> QPL-1.0
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
using System.Web.Script.Serialization;

namespace IRC
{
    public class Insult : Module
    {
        #region
            
            public override string pattern ()
            {
                return ("^!(?:insult|abuse|disrespect|taunt)\\s+(\\S+)$");
            }
            
        #endregion
        
        #region
            
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
            public override void OnChannelMessage (string user, string room, string text)
            {
                Console.WriteLine("A");
                
                try
                {
                    request = (HttpWebRequest)WebRequest.Create("http://quandyfactory.com/insult/json");
                    
                    request.AllowAutoRedirect = false;
                    
                    request.BeginGetResponse(new AsyncCallback(AsyncResponse), new object[3] {user, room, text});
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            
        #endregion
        
        #region
            
            public void AsyncResponse (IAsyncResult result)
            {
                object[] parameters = (object[])result.AsyncState;
                
                var myHttpWebRequest = request;
                
                using (var response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(result))
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
                    {
                        var outputText = serializer.Deserialize<Dictionary<string, string>>(reader.ReadToEnd())["insult"];
                        
                        if (outputText.Length > 0)
                        {
                            Anxious.Send((string)parameters[1], (string)parameters[2] + ", " + outputText);
                        }
                    }
                }
            }
            
            public HttpWebRequest       request     = null;
            
            public JavaScriptSerializer serializer  = new JavaScriptSerializer();
            
        #endregion
    }
}
