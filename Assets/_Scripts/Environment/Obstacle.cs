using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>  //adding IComparable to the class allows us to properly handle the sorting
{

    public SpriteRenderer MySpriteRenderer { get; set; }

    private Color defaultColor;

    private Color fadedColor;

    public int CompareTo(Obstacle other)
    {
        if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    // Use this for initialization
    void Start ()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();

        defaultColor = MySpriteRenderer.color;

        fadedColor = defaultColor;
        fadedColor.a = 0.7f;   //70% faded color
	}
    
    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }

}
