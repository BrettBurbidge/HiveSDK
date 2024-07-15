using System.Text.Json;
using Node.SDK;

public class HiveServerClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    public HiveServerClient(string baseUrl)
    {
        _httpClient = new HttpClient();
        _baseUrl = baseUrl.TrimEnd('/');
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<NodeAction> CheckForWorkAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/WorkMonitor");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            
            //SDK & BRAND NODE IMPLEMENTATION VERSION SPECIFIC CODE... HOW TO MANAGE THIS?

            if(!string.IsNullOrEmpty(responseBody))
            {
                NodeAction action = JsonSerializer.Deserialize<NodeAction>(responseBody, _jsonOptions);

                Console.WriteLine($"Work response for action: {action?.Name}");
                return action;
            } else
            {
                Console.WriteLine($"No action found to perform.");
                return null;
            }
        }

        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error checking for work with HiveServer: {e.Message}");
            return null;
        }
    }
}