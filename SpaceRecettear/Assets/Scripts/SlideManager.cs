using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideManager : MonoBehaviour
{
    [SerializeField]List<GameObject> slides;

    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextSlide()
    {
        slides[currentIndex].SetActive(false);
        currentIndex += 1;
        slides[currentIndex].SetActive(true);
    }

    public void PreviousSlide()
    {
        slides[currentIndex].SetActive(false);
        currentIndex -= 1;
        slides[currentIndex].SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
