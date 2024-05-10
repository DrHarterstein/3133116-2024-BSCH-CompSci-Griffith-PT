using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{

    public float damageAmount;
    public float damageInterval;

    private bool isPlayerInside = false;
    private Coroutine damageCoroutine;


    public GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManagerScript>(); //finds the game manager object and gets the GameManagerScript component
        //Debug.Log(gameManager);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!isPlayerInside)
            {
                isPlayerInside = true;
                gameManager.TakeDamage(damageAmount);
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(DamageOverTime());
                }
            }
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageOverTime());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;

            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    IEnumerator DamageOverTime()
    {
        yield return new WaitForSeconds(damageInterval);

        while (isPlayerInside)
        {
            gameManager.TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
