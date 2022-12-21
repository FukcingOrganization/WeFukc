using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPanel : MonoBehaviour
{
    [SerializeField] float addHeight;
    [SerializeField] float columnNumber;
    [SerializeField] float noScrollCapacity;
    [SerializeField] float testObjects;

    [SerializeField] GameObject container;

    RectTransform initialRect;
    Transform initialTransform;

    void Start()
    {
        for (int i = 0; i < testObjects; i++)
        {
            Instantiate(container, gameObject.transform);
        }

        initialRect = gameObject.GetComponent<RectTransform>();
        initialTransform = transform;

        if (testObjects > noScrollCapacity)
        {
            float addH = Mathf.Ceil((testObjects - noScrollCapacity) / columnNumber) * addHeight;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(initialRect.sizeDelta.x, initialRect.sizeDelta.y + addH);

            float subY = Mathf.Ceil((testObjects - noScrollCapacity) / columnNumber) * addHeight / 2;
            transform.localPosition = new Vector2(initialTransform.localPosition.x, initialTransform.localPosition.y - subY);
        }
    }
}
