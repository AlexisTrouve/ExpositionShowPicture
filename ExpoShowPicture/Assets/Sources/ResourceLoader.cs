using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct picData
{
    public string _dataref;
    public Dictionary<string, Sprite> _datasub;

    public picData(string dataref, string datasubref, string spriteFileName)
    {
        _dataref = dataref;
        _datasub = new Dictionary<string, Sprite>();
        if (File.Exists(spriteFileName))
        {
            byte[] fileData;

            fileData = File.ReadAllBytes(spriteFileName);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            _datasub.Add(datasubref, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1));
        }
    }

    public void addataSub(string subref, string spriteFileName)
    {
        if (File.Exists(spriteFileName))
        {
            byte[] fileData;

            fileData = File.ReadAllBytes(spriteFileName);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            _datasub.Add(subref, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1));
        }
    }

    public Sprite getSpriteIndex(int index)
    {
        foreach (KeyValuePair<string, Sprite> elem in _datasub)
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

    public void load()
    {
        _datas = new Dictionary<string, picData>();
        var ImgData = new DirectoryInfo("ImgData");
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
                    _datas[data.Name].addataSub(sub.Name, sub.FullName);
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
}
