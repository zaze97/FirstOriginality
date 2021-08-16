using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

enum FINGER { FINGER_STATE_NUL, FINGER_STATE_TOUCH, FINGER_STATE_ADD }
public class CardMove : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    public List<Sprite> SpriteList;
    public float startAngle = -90;//中间卡牌的角度
    public Vector2 center;//椭圆中心点
    public float Y_axis = 400;//long axis
    public float X_axis = 100;//short axis
    public float mouseSpeed = 1;
    public PageItem[] pageItem;
    private int halfSize;
    private Vector2 screenCenterPos;
    private float deltaAngle = 20;//相邻卡牌的角度差值
    private string currentSelectName;
    private int cardcount;//卡牌数量
    private float fingerActionSensitivity = Screen.width * 0.05f; //手指动作的敏感度，这里设定为 二十分之一的屏幕宽度.
    private float fingerBeginX,fingerBeginY,fingerCurrentX,fingerCurrentY,fingerSegmentX,fingerSegmentY;
    private FINGER fingerTouchState;
    private float angle;
    private Action MoveEndAction;
    private bool isdrag;
    void Start()
    {
       
        deltaAngle = Screen.width / 100f;
        Y_axis = Screen.width / 3;
     
        UpdateChild();
    }

    /// <summary>
    /// 初始化卡牌显示位置
    /// </summary>
    void InitSprites()
    {
        screenCenterPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        cardcount = SpriteList.Count;
        pageItem = new  PageItem[SpriteList.Count];
        if(cardcount%2==0)
        halfSize = (cardcount) / 2;
        else
        halfSize = (cardcount - 1) / 2;
        for (int i = 0; i < cardcount; i++)
        {
            pageItem[i] =new PageItem();
            pageItem[i].SetParent(transform);
            pageItem[i].SetImage(SpriteList[i]);
            SetPosition(i, false);
            SetDeeps(i);
        }

    }
    /// <summary>
    /// 椭圆的半长轴为A，半短轴为B，计算椭圆上一点的位置
    /// x=A*cos(angle),y=B*sin(angle)
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="userTweener">是否使用tween动画.</param>
    void SetPosition(int index, bool userTweener = true)
    {
        //计算每一张卡片在椭圆上相对中间卡牌的角度
        angle = 0;
        
        //计算缩放的值
        float normal = 1.0f / halfSize;
        float gap = Mathf.Abs(index - halfSize);
        
        if (index < halfSize)
        {//left
            angle = startAngle - (halfSize - index) * deltaAngle;
            pageItem[index].GetTect.DOScale(1.5f-(normal*gap), 0.3f);
        }
        else if (index > halfSize)
        {//right

            angle = startAngle + (index - halfSize) * deltaAngle;
            pageItem[index].GetTect.DOScale(1.5f-(normal*gap), 0.3f);
            Debug.Log(index);
        }
        else
        {//medim
            angle = startAngle;
            pageItem[index].GetTect.DOScale(1.5f, 0.3f).OnComplete(MoveEnd);

        }
        Debug.Log(normal+"---"+gap+"---"+normal*gap);
        //通过卡牌的角度，计算对应的位置
        float xpos = Y_axis * Mathf.Cos((angle / 180) * Mathf.PI) + center.x;
        float ypos = X_axis * Mathf.Sin((angle / 180) * Mathf.PI) + center.y;

        Vector2 pos = new Vector2(xpos, ypos);
        // Vector2 pos = new Vector2(center.x, center.y);
        if (!userTweener)
        {
            pageItem[index].GetTect.DOMove(new Vector2(screenCenterPos.x + pos.x, screenCenterPos.y + pos.y), 0f);
        }
        else
            pageItem[index].GetTect.DOMove(new Vector2(screenCenterPos.x + pos.x, screenCenterPos.y + pos.y), 0.2f);


    }
    /// <summary>
    /// 选择移动动画结束后的设置当前选择的名字和回调
    /// </summary>
    void MoveEnd()
    {
        currentSelectName = transform.GetChild(transform.childCount - 1).name;
        if (MoveEndAction != null) MoveEndAction();

    }

    /// <summary>
    /// 计算每一张卡片的层级
    /// </summary>
    /// <param name="index">Index.</param>
    void SetDeeps(int index)
    {
        int deep = 0;
        if (index < halfSize)
        {//左侧卡牌层级，从左侧到中间，层级依此递增
            deep = index;
        }
        else if (deep > halfSize)
        {//右侧卡牌层级，从中间到右侧，层级依此递减
            deep = pageItem.Length - (index + 1);
        }
        else
        {
            deep = halfSize;
        }
        pageItem[index].GetTect.SetSiblingIndex(deep);
    }
    public void RightMove()
    {
        int length = pageItem.Length;

        PageItem temp = pageItem[0];
        for (int i = 0; i < length; i++)
        {//移动卡片在数组中的位置，依此向前移动一位
            if (i == length - 1)
                pageItem[i] = temp;
            else
                pageItem[i] = pageItem[i + 1];

        }

        for (int i = 0; i < length; i++)
        {//更新数组卡片需要显示的位置和层级
            if (i < length - 1) SetPosition(i);
            else SetPosition(i, false);
            SetDeeps(i);



        }
    }
    public void LeftMove()
    {
        int length = pageItem.Length;
        PageItem temp = pageItem[length - 1];
        for (int i = length - 1; i >= 0; i--)
        {
            if (i == 0)
                pageItem[i] = temp;
            else
                pageItem[i] = pageItem[i - 1];
        }
        for (int i = 0; i < length; i++)
        {
            if (i == 0) SetPosition(i, false);
            else SetPosition(i);
            SetDeeps(i);
        }
    }
    private void Update()
    {
        if(isdrag)
        CheckMouse();
    }
    private void AddFingerAction()
    {
        fingerTouchState = FINGER.FINGER_STATE_ADD;
        if (Mathf.Abs(fingerSegmentX) > Mathf.Abs(fingerSegmentY))
        {
            fingerSegmentY = 0;
        }
        else
        {
            fingerSegmentX = 0;
        }

        if (fingerSegmentY == 0)
        {
            if (fingerSegmentX < 0)
            {
                RightMove();
            }
            else
            {
                LeftMove();
            }
        }

    }

    readonly string MYMouseScrollWheel = "Mouse ScrollWheel";
    float timer, axis;//timer可以防止快速滚轮导致速度过快动画得bug
    void CheckMouse()
    {

        switch (fingerTouchState)
        {
            case FINGER.FINGER_STATE_NUL:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    fingerTouchState = FINGER.FINGER_STATE_TOUCH;
                    fingerBeginX = Input.mousePosition.x;
                    fingerBeginY = Input.mousePosition.y;
                }
                break;
            case FINGER.FINGER_STATE_TOUCH:
                fingerCurrentX = Input.mousePosition.x;
                fingerCurrentY = Input.mousePosition.y;
                fingerSegmentX = fingerCurrentX - fingerBeginX;
                fingerSegmentY = fingerCurrentY - fingerBeginY;

                float fingerDistance = fingerSegmentX * fingerSegmentX + fingerSegmentY * fingerSegmentY;

                if (fingerDistance > (fingerActionSensitivity * fingerActionSensitivity))
                {
                    AddFingerAction();
                }
                break;
            case FINGER.FINGER_STATE_ADD:
                break;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            fingerTouchState = FINGER.FINGER_STATE_NUL;
        }
        timer += Time.deltaTime;

        axis = Input.GetAxis(MYMouseScrollWheel);
        if (axis > 0 && timer > 0.2f)
        {
            timer = 0;
            LeftMove();
        }
        else if (axis < 0 && timer > 0.2f)
        {
            timer = 0;
            RightMove();
        }

    }
    void InitData()
    {
        fingerActionSensitivity = Screen.width * 0.05f;
        fingerBeginX = 0;
        fingerBeginY = 0;
        fingerCurrentX = 0;
        fingerCurrentY = 0;
        fingerSegmentX = 0;
        fingerSegmentY = 0;
    }


    public void UpdateChild()
    {
        InitSprites();
        InitData();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isdrag = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isdrag = false;
    }
}
