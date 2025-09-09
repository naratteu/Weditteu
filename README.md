# WEb EDITor by naraTTEU for local files

## 이게 뭡니까?

로컬파일을 읽거나 편집하는 과정에서 NativeGUI를 배제하고 웹에 의존하기위한 실험입니다.

## 어떻게 써볼수 있습니까?

윈도우 샌드박스 환경에서 테스트해보는걸 권장합니다.

- [WeditteuSetup.exe](https://github.com/naratteu/Weditteu/releases) 를 다운로드해 설치합니다.
- [sample.json.wdit](docs/samples/sample.json.wdit) 을 다운로드해 열어봅니다.

`.json.wdit` 파일이 웹앱스타일의 브라우저에서 열리며 파일내부의 `$wdit` 에 명시된 웹페이지에서 편집할수 있게됩니다.

## 한계

브라우저를 제어하여 `<input>` 태그에 file을 주입하는 과정은 자동화 가능하나,
`FileSystemHandle` 을 사용자대신 확보하는것에는 실패하여 파일로부터 앱을 열더라도 로컬파일에 직접적으로 접근하려면 `새 이름으로 저장`을 해야합니다.

- https://web.dev/native-file-system
- https://github.com/Amatewasu/browser-fs-access
- https://github.com/jimmywarting/native-file-system-adapter
- https://github.com/chromedp/chromedp/issues/528
- https://github.com/microsoft/playwright/issues/18267
- https://github.com/puppeteer/puppeteer/issues/7941

## Todo

- .xml.wdit
- .yaml.wdit
- .ipynb.wdit
- .dib.wdit
- .wdit.json ?
