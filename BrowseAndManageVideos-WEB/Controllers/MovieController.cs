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

namespace BrowseAndManageVideos_WEB.Controllers
{
    [Route("movies")]
    [ApiController]
    public class MovieController : Controller
    {
        DataContext _dataContext;

        public MovieController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
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

        private void UpdateFilePropertyFromDatabase() 
        {
            try
            {
                List<Movie> movies = _dataContext.Movies.ToList();
                

            }
            catch (Exception e)
            {
               
            }

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

        [HttpGet]
        [Route("")]
        public IActionResult GetMoviesNoParameter()
        {
            try
            {
                List<Movie> movies = _dataContext.Movies.Take(100).ToList();
                ViewData["movies"] = movies;

                return View("/Views/Movie/GetMovies.cshtml");
            }
            catch (Exception e)
            {

            }
            return BadRequest("Error");
        }

        [HttpGet]
        [Route("page={page}")]
        public IActionResult GetMoviesWithPaging(int page)
        {
            try
            {
                // list
                List<Movie> listAfterFilter = _dataContext.Movies.ToList();
                // paging
                int totalPage = listAfterFilter.Count / 100;
                listAfterFilter = _dataContext.Movies.Skip(totalPage * (page - 1)).Take(100).ToList();
                ViewData["movies"] = listAfterFilter;

                return View("GetMovies.cshtml");
            }
            catch(Exception e) 
            { 
                
            }
            return BadRequest("Error");
        }

        [HttpGet]
        [Route("page={page}/search={search}")]
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
                return View("GetMovies.cshtml");
            }
            catch (Exception e)
            {

            }
            return BadRequest("Error");
        }

        [HttpPost]
        [Route("openfile={id}")]
        public IActionResult OpenFile(int id)
        {
            try
            {
                Movie movie = _dataContext.Movies.FirstOrDefault(x => x.Id == id);
                if(movie == null)
                {
                    ViewBag["openfileresult"].Message = "Can not find selected file!";
                    return View("/Views/Movie/GetMovies.cshtml");
                }
                else
                {
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(movie.Path)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                    ViewBag["openfileresult"].Message = "File openned";
                    return View("/Views/Movie/GetMovies.cshtml");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Error");
            }
        }

        [HttpGet]
        [Route("movies/id")]
        public List<Movie> Movies()
        {
            List<Movie> movies = new List<Movie>();
            Debug.WriteLine(movies);
            return movies;
        }

        [HttpGet]
        [Route("movies/autoupdateall/{path}")]
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
                
                return 0;
            }
            catch (FileNotFoundException fnfe)
            {
                
                return 0; 
            }
            catch (Exception e)
            {
                
                return 0;
            }
        }

        [HttpGet]
        [Route("movies/insertall")]
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

        private List<Movie> GetNotAddedMovie(List<Movie> database, List<Movie> folderE, List<Movie> folderF)
        {
            var listNotAddedFolderE = (from videoE in folderE
                                       join videoDatabase in database
                                       on videoE.Name equals videoDatabase.Name
                                       select videoE).ToList();
            return listNotAddedFolderE;
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

        private string MakeDataTable(List<Movie> movies)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<table>");
                sb.AppendLine("" +
                    "<tr>" +
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
            catch(Exception ex)
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
    }
}
