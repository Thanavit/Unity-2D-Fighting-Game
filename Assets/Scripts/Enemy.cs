using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public float health;
    public Image healthBar;


    public Transform target;
    public float engageDistance;
    public float attackDistance;
    public float moveSpeed;
    private bool facingLeft = true;

    private bool movingRight = true;
    public Transform groundDetection;
    public float rayDistance;

    public static bool isAttacking = false;
    public static bool isPlayerDead = false;

    public static float enemySwordDamage = 10f;




    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(10, 10);
    }

    // Update is called once per frame
    void Update()
    {       
        animator.SetBool("moving", true);
        //get direction of the target
        if (isPlayerDead == false)
        {
            //check if the player is within engaing distance and if is alive
            if (Vector3.Distance(target.position, this.transform.position) < engageDistance && animator.GetBool("IsDead") == false)
            {
                Vector3 direction = target.position - this.transform.position;
                //check facing
                if (Mathf.Sign(direction.x) == 1 && facingLeft)
                {
                    Flip();
                }
                else if (Mathf.Sign(direction.x) == -1 && !facingLeft)
                {
                    Flip();
                }

                //move to player if within certain distance
                if (direction.magnitude >= attackDistance)
                {
                    Debug.DrawLine(target.position, this.transform.position, Color.yellow);

                    if (facingLeft && target.transform.position.y - this.transform.position.y < 3)
                    {
                        this.transform.Translate(Vector3.left * (Time.deltaTime * moveSpeed));
                    }
                    else if (!facingLeft && target.transform.position.y - this.transform.position.y < 3)
                    {
                        this.transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed));
                    }                   
                }

                //check if within attacking distance
                if (direction.magnitude < attackDistance)
                {
                    animator.SetBool("moving", false);
                    Debug.DrawLine(target.position, this.transform.position, Color.red);
                }
                

                //check if enemy is attacking
                if (isAttacking)
                {
                    animator.SetBool("moving", false);
                    Debug.DrawLine(target.position, this.transform.position, Color.red);
                    animator.SetBool("enemyAttack", true);
                }
                else
                {
                    animator.SetBool("enemyAttack", false);
                    animator.SetBool("moving", true);
                }

            }
            //if not within distance, Enemy will roam along the platforms or floor if is still alive
            else if (Vector3.Distance(target.position, this.transform.position) > engageDistance && animator.GetBool("IsDead") == false)
            {
                //debug
                Debug.DrawLine(target.position, this.transform.position, Color.green);
                enemyRoam();
            }
        }
        if (isPlayerDead == true)
        {
            animator.SetBool("enemyAttack", false);
            animator.SetBool("moving", true);
            enemyRoam();
        }

    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void enemyRoam()
    {
        //check facing and move to correct direction
        if (facingLeft)
        {
            this.transform.Translate(Vector3.left * (Time.deltaTime * moveSpeed));
        }
        else if (!facingLeft)
        {
            this.transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed));
        }

        //ray cast for ground detection
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, rayDistance);
        //debug
        if (groundInfo.collider == true)
        {
            //debug
            Debug.DrawRay(groundDetection.position, Vector2.down, Color.red);
        }
        if (groundInfo.collider == false)
        {
            //debug
            Debug.DrawRay(groundDetection.position, Vector2.down, Color.green);
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = true;
            }
        }
    }

    //check if player is hitting the enemy
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("playerSword"))
        {
            health -= CharacterMove.playerSwordDamage;
            healthBar.fillAmount = health / 100f;
            if (health <= 0)
            {
                animator.SetBool("enemyAttack", false);
                animator.SetBool("IsDead", true);                           
                StartCoroutine(deathBeforeDestroy());
            }
        }
    }

    IEnumerator deathBeforeDestroy()
    {      
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

}
