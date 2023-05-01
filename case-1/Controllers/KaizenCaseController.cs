using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace kaizen_case.Controllers;

[ApiController]
[Route("[controller]")]
public class KaizenCaseController : ControllerBase
{
    static List<string> resultList = new List<string>();

    private static string GenerateString(int length)
    {
        string result = "";
        const string characters = "ACDEFGHKLMNPRTXYZ234579";
        Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            result += characters[random.Next(characters.Length)];
        }

        var isValid = Regex.IsMatch(result, "[ACDEFGHKLMNPRTXYZ234579]{8}");
        if (isValid)
            resultList.Add(result);

        return result;
    }

    [HttpPost("[action]")]
    public List<string> GenerateCodes()
    {
        for (int a = 0; a < 1000; a++)
        {
            GenerateString(8);
        }

        var groupByArr = resultList.GroupBy(s => s).ToDictionary(g => g.Key, g => g.Count());
        Console.WriteLine("Check arr length: " + groupByArr.Keys.Count);

        return resultList;
    }

    [HttpPost("[action]")]
    public string CheckCode(string code)
    {
        var checkCode = resultList.FirstOrDefault(r => r == code);

        return checkCode == null ? "Kod Gecersiz" : "Kod Kullanilabilir";
    }
}
