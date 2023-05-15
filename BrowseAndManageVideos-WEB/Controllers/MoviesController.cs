using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using BrowseAndManageVideos_WEB.Models;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace BrowseAndManageVideos_WEB.Controllers
{
    public class MoviesController
    {
        private SqlConnection dbConnection = DatabaseController.GetDataBaseConnection();
        List<BrowseAndManageVideos_WEB.Models.Movie> movieList = new List<BrowseAndManageVideos_WEB.Models.Movie>();

        private List<BrowseAndManageVideos_WEB.Models.Movie> GetMoviesFromFile(string folderPath)
        {
            try
            {
                // Số file đã đọc
                var readFiles = EnumerateFiles(folderPath, "*.*").ToList();
                // Khai báo list video để lưu
                List<BrowseAndManageVideos_WEB.Models.Movie> readMovies = new List<BrowseAndManageVideos_WEB.Models.Movie>();

                foreach (var file in readFiles.Select((value, i) => new { i, value }))
                {
                    ShellObject shellItem = ShellObject.FromParsingName(file.value);
                    ShellPropertyCollection properties = shellItem.Properties.DefaultPropertyCollection;

                    BrowseAndManageVideos_WEB.Models.Movie movie = new BrowseAndManageVideos_WEB.Models.Movie();
                    if (file.value.Contains(".mp4") || file.value.Contains(".ts"))
                    {
                        movie.Name = properties["System.ItemNameDisplayWithoutExtension"].ValueAsObject.ToString();
                        movie.Path = properties["System.ItemFolderNameDisplay"].ValueAsObject.ToString();
                        movie.Actor = properties["System.Music.DisplayArtist"].ValueAsObject.ToString();
                        movie.FrameWidth = properties["System.Video.FrameWidth"].ValueAsObject.ToString();
                        movie.FrameHeight = properties["System.Video.FrameHeight"].ValueAsObject.ToString();
                        movie.ContentType = properties["System.ContentType"].ValueAsObject.ToString();
                        movie.IsDeleted = false;
                        movie.Rating = properties["System.RatingText"].ValueAsObject.ToString();
                        movie.TotalBitrate = properties["System.Video.EncodingBitrate"].ValueAsObject.ToString();
                        movie.EncodingBitrate = properties["System.Video.TotalBitrate"].ValueAsObject.ToString();
                        movie.Size = properties["System.Size"].ValueAsObject.ToString();
                    }
                    else
                    {
                        movie.Name = properties["System.ItemNameDisplayWithoutExtension"].ValueAsObject.ToString();
                        movie.Path = properties["System.ItemFolderNameDisplay"].ValueAsObject.ToString();
                        movie.Actor = properties["System.Music.DisplayArtist"].ValueAsObject.ToString();
                        movie.IsDeleted = true;
                    }
                    readMovies.Add(movie);
                }
                return readMovies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new List<BrowseAndManageVideos_WEB.Models.Movie>();
        }

        private List<BrowseAndManageVideos_WEB.Models.Movie> GetMoviesFromDatabase()
        {
            try
            {
                using (dbConnection)
                {
                    string querryString = "Select * from Movies";
                    SqlCommand sqlCommand = new SqlCommand(querryString, dbConnection);
                    List<BrowseAndManageVideos_WEB.Models.Movie> readMovies = new List<BrowseAndManageVideos_WEB.Models.Movie>();
                    dbConnection.Open();
                    using (SqlDataReader oReader = sqlCommand.ExecuteReader())
                    {
           
                        while (oReader.Read())
                        {
                            BrowseAndManageVideos_WEB.Models.Movie movie = new BrowseAndManageVideos_WEB.Models.Movie();
                            movie.Id = int.Parse(oReader["Id"].ToString());
                            movie.Name = oReader["Name"].ToString();
                            movie.Path = oReader["Path"].ToString();
                            movie.Actor = oReader["Actor"].ToString();
                            movie.FrameWidth = oReader["FrameWitdh"].ToString();
                            movie.FrameHeight = oReader["FarmeHeight"].ToString();
                            movie.ContentType = oReader["ContentType"].ToString();
                            if (oReader["IsDeleted"] is bool)
                            {
                                movie.IsDeleted = true;
                            }
                            else
                            {
                                movie.IsDeleted = false;
                            }
                            movie.Rating = oReader["Rating"].ToString();
                            movie.TotalBitrate = oReader["TotalBitrate"].ToString();
                            movie.EncodingBitrate = oReader["EncodingBitrate"].ToString();
                            movie.Size = oReader["Size"].ToString();
                            readMovies.Add(movie);
                        }
                        dbConnection.Close();
                    }
                    return readMovies;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new List<BrowseAndManageVideos_WEB.Models.Movie>();
        }

        public bool WriteMoviesToDatabase(BrowseAndManageVideos_WEB.Models.Movie movie)
        {
            try
            {
                using (dbConnection)
                {
                    string querryString = 
                        "INSERT INTO [dbo].[Movies] " +
                        "(Name)" +
                        "(Path)" +
                        "(Actor)" +
                        "(FrameWitdh)" +
                        "(FarmeHeight)" +
                        "(ContentType)" +
                        "(IsDeleted)" +
                        "(Rating)" +
                        "(TotalBitrate)" +
                        "(EncodingBitrate)" +
                        "(Size)" +
                        "values" +
                        "(@name)" +
                        "(@path)" +
                        "(@actor)" +
                        "(@frameWitdh)" +
                        "(@farmeHeight)" +
                        "(@contentType)" +
                        "(@isDeleted)" +
                        "(@rating)" +
                        "(@totalBitrate)" +
                        "(@encodingBitrate)" +
                        "(@size)";
                    SqlCommand sqlCommand = new SqlCommand(querryString, dbConnection);
        
                    sqlCommand.Parameters.Add("@name", SqlDbType.NChar);
                    sqlCommand.Parameters["@name"].Value= movie.Name;

                    sqlCommand.Parameters.Add("@path", SqlDbType.NChar);
                    sqlCommand.Parameters["@path"].Value = movie.Path;

                    sqlCommand.Parameters.Add("@actor", SqlDbType.NChar);
                    sqlCommand.Parameters["@actor"].Value = movie.Actor;

                    sqlCommand.Parameters.Add("@frameWitdh", SqlDbType.NChar);
                    sqlCommand.Parameters["@frameWitdh"].Value = movie.FrameWidth;

                    sqlCommand.Parameters.Add("@farmeHeight", SqlDbType.NChar);
                    sqlCommand.Parameters["@farmeHeight"].Value = movie.FrameHeight;

                    sqlCommand.Parameters.Add("@farmeHeight", SqlDbType.NChar);
                    sqlCommand.Parameters["@contentType"].Value = movie.ContentType;

                    sqlCommand.Parameters.Add("@isDeleted", SqlDbType.Bit);
                    sqlCommand.Parameters["@isDeleted"].Value = movie.IsDeleted;

                    sqlCommand.Parameters.Add("@rating", SqlDbType.NChar);
                    sqlCommand.Parameters["@rating"].Value = movie.Rating;

                    sqlCommand.Parameters.Add("@totalBitrate", SqlDbType.NChar);
                    sqlCommand.Parameters["@totalBitrate"].Value = movie.TotalBitrate;

                    sqlCommand.Parameters.Add("@encodingBitrate", SqlDbType.NChar);
                    sqlCommand.Parameters["@encodingBitrate"].Value = movie.EncodingBitrate;

                    sqlCommand.Parameters.Add("@size", SqlDbType.NChar);
                    sqlCommand.Parameters["@size"].Value = movie.Size;
                    dbConnection.Open();
                    return sqlCommand.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public static IEnumerable<string> EnumerateFiles(string root, string searchPattern)
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


    }
}
