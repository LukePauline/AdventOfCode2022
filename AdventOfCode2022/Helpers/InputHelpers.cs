using System.Net;
using System.Net.Http;
using System.Text;

namespace AdventOfCode2022.Helpers
{
    public static class InputHelpers
    {
        public static StreamReader ConvertStringToStream(string input) => new(new MemoryStream(Encoding.UTF8.GetBytes(input)));

        public static async Task<StreamReader> GetInput(int day)
        {
            string file = $"day{day}.txt";

            if (!File.Exists(file))
            {
                string input = await GetInputFromUrl(day);
                using StreamWriter writer = new(file);
                writer.Write(input);
            }

            return new StreamReader(file);
        }

        private static async Task<string> GetInputFromUrl(int day)
        {
            string sessionCookie = File.ReadAllText("cookie.txt");

            var client = new HttpClient();
            HttpRequestMessage request = new(HttpMethod.Get, $"https://adventofcode.com/2022/day/{day}/input");
            request.Headers.TryAddWithoutValidation("Cookie", $"session={sessionCookie}");
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
