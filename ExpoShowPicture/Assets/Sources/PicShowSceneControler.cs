using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Step
{
    public Color32 _imgColor;
    public Vector2 _anchorMin;
    public Vector2 _anchorMax;

    public Step(Color32 imgColor, Vector2 anchorMin, Vector2 anchorMax)
    {
        _imgColor = imgColor;
        _anchorMin = anchorMin;
        _anchorMax = anchorMax;
    }
}

public class anchorPos
{
    public Vector2 _anchorMin;
    public Vector2 _anchorMax;

    public anchorPos(Vector2 anchorMin, Vector2 anchorMax)
    {
        _anchorMin = anchorMin;
        _anchorMax = anchorMax;
    }
}

public class PicShowSceneControler : MonoBehaviour
{
    public InputField infieldRef;
    public GameObject[] pics;
    public int numberStepTimer = 5;
    public GameObject SidePanel;
    public GameObject mainScreen;
    public RectTransform canvasrecttrans;
    public GameObject windowPrefab;
    public int maxPicShow = 30;

    //subImgManager
    public GameObject subImgPanel;
    public GameObject[] subImgBtn;
    private string ImgIndexSave;

    //internData
    private GameObject[] picShow;
    private int picShowIndex = 0;
    private int _picIndex = 0;
    private ResourceLoader resources;
    private Dictionary<string, string> picLink;
    private int actualStepTimer = 5;
    private Step[] steps;
    private int IndexStep = 0;
    private bool NextModeOnStep = true;

    //companyPics
    public GameObject PicSidePanelTop;
    public GameObject PicBackgroundTop;

    //prepos
    private anchorPos[] ImgPrePos;
    private int preposIndex;

