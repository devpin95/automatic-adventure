using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PaginationButton : MonoBehaviour
{
    public GameObject pager;
    public GameObject paginatorPartner;
    [FormerlySerializedAs("paginateUp")] [Tooltip("True: Page down, False: Page up")] public bool slideUp;
    public float itemHeight;
    public float itemsPerPage;
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

    // Start is called before the first frame update
    void Start()
    {
        _pageRect = pager.GetComponent<RectTransform>();
        _yMin = _pageRect.position.y;
        _currentY = _yMin;
        _yMax = _yMin + pager.transform.childCount * itemHeight;
        _yTransOnPage = (itemsPerPage + 1) * itemHeight;
        if (!slideUp) _yTransOnPage = -_yTransOnPage;
    }

    // Update is called once per frame
    void Update()
    {
        _currentY = _pageRect.position.y;

        if (_lerping)
        {
            _lerpCounter += Time.deltaTime * slideSpeed;
            pager.transform.position = Vector3.Lerp(_oldPos, _newPos, _lerpCounter);
            
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
