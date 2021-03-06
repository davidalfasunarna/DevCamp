﻿using DevCamp.WebApp.Utils;
using IncidentAPI;
using IncidentAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevCamp.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<ActionResult> Index()
        {
            //TODO: BEGIN Replace with API Data code
            //##### API DATA HERE #####
            List<Incident> incidents;
            using (var client = IncidentApiHelper.GetIncidentAPIClient())
            {
                //TODO: BEGIN ADD Caching
                //##### ADD CACHING HERE #####
                //##### Add caching here #####
                int CACHE_EXPIRATION_SECONDS = 60;

                //Check Cache
                string cachedData = string.Empty;
                if (RedisCacheHelper.UseCachedDataSet(Settings.REDISCCACHE_KEY_INCIDENTDATA, out cachedData))
                {
                    incidents = JsonConvert.DeserializeObject<List<Incident>>(cachedData);
                }
                else
                {
                    //If stale refresh
                    var results = await client.IncidentOperations.GetAllIncidentsAsync();
                    Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)results;
                    incidents = ja.ToObject<List<Incident>>();
                    RedisCacheHelper.AddtoCache(Settings.REDISCCACHE_KEY_INCIDENTDATA, incidents, CACHE_EXPIRATION_SECONDS);
                }
                //##### Add caching here #####
                //TODO: END ADD Caching
            }
            return View(incidents);
            //##### API DATA HERE #####
            //TODO: END Replace with API Data code
        }
    }
}