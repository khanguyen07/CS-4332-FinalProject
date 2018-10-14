using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    [SerializeField]
    private float speed;

    public Transform MyTarget { get; private set; }

    private Transform source;

    //Damage value for a Spell
    private int damage;

	// Use this for initialization
	void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

    }

    public void Initialize (Transform target, int damage, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    private void FixedUpdate ()
    {
        if (MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position;  //Calculates the vector between target and 
                                                                         //spell origination point

            myRigidBody.velocity = direction.normalized * speed;       //determines how fast the spell should travel 
                                                                       // to the target

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //i understand that this pint for this is 
                                                                                 //to calculate the rotation the spell sprite needs to 
                                                                                 //properly orient relative to the player and 
                                                                                 //target, but i am going to have to read a bit here                                                                    

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);  //changes the actuial rotation of the sprite as it 
                                                                                //is added to the scene.  need to read up on quaternion  
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage, source);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}
