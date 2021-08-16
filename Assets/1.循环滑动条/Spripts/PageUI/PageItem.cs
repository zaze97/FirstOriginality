using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PageItem 
{

    private Vector2 ItemSize=new Vector2(200,300);
    
    private Image myImage;
    private RectTransform tect;

    public RectTransform GetTect
    {
        get
        {
            if (tect != null) return tect;
            else
            {
                Debug.Log("RectTransform组件没有初始化");
                return null;
            };
        }
    }
    public PageItem()
    {
        CreateTempate();
    }
    public void SetParent(Transform parent) {
        tect.SetParent(parent);
    }
    public void SetImage(Sprite sprite) {
        myImage.sprite = sprite;
    }
    //创建并初始化卡牌物体
    private GameObject CreateTempate()
    {
        GameObject temp = new GameObject("Template");
        tect=temp.AddComponent<RectTransform>();
        tect.sizeDelta = ItemSize;
        myImage=temp.AddComponent<Image>();

        return temp;
    }
    
    
}
