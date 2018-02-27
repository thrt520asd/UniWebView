using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Tools.WebView ;
public class NGUITest : MonoBehaviour {
	// Use this for initialization
	public UIWidget referenceRect ;
	public UIInput input ;
	private NGUIWebView nguiWebView ;
	private UILabel label ;
	private GUIStyle style ;
	void Start(){
		UIButton btn  = transform.Find("panel/button").GetComponent<UIButton>();
		UIButton btn2 = transform.Find("panel/button2").GetComponent<UIButton>();
		UIEventListener.Get(btn.gameObject).onClick = OnClick ;
		UIEventListener.Get(btn2.gameObject).onClick = OnClick2 ;
		input = GetComponentInChildren<UIInput>();
		input.value = "http://www.baidu.com";	
		label = transform.Find("panel/label").GetComponent<UILabel>();
//		 Invoke("ShowWebView" , 0.1f);
//		 Invoke("CloseView" , 5);
//		 Invoke("ShowWebView" , 6);
		style = new GUIStyle ();
		style.fontSize = 40;
		
	}

	private void DisposeWebView(){
		nguiWebView.DisposeView();
	}

	private void DestoryView(){
		Destroy(nguiWebView.gameObject);
		nguiWebView = null ;
	}

	private void OnClick(GameObject btn ){
		ShowWebView();
	}

	private void ShowWebView(){

		if(nguiWebView == null){
			GameObject go = new GameObject("webView");
			nguiWebView = go.AddComponent<NGUIWebView>();
			nguiWebView.SetOnLoadComplete(OnLoadComplete);
			nguiWebView.SetOnClose(OnClose);
			nguiWebView.SetOnMsg(Message);
		}
		nguiWebView.ShowTooBar(true);
		nguiWebView.SetInsetFormWidget(referenceRect);
		nguiWebView.Load(input.value);
		var inset = nguiWebView.webView.insets;
		label.text = "top:"+inset.top.ToString() + " left :"+inset.left.ToString() + " down :"+inset.bottom.ToString()+" right :"+inset.right.ToString();
	}

	void OnGUI(){
		
		GUI.Label (new Rect (0, 0, 100, 100 ), "h:" + Screen.height + "  w:" + Screen.width , style);
	}

	private void CloseView(){
		Debug.Log("close    11111") ;
		nguiWebView.HideView();
	}

	private void OnClick2(GameObject go){
		int top =int.Parse(transform.Find("panel/top").GetComponent<UIInput>().value);
		int left =int.Parse(transform.Find("panel/left").GetComponent<UIInput>().value);
		int bottom =int.Parse(transform.Find("panel/bottom").GetComponent<UIInput>().value);
		int right =int.Parse(transform.Find("panel/right").GetComponent<UIInput>().value);
		nguiWebView.SetInsets(top , left , bottom , right);
	}

	private void OnLoadComplete(bool b ){
		Debug.Log("load complete"+b.ToString()) ;
	}

	private void OnClose(){
		Debug.Log(" cloes ") ;
	}

	private void Message(string path , Dictionary<string , string> args = null){
		Debug.Log("path"+path) ;
		string content = "" ;
		foreach(var keyvaluePair in args){
			content+="key ："+keyvaluePair.Key + " value:"+keyvaluePair.Value ;
		}
		Debug.Log("content" + content);
	}
	
	

}
