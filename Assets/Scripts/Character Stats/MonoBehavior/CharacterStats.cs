using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public CharacterData_SO characterData;

    public AttackData_SO attackData;
    [HideInInspector]
    //隐藏下面这个暴击数值
    public bool isCritical;

    //重载获取数字设置数值的引用方法语句
    #region Read from Data_SO
    public int MaxHealth
    {
        //直接读取
        get{ if (characterData != null) return characterData.maxHealth;else return 0;}
        set{characterData.maxHealth = value;}
        //希望直接更改
    }
    public int CurrentHealth
    {
        //直接读取
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
        //希望直接更改
    }
    public int BaseDefence
    {
        //直接读取
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
        //希望直接更改
    }
    public int CurrentDefence
    {
        //直接读取
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
        //希望直接更改
    }
    #endregion
}
