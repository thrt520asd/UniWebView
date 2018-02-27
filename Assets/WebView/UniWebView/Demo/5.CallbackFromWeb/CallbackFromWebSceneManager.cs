using UnityEngine;
using UnityEngine.UI;

public class CallbackFromWebSceneManager : MonoBehaviour {
    public Text result;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8

    private UniWebView _webView;
    private string _fileName = "UniWebViewDemo/callback.html";

    public void LoadFromFile() {
        if (_webView != null) {
            return;
        }

        _webView = CreateWebView();
        _webView.url = UniWebViewHelper.streamingAssetURLForPath(_fileName);

        // Set the height of webview half of the screen height.
        int bottomInset = UniWebViewHelper.screenHeight;
        _webView.insets = new UniWebViewEdgeInsets(0, 0, bottomInset / 2, 0);

        // If a url with format of "uniwebview://yourPath?param1=value1&param2=value2" clicked, 
        // a message will be sent from your web page to Unity,
        // the OnReceivedMessage event will get raised with a "UniWebViewMessage" object as argument.
        _webView.OnReceivedMessage += OnReceivedMessage;

        _webView.Load();
        _webView.Show();
    }

    void OnReceivedMessage(UniWebView webView, UniWebViewMessage message) {

        // You can check the message path and arguments to know which `uniwebview` link is clicked.
        // UniWebView will help you to parse your link if it follows the url argument format.
        // However, there is also a "rawMessage" property you could use if you need to use some other formats and want to parse it yourself.
        if (message.path == "close") {
            result.text = "";
            Destroy(webView);
            _webView = null;
        }

        if (message.path == "add") {
            int num1 = 0;
            int num2 = 0;

            // num1 and num2 will be got from the url argument.
            if (int.TryParse(message.args["num1"], out num1) && int.TryParse(message.args["num2"], out num2)) {
                int sum = num1 + num2;
                result.text = num1 + " + " + num2 + " = " + sum;
            } else {
                result.text = "Invalid Input";
            }
        }
    }

    UniWebView CreateWebView() {
        var webViewGameObject = GameObject.Find("WebView");
        if (webViewGameObject == null) {
            webViewGameObject = new GameObject("WebView");
        }
        
        var webView = webViewGameObject.AddComponent<UniWebView>();
        
        webView.toolBarShow = true;
        return webView;
    }

#else
    void Start() {
        Debug.LogWarning("UniWebView only works on iOS/Android/WP8. Please switch to these platforms in Build Settings.");
    }
#endif
}
