using System.Text;
using Newtonsoft.Json;

FileStream fileStream = new FileStream("response.json", FileMode.Open);

string file = string.Empty;

using (StreamReader reader = new StreamReader(fileStream))
{
    file = reader.ReadToEnd();
}

var response = JsonConvert.DeserializeObject<List<ResultModel>>(file);


var endOfY = response.FirstOrDefault().BoundingPoly.Vertices.Last().Y;

response.Remove(response.First());
response = response.OrderBy(r => r.BoundingPoly.Vertices.LastOrDefault().Y).ToList();

StringBuilder sb = new StringBuilder();
var count = 1;


foreach (var item in response)
{
    var indexOfY = response.IndexOf(item);

    if (indexOfY + 1 >= response.Count)
        continue;

    var nextItem = response[indexOfY + 1];

    var yValue = item.BoundingPoly.Vertices.LastOrDefault().Y;
    var nextYValue = nextItem.BoundingPoly.Vertices.LastOrDefault().Y;

    if (nextYValue - yValue >= 17)
    {
        if (!sb.ToString().Contains(item.Description))
            sb.Append($"{count} - {item.Description}");

        sb.Append($"\n");
        sb.Append($"{count + 1} - {nextItem.Description}");
        count++;
    }
    else
    {
        sb.Append($" {nextItem.Description}");
    }
}

System.Console.WriteLine(sb.ToString());

public class ResultModel
{
    [JsonProperty("locale")]
    public string Locale { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("boundingpoly")]
    public BoundingPolyModel BoundingPoly { get; set; }
}


public class BoundingPolyModel
{
    [JsonProperty("vertices")]
    public List<VerticeModel> Vertices { get; set; }
}

public class VerticeModel
{
    [JsonProperty("x")]
    public int X { get; set; }

    [JsonProperty("y")]
    public int Y { get; set; }
}