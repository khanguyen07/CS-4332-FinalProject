using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour //Abstract classes are used for functionality that will be inherited by other classes
                                                // in this case Players and NPCs
{

    [SerializeField] //use Serialize field instead of public so that it cannot be accessed by any other script in the project
    private float speed;

    private Vector2 direction;  //Holds character Directon for the current update
                                //Protected means that only scripts that inherit from this Class can access this variable

    private Rigidbody2D myRigidBody; //reference to the Player Animator Component

    public Animator MyAnimator { get; set; } //reference to the Player Animator Component

    public bool IsAttacking { get; set; }

    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;

    //Thew characters working health value
    [SerializeField]
    protected Stat health;

    public Transform MyTarget { get; set; }

    //we are creating a public variable so that character health values can be accessed but not written to by other scripts/functions/etc
    public Stat MyHealth
    {
        get
        {
            return health;
        }
    }

    //The characters starting Health Value
    [SerializeField]
    private float initHealth;


    public bool IsMoving

    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    // Use this for initialization
    protected virtual void Start ()
    {

        health.Initialize(initHealth, initHealth);

        myRigidBody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    //Protected virtual means that the contents of this upate can be overridden by the update function of child scripts
    protected virtual void Update ()
    {

        HandleLayers();
    }

    //use fixedupdate everytime you manipulate with a rigidbody object.  Framerate independant
    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (IsAlive)
        {
            //Uses the physics System to move. This is framerate indepentant.  Therefore time.deltatime is not needed
            myRigidBody.velocity = Direction.normalized * Speed;  //use of normalized here prevents movement speed from being increased
                                                                  //when multiple buttons are held down
        }
    }
    
    //When this function is called, it controls which animation is being used
    public void HandleLayers()
    {
        //Checks to see if there is any movement direction input
        if (IsAlive)
        {
            if (IsMoving)
            {

                //calls the ActivateLayer function to set the active layer to WalkLayer when keys are being pressed
                //as represented by the IsMoving Boolean being true
                ActivateLayer("WalkLayer");

                //Sets the Animation facing based on the keypress directions
                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);

            }
            else if (IsAttacking)
            {

                ActivateLayer("AttackLayer");

            }
            else
            {

                //calls the ActivateLayer function to set the active layer to IdleLayer when no keys are being pressed
                //as represented by the IsMoving Boolean being false
                ActivateLayer("IdleLayer");

            }

        }
        else
        {
            ActivateLayer("DeathLayer");
        }



    }

    public void ActivateLayer (string layerName)
    {

        //this is a Looping structure.  initially i is set to 0 (inactive).  Then the loop runs while i is less than the number of layers
        // adding 1 to i in each loop
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            //sets layer weight to 0 for every layer as the loop runs. this "resets" all the layers
            MyAnimator.SetLayerWeight(i, 0);
        }

        //sets the Layer Weight for the layer specified in layername to 1 (active)
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);

    }



    public virtual void TakeDamage(float damage, Transform source)
    {
      
        //health reduced
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <=0)
        {
            Direction = Vector2.zero;
            myRigidBody.velocity = Direction;
            MyAnimator.SetTrigger("die");

        }
    }
}
