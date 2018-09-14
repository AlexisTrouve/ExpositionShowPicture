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

public class PicShowSceneControler : MonoBehaviour
{
    public InputField infieldRef;
    public GameObject[] pics;
    public int numberStepTimer = 5;
    public GameObject SidePanel;
    public GameObject mainScreen;
    public GameObject windowPrefab;
    public int maxPicShow = 30;

    //subImgManager
    public GameObject subImgPanel;
    public GameObject[] subImgBtn;
    private int ImgIndexSave;

    //internData
    private GameObject[] picShow;
    private int picShowIndex = 0;
    private int _picIndex = 0;
    private ResourceLoader resources;
    private Dictionary<string, int> picLink;
    private int actualStepTimer = 5;
    private Step[] steps;
    private int IndexStep = 0;
    private bool NextModeOnStep = true;

    // Use this for initialization
    void Start()
    {
        steps = new Step[7];
        steps[0] = new Step(new Color32(255, 255, 255, 0), new Vector2(0.1f, 0.797f), new Vector2(0.7f, 0.976f));
        steps[1] = new Step(new Color32(255, 255, 255, 140), new Vector2(0.1f, 0.797f), new Vector2(0.7f, 0.976f));
        steps[2] = new Step(new Color32(255, 255, 255, 210), new Vector2(0.2f, 0.603f), new Vector2(0.8f, 0.783f));
        steps[3] = new Step(new Color32(255, 255, 255, 255), new Vector2(0.24f, 0.41f), new Vector2(0.84f, 0.59f));
        steps[4] = new Step(new Color32(255, 255, 255, 210), new Vector2(0.2f, 0.217f), new Vector2(0.8f, 0.397f));
        steps[5] = new Step(new Color32(255, 255, 255, 140), new Vector2(0.1f, 0.024f), new Vector2(0.7f, 0.203f));
        steps[6] = new Step(new Color32(255, 255, 255, 0), new Vector2(0.1f, 0.024f), new Vector2(0.7f, 0.203f));
        picLink = new Dictionary<string, int>();
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
            img.sprite = resources._datas[picLink[pics[picIndex].name]].getSpriteIndex(0);
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

    private void createNewWindow(int pictureIndex, int spriteIndex)
    {
        GameObject go = Instantiate(windowPrefab, mainScreen.transform);
        Image[] imgs = go.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
            if (img.name == "InternPicture")
                img.sprite = resources._arraydatas[pictureIndex].getSpriteIndex(spriteIndex);
        WindowButton winbtn = go.GetComponentInChildren<WindowButton>();
        winbtn.sideMenu = SidePanel;
        go.transform.SetAsLastSibling();
        SidePanel.transform.SetAsLastSibling();
        if (picShow[picShowIndex] != null)
            Destroy(picShow[picShowIndex]);
        picShow[picShowIndex] = go;
        ++picShowIndex;
        picShowIndex = picShowIndex % maxPicShow;
    }

    public void clickNewPic(int index)
    {
        Debug.Log("pic id : " + picLink[pics[index].name]);

        if (resources._arraydatas[picLink[pics[index].name]]._datasub.Count == 1)
            createNewWindow(picLink[pics[index].name], 0);
        else
            showSubImgPanel(picLink[pics[index].name]);
    }

    public void showSubImgPanel(int id)
    {
        RectTransform panelRect = subImgPanel.GetComponent<RectTransform>();
        panelRect.anchorMax = new Vector2(1, panelRect.anchorMax.y);
        panelRect.offsetMax = new Vector2(0, 0);
        ImgIndexSave = id;
        int i = 0;
        foreach (GameObject subImg in subImgBtn)
        {
            RectTransform btnRect = subImg.GetComponent<RectTransform>();
            if (resources._arraydatas[id].getSpriteIndex(i) != null)
            {
                btnRect.sizeDelta = new Vector2(220, btnRect.sizeDelta.y);
                Image img = subImg.GetComponent<Image>();
                img.sprite = resources._arraydatas[id].getSpriteIndex(i);
                Text txt = subImg.GetComponentInChildren<Text>();
                txt.text = "" + i;
            }
            else
                btnRect.sizeDelta = new Vector2(-1, btnRect.sizeDelta.y);
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
}
