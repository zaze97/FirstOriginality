using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadScene : MonoBehaviour
{
	private Slider lodingslider;
	private TextMeshProUGUI lodingtext;
	private void Start()
	{
		lodingslider=transform.GetComponentInChildren<Slider>();
		lodingtext=transform.GetComponentInChildren<TextMeshProUGUI>();
		if (lodingslider&&lodingtext) {
			lodingslider.value=0;
			lodingtext.text = "Loading... 0%";
		}
		StartCoroutine ("StartLoading");
	}

	private IEnumerator StartLoading() {
 		int displayProgress = 0;
 		int toProgress = 0;
 		AsyncOperation op =SceneManager.LoadSceneAsync("Main");
 		op.allowSceneActivation = false;
        while(op.progress < 0.9f) {
 			toProgress = (int)op.progress * 100;
 			while(displayProgress < toProgress) {
 				++displayProgress;
 				SetLoadingPercentage(displayProgress);
 				yield return new WaitForEndOfFrame();
 			}
 		}
 		toProgress = 100;
 		while(displayProgress < toProgress){
 			++displayProgress;
 			SetLoadingPercentage(displayProgress);
 			yield return new WaitForEndOfFrame();
 		}
 		op.allowSceneActivation = true;
 	}

	private void SetLoadingPercentage(float displayProgress)
	{
		lodingslider.value = displayProgress/100;
		lodingtext.text = "Loading... "+displayProgress/100+"%";
	}
}
