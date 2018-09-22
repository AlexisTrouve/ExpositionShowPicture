using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string idPic;
    public int subIndex;
    public PicShowSceneControler controler;
    public GameObject window;
    bool selected = false;
    int timer = 0;
    bool MaxMode = false;
    anchorPos savePos = null;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (selected == true && MaxMode == false)
        {
            RectTransform rect = window.GetComponent<RectTransform>();
            Vector2 tmpdiffPos = new Vector2((Input.GetAxis("Mouse X") * 15),(Input.GetAxis("Mouse Y") * 15));
            rect.anchorMin = new Vector2(rect.anchorMin.x + tmpdiffPos.x / controler.canvasrecttrans.sizeDelta.x, rect.anchorMin.y + tmpdiffPos.y / controler.canvasrecttrans.sizeDelta.y);
            rect.anchorMax = new Vector2(rect.anchorMax.x + tmpdiffPos.x / controler.canvasrecttrans.sizeDelta.x, rect.anchorMax.y + tmpdiffPos.y / controler.canvasrecttrans.sizeDelta.y);
        }
        ++timer;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (timer < 15)
        {
            MaxMode = !MaxMode;
            RectTransform rect = window.GetComponent<RectTransform>();
            if (MaxMode == true)
            {
                Image[] imgs = GetComponentsInChildren<Image>();
                foreach (Image img in imgs)
                    if (img.name == "InternPicture")
                    {
                        if (controler.getResources()._datas[idPic].getshowPicIndex(subIndex)._zoomImg != null)
                            img.sprite = controler.getResources()._datas[idPic].getshowPicIndex(subIndex)._zoomImg;
                        /*else
                            img.sprite = controler.getResources()._datas[idPic].getshowPicIndex(subIndex)._normalImg;*/
                    }
                savePos = new anchorPos(rect.anchorMin, rect.anchorMax);
                rect.anchorMin = new Vector2(0.1798f, 0.014f);
                rect.anchorMax = new Vector2(0.671f, 0.887f);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
            else
            {
                Image[] imgs = GetComponentsInChildren<Image>();
                foreach (Image img in imgs)
                    if (img.name == "InternPicture")
                        img.sprite = controler.getResources()._datas[idPic].getshowPicIndex(subIndex)._normalImg;
                rect.anchorMin = savePos._anchorMin;
                rect.anchorMax = savePos._anchorMax;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                savePos = null;
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
        controler.SidePanel.transform.SetAsLastSibling();
        controler.PicBackgroundTop.transform.SetAsLastSibling();
        selected = true;
    }
}
