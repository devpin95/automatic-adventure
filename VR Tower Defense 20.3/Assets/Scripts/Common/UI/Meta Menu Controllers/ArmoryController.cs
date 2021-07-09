using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ArmoryController : MonoBehaviour
{
    public GameData gameData;
    public MainTowerAttributes mainTowerAttributes;
    
    public CanvasGroup panelGroup;
    public GameObject infoPanel;
    public GameObject emptyItemPanel;

    [Header(("Panel Fields"))] 
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI baseDamageField;
    public TextMeshProUGUI itemTypeField;
    public TextMeshProUGUI fireRateField;
    public TextMeshProUGUI upgradeableField;
    public TextMeshProUGUI itemCountField;
    
    [Header("Purchase Group")]
    public TextMeshProUGUI costField;
    public Button purchaseButton;
    public GameObject fundWarningText;

    [Header("Item Preview")] 
    public Transform itemAttachPoint;
    public Camera itemPreviewCamera;
    private GameObject _itemPreviewInstance;

    [Header("Lists")] 
    public HeavyWeaponList heavyWeaponList;
    private List<HeavyWeaponCard> _activeItems;
    [FormerlySerializedAs("pageLeftButton")] public GameObject pageLeftButtonObj;
    [FormerlySerializedAs("pageRightButton")] public GameObject pageRightButtonObj;
    private Button _pageLeftButton;
    private CanvasGroup _pageLeftButtonCG;
    private Button _pageRightButton;
    private CanvasGroup _pageRightButtonCG;
    
    private int _itemIndex = 0;
    private HeavyWeaponCard _currentCard;

    [SerializeField] private bool _fadingOut = false;
    [SerializeField] private bool _fadingIn = false;
    [SerializeField] private float _fadeCounter = 0;
    private float _cameraOrthoSizeCounter = 0;
    private float _currentOrthoSize;
    private float _targetOrthoSize;
    
    // Start is called before the first frame update
    void Start()
    {
        // heavyWeaponList.ResetObject();
        _activeItems = heavyWeaponList.GetActiveCardList();

        if ( _activeItems != null )
        {
            _currentCard = _activeItems[0];
            _currentOrthoSize = _currentCard.cameraOrthographicSize;
            itemPreviewCamera.orthographicSize = _currentOrthoSize;
        }
        
        _pageLeftButton = pageLeftButtonObj.GetComponent<Button>();
        _pageLeftButtonCG = pageLeftButtonObj.GetComponent<CanvasGroup>();
        _pageLeftButton.onClick.AddListener(PageLeft);
        
        _pageRightButton = pageRightButtonObj.GetComponent<Button>();
        _pageRightButtonCG = pageRightButtonObj.GetComponent<CanvasGroup>();
        _pageRightButton.onClick.AddListener(PageRight);

        ShowItemCard();
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadingOut)
        {
            _fadeCounter += Time.deltaTime * 5;
            panelGroup.alpha = Mathf.Lerp(1, 0, _fadeCounter);
            if (_fadeCounter >= 1)
            {
                _fadeCounter = 0;
                _fadingOut = false;
                _fadingIn = true;
                ShowItemCard();
            }
        } 
        else if (_fadingIn)
        {
            _fadeCounter += Time.deltaTime * 5;
            panelGroup.alpha = Mathf.Lerp(0, 1, _fadeCounter);
            itemPreviewCamera.orthographicSize = Mathf.SmoothStep(_currentOrthoSize, _targetOrthoSize, _fadeCounter);
            if (_fadeCounter >= 1)
            {
                _fadeCounter = 0;
                _fadingOut = false;
                _fadingIn = false;
            }
        }
    }

    private void PageLeft()
    {
        if (_fadingIn)
        {
            _fadingIn = false;
            _fadingOut = true;
        }
        
        Debug.Log("PAGE LEFT");
        --_itemIndex;

        if (_itemIndex < 0)
        {
            _itemIndex = _activeItems.Count - 1;
        }
        
        _currentCard = _activeItems[_itemIndex];
        _fadingOut = true;

        _currentOrthoSize = _currentCard.cameraOrthographicSize * 1.5f;
        _targetOrthoSize = _currentCard.cameraOrthographicSize;
    }

    private void PageRight()
    {
        if (_fadingIn)
        {
            _fadingIn = false;
            _fadingOut = true;
        }

        Debug.Log("PAGE RIGHT");
        ++_itemIndex;

        if (_itemIndex >= _activeItems.Count)
        {
            _itemIndex = 0;
        }
        
        _currentCard = _activeItems[_itemIndex];
        _fadingOut = true;
        
        _currentOrthoSize = _currentCard.cameraOrthographicSize * 1.5f;
        _targetOrthoSize = _currentCard.cameraOrthographicSize;
    }

    private void ShowItemCard()
    {
        if (_activeItems == null || _activeItems.Count == 0 || _currentCard == null)
        {
            infoPanel.SetActive(false);
            emptyItemPanel.SetActive(true);

            return;
        } 
        
        if (_activeItems.Count == 1)
        {
            _pageLeftButtonCG.alpha = 0f;
            _pageLeftButtonCG.interactable = false;
            _pageRightButtonCG.alpha = 0f;
            _pageRightButtonCG.interactable = false;
        }
        
        itemCountField.text = (_itemIndex + 1) + "/" + _activeItems.Count;
        itemName.text = _currentCard.weaponName;
        itemDescription.text = _currentCard.description;
        baseDamageField.text = _currentCard.baseDamage.ToString("n0");
        itemTypeField.text = HeavyWeaponCard.HeavyWeaponTypeNames[_currentCard.heavyWeaponType];
        fireRateField.text = _currentCard.rpm.ToString("n2");
        upgradeableField.text = _currentCard.isUpgradeable ? "Yes" : "No";
        
        if ( _itemPreviewInstance != null ) Destroy(_itemPreviewInstance);

        _itemPreviewInstance =
            Instantiate(_currentCard.prefab, itemAttachPoint.position + _currentCard.previewOffset, _currentCard.prefab.transform.rotation);

        costField.text = "C." + _currentCard.cost.ToString("n0");

        if (gameData.gold < _currentCard.cost)
        {
            fundWarningText.SetActive(true);
            purchaseButton.gameObject.SetActive(false);
        }
        else
        {
            fundWarningText.SetActive(false);
            purchaseButton.gameObject.SetActive(true);
            
            var capturedCardInstance = _currentCard;
            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(() => PurchaseItem(capturedCardInstance));   
        }
    }

    public void PurchaseItem(HeavyWeaponCard item)
    {
        Debug.Log("Purchasing " + item.weaponName);
        
        gameData.gold -= item.cost;
        item.purchased = true;
        heavyWeaponList.PurchaseItem(item);
        
        _activeItems = heavyWeaponList.GetActiveCardList();

        if (_activeItems != null)
        {
            _itemIndex = 0;
            _currentCard = _activeItems[0];
            _currentOrthoSize = _currentCard.cameraOrthographicSize * 1.5f;
            _targetOrthoSize = _currentCard.cameraOrthographicSize;
        }

        _fadingOut = true;

        if (mainTowerAttributes.heavyWeaponPrefab == null)
        {
            mainTowerAttributes.heavyWeaponPrefab = _currentCard.prefab;
        }
    }

    private void OnDestroy()
    {
        heavyWeaponList.ResetObject();
    }
}
