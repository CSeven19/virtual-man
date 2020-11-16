using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSort : MonoBehaviour
{
    public List<Image> imgs;
    public bool isEnemyAttack;
    Sprite enemySprite;

    void Start()
    {
        isEnemyAttack = true;
        StartCoroutine(ToggleImage());
        enemySprite = imgs[0].overrideSprite;
    }

    void Update()
    {
    }

    IEnumerator ToggleImage()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            Sprite tempSprite = imgs[0].overrideSprite;
            imgs[0].overrideSprite = imgs[1].overrideSprite;
            imgs[1].overrideSprite = tempSprite;
            isEnemyAttack = imgs[0].overrideSprite == enemySprite ? true : false;
        }
    }
}
