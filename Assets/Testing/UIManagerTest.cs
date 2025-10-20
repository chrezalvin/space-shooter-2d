using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerTest : MonoBehaviour
{
    public Canvas canvas;
    public GameObject gameScreenUIPrefab;

    private int m_total = 100;
    private int m_current = 100;

    private GameScreenUI m_gameScreenUI;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameScreenUIObj = Instantiate(gameScreenUIPrefab, canvas.transform);
        if (gameScreenUIObj.TryGetComponent<GameScreenUI>(out m_gameScreenUI))
        {
            m_gameScreenUI.Init();
            m_gameScreenUI.SetHealth(m_total, m_current);
        }
        else
            Debug.LogError("Failed to instantiate GameScreenUI from prefab.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_current -= 10;
            if (m_current < 0)
                m_current = 0;

            Debug.Log(m_gameScreenUI);

            m_gameScreenUI.SetHealth(m_total, m_current);
        }
    }

    public void TestFunction(int i1, float f1, string s1)
    {
        Debug.Log("TestFunction called with parameters: " + i1 + ", " + f1 + ", " + s1);
    }

    public void TestFunctionNoParams()
    {
        Debug.Log("TestFunctionNoParams called.");
    }

    public void TestFunctionOneParam(Ship ship)
    {
        Debug.Log("TestFunctionOneParam called with parameter: ");
    }

    public void TestFunctionTwoParams(int i1, float f1)
    {
        Debug.Log("TestFunctionTwoParams called with parameters: " + i1 + ", " + f1);
    }
}
