using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    ParticleSystem coin_ps;
    public GameObject coinObject;
    private CircleCollider2D circCol;

    // Start is called before the first frame update
    void Start()
    {
        coin_ps = GetComponent<ParticleSystem>();
        circCol = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            coin_ps.Play();
            circCol.enabled = false;
            coinObject.SetActive(false);
        }
    }
}
