using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    private Image content;

    [SerializeField]
    private Text statValue;

    private float currentValue;

    private float currentFill;

    [SerializeField]
    private float lerpSpeed;

    public float MyMaxValue { get; set; }

    //We create this Public Propery (myCurrentValue) so that other scripts can access the 
    //currentFill value without being able to manipulate it directly.  This allows us to put 
    //bounds and checks on what values the currentFill can have.

    public float MyCurrentValue
    {                           
        get                     
        {
            return currentValue;
        }

        set
        {
            if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;

            if(statValue != null)
            {
                //make sure that we update the Stat value text
                statValue.text = currentValue + "/" + MyMaxValue;
            }

        }
    }

	// Use this for initialization
	void Start ()
    {

        content = GetComponent<Image>();
        //content.fillAmount = 0.5f; //the addition of the f at the end forces floating piont division.
                                     //otherwise it would try to do this as an integer operation and fail.
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(currentFill != content.fillAmount)
        {
            //Lerp is used so that the health/mana bar value decreases or increases smoothly instead of suddently
            //I will need to do some reading to understand this better
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
        
	}

    public void Initialize (float currentValue, float maxValue)
    {
        if(content == null)
        {
            content = GetComponent<Image>();
        }

        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue/MyMaxValue;
    }
}
