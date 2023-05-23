using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using BrowseAndManageVideos_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic.Devices;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Linq.Expressions;
using BrowseAndManageVideos_WEB.Utils;

namespace BrowseAndManageVideos_WEB.Controllers
{
    [Route("Movie")]
    [ApiController]
    public class MovieController : Controller
    {
        DataContext _dataContext;

        public MovieController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetMoviesNoParameter()
        {
            try
            {
                List<Movie> movies = _dataContext.Movies.Take(100).ToList();
                int totalPage = _dataContext.Movies.ToList().Count / 100;
                ViewData["pages"] = totalPage;
                ViewData["movies"] = movies;
                ViewData["currentpage"] = 1;
                WriteLogFile.WriteLog("Information-Controller: GetMoviesNoParameter");
                return View("/Views/Movie/Index.cshtml");
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: GetMoviesNoParameter " + e.Message);
            }
            return BadRequest("Error");
        }

        [HttpGet]
        [Route("{Page}")]
        public IActionResult GetMoviesWithPaging([FromRoute] int page)
        {
            try
            {
                // list
                List<Movie> listAfterFilter = _dataContext.Movies.ToList();
                // paging
                int totalPage = listAfterFilter.Count / 100;
                listAfterFilter = _dataContext.Movies.Skip(100 * (page - 1)).Take(100).ToList();
                ViewData["movies"] = listAfterFilter;
                ViewData["pages"] = totalPage;
                ViewData["currentpage"] = page;
                WriteLogFile.WriteLog("Information-Controller: GetMoviesWithPaging");
                return RedirectToAction("/Views/Movie/Index.cshtml");
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: GetMoviesWithPaging " + e.Message);
            }
            return BadRequest("Error");
        }

        [HttpGet]
        [Route("{page}/s={search}")]
        public IActionResult GetMoviesWidthSearch(int page, string search)
        {
            try
            {
                // list
                List<Movie> listAfterFilter = _dataContext.Movies.Take(100).ToList();
                // paging
                int totalPage = listAfterFilter.Count / 100;
                listAfterFilter = listAfterFilter.Skip(totalPage * (page - 1)).Take(100).ToList();
                ViewData["movies"] = listAfterFilter;
                ViewData["pages"] = totalPage;
                ViewData["currentpage"] = page;
                WriteLogFile.WriteLog("Information-Controller: GetMoviesWidthSearch");
                return View("/Views/Movie/Index.cshtml");
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: GetMoviesWidthSearch " + e.Message);
            }
            return BadRequest("Error");
        }


        [HttpPatch]
        [Route("Update")]
        public IActionResult UpdateMovie([FromBody] JsonElement data)
        {
            try
            {
                //var newdetail = JsonConvert.DeserializeObject<UdpdateMovieNameData>(data);
                UdpdateMovieNameData? udpdateMovieNameData = JsonSerializer.Deserialize<UdpdateMovieNameData>(data);
                if(udpdateMovieNameData != null)
                {
                    var id = udpdateMovieNameData.Id;
                    var name = udpdateMovieNameData.Name;
                    Movie movie = _dataContext.Movies.Where(m => m.Id == int.Parse(id)).FirstOrDefault();
                    if(movie != null)
                    {
                        var oldPath = movie.Path;
                        string directory = Path.GetDirectoryName(movie.Path);
                        string extension = Path.GetExtension(movie.Path);
                        string newPath = Path.Combine(directory, name + extension);
                        movie.Name = name;
                        movie.Path = newPath;
                        _dataContext.Movies.Update(movie);
                        _dataContext.SaveChanges();
                        System.IO.File.Move(oldPath, newPath);
                        WriteLogFile.WriteLog("Information-Controller: UpdateMovie");
                    }
                    else
                    {
                        WriteLogFile.WriteLog("Warning-Controller: UpdateMovie - Movie not exist");
                    }
                }
                else
                {
                    WriteLogFile.WriteLog("Warning-Controller: UpdateMovie - Data receive error");
                }
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: UpdateMovie " + e.Message);
            }
            return View();
        }

