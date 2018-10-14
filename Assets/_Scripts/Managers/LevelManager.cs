using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    private Transform map;

    //we are using an array here so that we can more easily use different layers generated in the map
    [SerializeField]
    private Texture2D[] mapData;

    [SerializeField]
    private MapElement[] mapElements;

    [SerializeField]
    private Sprite defaultTile; //provides a ruler to measure the boundaries of a tile with

    //This is a Dictionary that will hold x,y coordinates from the Map and the GameObject Identifier for the Tile found there
    //   this is used as part of the process of determining what specific Tile should be placed over the default water tile
    //   that was created from MapLayer1.png
    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();

    [SerializeField]
    private SpriteAtlas waterAtlas;

    //location of the First Tile to be placed
    private Vector3 WorldStartPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); //returns the world position of the bottom left of the camera
        }
    }


	// Use this for initialization
	void Start ()
    {
        GenerateMap();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;

        for (int i = 0; i <mapData.Length; i++)
        {
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y);

                    //check if we have a Tile that suits the color of the pixel on the map
                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == c);

                    //if we have found an element that matches the color
                    if (newElement != null)
                    {
                        //xPosition and yPosition are used to set the placement of the next tile in the loop based on the previous tiles position relaive to
                        // the world start position using the default tile to determine size
                        float xPosition = WorldStartPosition.x + (defaultTile.bounds.size.x * x);
                        float yPosition = WorldStartPosition.y + (defaultTile.bounds.size.y * y);

                        //create the Tile
                        GameObject go = Instantiate(newElement.MyElementPrefab);

                        //Set the tile's Position
                        go.transform.position = new Vector2(xPosition, yPosition);

                        //check to see if the Tile placed here is Tagged as water (see levelmanager script).  
                        if(newElement.MyTileTag == "Water")
                        {
                            //add Tile GameObject to the waterTiles Dictionary
                            waterTiles.Add(new Point(x,y), go);
                        }

                        //Are we adding a tree Element
                        if (newElement.MyTileTag == ("Tree"))
                        {
                            //this will set the sorting order from bottom to top so that the trees always have a layer order that is even
                            //  this allows us to have the player walk between trees
                            go.GetComponent<SpriteRenderer>().sortingOrder = height*2 - y*2;
                        }

                        //Make the tile a Child of Map (so as not to clutter up the hierarchy in Unity)
                        go.transform.parent = map;

                    }
                }
            }

        }

        CheckWater();
    }

    public void CheckWater()
    {
        //This will iterate through the waterTiles Dictionary and return the point and gameobject as a value called tile
        foreach (KeyValuePair<Point, GameObject> tile in waterTiles)
        {
            //this adds the key from the specified tile in the dictionary to the composition string
            string composition = TileCheck(tile.Key);

            //looks a the string composition in positions 1,3,4,and 6 to see if they mack a specific set of tile types so that 
            //  we can determine what kind of water tile to place
            //  this will require an if statement for each possible combination of tiles 1,3,4, and 6
            //  in this case, we are checking to see if the water tile has earth to the West and North, and Water to the East and South.
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                //Set the Tile component value to tile 0 in the WaterAtlas (Water Edge top left corner)
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("0");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("1");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("2");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("3");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("4");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("5");
            }
            if (composition[1] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("6");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("7");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("8");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("9");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("10");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("11");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("12");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("13");
            }
            //This If statement places the corner piece tiles so that we have a nice smooth waters edge.  
            //  this will also require a number of if statements specific to each possible configuration
            if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
            {
                //Set the Tile component value to tile 15 in the WaterAtlas (Join Earth Edge top left corner)
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("14");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("15");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("16");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("17");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            //if the surrounding tile criteria are met, there is a 15% chance to place a wave tile
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 15)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("19");
                }
            }
            //if the surrounding tile criteria are met, there is a 10% chance to place a lillypad tile
            if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W')
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 10)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("20");
                }

            }

        }
    }

    private string TileCheck(Point currentPoint)
    {
        //set composition string default to empty
        string composition = string.Empty;

        //This Nested loop Pair Iterates through all the Tiles that are Directly Adjacent to the Tile being Examined to Determine if they
        //  are Type Earth or Type Water.  -1.-1 is Bottom Left tile, -1.1 is top left, 1.0 is Middle right, etc
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //Tile will never check itself to see if it is a water tile because we know that is true
                if (x!= 0 || y != 0)
                {
                    //checks to see if the water tile being examined(currentPoint) has another watertile in
                    // and adjacent Tile
                    if (waterTiles.ContainsKey(new Point(currentPoint.MyX + x, currentPoint.MyY + y)))
                    {
                        //adds a W to the string that describes the tiles surrounding the current Point
                        composition += "W";
                    }
                    else
                    {
                        //adds an E to the string that describes the tiles surrounding the current Point
                        composition += "E";
                    }

                }
            }
        }

        return composition;
    }
}




[Serializable]
public class MapElement
{
    [SerializeField]
    private string tileTag;

    [SerializeField]
    private Color color;

    [SerializeField]
    private GameObject elementPrefab;

    public GameObject MyElementPrefab
    {
        get
        {
            return elementPrefab;
        }
    }

    public Color MyColor
    {
        get
        {
            return color;
        }
    }

    public string MyTileTag
    {
        get
        {
            return tileTag;
        }
    }



}

//a Struct is similr to a class, but more lightweight amd not a reference type
public struct Point
{
    public int MyX { get; set; }
    public int MyY { get; set; }

    public Point ( int x, int y)
    {
        this.MyX = x;
        this.MyY = y;
    }
}
