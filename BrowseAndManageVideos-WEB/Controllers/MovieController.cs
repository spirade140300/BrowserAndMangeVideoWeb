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
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult GetMoviesNoParameter([FromQuery]int stateID, int count)
        {
            try
            {
                List<Movie> movies = new List<Movie>();
                if (stateID == 0)
                {
                    movies = _dataContext.Movies.OrderBy(m => m.Name).Take(count).ToList();
                }
                else if(stateID == 1)
                {
                    movies = _dataContext.Movies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Take(count).ToList();
                }
                else if(stateID == 2)
                {
                    movies = _dataContext.Movies.Where(m => m.IsDeleted == true).OrderBy(m => m.Name).Take(count).ToList();
                }
                ViewData["movies"] = movies;
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
        [Route("Search/{search}")]
        public IActionResult GetMoviesWidthSearch([FromRoute] string search)
        {
            try
            {
                if (search != "")
                {
                    // list
                    List<Movie> listAfterFilter = _dataContext.Movies.Where(m => m.Name.ToLower().Contains(search.ToLower()) || m.Actor.ToLower().Contains(search.ToLower()) || m.Description.ToLower().Contains(search.ToLower())).OrderBy(m => m.Name).ToList();
                    // paging
                    ViewData["movies"] = listAfterFilter;
                    WriteLogFile.WriteLog("Information-Controller: GetMoviesWidthSearch");
                    return PartialView("_MovieDataPartial", listAfterFilter);
                }
                else
                {
                    WriteLogFile.WriteLog("Warning-Controller: UpdateMovie - Data receive error");
                }
                
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: GetMoviesWidthSearch " + e.Message);
            }
            return BadRequest("Error");
        }


        [HttpPatch]
        [Route("{Update}")]
        public IActionResult UpdateMovie([FromBody] JsonElement data)
        {
            try
            {
                //var newdetail = JsonConvert.DeserializeObject<UdpdateMovieNameData>(data);
                UpdateMovieNameData? updateMovieNameData = JsonSerializer.Deserialize<UpdateMovieNameData>(data);
                if(updateMovieNameData != null)
                {
                    var id = updateMovieNameData.Id;
                    var name = updateMovieNameData.Name;
                    var description = updateMovieNameData.Description;
                    var actor = updateMovieNameData.Actor;
                    var rating = updateMovieNameData.Rating;
                    Movie movie = _dataContext.Movies.Where(m => m.Id == int.Parse(id)).FirstOrDefault();
                    if(movie != null)
                    {
                        var oldPath = "";
                        var newPath = "";
                        // set name
                        if (movie.Name != name) {
                            movie.Name = name;
                            // set path
                            oldPath = movie.Path;
                            string directory = Path.GetDirectoryName(movie.Path);
                            string extension = Path.GetExtension(movie.Path);
                            newPath = Path.Combine(directory, name + extension);
                            movie.Path = newPath;
                        }
                        
                        // set description
                        if(description != movie.Description)
                        {
                            movie.Description = description;
                        }

                        if (actor != movie.Actor)
                        {
                            movie.Actor = actor;
                        }

                        if (rating != movie.Rating)
                        {
                            movie.Rating = rating;
                        }
                        movie.AcessTime = DateTime.Now.ToString();
                        _dataContext.Movies.Update(movie);
                        _dataContext.SaveChanges();
                        if (oldPath != "")
                        {
                            System.IO.File.Move(oldPath, newPath);
                        }
                        WriteLogFile.WriteLog("Information-Controller: UpdateMovie - ID = " + movie.Id);
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
            List<Movie> movies = _dataContext.Movies.OrderBy(m => m.Name).Take(500).ToList();
            return PartialView("_MovieDataPartial", movies);
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
                    movie.AcessTime = DateTime.Now.ToString();
                    _dataContext.Update(movie);
                    _dataContext.SaveChanges();
                    p.StartInfo = new ProcessStartInfo(movie.Path)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
                WriteLogFile.WriteLog("Information-Controller: OpenFile - FileName: " + movie.Name);
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: OpenFile " + e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public Movie GetMovie([FromRoute] string id)
        {
            try
            {
                Movie movie = _dataContext.Movies.Where(m => m.Id.Equals(int.Parse(id))).FirstOrDefault();
                WriteLogFile.WriteLog("Information-Controller: GetMovie");
                return movie;
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: GetMovie " + e.Message);
            }
            return new Movie();
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
        [Route("Insertall")]
        public void AutoUpdateAll()
        {
            try
            {
                List<Movie> allMoviesInFolder = GetMoviesFromFile(@"F:\");
                allMoviesInFolder.AddRange(GetMoviesFromFile(@"E:\").ToList());
                List<Movie> allMoviesInDatabase = _dataContext.Movies.ToList();
                foreach(Movie movie in allMoviesInFolder)
                {
                    Debug.WriteLine("Current check item: " + movie.Name);
                    Movie databaseMovie = allMoviesInDatabase.Where(m => m.Name == movie.Name).FirstOrDefault();
                    if (databaseMovie == null)
                    {
                        _dataContext.Movies.Add(movie);
                        _dataContext.SaveChanges();
                    }
                    else
                    {
                        if(databaseMovie.Name != movie.Name || databaseMovie.Path != movie.Path || databaseMovie.Size != movie.Size || databaseMovie.IsDeleted != movie.IsDeleted)
                        {
                            databaseMovie.Name = movie.Name;
                            databaseMovie.Path = movie.Path;
                            databaseMovie.Size = movie.Size;
                            databaseMovie.IsDeleted = movie.IsDeleted;
                            _dataContext.Movies.Update(databaseMovie);
                            _dataContext.SaveChanges();
                        }
                    }
                }
            }
            catch (FileNotFoundException fnfe)
            {
                WriteLogFile.WriteLog("Error-Controller FileNotFoundException: AutoUpdateAll " + fnfe.Message);
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller Exception: AutoUpdateAll " + e.Message);
            }

        }

        [HttpDelete]
        [Route("Delete")]
        public ActionResult DeleteMovie([FromQuery] int id)
        {
            try
            {
                List<Movie> movies = _dataContext.Movies.ToList();
                Movie movie = movies.Where(m => m.Id == id).FirstOrDefault();
                if (movie != null)
                {
                    if (movie.IsDeleted == true)
                    {
                        return PartialView("_MovieDataPartial", movies);
                    }
                    else
                    {
                        string currentName = movie.Name;
                        string currentPath = movie.Path;
                        string directory = Path.GetDirectoryName(currentPath);
                        string delete = @".deleted";
                        string newPath = directory + Path.Combine(delete, currentName);

                        movie.Path = newPath;
                        movie.ContentType = "";
                        movie.IsDeleted = true;
                        movie.Rating = "";
                        movie.Size = "0";
                        movie.EncodingBitrate = "";
                        movie.FrameWidth = "";
                        movie.FrameHeight = "";
                        movie.TotalBitrate = "";
                        movie.AcessTime = "";
                        _dataContext.Movies.Update(movie);
                        _dataContext.SaveChanges();

                        System.IO.File.Delete(currentPath);
                        System.IO.File.Create(newPath);
                        movies = _dataContext.Movies.ToList();
                        movie = movies.Where(m => m.Id == id).FirstOrDefault();
                        WriteLogFile.WriteLog("Information-Controller: DeleteMovie - Name: " + movie.Name);
                        return PartialView("_MovieDataPartial", movies);
                    }
                    
                }
                WriteLogFile.WriteLog("Error-Controller: Delete");
            }
            catch (Exception e)
            {
                WriteLogFile.WriteLog("Error-Controller: Delete " + e.Message);
            }
            return PartialView("_MovieDataPartial");
        }

        // Fast read file with no detail
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
                    FileInfo details= new FileInfo(file.value);
                    Movie movie = new Movie();
                    if (file.value.Contains(".mp4") || file.value.Contains(".ts"))
                    {
                        movie.Path = details.FullName;
                        movie.Name = System.IO.Path.GetFileNameWithoutExtension(details.Name);
                        movie.Size = details.Length.ToString();
                        movie.AcessTime = DateTime.MinValue.ToString();
                        movie.IsDeleted = false;
                    }
                    else if (details.Length == 0)
                    {
                        movie.Path = details.FullName;
                        movie.Name = System.IO.Path.GetFileNameWithoutExtension(details.Name);
                        movie.Size = "0";
                        movie.IsDeleted = true;
                        movie.AcessTime = DateTime.MinValue.ToString();
                        movie.EncodingBitrate = null;
                        movie.FrameWidth = null;
                        movie.FrameHeight = null;
                        movie.TotalBitrate = null;
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

        // Slow read file with detail
        //private List<Movie> GetMoviesFromFile(string folderPath)
        //{
        //    try
        //    {
        //        //Số file đã đọc
        //        List<string> readFiles = EnumerateFiles(folderPath, "*.*").ToList();
        //        readFiles.AddRange(EnumerateFiles(Path.Combine(folderPath, ".deleted"), "*.*").ToList());
        //        //Khai báo list video để lưu
        //        List<Movie> readMovies = new List<Movie>();

        //        foreach (var file in readFiles.Select((value, i) => new { i, value }))
        //        {
        //            ShellObject shellItem = ShellObject.FromParsingName(file.value);
        //            ShellPropertyCollection properties = shellItem.Properties.DefaultPropertyCollection;
        //            Movie movie = new Movie();
        //            if (file.value.Contains(".mp4") || file.value.Contains(".ts"))
        //            {
        //                //Name
        //                movie.Name = properties.Where(prop => prop.CanonicalName == "System.ItemNameDisplayWithoutExtension").FirstOrDefault() == null ? "" : properties["System.ItemNameDisplayWithoutExtension"].ValueAsObject.ToString();
        //                //path
        //                movie.Path = properties.Where(prop => prop.CanonicalName == "System.ItemFolderPathDisplay").FirstOrDefault() == null ? "" : file.value;
        //                // Description
        //                movie.Description = "";
        //                //actor
        //                movie.Actor = properties.Where(prop => prop.CanonicalName == "System.Music.DisplayArtist").FirstOrDefault() == null ? "" : properties["System.Music.DisplayArtist"].ValueAsObject.ToString();
        //                //FrameWidth
        //                movie.FrameWidth = properties.Where(prop => prop.CanonicalName == "System.Video.FrameWidth").FirstOrDefault() == null ? "" : properties["System.Video.FrameWidth"].ValueAsObject.ToString();
        //                //FrameHeight
        //                movie.FrameHeight = properties.Where(prop => prop.CanonicalName == "System.Video.FrameHeight").FirstOrDefault() == null ? "" : properties["System.Video.FrameHeight"].ValueAsObject.ToString();
        //                //ContentType
        //                movie.ContentType = properties.Where(prop => prop.CanonicalName == "System.ContentType").FirstOrDefault() == null ? "" : properties["System.ContentType"].ValueAsObject.ToString();
        //                //IsDeleted
        //                movie.IsDeleted = false;
        //                //Rating
        //                movie.Rating = properties.Where(prop => prop.CanonicalName == "System.RatingText").FirstOrDefault() == null ? "" : properties["System.RatingText"].ValueAsObject.ToString();
        //                //TotalBitrate
        //                movie.TotalBitrate = properties.Where(prop => prop.CanonicalName == "System.Video.EncodingBitrate").FirstOrDefault() == null ? "" : properties["System.Video.EncodingBitrate"].ValueAsObject.ToString();
        //                //EncodingBitrate
        //                movie.EncodingBitrate = properties.Where(prop => prop.CanonicalName == "System.Video.TotalBitrate").FirstOrDefault() == null ? "" : properties["System.Video.TotalBitrate"].ValueAsObject.ToString();
        //                //Size
        //                movie.Size = properties["System.Size"].ValueAsObject == null ? "" : properties["System.Size"].ValueAsObject.ToString();
        //                movie.AcessTime = "";
        //            }
        //            else if (int.Parse(properties["System.Size"].ValueAsObject.ToString()) == 0)
        //            {
        //                movie.Name = properties.Where(prop => prop.CanonicalName == "System.ItemNameDisplayWithoutExtension").FirstOrDefault() == null ? "" : properties["System.ItemNameDisplayWithoutExtension"].ValueAsObject.ToString();
        //                //path
        //                movie.Path = properties.Where(prop => prop.CanonicalName == "System.ItemFolderNameDisplay").FirstOrDefault() == null ? "" : file.value;
        //                // Description
        //                movie.Description = "";
        //                //actor
        //                movie.Actor = properties.Where(prop => prop.CanonicalName == "System.Music.DisplayArtist").FirstOrDefault() == null ? "" : properties["System.Music.DisplayArtist"].ValueAsObject.ToString();
        //                //FrameWidth
        //                movie.FrameWidth = properties.Where(prop => prop.CanonicalName == "System.Video.FrameWidth").FirstOrDefault() == null ? "" : properties["System.Video.FrameWidth"].ValueAsObject.ToString();
        //                //FrameHeight
        //                movie.FrameHeight = properties.Where(prop => prop.CanonicalName == "System.Video.FrameHeight").FirstOrDefault() == null ? "" : properties["System.Video.FrameHeight"].ValueAsObject.ToString();
        //                //ContentType
        //                movie.ContentType = properties.Where(prop => prop.CanonicalName == "System.ContentType").FirstOrDefault() == null ? "" : properties["System.ContentType"].ValueAsObject.ToString();
        //                //IsDeleted
        //                movie.IsDeleted = true;
        //                //Rating
        //                movie.Rating = properties.Where(prop => prop.CanonicalName == "System.RatingText").FirstOrDefault() == null ? "" : properties["System.RatingText"].ValueAsObject.ToString();
        //                //TotalBitrate
        //                movie.TotalBitrate = properties.Where(prop => prop.CanonicalName == "System.Video.EncodingBitrate").FirstOrDefault() == null ? "" : properties["System.Video.EncodingBitrate"].ValueAsObject.ToString();
        //                //EncodingBitrate
        //                movie.EncodingBitrate = properties.Where(prop => prop.CanonicalName == "System.Video.TotalBitrate").FirstOrDefault() == null ? "" : properties["System.Video.TotalBitrate"].ValueAsObject.ToString();
        //                //Size
        //                movie.Size = properties["System.Size"].ValueAsObject == null ? "" : properties["System.Size"].ValueAsObject.ToString();
        //                movie.AcessTime = "";
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //            readMovies.Add(movie);
        //            Debug.WriteLine("Load file: " + movie.Name + ". Position: " + file.i + " / " + readFiles.Count);
        //        }
        //        return readMovies;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return new List<Movie>();
        //}

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