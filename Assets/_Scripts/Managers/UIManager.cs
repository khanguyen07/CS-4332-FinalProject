using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //this will be a singleton which is a template you can reuse in your code to solve a recurring problem
    private static UIManager instance;

    //when something is declared as Static it is Shared across all instances so only use it when you need to 
    //  for example if enemy health was static it would be shared across all enemies and they would all die when one was killed
    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    private CanvasGroup keybindMenu;

    private GameObject[] keyBindButtons;


    private void Awake()
    {
        keyBindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }
    // Use this for initialization
    void Start()
    {
        
        //set target healthstat so that taget healthbar updates
        healthStat = targetFrame.GetComponentInChildren<Stat>();

        //this will set the First Actionbutton ("0") to the Spell Fireball
        SetUseable(actionButtons[0], SpellBook.MyInstance.GetSpell("Fireball"));
        SetUseable(actionButtons[1], SpellBook.MyInstance.GetSpell("Frostbolt"));
        SetUseable(actionButtons[2], SpellBook.MyInstance.GetSpell("Thunderbolt"));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }
    }   

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        //fetches the appropriate portrait from the target and adds it to the targetframe
        portraitFrame.sprite = target.MyPortrait;

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float value)
    {
        healthStat.MyCurrentValue = value;
    }

    public void OpenCloseMenu()
    {

        //this is an alternative way of writing if then statements where only a single value changes based on the results of 
        keybindMenu.alpha = keybindMenu.alpha > 0 ? 0 : 1;
        keybindMenu.blocksRaycasts = keybindMenu.blocksRaycasts == true ? false : true;
        Time.timeScale = Time.timeScale > 0 ? 0 : 1;  //Pauses the Game
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keyBindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void SetUseable(ActionButton btn, IUseable useable)
    {
        btn.MyButton.image.sprite = useable.MyIcon;
        btn.MyButton.image.color = Color.white;
        btn.MyUseable = useable;
    }
}
