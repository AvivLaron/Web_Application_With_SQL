using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_Application_Get.Models;

namespace Web_Application_Get.Controllers
{
    public class MoviesController : ApiController
    {
        private DBConnection dbc = DBConnection.GetInstance();
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(dbc.GetMovies());
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("api/Movies/{id}")]
        public IHttpActionResult GetMovie(int id)
        {
            try
            {
                Movie mv = dbc.GetMovieById(id.ToString());
                if (mv == null)
                {
                    return Content(HttpStatusCode.NotFound, $"Movie with id {id} was not found");
                }
                return Ok(mv);
            }
            catch (Exception ex)
            {

                return Content(HttpStatusCode.InternalServerError, ex);
            }
        }


        public IHttpActionResult Post([FromBody] Movie value)
        {
            try
            {
                if (value == null)
                {
                    throw new NullReferenceException("Cannot insert null!");
                }
                Movie mv = dbc.PostMovie(value);
                return Created(new Uri(Request.RequestUri.AbsoluteUri + mv.ID), mv);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        [Route("api/Movies/{id}")]
        public IHttpActionResult Put(int id, [FromBody] Movie value)
        {
            try
            {
                if (value == null)
                {
                    throw new NullReferenceException("Cannot update null object!");
                }
                Movie mv = dbc.PutMovie(id, value);
                return Ok(mv);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/Movies/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                int res = dbc.DeleteMovie(id);
                string message = $"{res} rows affected";
                return Ok(message);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}