    // Use this for initialization
    void Start()
    {
        preposIndex = 0;
        ImgPrePos = new anchorPos[15];
        ImgPrePos[0] = new anchorPos(new Vector2(0.04f, 0.612f), new Vector2(0.202f, 0.9f));
        ImgPrePos[1] = new anchorPos(new Vector2(0.202f, 0.612f), new Vector2(0.364f, 0.9f));
        ImgPrePos[2] = new anchorPos(new Vector2(0.364f, 0.612f), new Vector2(0.526f, 0.9f));
        ImgPrePos[3] = new anchorPos(new Vector2(0.526f, 0.612f), new Vector2(0.688f, 0.9f));
        ImgPrePos[4] = new anchorPos(new Vector2(0.688f, 0.612f), new Vector2(0.85f, 0.9f));

        ImgPrePos[5] = new anchorPos(new Vector2(0.04f, 0.324f), new Vector2(0.202f, 0.612f));
        ImgPrePos[6] = new anchorPos(new Vector2(0.202f, 0.324f), new Vector2(0.364f, 0.612f));
        ImgPrePos[7] = new anchorPos(new Vector2(0.364f, 0.324f), new Vector2(0.526f, 0.612f));
        ImgPrePos[8] = new anchorPos(new Vector2(0.526f, 0.324f), new Vector2(0.688f, 0.612f));
        ImgPrePos[9] = new anchorPos(new Vector2(0.688f, 0.324f), new Vector2(0.85f, 0.612f));

        ImgPrePos[10] = new anchorPos(new Vector2(0.04f, 0.036f), new Vector2(0.202f, 0.324f));
        ImgPrePos[11] = new anchorPos(new Vector2(0.202f, 0.036f), new Vector2(0.364f, 0.324f));
        ImgPrePos[12] = new anchorPos(new Vector2(0.364f, 0.036f), new Vector2(0.526f, 0.324f));
        ImgPrePos[13] = new anchorPos(new Vector2(0.526f, 0.036f), new Vector2(0.688f, 0.324f));
        ImgPrePos[14] = new anchorPos(new Vector2(0.688f, 0.036f), new Vector2(0.85f, 0.324f));
        steps = new Step[7];
        steps[0] = new Step(new Color32(255, 255, 255, 0), new Vector2(0.1f, 0.797f), new Vector2(0.7f, 0.976f));
        steps[1] = new Step(new Color32(255, 255, 255, 140), new Vector2(0.1f, 0.797f), new Vector2(0.7f, 0.976f));
        steps[2] = new Step(new Color32(255, 255, 255, 210), new Vector2(0.2f, 0.603f), new Vector2(0.8f, 0.783f));
        steps[3] = new Step(new Color32(255, 255, 255, 255), new Vector2(0.24f, 0.41f), new Vector2(0.84f, 0.59f));
        steps[4] = new Step(new Color32(255, 255, 255, 210), new Vector2(0.2f, 0.217f), new Vector2(0.8f, 0.397f));
        steps[5] = new Step(new Color32(255, 255, 255, 140), new Vector2(0.1f, 0.024f), new Vector2(0.7f, 0.203f));
        steps[6] = new Step(new Color32(255, 255, 255, 0), new Vector2(0.1f, 0.024f), new Vector2(0.7f, 0.203f));
        picLink = new Dictionary<string, string>();
        resources = new ResourceLoader();
        resources.load();
        int i = pics.Length - 2;
        foreach (GameObject go in pics)
        {
            --i;
            if (i >= 0)
                picLink.Add(go.name, resources._arraydatas[i]._dataref);
            else
                picLink.Add(go.name, resources._arraydatas[0]._dataref);
        }
        rawUpdatePic();
        picShow = new GameObject[maxPicShow];
        for (i = 0; i < maxPicShow; i++)
            picShow[i] = null;
        hideSubImgPanel();
        Image backImg = PicBackgroundTop.GetComponent<Image>();
        backImg.sprite = resources.BackTopPic;
        Image sideImg = PicSidePanelTop.GetComponent<Image>();
        sideImg.sprite = resources.SidePanelTopPic;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualStepTimer < numberStepTimer)
            softUpdatePic();
        else if (actualStepTimer == numberStepTimer && NextModeOnStep == true)
        {
            ++IndexStep;
            if (IndexStep >= steps.Length)
                IndexStep = 0;
            rawUpdatePic();
            ++actualStepTimer;
            ++_picIndex;
        }
        else if (actualStepTimer == numberStepTimer && NextModeOnStep == false)
        {
            --IndexStep;
            if (IndexStep < 0)
                IndexStep = steps.Length - 1;
            rawUpdatePic();
            ++actualStepTimer;
            --_picIndex;
            if (_picIndex < 0)
                _picIndex = resources._datas.Count - 1;
        }
    }

    void softUpdatePic()
    {
        int stepIndex = IndexStep;
        int picIndex = 0;
        int nextStepIndex = stepIndex + ((NextModeOnStep == true) ? 1 : -1);
        if (nextStepIndex == steps.Length)
            nextStepIndex = 0;
        else if (nextStepIndex < 0)
            nextStepIndex = steps.Length - 1;

        do
        {
            RectTransform rect = pics[picIndex].GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(rect.anchorMin.x + (steps[nextStepIndex]._anchorMin.x - steps[stepIndex]._anchorMin.x) / numberStepTimer
                , rect.anchorMin.y + (steps[nextStepIndex]._anchorMin.y - steps[stepIndex]._anchorMin.y) / numberStepTimer);
            rect.anchorMax = new Vector2(rect.anchorMax.x + (steps[nextStepIndex]._anchorMax.x - steps[stepIndex]._anchorMax.x) / numberStepTimer
                , rect.anchorMax.y + (steps[nextStepIndex]._anchorMax.y - steps[stepIndex]._anchorMax.y) / numberStepTimer);
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
            Image img = pics[picIndex].GetComponent<Image>();
            img.color = new Color32((byte)((byte)(img.color.r * 255) + (byte)((steps[nextStepIndex]._imgColor.r - steps[stepIndex]._imgColor.r) / numberStepTimer))
                , (byte)((byte)(img.color.g * 255) + (byte)((steps[nextStepIndex]._imgColor.g - steps[stepIndex]._imgColor.g) / numberStepTimer))
                , (byte)((byte)(img.color.b * 255) + (byte)((steps[nextStepIndex]._imgColor.b - steps[stepIndex]._imgColor.b) / numberStepTimer))
                , (byte)((byte)(img.color.a * 255) + (byte)((steps[nextStepIndex]._imgColor.a - steps[stepIndex]._imgColor.a) / numberStepTimer)));
            if (stepIndex != 0 && stepIndex != steps.Length - 1)
                pics[picIndex].transform.SetAsLastSibling();
            ++stepIndex;
            ++picIndex;
            if (stepIndex == steps.Length)
                stepIndex = 0;
            nextStepIndex = stepIndex + ((NextModeOnStep == true) ? 1 : -1);
            if (nextStepIndex < 0)
                nextStepIndex = steps.Length - 1;
            if (nextStepIndex == steps.Length)
                nextStepIndex = 0;
        } while (stepIndex != IndexStep);
        ++actualStepTimer;
    }

    void rawUpdatePic()
    {
        int stepIndex = IndexStep;
        int picIndex = 0;

        do
        {
            RectTransform rect = pics[picIndex].GetComponent<RectTransform>();
            rect.anchorMin = steps[stepIndex]._anchorMin;
            rect.anchorMax = steps[stepIndex]._anchorMax;
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
            Image img = pics[picIndex].GetComponent<Image>();
            img.color = steps[stepIndex]._imgColor;
            if (stepIndex == 0)
                picLink[pics[picIndex].name] = resources._arraydatas[(_picIndex + 4) % resources._datas.Count]._dataref;
            else if (stepIndex == 6)
            {
                picLink[pics[picIndex].name] = resources._arraydatas[(((_picIndex - 2) < 0) ? picLink.Count - (_picIndex - 2) - 1 : _picIndex - 2) % resources._datas.Count]._dataref;
            }
            img.sprite = resources._datas[picLink[pics[picIndex].name]].getshowPicIndex(0)._normalImg;
            if (stepIndex != 0 && stepIndex != steps.Length - 1)
                pics[picIndex].transform.SetAsLastSibling();
            ++stepIndex;
            ++picIndex;
            if (stepIndex == pics.Length)
                stepIndex = 0;
        } while (stepIndex != IndexStep);
    }

    public void nextStep()
    {
        Debug.Log("_picindex" + _picIndex);
        Debug.Log("picShowIndex" + picShowIndex);
        rawUpdatePic();
        NextModeOnStep = true;
        actualStepTimer = 0;
    }
    
    public void prevStep()
    {
        rawUpdatePic();
        NextModeOnStep = false;
        actualStepTimer = 0;
    }

    private void createNewWindow(string pictureIndex, int spriteIndex)
    {
        GameObject go = Instantiate(windowPrefab, mainScreen.transform);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.localPosition = Vector2.zero;
        rect.anchorMin = ImgPrePos[preposIndex]._anchorMin;
        rect.anchorMax = ImgPrePos[preposIndex]._anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        Image[] imgs = go.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
            if (img.name == "InternPicture")
                img.sprite = resources._datas[pictureIndex].getshowPicIndex(spriteIndex)._normalImg;
        WindowButton winbtn = go.GetComponentInChildren<WindowButton>();
        winbtn.controler = this;
        winbtn.idPic = pictureIndex;
        winbtn.subIndex = spriteIndex;
        go.transform.SetAsLastSibling();
        SidePanel.transform.SetAsLastSibling();
        if (picShow[picShowIndex] != null)
            Destroy(picShow[picShowIndex]);
        picShow[picShowIndex] = go;
        ++picShowIndex;
        picShowIndex = picShowIndex % maxPicShow;
        ++preposIndex;
        preposIndex = preposIndex % ImgPrePos.Length;
    }

    public void createAllSubWin()
    {
        int i = 0;

        foreach (KeyValuePair<string, showPic> elem in resources._datas[ImgIndexSave]._datasub)
        {
            createNewWindow(ImgIndexSave, i);
            ++i;
        }
    }

    public ResourceLoader getResources()
    {
        return (resources);
    }

    public void clickNewPic(int index)
    {
        if (resources._datas[picLink[pics[index].name]]._datasub.Count == 1)
            createNewWindow(picLink[pics[index].name], 0);
        else
            showSubImgPanel(picLink[pics[index].name]);
    }

    public void showSubImgPanel(string id)
    {
        RectTransform panelRect = subImgPanel.GetComponent<RectTransform>();
        panelRect.anchorMax = new Vector2(1, panelRect.anchorMax.y);
        panelRect.offsetMax = new Vector2(0, 0);
        ImgIndexSave = id;
        int i = 0;
        foreach (GameObject subImg in subImgBtn)
        {
            RectTransform btnRect = subImg.GetComponent<RectTransform>();
            if (resources._datas[id].getshowPicIndex(i) != null)
            {
                btnRect.anchorMax = new Vector2(btnRect.anchorMin.x + 0.16f, btnRect.anchorMax.y);
                btnRect.offsetMax = new Vector2(0, 0);
                Image img = subImg.GetComponent<Image>();
                img.sprite = resources._datas[id].getshowPicIndex(i)._normalImg;
                Text txt = subImg.GetComponentInChildren<Text>();
                txt.text = "" + i;
            }
            else
            {
                btnRect.anchorMax = new Vector2(btnRect.anchorMin.x, btnRect.anchorMax.y);
                btnRect.offsetMax = new Vector2(-1, 0);
            }
            ++i;
        }
    }

    public void hideSubImgPanel()
    {
        RectTransform panelRect = subImgPanel.GetComponent<RectTransform>();
        panelRect.anchorMax = new Vector2(panelRect.anchorMin.x, panelRect.anchorMax.y);
        panelRect.offsetMax = new Vector2(-1, 0);
        foreach (GameObject subImg in subImgBtn)
        {
            RectTransform btnRect = subImg.GetComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(-1, btnRect.sizeDelta.y);
        }
    }

    public void clickSubImg(int index)
    {
        hideSubImgPanel();
        createNewWindow(ImgIndexSave, index);
    }

    public void SearchRef()
    {
        int i = 0;

        foreach (KeyValuePair<string, picData> elem in resources._datas)
        {
            if (infieldRef.text == elem.Key)
            {
                _picIndex = i;
                i = i - 2;
                foreach (GameObject pic in pics)
                {
                    picLink[pic.name] = resources._arraydatas[i]._dataref;
                    ++i;
                }
                rawUpdatePic();
                infieldRef.text = "";
                return;
            }
            ++i;
        }
    }
}
