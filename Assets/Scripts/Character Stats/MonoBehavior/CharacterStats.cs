using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public CharacterData_SO characterData;

    public AttackData_SO attackData;
    [HideInInspector]
    //�����������������ֵ
    public bool isCritical;

    //���ػ�ȡ����������ֵ�����÷������
    #region Read from Data_SO
    public int MaxHealth
    {
        //ֱ�Ӷ�ȡ
        get{ if (characterData != null) return characterData.maxHealth;else return 0;}
        set{characterData.maxHealth = value;}
        //ϣ��ֱ�Ӹ���
    }
    public int CurrentHealth
    {
        //ֱ�Ӷ�ȡ
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
        //ϣ��ֱ�Ӹ���
    }
    public int BaseDefence
    {
        //ֱ�Ӷ�ȡ
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
        //ϣ��ֱ�Ӹ���
    }
    public int CurrentDefence
    {
        //ֱ�Ӷ�ȡ
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
        //ϣ��ֱ�Ӹ���
    }
    #endregion
}
