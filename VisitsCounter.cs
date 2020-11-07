using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace apitest
{
  class VisitsCounter {

        public static async Task<List<int>> GetUniverseIDs(List<uint> GameIDs) {

            List<int> universeIds = new List<int>();

            // Transforms the place ids to universe ids
            for (int i = 0; i < GameIDs.Count; i++) {
                string url = "https://api.roblox.com/universes/get-universe-containing-place?placeid=" + GameIDs[i].ToString();
                string response = await HttpManager.CreateHttpRequest(url);

                try {
                    JsonDocument universeIdJson = UserGameCollector.CreateDocument(response);
                    int id = int.Parse(universeIdJson.RootElement.GetProperty("UniverseId").ToString());
                    universeIds.Add(id);
                }
                catch(Exception e) {}
            }

            return universeIds;
        }

        // Gets the place visits from the Universe ids 
        public static async Task<List<int>> GetPlaceVisits(List<int> UniverseIDs)
        {
            List<int> visits = new List<int>();
            for (int i = 0; i < UniverseIDs.Count; i++) {

                string url = "https://games.roblox.com/v1/games?universeIds=" + UniverseIDs[i].ToString();
                string response = await HttpManager.CreateHttpRequest(url);

                JsonDocument universeIdJson = UserGameCollector.CreateDocument(response);
                JsonElement visit = universeIdJson.RootElement.GetProperty("data")[0].GetProperty("visits");
                Console.WriteLine(visit);

                visits.Add(int.Parse(visit.ToString()));
            }

            return visits;
        }
    }
}
