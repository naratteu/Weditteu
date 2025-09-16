using PuppeteerSharp;

static class LGTMTextEditorPatch
{
    public const string URL = "https://googlechromelabs.github.io/text-editor";
    public static async Task Init(string path, IPage page)
    {
        await page.ExposeFunctionAsync("wdit_name", () => Task.FromResult(path));
        await page.ExposeFunctionAsync("wdit_read", () => File.ReadAllTextAsync(path));
        await page.ExposeFunctionAsync("wdit_write", async (string msg) =>
        {
            await File.WriteAllTextAsync(path, msg);
            return 0; //void 반환시 브라우저에서 에러 발생. todo: PuppeteerSharp에 이슈남겨야함.
        });
        await page.EvaluateExpressionAsync("""
        {
            const original = window.showOpenFilePicker;
            window.showOpenFilePicker = async () => {
                window.showOpenFilePicker = original; // 자동화를 위해 최초1회만 대체하고 되돌립니다.
                const name = await wdit_name();
                return [{
                    name,
                    getFile: async () => new File([await wdit_read()], name),
                    createWritable: async () => ({
                        write: async msg => await wdit_write(msg),
                        close: async () => {}
                    })
                }];
            };
        }
        """);
        await page.WaitForFunctionAsync("() => typeof app === 'object'");
        await page.EvaluateExpressionAsync("app.openFile()");
        await page.Browser.Process.WaitForExitAsync();
    }
}