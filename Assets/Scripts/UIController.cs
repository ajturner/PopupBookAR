using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Button infoPanel;    

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private Button loadDesignerButton;

    [SerializeField]
    private Button loadLensButton;

    void Awake()
    {
        dismissButton.onClick.AddListener(Dismiss);

        loadDesignerButton.onClick.AddListener(LoadDesignerScene);

        loadLensButton.onClick.AddListener(LoadLensScene);

        infoPanel.onClick.AddListener(Show);
    }
    
    private void LoadDesignerScene()
    {
        SceneManager.LoadScene("CommunityDesigner");
    }

    private void LoadLensScene()
    {
        SceneManager.LoadScene("WorldLens");
    }

    private void Show() => welcomePanel.SetActive(true);

    private void Dismiss() => welcomePanel.SetActive(false);

}