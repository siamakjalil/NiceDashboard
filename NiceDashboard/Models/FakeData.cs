namespace NiceDashboard.Models
{
    public static class FakeData
    {
        public class Data
        {
            public int UserId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
        }

        public static async Task<Tuple<List<Data>,int>> GetFakeList(int skip , int take)
        {
            var client = new HttpClient();
            var res = await client.GetAsync("https://jsonplaceholder.typicode.com/albums");
            if (!res.IsSuccessStatusCode) return new Tuple<List<Data>, int>(new List<Data>(),0);
            var model = await res.Content.ReadFromJsonAsync<List<Data>>();
            return new Tuple<List<Data>, int>(model.OrderBy(u => u.Id).Skip(skip).Take(take).ToList(),model.Count);

        }
    }
    
}
