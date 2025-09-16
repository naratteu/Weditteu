using System.Text.Json;
using System.Text.Json.Nodes;
using PuppeteerSharp;

var fetch = new BrowserFetcher().DownloadAsync();

string path = args[0];
JsonNode? wdit = null;
Func<IPage, Task>? postInit = null;
switch (path)
{
    case { } when path.EndsWith(".json.wdit") || path.EndsWith(".json"):
        await using (var stream = File.OpenRead(path))
        {
            using var jdoc = await JsonDocument.ParseAsync(stream);
            if (jdoc.RootElement.TryGetProperty("$wdit", out var w))
                wdit = JsonNode.Parse(w.GetRawText());
        }
        break;
    case { } when path.EndsWith(".txt.wdit") || path.EndsWith(".txt"):
        wdit = LGTMTextEditorPatch.URL;
        postInit = page => LGTMTextEditorPatch.Init(path, page);
        break;
}
var obj = wdit switch
{
    JsonObject o => o,
    JsonValue v => new() { ["--app"] = v },
    _ => []
};
await fetch;
/*await using*/var browser = await Puppeteer.LaunchAsync(new LaunchOptions
{
    Headless = false,
    Args = [.. obj.Select(kv => $"{kv.Key}={kv.Value}")]
});

var pages = await browser.PagesAsync();
var page = pages.Single();

if (postInit is { } pi) await pi(page);
else
{
    var fileInput = await page.WaitForSelectorAsync("input.weditteu");
    await Task.Delay(500);
    await fileInput.UploadFileAsync(path);
}