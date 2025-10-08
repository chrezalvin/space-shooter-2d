using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipSelectionManager : MonoBehaviour
{
    public static Action<Buff> OnBuffSelected;
    public static Action<string> OnShipSelected;

    public GameObject shipPreviewContainer;

    public TextMeshProUGUI shipName;
    public TextMeshProUGUI shipDesc;
    public DiscreteBarUI shipHP;
    public BarSliderUI shipMovSpd;
    public BarSliderUI shipAtkSpd;

    public Button selectButton;
    public Text selectButtonText;

    public RenderTexture shipPreviewTexture;
    public RawImage shipPreviewRawImage;
    public Image evolveSpriteImg;

    private ShipEntry m_shipEntry;
    private GameObject m_shipPreviewObj;

    public void Set(ShipEntry shipEntry)
    {
        string selectedShipName = PlayerPrefs.GetString("SelectedShip", "Default");

        m_shipEntry = shipEntry;
        shipPreviewRawImage.texture = shipEntry.shipIcon.texture;
        evolveSpriteImg.sprite = shipEntry.ship.GetEvolveBuff().GetIcon();

        shipName.text = m_shipEntry.ship.GetShipName();
        shipDesc.text = m_shipEntry.ship.GetDescription();
        shipHP.SetValue(m_shipEntry.ship.GetHP());
        shipMovSpd.SetPercentage(m_shipEntry.ship.GetMovementSpeed() / 20f);
        shipAtkSpd.SetPercentage(1 - m_shipEntry.ship.GetAttackSpeed() / 1f);

        selectButton.interactable = selectedShipName != m_shipEntry.ship.GetShipName();
        if (selectedShipName != m_shipEntry.ship.GetShipName())
        {
            selectButton.onClick.AddListener(OnSelectButtonClicked);
            selectButtonText.text = "Choose Ship";
        }
        else
        {
            selectButtonText.text = "Selected";
        }
    }

    public void OnEvolveIconClicked()
    {
        OnBuffSelected?.Invoke(m_shipEntry.ship.GetEvolveBuff());
    }

    private void OnSelectButtonClicked()
    {
        string selectedShipName = m_shipEntry.ship.GetShipName();
        // set ship preference
        PlayerPrefs.SetString("SelectedShip", selectedShipName);
        Debug.Log("Selected ship: " + selectedShipName);

        OnShipSelected?.Invoke(selectedShipName);
    }

    public void SetShipPreviewPlayer(bool active)
    {
        if (active && m_shipPreviewObj == null && m_shipEntry != null)
        {
            // instantiate ship at the center view of camera
            GameObject previewCameraShipObj = Instantiate(m_shipEntry.shipPrefab, shipPreviewContainer.transform);
            PreviewPlayerBehaviour previewPlayerBehaviour = previewCameraShipObj.AddComponent<PreviewPlayerBehaviour>();
            previewPlayerBehaviour.Init(m_shipEntry);

            m_shipPreviewObj = previewCameraShipObj;
        }

        if (active)
            shipPreviewRawImage.texture = shipPreviewTexture;
        else
            shipPreviewRawImage.texture = m_shipEntry.shipIcon.texture;
            
        shipPreviewContainer.SetActive(active);
    }
}
