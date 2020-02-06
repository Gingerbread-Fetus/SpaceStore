using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleTextManager : MonoBehaviour
{
    [SerializeField] TMP_Text textComponent;
    [SerializeField] GameObject buttons;
    [SerializeField] float textSpeed = .5f;//TODO: Make text speed a global game variable

    IEnumerator animateText;
    // Start is called before the first frame update
    void Start()
    {
        animateText = AnimateTextCoroutine();
        textComponent.maxVisibleCharacters = 0;
        StartCoroutine(animateText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if ((textComponent.maxVisibleCharacters == textComponent.text.Length))
            {
                gameObject.SetActive(false); 
            }
            else
            {
                textComponent.maxVisibleCharacters = textComponent.text.Length;
                StopCoroutine(animateText);
            }
        }
    }

    private void OnDisable()
    {
        buttons.SetActive(true);
    }

    private void OnEnable()
    {
        buttons.SetActive(false);
    }

    private IEnumerator AnimateTextCoroutine()
    {
        string storytext = textComponent.text;
        int i = 0;
        while (i < storytext.Length + 1)
        {
            textComponent.maxVisibleCharacters = i;
            i++;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
