using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class showPic
{
    public Sprite _normalImg;
    public Sprite _zoomImg;

    public showPic(Sprite normalImg)
    {
        _normalImg = normalImg;
        _zoomImg = null;
    }
}

public struct picData
{
    public string _dataref;
    public Dictionary<string, showPic> _datasub;

    public picData(string dataref, string datasubref, string spriteFileName)
    {
        _dataref = dataref;
        _datasub = new Dictionary<string, showPic>();
        if (File.Exists(spriteFileName))
        {
            byte[] fileData;

            fileData = File.ReadAllBytes(spriteFileName);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            _datasub.Add(datasubref, new showPic(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1)));
        }
    }

    public void addZoomPic(string subref, string spriteFileName)
    {
        if (File.Exists(spriteFileName))
        {
            byte[] fileData;

            fileData = File.ReadAllBytes(spriteFileName);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            _datasub[subref]._zoomImg = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1);
        }
    }

    public void adddataSub(string subref, string spriteFileName)
    {
        if (File.Exists(spriteFileName))
        {
            byte[] fileData;

            fileData = File.ReadAllBytes(spriteFileName);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            _datasub.Add(subref, new showPic(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1)));
        }
    }

    public showPic getshowPicIndex(int index)
    {
        foreach (KeyValuePair<string, showPic> elem in _datasub)
        {
            if (index <= 0)
                return (elem.Value);
            --index;
        }
        return (null);
    }
}

public class ResourceLoader
{
    public Dictionary<string, picData> _datas;
    public picData[] _arraydatas;
    public Sprite BackTopPic;
    public Sprite SidePanelTopPic;

    public void load()
    {
        loadImgData();
        loadImgZoom();
        loadCompanyPics();
    }

    public void loadImgData()
    {
        _datas = new Dictionary<string, picData>();
        var ImgData = new DirectoryInfo("Resources/ImgData");
        var Datas = ImgData.GetDirectories();
        foreach (var data in Datas)
        {
            var catData = new DirectoryInfo(data.FullName);
            var Subs = catData.GetFiles();
            int j = 0;
            foreach (var sub in Subs)
            {
                if (j == 0)
                    _datas.Add(data.Name, new picData(data.Name, sub.Name, sub.FullName));
                else
                    _datas[data.Name].adddataSub(sub.Name, sub.FullName);
                ++j;
            }
        }
        _arraydatas = new picData[_datas.Count];
        int i = 0;
        foreach (KeyValuePair<string, picData> data in _datas)
        {
            _arraydatas[i] = data.Value;
            ++i;
        }
    }

    public void loadImgZoom()
    {
        var ImgData = new DirectoryInfo("Resources/ImgZoom");
        var Datas = ImgData.GetDirectories();
        foreach (var data in Datas)
        {
            var catData = new DirectoryInfo(data.FullName);
            var Subs = catData.GetFiles();
            foreach (var sub in Subs)
                if (_datas.ContainsKey(data.Name) == true)
                    _datas[data.Name].addZoomPic(sub.Name, sub.FullName);
        }
    }

    public void loadCompanyPics()
    {
        byte[] fileData;

        fileData = File.ReadAllBytes("Resources/CompanyPics/BackTop.jpg");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        BackTopPic = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1);

        fileData = File.ReadAllBytes("Resources/CompanyPics/SidePanelTop.jpg");
        texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        SidePanelTopPic = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1);
    }
}
