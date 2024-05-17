using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SaveLoadBase
{
    //Folder name under which you want to save file for e.g. You want to save avatar data
    //Only add folder name not actual directory path.
    public string m_DirPath;
    //Actual file onto which data is save
    //Don't add extension like .json
    public string m_FileName;


}