        [HttpPost]
        [Route("Openfile")]
        public void OpenFile([FromQuery]int id)
        {
            try
            {
                Movie movie = _dataContext.Movies.Where(m => m.Id == id).FirstOrDefault();
                if (movie != null)
                {
                    if (movie.IsDeleted == true)
                    {
                        return;
                    }
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo(movie.Path)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
                WriteLogFile.WriteLog("Information-Controller: OpenFile");
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: OpenFile " + e.Message);
            }
        }

        [HttpGet]
        [Route("Movies/id")]
        public List<Movie> Movies()
        {
            List<Movie> movies = new List<Movie>();
            Debug.WriteLine(movies);
            return movies;
        }

        [HttpGet]
        [Route("Movies/AutoUpdateall/{path}")]
        public int AutoUpdateAll(string? path)
        {
            try
            {
                if (path == null)
                {
                    throw new FileNotFoundException("Path can not be null");
                }
                List<Movie> allMoviesInDatabase = GetMoviesFromDatabase();
                List<Movie> allMoviesInFolder = GetMoviesFromFile(path);
                WriteLogFile.WriteLog("Information-Controller: AutoUpdateAll");
                return 0;
            }
            catch (FileNotFoundException fnfe)
            {
                WriteLogFile.WriteLog("Error-Controller: AutoUpdateAll " + fnfe.Message);
                return 0;
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: AutoUpdateAll " + e.Message);
                return 0;
            }
        }

        [HttpGet]
        [Route("Movies/Insertall")]
        public bool AutoUpdateAll()
        {
            try
            {
                List<Movie> allMoviesInFolderF = GetMoviesFromFile(@"F:\");
                List<Movie> allMoviesInFolderE = GetMoviesFromFile(@"E:\");
                List<Movie> allMoviesInDatabase = _dataContext.Movies.ToList();
                List<Movie> folderENotInDatebase = allMoviesInFolderE.Except(allMoviesInDatabase).ToList();
                List<Movie> folderFNotInDatebase = allMoviesInFolderF.Except(allMoviesInDatabase).ToList();
                InsertMoviesToDatabase(folderENotInDatebase);
                InsertMoviesToDatabase(folderFNotInDatebase);
                return true;
            }
            catch (FileNotFoundException fnfe)
            {

                return false;
            }
            catch (Exception e)
            {

                return false;
            }

        }

        [HttpPost]
        [Route("movies/save")]
        public ActionResult Index()
        {
            Debug.WriteLine("here 2");
            return View();
        }

        //[Route("Delete")]
        //public ActionResult Delete()
        //{
        //    Debug.WriteLine("here 3");
        //    return View();
        //}

        private List<Movie> GetMoviesFromFile(string folderPath)
        {
            try
            {
                //Số file đã đọc
                List<string> readFiles = EnumerateFiles(folderPath, "*.*").ToList();
                readFiles.AddRange(EnumerateFiles(Path.Combine(folderPath, ".deleted"), "*.*").ToList());
                //Khai báo list video để lưu
                List<Movie> readMovies = new List<Movie>();

                foreach (var file in readFiles.Select((value, i) => new { i, value }))
                {
                    ShellObject shellItem = ShellObject.FromParsingName(file.value);
                    ShellPropertyCollection properties = shellItem.Properties.DefaultPropertyCollection;
                    Movie movie = new Movie();
                    if (file.value.Contains(".mp4") || file.value.Contains(".ts"))
                    {
                        //Name
                        movie.Name = properties.Where(prop => prop.CanonicalName == "System.ItemNameDisplayWithoutExtension").FirstOrDefault() == null ? "" : properties["System.ItemNameDisplayWithoutExtension"].ValueAsObject.ToString();
                        //path
                        movie.Path = properties.Where(prop => prop.CanonicalName == "System.ItemFolderPathDisplay").FirstOrDefault() == null ? "" : file.value;
                        // Description
                        movie.Description = "";
                        //actor
                        movie.Actor = properties.Where(prop => prop.CanonicalName == "System.Music.DisplayArtist").FirstOrDefault() == null ? "" : properties["System.Music.DisplayArtist"].ValueAsObject.ToString();
                        //FrameWidth
                        movie.FrameWidth = properties.Where(prop => prop.CanonicalName == "System.Video.FrameWidth").FirstOrDefault() == null ? "" : properties["System.Video.FrameWidth"].ValueAsObject.ToString();
                        //FrameHeight
                        movie.FrameHeight = properties.Where(prop => prop.CanonicalName == "System.Video.FrameHeight").FirstOrDefault() == null ? "" : properties["System.Video.FrameHeight"].ValueAsObject.ToString();
                        //ContentType
                        movie.ContentType = properties.Where(prop => prop.CanonicalName == "System.ContentType").FirstOrDefault() == null ? "" : properties["System.ContentType"].ValueAsObject.ToString();
                        //IsDeleted
                        movie.IsDeleted = false;
                        //Rating
                        movie.Rating = properties.Where(prop => prop.CanonicalName == "System.RatingText").FirstOrDefault() == null ? "" : properties["System.RatingText"].ValueAsObject.ToString();
                        //TotalBitrate
                        movie.TotalBitrate = properties.Where(prop => prop.CanonicalName == "System.Video.EncodingBitrate").FirstOrDefault() == null ? "" : properties["System.Video.EncodingBitrate"].ValueAsObject.ToString();
                        //EncodingBitrate
                        movie.EncodingBitrate = properties.Where(prop => prop.CanonicalName == "System.Video.TotalBitrate").FirstOrDefault() == null ? "" : properties["System.Video.TotalBitrate"].ValueAsObject.ToString();
                        //Size
                        movie.Size = properties["System.Size"].ValueAsObject == null ? "" : properties["System.Size"].ValueAsObject.ToString();
                    }
                    else if (int.Parse(properties["System.Size"].ValueAsObject.ToString()) == 0)
                    {
                        movie.Name = properties.Where(prop => prop.CanonicalName == "System.ItemNameDisplayWithoutExtension").FirstOrDefault() == null ? "" : properties["System.ItemNameDisplayWithoutExtension"].ValueAsObject.ToString();
                        //path
                        movie.Path = properties.Where(prop => prop.CanonicalName == "System.ItemFolderNameDisplay").FirstOrDefault() == null ? "" : file.value;
                        // Description
                        movie.Description = "";
                        //actor
                        movie.Actor = properties.Where(prop => prop.CanonicalName == "System.Music.DisplayArtist").FirstOrDefault() == null ? "" : properties["System.Music.DisplayArtist"].ValueAsObject.ToString();
                        //FrameWidth
                        movie.FrameWidth = properties.Where(prop => prop.CanonicalName == "System.Video.FrameWidth").FirstOrDefault() == null ? "" : properties["System.Video.FrameWidth"].ValueAsObject.ToString();
                        //FrameHeight
                        movie.FrameHeight = properties.Where(prop => prop.CanonicalName == "System.Video.FrameHeight").FirstOrDefault() == null ? "" : properties["System.Video.FrameHeight"].ValueAsObject.ToString();
                        //ContentType
                        movie.ContentType = properties.Where(prop => prop.CanonicalName == "System.ContentType").FirstOrDefault() == null ? "" : properties["System.ContentType"].ValueAsObject.ToString();
                        //IsDeleted
                        movie.IsDeleted = true;
                        //Rating
                        movie.Rating = properties.Where(prop => prop.CanonicalName == "System.RatingText").FirstOrDefault() == null ? "" : properties["System.RatingText"].ValueAsObject.ToString();
                        //TotalBitrate
                        movie.TotalBitrate = properties.Where(prop => prop.CanonicalName == "System.Video.EncodingBitrate").FirstOrDefault() == null ? "" : properties["System.Video.EncodingBitrate"].ValueAsObject.ToString();
                        //EncodingBitrate
                        movie.EncodingBitrate = properties.Where(prop => prop.CanonicalName == "System.Video.TotalBitrate").FirstOrDefault() == null ? "" : properties["System.Video.TotalBitrate"].ValueAsObject.ToString();
                        //Size
                        movie.Size = properties["System.Size"].ValueAsObject == null ? "" : properties["System.Size"].ValueAsObject.ToString();
                    }
                    else
                    {
                        continue;
                    }
                    readMovies.Add(movie);
                    Debug.WriteLine("Load file: " + movie.Name + ". Position: " + file.i + " / " + readFiles.Count);
                }
                return readMovies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new List<Movie>();
        }

        private string MakeDataTable(List<Movie> movies)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<table>");
                sb.AppendLine("" +
                    "<tr>" +
                    "<th>Checkbox</th>" +
                    "<th>ID</th>" +
                    "<th>Name</th>" +
                    "<th>Path</th>" +
                    "<th>Description</th>" +
                    "<th>Actor</th>" +
                    "<th>ContentType</th>" +
                    "<th>IsDeleted</th>" +
                    "<th>Rating</th>" +
                    "<th>Size</th>" +
                    "<th>FrameWidth</th>" +
                    "<th>FrameHeight</th>" +
                    "<th>TotalBitrate</th>" +
                    "<th>EncodingBitrate</th>" +
                    "</tr>");
                foreach (Movie movie in movies)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td><input type=\"checkbox\"></td>");
                    sb.AppendLine($"<td>{movie.Id}</td>");
                    sb.AppendLine($"<td>{movie.Name}</td>");
                    sb.AppendLine($"<td>{movie.Path}</td>");
                    sb.AppendLine($"<td>{movie.Description}</td>");
                    sb.AppendLine($"<td>{movie.Actor}</td>");
                    sb.AppendLine($"<td>{movie.ContentType}</td>");
                    sb.AppendLine($"<td>{movie.IsDeleted}</td>");
                    sb.AppendLine($"<td>{movie.Rating}</td>");
                    sb.AppendLine($"<td>{movie.Size}</td>");
                    sb.AppendLine($"<td>{movie.FrameWidth}</td>");
                    sb.AppendLine($"<td>{movie.FrameHeight}</td>");
                    sb.AppendLine($"<td>{movie.TotalBitrate}</td>");
                    sb.AppendLine($"<td>{movie.EncodingBitrate}</td>");
                    sb.AppendLine("</tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }

        private string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        private List<Movie> GetNotAddedMovie(List<Movie> database, List<Movie> folderE, List<Movie> folderF)
        {
            var listNotAddedFolderE = (from videoE in folderE
                                       join videoDatabase in database
                                       on videoE.Name equals videoDatabase.Name
                                       select videoE).ToList();
            return listNotAddedFolderE;
        }

        private List<Movie> GetMoviesFromDatabase()
        {
            try
            {
                List<Movie> movies = _dataContext.Movies.ToList();
                return movies;

            }
            catch (Exception e)
            {

            }
            return new List<Movie>();
        }

        private bool InsertMoviesToDatabase(List<Movie> movies)
        {
            try
            {
                _dataContext.Movies.AddRange(movies);
                _dataContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
        private static IEnumerable<string> EnumerateFilesWithSubFolder(string root, string searchPattern)
        {
            Stack<string> pending = new Stack<string>();
            pending.Push(root);
            while (pending.Count != 0)
            {
                var path = pending.Pop();
                string[] next = null;
                try
                {
                    next = Directory.GetFiles(path, searchPattern);
                }
                catch { }
                if (next != null && next.Length != 0)
                    foreach (var file in next) yield return file;
                try
                {
                    next = Directory.GetDirectories(path);
                    foreach (var subdir in next) pending.Push(subdir);
                }
                catch { }
            }
        }

        private static IEnumerable<string> EnumerateFiles(string root, string searchPattern)
        {
            string[] files = null;
            try
            {
                files = Directory.GetFiles(root, searchPattern);
            }
            catch { }

            if (files != null && files.Length != 0)
            {
                foreach (var file in files)
                {
                    yield return file;
                }
            }
        }

        private Movie GetMovieFromDatabase(int id)
        {
            try
            {
                Movie movie = _dataContext.Movies.Where(m => m.Id == id).FirstOrDefault();
                if (movie == null)
                {
                    return new Movie();
                }
                else
                {
                    return movie;
                }
            }
            catch (Exception e)
            {

            }
            return new Movie();
        }
    }
}