using UnityEngine;
using UnityEngine.UI;

public class RunJavaScriptInWebSceneManager : MonoBehaviour {
    public Text result;

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8

    private UniWebView _webView;
    private string _fileName = "UniWebViewDemo/demo.html";

    public void LoadFromFile() {
        if (_webView != null) {
            return;
        }
        
        _webView = CreateWebView();
        _webView.url = UniWebViewHelper.streamingAssetURLForPath(_fileName);

        // Set the height of webview half of the screen height.
        int bottomInset = UniWebViewHelper.screenHeight;
        _webView.insets = new UniWebViewEdgeInsets(0, 0, bottomInset / 2, 0);

        // `OnEvalJavaScriptFinished` will be called after you invoking some JavaScript function.
        _webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;

        _webView.OnWebViewShouldClose += (webView) => {
            _webView = null;
            return true;
        };

        _webView.Load();
        _webView.Show();
    }

    public void AddScript(InputField input) {
        if (_webView == null) {
            result.text = "Please open the web view first.";
            return;
        }
        // Use `AddJavaScript` to add some JavaScript function to the web page.
        // Be caution in Android it is a async method so you need to wait at least one frame to call the new added function.
        _webView.AddJavaScript(input.text);
    }

    public void RunScript(InputField input) {
        if (_webView == null) {
            result.text = "Please open the web view first.";
            return;
        }

        // Execute the JavaScript. The result will be returned in OnEvalJavaScriptFinished event.
        _webView.EvaluatingJavaScript(input.text);
    }

    void OnEvalJavaScriptFinished(UniWebView webView, string r) {
        result.text = r;
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
