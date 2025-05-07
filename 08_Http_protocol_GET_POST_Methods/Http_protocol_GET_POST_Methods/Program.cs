
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Post
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
}
internal class Program
{
    private static async Task Main(string[] args)
    {
        // Get

        /*var url = "https://jsonplaceholder.typicode.com/users";
        using var client = new HttpClient();

        var response = await client.GetAsync(url);

        Console.WriteLine($"Status : {response.StatusCode}");
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);*/

        // Post
        Post post = new()
        {
            Title = "TestTitle",
            Body = "TestBody",
            UserId = 1
        };

        var json = JsonSerializer.Serialize(post);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var url = "https://jsonplaceholder.typicode.com/posts";

        using var client = new HttpClient();
        var response = await client.PostAsync(url, data);

        Console.WriteLine($"Status : {response.StatusCode}");
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);


    }
}