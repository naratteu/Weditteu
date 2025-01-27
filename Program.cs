using System.Text.Json;
using System.Text.Json.Nodes;
using PuppeteerSharp;

var fetch = new BrowserFetcher().DownloadAsync();

string path = args[0];
JsonNode? wdit = null;
await using (var stream = File.OpenRead(path))
{
    var jdoc = await JsonDocument.ParseAsync(stream);
    if (jdoc.RootElement.TryGetProperty("$wdit", out var w))
        wdit = JsonNode.Parse(w.GetRawText());
    wdit ??= new JsonObject();
}
var obj = wdit switch
{
    JsonObject o => o,
    JsonValue v => new() { ["--app"] = v },
};
await fetch;
/*await using*/var browser = await Puppeteer.LaunchAsync(new LaunchOptions
{
    Headless = false,
    Args = [.. obj.Select(kv => $"{kv.Key}={kv.Value}")]
});

var pages = await browser.PagesAsync();
var page = pages.Single();

var fileInput = await page.WaitForSelectorAsync("input.weditteu");
await Task.Delay(500);
await fileInput.UploadFileAsync(path);