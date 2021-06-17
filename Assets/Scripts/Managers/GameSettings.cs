using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "0.0.1";
    public string GameVersion { get { return _gameVersion; } }

    [SerializeField]
    private string _nickName = "DefaultName";
    public string NickNameTag
    {
        get
        {
            int value = Random.Range(0, 9999);
            if(value < 10)
            {
                return "#000" + value.ToString();
            }
            else if(value < 100)
            {
                return "#00" + value.ToString();
            }
            else if(value < 1000)
            {
                return "#0" + value.ToString();
            }
            else
            {
                return "#" + value.ToString();
            }
        }
    }

    public string NickName
    {
        get
        {
            return _nickName;
        }
    }
}
