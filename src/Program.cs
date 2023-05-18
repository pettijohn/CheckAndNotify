using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;


// Build Environment
var wxStation = Environment.GetEnvironmentVariable("WXSTATION");
if (string.IsNullOrEmpty(wxStation)) throw new ArgumentNullException("WXSTATION");
var http = new HttpClient();
var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
http.DefaultRequestHeaders.Add("User-Agent", $"CheckAndNotify/{version}"); // required by NWS
//http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

var url = $"https://w1.weather.gov/xml/current_obs/{wxStation}.xml";
var rawXml = await http.GetStringAsync(url);

XmlSerializer serializer = new XmlSerializer(typeof(CurrentObservation));
using (StringReader reader = new StringReader(rawXml))
{
   var test = (CurrentObservation)serializer.Deserialize(reader);
}
