using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int totalTarget;
    bool isJump = true;
    bool isDead = false;
    int idMove = 0;
    Animator anim;
    private int duit;
    private int target;
    public Text targetTx;
    public Text duitTx;
    public GameObject panelTertangkap;
    public GameObject panelCollectible;
    public GameObject panelGameOver1;
    public GameObject panelGameOver2;
    public GameObject panelBoss;
    public GameObject panelPause;
    public GameObject panelWin;
	private GameObject currentCollect;
	private GameObject currentEnemy;
    public SoundEffectManager soundEffectManager;

    // Use this for initialization
    private void Start()
    {
        panelPause.SetActive(false);
        panelWin.SetActive(false);
        panelBoss.SetActive(false);
        panelGameOver1.SetActive(false);
        panelGameOver2.SetActive(false);		
        panelCollectible.SetActive(false);
        panelTertangkap.SetActive(false);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        duitTx.text = "Rp. " + duit;
        targetTx.text = "Target: " + target + " / " + totalTarget;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Idle();
        }

        Move();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isJump)
        {
            anim.ResetTrigger("jump");
            if (idMove == 0) anim.SetTrigger("idle");
            isJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetTrigger("jump");
        anim.ResetTrigger("run");
        anim.ResetTrigger("idle");
        isJump = true;
    }

    public void MoveRight()
    {
        idMove = 1;
    }

    public void MoveLeft()
    {
        idMove = 2;
    }

    private void Move()
    {
        if (idMove == 1 && !isDead)
        {
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(1 * Time.deltaTime * 6.5f, 0, 0);
            transform.localScale = new Vector3(0.3158359f, 0.3158359f, 0.3158359f);
        }
        if (idMove == 2 && !isDead)
        {
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(-1 * Time.deltaTime * 7f, 0, 0);
            transform.localScale = new Vector3(-0.3158359f, 0.3158359f, 0.3158359f);
        }
    }

    public void Jump()
    {
        if (!isJump)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 375f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Collect"))
        {
            panelCollectible.SetActive(true);
            Time.timeScale = 0f;
			currentCollect = collision.gameObject;
            soundEffectManager.PlaySoundEffect(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Enemy"))
        {
            if(duit > 0)
            {
                panelTertangkap.SetActive(true);
                Time.timeScale = 0;
				currentEnemy = collision.gameObject;
                soundEffectManager.PlaySoundEffect(2);
            }
            else
            {
                Dead1();
            }
        }

        if (collision.transform.tag.Equals("Boss"))
        {
            panelBoss.SetActive(true);
            Time.timeScale = 0;
            soundEffectManager.PlaySoundEffect(1);
        }
    }

    public void Idle()
    {
        if (!isJump)
        {
            anim.ResetTrigger("jump");
            anim.ResetTrigger("run");
            anim.SetTrigger("idle");
        }
        idMove = 0;
    }

    private void Dead1()
    {
        panelGameOver1.SetActive(true);
        Time.timeScale = 0f;
        soundEffectManager.PlaySoundEffect(3);
    }

    private void Dead2()
    {
        panelGameOver2.SetActive(true);
        Time.timeScale = 0f;
        soundEffectManager.PlaySoundEffect(3);
    }

    private void Win()
    {
        panelWin.SetActive(true);
        Time.timeScale = 0f;
        soundEffectManager.PlaySoundEffect(4);
    }

    public void BantuAnak()
    {
        panelCollectible.SetActive(false);
        duit += 1750000;
        target += 1;
        Time.timeScale = 1f;
        soundEffectManager.PlaySoundEffect(0);
		
		if (currentCollect != null)
		{
		    Destroy(currentCollect);
			currentCollect = null;
		}
    }

    public void Abaikan()
    {
        panelCollectible.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BeriHadiah()
    {
        panelTertangkap.SetActive(false);
        duit -= 300000;
        Time.timeScale = 1f;
		
		if (currentEnemy != null)
		{
		    Destroy(currentEnemy);
			currentEnemy = null;
		}

        soundEffectManager.PlaySoundEffect(0);
    }

    public void Menyerah()
    {
        panelTertangkap.SetActive(false);
        Dead1();
    }

    public void Beli()
    {
        panelBoss.SetActive(false);
		
        if (duit >= 10000000)
		{
		Win();
		}
		
		else
		{
		Dead2();
		}
    }

    public void MungkinNanti()
    {
        panelBoss.SetActive(false);
        Dead2();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void Next()
    {
        SceneManager.LoadScene("Gameplay2");
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1f;
    }
}
