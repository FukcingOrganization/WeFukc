using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPanel : MonoBehaviour
{
    [SerializeField] float initialHeight;
    [SerializeField] float initialPosY;
    [SerializeField] float addHeight;
    [SerializeField] float subtractPosY;
    [SerializeField] float excludeCount = 4f;

    [SerializeField] GameObject container;
 

    void Start()
    {
        for (int i = 0; i < 13; i++)
        {
            Instantiate(container, gameObject.transform);
        }

        float addH = Mathf.Ceil((13f - excludeCount) / 2f) * addHeight;

        RectTransform newRect = gameObject.GetComponent<RectTransform>();
        newRect.sizeDelta = new Vector2(newRect.sizeDelta.x, newRect.sizeDelta.y + addH);

        float subY = Mathf.Ceil((13f - excludeCount) / 2f) * subtractPosY;
        transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - subY);
    }
}
