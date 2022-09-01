using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSRD.Models;
using SSRD.Data;
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
using Microsoft.EntityFrameworkCore;

namespace SSRD.Controllers
{
    public class ParseController : BaseController
    {
        private const string URL = "https://feeds.meteoalarm.org/feeds/meteoalarm-legacy-atom-austria";
        private readonly DataContext _context;
        private readonly ILogger<ParseController> _logger;

        public ParseController(ILogger<ParseController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ScrapeWarnings()
        {
            _logger.LogInformation("Scrapping weather warnings");
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(URL);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                
                XmlNodeList entries = doc.GetElementsByTagName("entry");
                foreach(XmlNode entry in entries)
                {
                    string newRegion = String.Empty;
                    string newSeverity = String.Empty;
                    DateTime? newOnset = null;
                    string newAuthorName = String.Empty;
                    string newAuthorURL = String.Empty;
                    foreach(XmlNode props in entry.ChildNodes)
                    {
                        // Looping through properties
                        if(props.Name.ToLower().Contains("areadesc"))
                        {
                            newRegion = props.InnerText;
                        }
                        else if (props.Name.ToLower().Contains("severity"))
                        {
                            newSeverity = props.InnerText;
                        }
                        else if (props.Name.ToLower().Contains("onset"))
                        {
                            newOnset = DateTime.Parse(props.InnerText);
                        }
                        else if(props.Name.ToLower().Contains("author"))
                        {
                            XmlNodeList authorInfo = props.ChildNodes;
                            foreach(XmlNode info in authorInfo)
                            {
                                if(info.Name.ToLower().Contains("name"))
                                {
                                    newAuthorName = info.InnerText;
                                }
                                else if(info.Name.ToLower().Contains("uri"))
                                {
                                    newAuthorURL = info.InnerText;
                                }
                            }
                        }
                    }
                    // Check for existing author
                    var author = await _context.Authors
                        .Where(a => a.Name.ToLower() == newAuthorName.ToLower() &&
                                    a.URL.ToLower() == newAuthorURL.ToLower())
                        .FirstOrDefaultAsync();
                    if(author == null)
                    {
                        author = new Author
                        {
                            Name = newAuthorName,
                            URL = newAuthorURL
                        };
                        _context.Authors.Add(author);
                        _context.SaveChanges();
                    }
                    // Adding new warning
                    var _warning = new Warning
                    {
                        Region = newRegion,
                        Severity = newSeverity,
                        //onset
                        AuthorId = author.Id,
                    };
                    if (newOnset != null)
                        _warning.Onset = (DateTime)newOnset;
                    _context.Warnings.Add(_warning);
                    int changes = await _context.SaveChangesAsync();
                    if (changes <= 0)
                    {
                        _logger.LogError("Failed to add scraped warnings - database error.");
                        return StatusCode(500);
                    }
                }
                var warnings = await _context.Warnings
                    .Include(x => x.Author)
                    .ToListAsync();
                return Ok(warnings);
            }
            catch (Exception err)
            {
                //Console.WriteLine(err.Message);
                return StatusCode(500);
            }
        }
    }
}
