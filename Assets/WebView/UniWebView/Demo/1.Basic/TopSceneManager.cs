using UnityEngine;

public class TopSceneManager : MonoBehaviour {

    public GameObject webview;
    public UnityEngine.UI.Text countDownText;

    int countDown = 10;

    // Use this for initialization
    void Start () {
        InvokeRepeating("Show", 1.0f, 1.0f);
    }

    void Show() {
        countDown -= 1;
        countDownText.text = "Show web view in " + countDown + "s";
        if (countDown == 0) {
            webview.SetActive(true);
            CancelInvoke();
        }
    }
}
