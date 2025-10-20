using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public ShipDatabase shipDatabase;
    public float fadeDuration = .5f;

    public Canvas canvas;
    public GameObject chooseShipMenuPrefab;
    public GameObject buffInfoPanelPrefab;
    public GameObject transitionScreenPrefab;

    public TextMeshProUGUI versionText;

    private GameObject m_buffInfoPanel;
    private GameObject m_chooseShipMenu;

    private Stack<GameObject> m_breadcrumb = new Stack<GameObject>();

    private BuffInfoManager m_buffInfoManager;
    private ChooseShipMenuManager m_chooseShipMenuManager;
    private TransitionScreenBehaviour m_transitionScreenBehaviour;

    void Start()
    {
        m_chooseShipMenu = Instantiate(chooseShipMenuPrefab, canvas.transform);
        if (m_chooseShipMenu.TryGetComponent<ChooseShipMenuManager>(out m_chooseShipMenuManager))
        {
            m_chooseShipMenuManager.Init(CloseCurrentMenu, OpenBuffInfoPanel, OnShipSelected);
            m_chooseShipMenu.SetActive(false);
        }
        
        m_buffInfoPanel = Instantiate(buffInfoPanelPrefab, canvas.transform);
        if (m_buffInfoPanel.TryGetComponent<BuffInfoManager>(out m_buffInfoManager))
        {
            m_buffInfoManager.Init(CloseCurrentMenu);
            m_buffInfoPanel.SetActive(false);
        }

        versionText.text = "version: " + Application.version;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && m_breadcrumb.Count > 0)
            CloseCurrentMenu();
    }

    public void LoadBattleScene()
    {
        GameObject transitionScreenObj = Instantiate(transitionScreenPrefab, canvas.transform);
        if (transitionScreenObj.TryGetComponent<TransitionScreenBehaviour>(out m_transitionScreenBehaviour))
        {
            m_transitionScreenBehaviour.TransitionScreen(fadeDuration, () => {
                SceneManager.LoadScene("BattleScene");
            });
            
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void CloseCurrentMenu()
    {
        if (m_breadcrumb.Count > 0)
        {
            GameObject topMenu = m_breadcrumb.Pop();
            topMenu.SetActive(false);
        }
    }

    public void OpenChooseShipMenu()
    {
        // open choose ship menu
        if (m_chooseShipMenu)
        {
            m_chooseShipMenu.SetActive(true);
            m_chooseShipMenuManager.Set(shipDatabase);
            m_breadcrumb.Push(m_chooseShipMenu);
        }
    }

    public void OnShipSelected(ShipEntry shipEntry)
    {
        PlayerPrefs.SetString("SelectedShip", shipEntry.ship.GetShipName());
        m_chooseShipMenuManager.Set(shipDatabase);
    }

    public void OpenBuffInfoPanel(Buff buff)
    {
        if (m_buffInfoPanel)
        {
            m_buffInfoPanel.SetActive(true);
            m_buffInfoManager.Set(buff);
            m_breadcrumb.Push(m_buffInfoPanel);
        }
    }
}
