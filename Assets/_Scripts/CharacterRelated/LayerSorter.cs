using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;   

public class LayerSorter : MonoBehaviour
{

    private SpriteRenderer parentRenderer;

    private List<Obstacle> obstacles = new List<Obstacle>();


    // Use this for initialization
    void Start ()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        //parentRenderer.sortingOrder = 200;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if we have made contact with an Obstable
        if (collision.tag == "Obstacle")
        {
            //Determine which obstacle it is
            Obstacle o = collision.GetComponent<Obstacle>();

            o.FadeOut();  //call to FadeOut function on Obstacle.cs.  Sets Alpha to 70%

            if (obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder)
            {
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;
            }

            //Addthe Identified Obstacle to the obstacles list
            obstacles.Add(o);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if we have have exited an Obstacle collision
        if (collision.tag == "Obstacle")
        {
            //Get the Component for that obstacle
            Obstacle o = collision.GetComponent<Obstacle>();

            o.FadeIn(); //call to FadeIn function on Obstacle.cs.  Sets Alpha to 100%

            //remove the identified Obstable from the obstacles list
            obstacles.Remove(o);

            //if we are not colliding with anything
            if (obstacles.Count == 0)
            {
                //set the parent renderer back to default (highest)
                parentRenderer.sortingOrder = 200;
            }
            //otherwise
            else
            {
                //order the obstacles List based on sorting order.  Lowest first
                obstacles.Sort();

                //set the parent renderer sort order to 1 less than the lowest sorting order in the Obstacle list
                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }

        }

    }

}
