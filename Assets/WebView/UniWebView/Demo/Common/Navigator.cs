using UnityEngine;

#if !(UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
using UnityEngine.SceneManagement;
#endif

public class Navigator : MonoBehaviour {
    public void NavigateTo(int level) {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel(level);
#else
        SceneManager.LoadScene(level);
#endif
    }
}
