using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web_Application_Get.Models
{
    public sealed class DBConnection
    {
        private static DBConnection instance = null;
        private string conStr;
        private SqlConnection con;

        private DBConnection()
        {

            conStr = ConfigurationManager.ConnectionStrings["Aviv_Local"].ConnectionString;
            con = new SqlConnection(conStr);
        }

        public static DBConnection GetInstance() 
        {
            if (instance == null)
            {
                instance = new DBConnection();
            }
            return instance;
        }

        public List<Movie> GetMovies()
        {
            SqlCommand cmd = new SqlCommand("SelectMoviesTable", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            List<Movie> movies = new List<Movie>();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                movies.Add(new Movie(dr["id"].ToString(), dr["title"].ToString(), dr["category"].ToString(), dr["release_date"].ToString()));
            }
            con.Close();
            return movies;
        }

        public Movie GetMovieById(string id)
        {
            SqlCommand cmd = new SqlCommand("SelectMovie", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", SqlDbType.NVarChar).Value = id;
            con.Open();
            Movie movie = null;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                movie = new Movie(dr["id"].ToString(), dr["title"].ToString(), dr["category"].ToString(), dr["release_date"].ToString());
            }
            con.Close();
            return movie;
        }

        public Movie PostMovie(Movie movie)
        {
            List<Movie> movies = GetMovies();
            int index = 0;
            if (movies.Count > 0)
            {
                index = int.Parse(movies[movies.Count-1].ID);
            }
            Movie m = new Movie((index+1).ToString(), movie.Title, movie.Category, movie.ReleaseDate );
            SqlCommand cmd = new SqlCommand("AddMovie", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", SqlDbType.NVarChar).Value = m.ID;
            cmd.Parameters.AddWithValue("@title", SqlDbType.NVarChar).Value = m.Title;
            cmd.Parameters.AddWithValue("@category", SqlDbType.NVarChar).Value = m.Category;
            cmd.Parameters.AddWithValue("@release_date", SqlDbType.NVarChar).Value = m.ReleaseDate;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return m;
        }

        public Movie PutMovie(int id, Movie movie)
        {
            SqlCommand cmd = new SqlCommand("UpdateMovieDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", SqlDbType.NVarChar).Value = id.ToString();
            cmd.Parameters.AddWithValue("@title", SqlDbType.NVarChar).Value = movie.Title;
            cmd.Parameters.AddWithValue("@category", SqlDbType.NVarChar).Value = movie.Category;
            cmd.Parameters.AddWithValue("@release_date", SqlDbType.NVarChar).Value = movie.ReleaseDate;
            con.Open();
            int index = cmd.ExecuteNonQuery();
            con.Close();
            if (index == 0)
            {
                throw new Exception("Cannot Update. movie does not exist!");
            }
            movie.ID = id.ToString();
            return movie;
        }

        public int DeleteMovie(int id)
        {
            SqlCommand cmd = new SqlCommand("DeleteMovie", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", SqlDbType.NVarChar).Value = id.ToString();
            con.Open();
            int index = cmd.ExecuteNonQuery();
            con.Close();
            if (index == 0)
            {
                throw new Exception("Cannot delete. movie does not exist!");
            }
            return index;
        }
    }
}
