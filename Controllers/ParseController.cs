using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSRD.Models;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;

namespace SSRD.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ParseController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                const string URL = "https://feeds.meteoalarm.org/feeds/meteoalarm-legacy-atom-austria";
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(URL);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                
                XmlNodeList entries = doc.GetElementsByTagName("entry");
                foreach(XmlNode entry in entries)
                {
                    string newAreDesc = String.Empty;
                    string newSeverity = String.Empty;
                    DateTime? newOnset = null;
                    string newAuthorName = String.Empty;
                    string newAuthorURL = String.Empty;
                    foreach(XmlNode props in entry.ChildNodes)
                    {
                        if(props.Name.ToLower().Contains("areadesc"))
                        {
                            //Console.WriteLine("AreaDesc:\t" + props.InnerText);
                            newAreDesc = props.InnerText;
                        }
                        else if (props.Name.ToLower().Contains("onset"))
                        {
                            //Console.WriteLine("Onset:\t" + props.InnerText);
                            newOnset = DateTime.Parse(props.InnerText);
                        }
                        else if(props.Name.ToLower().Contains("severity"))
                        {
                            //Console.WriteLine("Severity:\t" + props.InnerText);
                            newSeverity = props.InnerText;
                        }
                        else if(props.Name.ToLower().Contains("author"))
                        {
                            XmlNodeList authorInfo = props.ChildNodes;
                            if(authorInfo.Count == 2)
                            {
                                newAuthorName = authorInfo[0].InnerText;
                                newAuthorURL = authorInfo[1].InnerText;
                                //Console.WriteLine("Author:\t" + newAuthorName + '\t' + newAuthorURL);
                            }
                        }
                    }
                    // new Warning
                    Warning tmpWarning = new Warning
                    {
                        AreaDesc = newAreDesc,
                        Onset = (DateTime)newOnset,
                        Severity = newSeverity,
                        //AuthorId = null
                    };
                    // add to Database
                    break;
                }
            }
            catch (Exception err)
            {
                //Console.WriteLine(err.Message);
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
