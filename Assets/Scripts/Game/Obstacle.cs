using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{
    public bool right, left;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ObstacleMovement());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 100);
    }

    IEnumerator ObstacleMovement()
    {
        if (right)
        {
            gameObject.transform.DOMoveX(gameObject.transform.position.x + 3, .5f).OnComplete(() =>
             gameObject.transform.DOMoveX(gameObject.transform.position.x - 3, .5f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(ObstacleMovement());
        }
        if (left)
        {
            gameObject.transform.DOMoveX(gameObject.transform.position.x - 3, .5f).OnComplete(() =>
            gameObject.transform.DOMoveX(gameObject.transform.position.x + 3, .5f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(ObstacleMovement());
        }
    }
}
