using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


//This class will define everything that a spell does.  Damage, etc

[Serializable]
public class Spell : IUseable
{
    [SerializeField]
    private string name;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private Color barColor;

    public string MyName
    {
        get
        {
            return name;
        }
    }

    public int MyDamage
    {
        get
        {
            return damage;
        }
    }

    public float MySpeed
    {
        get
        {
            return speed;
        }
    }

    public float MyCastTime
    {
        get
        {
            return castTime;
        }
    }

    public GameObject MySpellPrefab
    {
        get
        {
            return spellPrefab;
        }
    }

    public Color MyBarColor
    {
        get
        {
            return barColor;
        }

    }

    public Sprite MyIcon
    {
        get
        {
            return icon;
        }

    }

    public void Use()
    {
        Player.MyInstance.CastSpell(MyName);
    }
}
