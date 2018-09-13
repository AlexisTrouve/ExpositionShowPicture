﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    public GameObject window;
    public GameObject sideMenu;
    bool selected = false;
    int timer = 0;
    bool MaxMode = false;

    private void Start()
    {
        RectTransform rect = window.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.localPosition = new Vector2(rect.localPosition.x, rect.localPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (selected == true && MaxMode == false)
        {
            RectTransform rect = window.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.localPosition = new Vector2(rect.localPosition.x + (Input.GetAxis("Mouse X") * 22.5f), rect.localPosition.y + (Input.GetAxis("Mouse Y") * 22.5f));
        }
        ++timer;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (timer < 15)
        {
            MaxMode = !MaxMode;
            RectTransform rect = window.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            if (MaxMode == true)
            {
                rect.sizeDelta = new Vector2(1000, 1000);
                rect.localPosition = new Vector2(0, 0);
            }
            else
            {
                rect.sizeDelta = new Vector2(400, 400);
                rect.localPosition = new Vector2(0, 0);
            }
            timer = 15;
        }
        else
            timer = 0;
        selected = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        window.transform.SetAsLastSibling();
        sideMenu.transform.SetAsLastSibling();
        selected = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
    }
}