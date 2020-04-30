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
    int arrayHead = 2;
    int arrayTail;
    private bool isMoving = false;
    
    void Start()
    {
        questHandler = GetComponentInParent<QuestHandler>();
        leftTransform = new Vector3(-2 * xOffset, 0, 0);
        rightTransform = new Vector3(2 * xOffset, 0, 0);
        availableCharacters = questHandler.GetAvailableAdventurers();
        arrayTail = availableCharacters.Length - 1;

        InstantiateImages();
        SetSelectedCharacter();
    }

    public void MoveToNextCharacter()
    {
        isMoving = true;
        MoveChildrenLeft();
        CreateNewItemOnRight();
    }

    //todo when a button is clicked while it's moving, skip to end of movement, or consider doing nothing
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
            newObj.GetComponent<Image>().sprite = GetAdventurer(i).customerSprite;
            newObj.transform.localPosition = startPos;
            startPos += offsetVector;
        }
        arrayHead = availableCharacters.Length - 1;
        arrayTail = availableCharacters.Length - 1;
    }

    private CustomerProfile GetAdventurer(int i)
    {
        if(i >= availableCharacters.Length)
        {
            int index = i - availableCharacters.Length;
            return availableCharacters[index];
        }
        return availableCharacters[i];
    }

    private void SetSelectedCharacter()
    {
        selectedAdventurer = availableCharacters[characterIndex];
    }

    private void CreateNewItemOnRight()
    {
        if(arrayTail == availableCharacters.Length) { arrayTail = 0; }
        if(arrayHead == availableCharacters.Length) { arrayHead = 0; }
        GameObject newAdventurer = Instantiate(adventurerPrefab, transform);
        newAdventurer.GetComponent<RectTransform>().anchoredPosition = new Vector2(2 * xOffset, 0);
        newAdventurer.GetComponent<Image>().sprite = GetAdventurer(arrayTail).customerSprite;
        arrayTail++;
        arrayHead++;
    }

    private void CreateNewItemOnLeft()
    {
        if(arrayHead <= 0) { arrayHead = availableCharacters.Length; }
        if(arrayTail <= 0) { arrayTail = availableCharacters.Length; }
        GameObject newAdventurer = Instantiate(adventurerPrefab, transform);
        newAdventurer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2 * xOffset, 0);
        newAdventurer.GetComponent<Image>().sprite = GetAdventurer(arrayHead).customerSprite;
        arrayHead--;
        arrayTail--;
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
