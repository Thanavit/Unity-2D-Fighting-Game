using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterMove : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 6.6f;
    public bool isGrounded = false;
    float checkMoving = 0f;

    public Animator animator;

    public float health;
    public float maxHealth;
    public Image playerHealthBar;


    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public int damage;

    public static float playerSwordDamage = 15f;




    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        checkMoving = Input.GetAxis("Horizontal") * moveSpeed;
        transform.position += movement * Time.deltaTime * moveSpeed;

    
        //flip character
        Vector3 characterScale = transform.localScale;
        if (Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = 5;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = -5;
        }
        transform.localScale = characterScale;

        animator.SetFloat("Speed", Mathf.Abs(checkMoving));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

    }

    void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            //player can attack
            if (Input.GetKey(KeyCode.E))
            {
                animator.SetBool("Attacking", true);
            }

            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
            animator.SetBool("Attacking", false);
        }
    }

    //check if enemy is hitting player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Weapon"))
        {
            health -= Enemy.enemySwordDamage;
            playerHealthBar.fillAmount = health / 100f;
            if (health <= 0)
            {
                Enemy.isPlayerDead = true;               
                animator.SetBool("playerDead", true);
                Physics2D.IgnoreLayerCollision(10, 11);
                StartCoroutine(deathBeforeDestroy());
                SceneManager.LoadScene("End");
            }
        }
    }

    IEnumerator deathBeforeDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
