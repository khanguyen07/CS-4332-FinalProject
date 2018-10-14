using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    
    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }


    [SerializeField]
    private Stat mana;

    //The character's initial mana setting
    private float initMana = 50;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    private SpellBook spellBook;

    private Vector3 min, max;

   protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();

        mana.Initialize(initMana, initMana);

        base.Start();
    }

    //Update is called once per frame
    //Protected override means that the contents of this section can override the update function of the parent script
    protected override void Update ()
    {
        GetInput(); //Gets current keypress input value

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);

        base.Update();  //submits the update to he parent Script/class function with the same name
	}

    //Changes direction of Player movement based on Keypresses
    //This is not added to the Character Script/Class because player movement and NPC movement are controlled differently
    private void GetInput()
    {
        Direction = Vector2.zero; //Resets direction so that the value of Direction is not continuoudly increasing as long as keys are held down

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["UP"]))
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFT"]))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHT"]))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }

       

        if (IsMoving)
        {
            StopAttack();
        }

        foreach(string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);
            }
        }

    }

    public void SetLimits(Vector3 min, Vector3 max)
    {

        this.min = min;
        this.max = max;

        

    }

    //we are using an IEnumerator so that we can use yield return
    private IEnumerator Attack(string spellName)
    {

        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.CastSpell(spellName);
     
        IsAttacking = true;

        MyAnimator.SetBool("attack", IsAttacking);

        yield return new WaitForSeconds(newSpell.MyCastTime);  //hardcoded cast time of 1 Seconds for debugging

        if (currentTarget != null && InLineOfSight())
        {
            //This will create prefab x in the Scene where x is the number in the spellPrefab Array
            //it will originate from the position specified in the prefab (relative to the) Player, 
            //and the quaternion will make sure there is no rotation relative to the player
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.MyDamage, transform);
        }

        StopAttack();

    }
        
    public void CastSpell(string spellName)
    {
        //Activate the colliders used in the InLineOfSight function
        Block();

        //This if statement impliments all the Checks to see if the player can attack a target when the Space button is pressed
        //Check to see if 1. There is a clickable target under the mouse pointer
        //                2. The Player is not currently attacking
        //                3. The Player is not currently Moving
        //                4. The Target is in Line of Sight
        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellName)); //a Coroutine is code that runs in parallel 
                                                                //to the rest of the script.  it is not threading, 
                                                                //but runs in the background waiting for input
        }
    }

    private bool InLineOfSight()
    {

        if (MyTarget != null) //check to make sure we have a target
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
        }
    
        return false;
    }

    private void Block()
    {
        foreach(Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();

    }

    public void StopAttack()
    {
        //Stop the Spellbook from Casting
        spellBook.StopCasting();

        IsAttacking = false;  //makes sure we are not attacking

        MyAnimator.SetBool("attack", IsAttacking);

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);

        }

    }
}

