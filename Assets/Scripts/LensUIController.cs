using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LensUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Button infoPanel;    

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private Button homeButton;

    void Awake()
    {
        dismissButton.onClick.AddListener(Dismiss);

        homeButton.onClick.AddListener(LoadHomeScene);

        infoPanel.onClick.AddListener(Show);
    }
    
    private void LoadHomeScene()
    {
        SceneManager.LoadScene("ARScene");
    }

    private void Show() => welcomePanel.SetActive(true);

    private void Dismiss() => welcomePanel.SetActive(false);

}