using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace YearInProgress.Logic
{
    public static class UpdateManager
    {
        public static async Task<Version> GetLatestVersionAsync()
        {
            using (HttpClient hc = new()
            {
                Timeout = TimeSpan.FromSeconds(10)
            })
            {
                hc.DefaultRequestHeaders.Add("User-Agent", $"YearInProgress/1.0 ({Constants.GITHUB_PROJECT_URL})");

                HttpResponseMessage response = await hc.GetAsync(Constants.GITHUB_LATEST_RELEASE_API_URL);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return default;
                }

                string json = await response.Content.ReadAsStringAsync();

                using (JsonDocument jDoc = JsonDocument.Parse(json))
                {
                    return new Version(jDoc.RootElement.GetProperty("tag_name").ToString().Replace("v", ""));
                }
            }
        }
    }
}
