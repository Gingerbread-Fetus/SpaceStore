using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRibbonPanel : MonoBehaviour
{
    [SerializeField] GameObject adventurerPrefab;
    [SerializeField] float xOffset = 150f;
    [SerializeField] float moveSpeed = 3f;

    Vector3 leftTransform;
    Vector3 rightTransform;
    QuestHandler questHandler;
    CustomerProfile selectedAdventurer;
    CustomerProfile[] availableCharacters;
    int characterIndex = 2;
    int arrayHead = 0;
    int arrayTail;
    private bool isMoving = false;
    
    // Start is called before the first frame update
    void Start()
    {
        questHandler = GetComponentInParent<QuestHandler>();
        leftTransform = new Vector3(-2 * xOffset, 0, 0);
        rightTransform = new Vector3(2 * xOffset, 0, 0);
        availableCharacters = questHandler.GetAvailableAdventurers();
        arrayTail = availableCharacters.Length - 1;

        SetSelectedCharacter();
        InstantiateImages();
    }

    public void MoveToNextCharacter()
    {
        isMoving = true;
        MoveChildrenLeft();
        CreateNewItemOnRight();
    }

    public void MoveToPreviousCharacter()
    {
        isMoving = true;
        MoveChildrenRight();
        CreateNewItemOnLeft();
    }

    private void InstantiateImages()
    {
        Vector3 startPos = leftTransform;
        Vector3 offsetVector = new Vector3(xOffset, 0, 0);
        for(int i = 0; i < 5; i++)
        {
            GameObject newObj = Instantiate(adventurerPrefab, transform);
            newObj.transform.localPosition = startPos;
            startPos += offsetVector;
        }
    }

    private void SetSelectedCharacter()
    {
        selectedAdventurer = availableCharacters[characterIndex];
    }


    private void CreateNewItemOnRight()
    {
        GameObject newAdventurer = Instantiate(adventurerPrefab, transform);
        newAdventurer.GetComponent<RectTransform>().anchoredPosition = new Vector2(2 * xOffset, 0);
        newAdventurer.GetComponent<Image>().sprite = GetNextAdventurer();
    }

    private Sprite GetNextAdventurer()
    {
        throw new NotImplementedException();
    }

    private void MoveChildrenLeft()
    {
        foreach (RectTransform child in transform)
        {
            StartCoroutine(MoveChildLeft(child));
        }
    }

    private void MoveChildrenRight()
    {
        foreach (RectTransform child in transform)
        {
            if (child != null)
            {
                StartCoroutine(MoveChildRight(child)); 
            }
        }
    }

    private void CreateNewItemOnLeft()
    {
        GameObject newAdventurer = Instantiate(adventurerPrefab, transform);
        newAdventurer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2 * xOffset, 0);
        newAdventurer.GetComponent<Image>().sprite = GetPreviousAdventurer();
    }

    private Sprite GetPreviousAdventurer()
    {
        throw new NotImplementedException();
    }

    private void CullOutOfBoundsChildren(RectTransform movingChild)
    {
        if (movingChild.anchoredPosition.x < leftTransform.x || movingChild.anchoredPosition.x > rightTransform.x)
        {
            DestroyImmediate(movingChild.gameObject);
        }
    }

    private IEnumerator MoveChildRight(RectTransform movingChild)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        Vector2 target = (new Vector2(xOffset, 0) + movingChild.anchoredPosition);
        float closeEnough = 0.1f;
        float distance = (movingChild.anchoredPosition - target).magnitude;
        while (distance >= closeEnough && isMoving)
        {
            movingChild.anchoredPosition = Vector2.MoveTowards(movingChild.anchoredPosition, target, moveSpeed * Time.deltaTime);
            yield return wait;

            distance = (movingChild.anchoredPosition - target).magnitude;
        }

        movingChild.anchoredPosition = target;
        CullOutOfBoundsChildren(movingChild);
        isMoving = false;
    }


    private IEnumerator MoveChildLeft(RectTransform movingChild)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        Vector2 target = (new Vector2(-xOffset, 0) + movingChild.anchoredPosition);
        float closeEnough = 0.1f;
        float distance = (movingChild.anchoredPosition - target).magnitude;
        while (distance >= closeEnough && isMoving)
        {
            movingChild.anchoredPosition = Vector2.MoveTowards(movingChild.anchoredPosition, target, moveSpeed * Time.deltaTime);
            yield return wait;

            distance = (movingChild.anchoredPosition - target).magnitude;
        }

        movingChild.anchoredPosition = target;
        CullOutOfBoundsChildren(movingChild);
        isMoving = false;
    }
}
