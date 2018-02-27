using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewSizeFilterForNGUI {
	private UIWidget widget ;
	private int[] insets  = new int[4];
	///获取webview inset 0top 1left 2down 3right
	public WebViewSizeFilterForNGUI(UIWidget widget){
		this.widget = widget;
		Vector2[] corners = new Vector2[widget.worldCorners.Length];
		float minX = int.MaxValue, maxX  = int.MinValue, minY = int.MaxValue, maxY = int.MinValue ;
		for(int i = 0 ; i < widget.worldCorners.Length ; i++){
			corners[i] = widget.anchorCamera.WorldToScreenPoint(widget.worldCorners[i]);
			if(minX > corners[i].x){
				minX= corners[i].x;
			}else if(maxX < corners[i].x){
				maxX =  corners[i].x ;
			}
			if(minY > corners[i].y){
				minY= corners[i].y;
			}else if(maxY < corners[i].y){
				maxY =  corners[i].y ;
			}
		}
		int height = Screen.height ;
		int width = Screen.width ;
		insets[0] =(int)Mathf.Clamp(height - maxY , 0 , height);
		insets[1] = (int)Mathf.Clamp(minX , 0 ,width) ;
		insets[2] = (int)Mathf.Clamp(minY , 0 , height);
		insets[3] = (int)Mathf.Clamp(width - maxX , 0 , width);
	}

	
	public int[] GetEdgeInsets(){
		return insets ;
	}

}
