using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace apitest
{
    class UserGameCollector
    {
        static List<string> gameIds = new List<string>();
        static List<string> universeIds = new List<string>();
        static List<string> visits = new List<string>();
        static List<string> names = new List<string>();

        static int GetRandomId() {
            // Selects a random id
            return new Random().Next(1, 1000000000);
        }

        //---------------------------MAIN---------------------------//

        static async Task Main() {

            try {
                // Creates a random ID
                int id = GetRandomId();
                Console.WriteLine("\nPlayer id: " + id);

                string url = "https://games.roblox.com/v2/users/" + id + "/games?sortOrder=Asc&limit=100";
                string response = await HttpManager.CreateHttpRequest(url);

                Task.Run(() => GetPlayerGames(response)).Wait();
            }
            catch(Exception e) { Console.WriteLine("Exception\n"+e.Message); }
        }

        //---------------------------FUNCTIONS---------------------------//

        static async Task GetPlayerGames(string json)
        {
            JsonDocument document = CreateDocument(json);

            JsonElement data = document.RootElement;
            JsonElement dataRoot = data.GetProperty("data");

            for (int i = 0; i < dataRoot.GetArrayLength(); i++) {
                gameIds.Add(dataRoot[i].GetProperty("rootPlace").GetProperty("id").ToString());
                Console.WriteLine(gameIds[i] + ": " + dataRoot[i].GetProperty("name"));
                names.Add(dataRoot[i].GetProperty("name").ToString());
            }
            Console.WriteLine("\n");

            universeIds = await VisitsCounter.GetUniverseIDs(gameIds);
            visits = await VisitsCounter.GetPlaceVisits(universeIds);

            int total = 0;
            int id = 0;
            foreach (string v in visits) {
                total += int.Parse(v);
                Console.WriteLine("[Visits: "+v+"]: " + names[id]);
                id++;
            }

            Console.WriteLine("\n["+total + "]: total visits");
        }

        public static JsonDocument CreateDocument(string jsonFile) {
            return JsonDocument.Parse(jsonFile);
        }
    }
}
