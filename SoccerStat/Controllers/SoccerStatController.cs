using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SoccerDataAccess;

namespace SoccerStat.Controllers
{
    public class SoccerStatController : ApiController
    {
        public List<PlayerStat> Get() 
        {
            using (SoccerStatsEntities entities = new SoccerStatsEntities()) 
            {
                return entities.PlayerStats.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (SoccerStatsEntities entities = new SoccerStatsEntities())
            {
                var entity = entities.PlayerStats.FirstOrDefault(p => p.Id == id);

                if (entity != null) {

                    return Request.CreateResponse(HttpStatusCode.OK, entity);                    
                 }
                else 
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Player with id " + id.ToString() + " not found.");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] PlayerStat player) 
        {
            try
            {
                using (SoccerStatsEntities entities = new SoccerStatsEntities())
                {
                    entities.PlayerStats.Add(player);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, player);
                    message.Headers.Location = new Uri(Request.RequestUri + player.Id.ToString());

                    return message;
                }
            }
            catch (Exception e) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,e);
            }
        }


        public HttpResponseMessage Delete(int id) 
        {
            try
            {
                using (SoccerStatsEntities entities = new SoccerStatsEntities())
                {
                    var entity = entities.PlayerStats.FirstOrDefault(p => p.Id == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Player with id " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.PlayerStats.Remove(entity);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

        }

    }
}
