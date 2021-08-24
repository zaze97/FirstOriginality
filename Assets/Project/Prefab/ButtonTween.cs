using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTween : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        this.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f).OnComplete(() =>
        {
            
            this.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
            
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.Find("Fx_Star_01").GetComponent<ParticleSystem>().Play();
        this.transform.DOScale(new Vector3(0.95f, 0.95f, 0.95f), 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.Find("Fx_Star_01").GetComponent<ParticleSystem>().Stop();
        this.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
    }
}
