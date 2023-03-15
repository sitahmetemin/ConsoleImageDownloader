using IronXL;
using System.Text;

public class Program
{
    private static void Main(string[] args)
    {
        var dwnld = new ImageGetAndDownloadService();
        dwnld.DowloadAndSave();

        Console.WriteLine("İşlem Bitti");
        Console.ReadLine();
    }
}

public class ImageGetAndDownloadService
{
    public const string UrlPattern = "https://source.file.url.information.right.here";

    public void DowloadAndSave()
    {
        WorkBook workBook = WorkBook.Load("C:\\file\\path\\excel\\file.xlsx");
        var cells = workBook.GetWorkSheet("Sayfa1");

        StringBuilder failedCodes = new StringBuilder();

        foreach (var item in cells["A:A"])
        {
            using var client = new HttpClient();

            var responseService = client.GetAsync(string.Format(UrlPattern, item)).Result;

            var byteArrayImage = responseService.Content.ReadAsByteArrayAsync().Result;

            if (responseService.StatusCode != System.Net.HttpStatusCode.OK)
            {
                failedCodes.AppendLine(string.Join(" - ", item.Text, responseService.StatusCode.ToString()));
                Console.WriteLine($"{item.Text} ERROR!! -> {responseService.StatusCode}");
            }
            else
            {
                File.WriteAllBytesAsync($"C:\\folder\\path\\and\\file\\name.png", byteArrayImage);
                Console.WriteLine($"{item.Text} ok");
            }
        }

        File.WriteAllText($"C:\\folder\\path\\and\\file\\name\\for\\fail.txt", failedCodes.ToString());
    }
}