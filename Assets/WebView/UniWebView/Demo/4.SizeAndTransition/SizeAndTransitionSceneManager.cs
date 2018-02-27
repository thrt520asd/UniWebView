using UnityEngine;
using UnityEngine.UI;

public class SizeAndTransitionSceneManager : MonoBehaviour {

#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8

    private UniWebView _webView;
    private bool _webViewReady;

    public bool fade {get; set;}
    public int transitionEdge {get; set;}

    public InputField top;
    public InputField left;
    public InputField bottom;
    public InputField right;

    public InputField x;
    public InputField y;
    public InputField width;
    public InputField height;

    void Start () {
        _webView = CreateWebView();
        _webView.Load("https://example.com");
    }

    public void ShowClicked() {
        if (_webViewReady) {
            // Call Show() or hide on the web view with transition option.
            // fade: Whether you need a fade effect when showing this web view.
            // transitionEdge: From which side of the screen you want the web view transit in.
            // duration: The duration of your animation effect.
            // Callback: will be called after the transition animation finishes.
            _webView.Show(fade, (UniWebViewTransitionEdge)transitionEdge, 0.4f, ()=>{
                Debug.Log("Show Finished.");
            });
            _webView.ShowToolBar(true);
            
        } else {
            Debug.Log("Web view is not prepared yet. Trying to reload.");
            _webView.Load("https://example.com");
        }
    }

    public void SetInsetsClicked() {
        var topValue = string.IsNullOrEmpty(top.text) ? 0 : int.Parse(top.text);
        var leftValue = string.IsNullOrEmpty(left.text) ? 0 : int.Parse(left.text);
        var bottomValue = string.IsNullOrEmpty(bottom.text) ? 0 : int.Parse(bottom.text);
        var rightValue = string.IsNullOrEmpty(right.text) ? 0 : int.Parse(right.text);
        
        // By setting the insets of web view, you can control the size of the web view.
        _webView.insets = new UniWebViewEdgeInsets(topValue, leftValue, bottomValue, rightValue);
    }
    
    UniWebView CreateWebView() {
        var webViewGameObject = GameObject.Find("WebView");
        if (webViewGameObject == null) {
            webViewGameObject = new GameObject("WebView");
        }
        
        var webView = webViewGameObject.AddComponent<UniWebView>();

        // We just set a ready flag to make sure we could show a page when the "Open" button is clicked.
        webView.OnLoadComplete += (view, success, errorMessage) => {
            if (success) {
                _webViewReady = true;
            } else {
                Debug.LogError("Loading failed: " + errorMessage);
            }
        };

        // The `OnWebViewShouldClose` will be called when user pressed Back button or Done button to exit the web view.
        // It will hide the web view without animation if you do not listen to this method or return true in this method.

        // If you want customized transition for hide,
        // You should at least listen to OnWebViewShouldClose, call Hide() with animation options and return false. 
        webView.OnWebViewShouldClose += (view) => {
            webView.Hide(fade, (UniWebViewTransitionEdge)transitionEdge, 0.4f, ()=>{
                Debug.Log("Hide Finished.");
            });
            webView.HideToolBar(true);
            return false;
        };

        return webView;
    }

#else //End of #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
    void Start() {
        Debug.LogWarning("UniWebView only works on iOS/Android/WP8. Please switch to these platforms in Build Settings.");
    }
#endif
}
