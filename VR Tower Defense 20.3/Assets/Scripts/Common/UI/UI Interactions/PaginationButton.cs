using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PaginationButton : MonoBehaviour
{
    public GameObject pager;
    public GameObject paginatorPartner;
    [FormerlySerializedAs("paginateUp")] [Tooltip("True: Page down, False: Page up")] public bool slideUp;
    public float itemHeight;
    [FormerlySerializedAs("itemsPerPage")] public int rowsPerPage;
    public float slideSpeed = 2;
    [SerializeField] private float _yMin;
    [SerializeField]  private float _yMax;
    [SerializeField] private float _currentY;
    private RectTransform _pageRect;
    private float _yTransOnPage;
    public bool _lerping = false;
    private float _lerpCounter = 0;
    private Vector3 _oldPos;
    private Vector3 _newPos;
    private float ySpacing;

    // Start is called before the first frame update
    void Start()
    {
        _pageRect = pager.GetComponent<RectTransform>();
        _yMin = _pageRect.position.y;
        _currentY = _yMin;
        _yMax = _yMin + pager.transform.childCount * itemHeight;
        
        ySpacing = pager.GetComponent<GridLayoutGroup>().spacing.y;
        _yTransOnPage = (rowsPerPage + 1) * itemHeight + (ySpacing * rowsPerPage);
        
        if (!slideUp) _yTransOnPage = -_yTransOnPage;
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentY = _pageRect.position.y;

        if (_lerping)
        {
            _lerpCounter += Time.deltaTime * slideSpeed;
            float smoothstepY = Mathf.SmoothStep(_oldPos.y, _newPos.y, _lerpCounter);
            pager.transform.position = new Vector3(pager.transform.position.x, smoothstepY, pager.transform.position.z);
            // pager.transform.position = Vector3.Lerp(_oldPos, _newPos, _lerpCounter);
            
            if (_lerpCounter >= 1)
            {
                _lerping = false;
                _lerpCounter = 0;
            }
        }
    }

    public void OnClick()
    {
        if (paginatorPartner.GetComponent<PaginationButton>()._lerping || this._lerping) return;
        
        float newYpos = _currentY + _yTransOnPage;
        
        Debug.Log("Trying to move to " + newYpos);

        if (newYpos < _yMax && newYpos >= _yMin - 0.1f)
        {
            _oldPos = pager.transform.position;
            _newPos = new Vector3(pager.transform.position.x,  newYpos, pager.transform.position.z);
            _lerping = true;
        }
    }
}
