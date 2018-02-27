// ----------------------------------------------------------------------------
// <copyright file="CameraShaker.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>21/11/2016</date>
namespace Assets.Scripts.Tools.WebView
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	public class NGUIWebView  :MonoBehaviour{
		[SerializeField]
		private UIWidget referenceWidget;

		private UniWebView _webView  = null ;
		private System.Action<bool> m_onLoadComplete = null ;
		private System.Action m_onClose = null ;
		private System.Action<string , Dictionary<string , string >> m_onReciveMsg = null ;
		private bool isNeedResume = false ;	
		public UniWebView webView{
			get{
				if(_webView == null){
					_webView = this.gameObject.AddComponent<UniWebView>();
					_webView.autoShowWhenLoadComplete = true ;
					_webView.OnLoadComplete += OnLoadComplete;
					_webView.OnWebViewShouldClose += OnClose;
					_webView.OnReceivedMessage += OnReciveMsg ;	
					_webView.toolBarShow = true;
				}
				return _webView ;
			}
		}
		
		public void SetOnLoadComplete(System.Action<bool> onLoadComplete){
			m_onLoadComplete = onLoadComplete ;
//			Debug.Log ("add SetOnLoadComplete");
		}
		
		public void SetOnClose(System.Action onClose  ){
			m_onClose = onClose ;
//			Debug.Log ("add SetOnClose");
		}
		
		public void SetOnMsg(System.Action<string , Dictionary<string , string >> onReciveMsg ){
			m_onReciveMsg = onReciveMsg ;
//			Debug.Log ("add SetOnMsg");
		}
		
		public void ShowTooBar(bool isShow){
			webView.ShowToolBar(isShow) ;
		}
		
		public void ReLoad(){
			if (string.IsNullOrEmpty (webView.url))
				return;
			webView.Load();
			RefreshInsets ();
		}
		
		public void Load(string url ){
			if (string.IsNullOrEmpty (url))
				return;
//			Debug.Log ("webview load url : " + url);
			webView.Load(url) ;
			RefreshInsets ();
		}

		public void SetInsetFormWidget(UIWidget widget){
			this.referenceWidget = widget;
			WebViewSizeFilterForNGUI sizeFilter = new WebViewSizeFilterForNGUI (referenceWidget);
			int[] insets = sizeFilter.GetEdgeInsets ();
			SetInsets(insets[0] , insets[1] , insets[2] , insets[3]);
		}

		public void SetInsets(int top , int left , int bottom , int right){
			#if UNITY_IOS && !UNITY_EDITOR
			float scaler = 1f ;
			var iosGen = UnityEngine.iOS.Device.generation;
			//已知设备
			if (iosGen == UnityEngine.iOS.DeviceGeneration.iPad3Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPad4Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPad5Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPadAir1 ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPadAir2 ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPadMini2Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPadMini3Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPadMini4Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPadPro1Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPadPro2Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPhone4 ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPhone4S ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPhone5 ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPhone5C ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPhone5S ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPhone6 ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPhone6S ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPhone7 ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPodTouch4Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPodTouch5Gen ||
			    iosGen == UnityEngine.iOS.DeviceGeneration.iPodTouch6Gen) {
				scaler = 2f;
			} else if (iosGen == UnityEngine.iOS.DeviceGeneration.iPhone6Plus ||
			           iosGen == UnityEngine.iOS.DeviceGeneration.iPhone6SPlus ||
			           iosGen == UnityEngine.iOS.DeviceGeneration.iPhone7Plus) {
				scaler = (float)1080/414;
			} else if (iosGen == UnityEngine.iOS.DeviceGeneration.iPadUnknown) { // 未知设备
				scaler = 2f;
			} else if (iosGen == UnityEngine.iOS.DeviceGeneration.iPhoneUnknown) {
				if (Screen.width == 750) {
					scaler = 2;
				} else if (Screen.width == 1080) {
					scaler = (float)1080/414;
					}
				else {
					scaler = 3;
				}
			} else if (iosGen == UnityEngine.iOS.DeviceGeneration.iPodTouchUnknown) {
				scaler = 2;
			}
			top = (int)(top / scaler);
			left = (int)(left / scaler);
			bottom = (int)(bottom / scaler);
			right = (int)(right / scaler);
			#endif
			webView.insets = new UniWebViewEdgeInsets(top , left , bottom , right) ;
		}

		private void RefreshInsets(){
			if (this.referenceWidget != null) {
				WebViewSizeFilterForNGUI sizeFilter = new WebViewSizeFilterForNGUI (referenceWidget);
				int[] insets = sizeFilter.GetEdgeInsets ();
				SetInsets (insets [0], insets [1], insets [2], insets [3]);
			}
		}

		[ContextMenu("Test callback load complete")]
		public void Test1(){
			this.OnLoadComplete (webView , true , "test");
		}
		[ContextMenu("Test callback on close")]
		public void Test2(){
			this.OnClose (webView);
		}
		[ContextMenu("Test callback on msg")]
		public void Test3(){
//			UniWebViewMessage msg = new UniWebViewMessage ();
			string path = "add";	
			var args = new Dictionary<string, string> ();
			args ["0"] = "00";
			args ["1"] = "11";
//			msg.args = args;
//			this.OnReciveMsg (webView, args);
			if (this.m_onReciveMsg != null) {
				this.m_onReciveMsg (path, args);
			}
		}
//		private UniWebViewEdgeInsets OnWebViewOrientation(UniWebView webView , UniWebViewOrientation orientation){
//			UniWebViewEdgeInsets insets ;
//			if(orientation == UniWebViewOrientation.Portrait){
//				insets = new UniWebViewEdgeInsets(top , left , bottom , right) ;
//			}else{
//				insets = new UniWebViewEdgeInsets(left , top , right , bottom) ;
//			}
//			return insets ;
//		}
//		
		private void OnLoadComplete(UniWebView webView , bool isSuccess , string errorMessage){
			if(this.m_onLoadComplete != null){
				this.m_onLoadComplete(isSuccess);
			}
			if(!isSuccess){
//				Debug.LogError("Loading failed: " + errorMessage);
			}
		}
		
		private bool OnClose(UniWebView view){
			if(this.m_onClose != null){
				m_onClose();
			}
			webView.HideToolBar(true);
			webView.CleanCache();
			_webView = null ;
			return true ;
		}
		
		private void OnReciveMsg(UniWebView webView , UniWebViewMessage message){		
			if(this.m_onReciveMsg != null){
				Dictionary<string , string> d = new Dictionary<string, string> ();
				var iter = d.GetEnumerator ();
				while (iter.MoveNext ()) {
					KeyValuePair<string,string> paris = iter.Current;
					Debug.Log ("key" + paris.Key + " value" + paris.Value);
				}
				this.m_onReciveMsg(message.path , message.args);
			}
		}
		
		public void HideView(){
			webView.Hide();
			webView.HideToolBar(true);
		}
		
		public void DisposeView(){
			if(_webView != null){
				HideView();
				Destroy(_webView);
				_webView = null ;
			}
		}
		
		void OnDestory(){
			DisposeView();
			m_onLoadComplete = null ;
			m_onReciveMsg = null ;
			m_onClose = null ;
		}
		
		void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus == true)
			{
				
				webView.Hide();
				isNeedResume = true;
				
			}
			else
			{
				if (isNeedResume)
				{
					webView.Show();
				}
				isNeedResume = false;
			}
		}	
	}
}
// ----------------------------------------------------------------------------
