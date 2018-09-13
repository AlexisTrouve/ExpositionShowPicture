using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct picData
{
    public int _dataref;
    public Dictionary<int, Sprite> _datasub;

    public picData(int dataref, int datasubref, string spriteFileName)
    {
        _dataref = dataref;
        _datasub = new Dictionary<int, Sprite>();
        Texture2D texture = Resources.Load("TestImgs/" + spriteFileName) as Texture2D;
        texture.Apply();
        _datasub.Add(datasubref, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1));
    }

    public void addataSub(int subref, string spriteFileName)
    {
        Texture2D texture = Resources.Load("TestImgs/" + spriteFileName) as Texture2D;
        texture.Apply();
        _datasub.Add(subref, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1));
    }

    public Sprite getSpriteIndex(int index)
    {
        foreach (KeyValuePair<int, Sprite> elem in _datasub)
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
    public Dictionary<int, picData> _datas;
    public picData[] _arraydatas;

    public void load()
    {
        _datas = new Dictionary<int, picData>();
        _datas.Add(0, new picData(0, 0, "arid"));
        _datas.Add(1, new picData(1, 1, "hot desert"));
        _datas.Add(2, new picData(2, 0, "coast"));
        _datas.Add(3, new picData(3, 0, "cold desert"));
        _datas.Add(4, new picData(4, 0, "ContinentaleGrass"));
        _datas[4].addataSub(1, "NordicGrass");
        _datas[4].addataSub(2, "OceanicGrass");
        _datas[4].addataSub(3, "mediteraneanGrass");
        _datas.Add(5, new picData(5, 0, "jungle"));
        _datas.Add(6, new picData(6, 0, "mountain"));
        _datas.Add(7, new picData(7, 0, "snow mountain"));
        _datas.Add(8, new picData(8, 0, "NordicCoast"));
        _datas.Add(9, new picData(9, 0, "riverwater coast"));
        _datas.Add(10, new picData(10, 0, "sea"));
        _arraydatas = new picData[_datas.Count];
        int i = 0;
        foreach (KeyValuePair<int, picData> data in _datas)
        {
            _arraydatas[i] = data.Value;
            ++i;
        }
    }
}
