namespace apitest
{
    class UserGameCollector
    {
        static List<uint> gameIds = new List<uint>();
        static List<int> universeIds = new List<int>();
        static List<int> visits = new List<int>();

        static int GetRandomId() {
            // Selects a random id
            return new Random().Next(1, 1000000000);
        }

        //---------------------------MAIN---------------------------//

        static async Task Main() {

            try {
                // Creates a random ID
                int id = GetRandomId();
                Console.WriteLine("Player id: " + id);

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
                gameIds.Add(uint.Parse(dataRoot[i].GetProperty("rootPlace").GetProperty("id").ToString()));
            }

            universeIds = await VisitsCounter.GetUniverseIDs(gameIds);
            visits = await VisitsCounter.GetPlaceVisits(universeIds);

            // Gets the total visits
            int totalVisits = 0;
            foreach(int a in visits) {
                totalVisits += a;
            }

            Console.WriteLine(totalVisits + " total player visits");
        }

        public static JsonDocument CreateDocument(string jsonFile) {
            return JsonDocument.Parse(jsonFile);
        }
    }
}
