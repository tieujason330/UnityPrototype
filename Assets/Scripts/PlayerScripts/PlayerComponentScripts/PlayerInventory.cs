﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{

    private GameObject head;
    private bool _open;

    public GameObject _slotObject;
    public int _initialSlotX;
    public int _initialSlotY;
    public int _maxColumnCount;
    public int _maxRowCount;
    public int _xOffset;
    public int _yOffset;

    public uint _size;
    //public Dictionary<int, GameObject> _slots;
    //public Dictionary<int, GameObject> _items;
    public GameObject[,] _slots;
    private int _currentRow = 0;
    private int _currentColumn = 0;
    private int _selected;
    private bool _equipping;
    private bool _swapping;
    private int _slotCount;
    private GameObject _currentSlot;

    private bool _selectButton;
    private float _inputHorizontal;
    private float _inputVertical;
    public float _joyStickInputRate = 0.25f;
    private float _nextJoyStickInput = 0.0f;
    public float _iconSize = 75.0f;
    public float _joystickTrigger = 0.5f;

    public float _iconSizeSelected = 90.0f;


    // This should be assigned the canvas gameobject so that we can edit it directly though the editor
    public GameObject _inventoryCanvas;

    // Use this for initialization
    void Start()
    {
        _open = false;
        //_slots = new Dictionary<int, GameObject>();
        _slotCount = _maxColumnCount * _maxRowCount;
        _slots = new GameObject[_maxRowCount, _maxColumnCount];

        for (int i = 0; i < _maxColumnCount; i++)
        {
            for (int j = 0; j < _maxRowCount; j++)
            {
                GameObject slot = Instantiate(_slotObject);
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(_initialSlotX + (i * _xOffset), _initialSlotY - (j * _yOffset), 0);
                slot.transform.SetParent(_inventoryCanvas.transform);
                _slots[j, i] = slot;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_open)
        {
            
        }
    }

    void OnGUI()
    {
        if (_open)
        {

        }
    }


    void UpdateInput()
    {
        _inputHorizontal = Input.GetAxis("InventoryHorizontal");
        _inputVertical = Input.GetAxis("InventoryVertical");
        _selectButton = Input.GetButtonDown("Jump");

        UpdateButtons(_selectButton);
        UpdateJoySticks(_inputHorizontal, _inputVertical);
        
    }
    

    // Take the button inputs and update the inventory gui as needed
    void UpdateButtons(bool select)
    {
        if (_selectButton)
        {
            GameObject item = _currentSlot.GetComponent<InventorySlot>()._slotItem;

            if (item != null)
            {
                item.GetComponent<BaseEquipment>().Equip();
            }
        }
    }


    // This fucked up shit makes me think we should do something else instead of list indices
    // Take the input data for the joysticks and update the inventory gui as needed
    void UpdateJoySticks(float horizontalInput, float verticalInput)
    {
        if (Time.time > _nextJoyStickInput)
        {
            int previousColumn = _currentColumn;
            int previousRow = _currentRow;

            // Math to figure out how much to move the row and column indices by
            if (Mathf.Abs(horizontalInput) > _joystickTrigger)
                _currentColumn = (_currentColumn + _maxColumnCount + (int)(horizontalInput / Mathf.Abs(horizontalInput))) % _maxColumnCount;

            else if (Mathf.Abs(verticalInput) > _joystickTrigger)
                _currentRow = (_currentRow + _maxRowCount + (int)(verticalInput / Mathf.Abs(verticalInput))) % _maxRowCount;

            // update the input rate settings and the current slot value
            if (previousColumn != _currentColumn || previousRow != _currentRow)
            {
                _nextJoyStickInput = Time.time + _joyStickInputRate;
                _currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector3(_iconSize, _iconSize, 0);
                _currentSlot = _slots[_currentRow, _currentColumn];
                _currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector3(_iconSizeSelected, _iconSizeSelected, 0);
            }
        }
    }

    // flip the open variable and return it
    public bool Toggle()
    {
        _open = !_open;
        _inventoryCanvas.SetActive(_open);

        // when the inventory is open the first item in it should be set to 
        // be the currently selected item
        if (_open)
        {
            _currentColumn = 0;
            _currentRow = 0;
            _currentSlot = _slots[0, 0];
            _currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector3(_iconSizeSelected, _iconSizeSelected, 0);
        }
        else
        {
            _currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector3(_iconSize, _iconSize, 0);
        }

        return _open;
    }
    public void PlayerUpdate()
    {
        UpdateInput();
    }

    public void StoreItem(GameObject pickup)
    {
        GameObject openSlot = null;

        // Find the first empy slot in the inventory
        for (int i = 0; i < _maxRowCount; i++)
        {
            for (int j = 0; j < _maxColumnCount; j++)
            {

                GameObject currentSlot = _slots[i, j];
                if (currentSlot.GetComponent<InventorySlot>()._slotItem == null)
                {
                    openSlot = _slots[i, j];
                }

                // once an empty slot is found we can exit both loops
                if (openSlot != null)
                    goto end_of_loop;
            }
        }
        end_of_loop:
            
        if (openSlot == null)
        {
            print("Not enough room in inventory");
        }
        else
        {
            openSlot.GetComponent<InventorySlot>().SetItem(pickup);
            pickup.SetActive(false);
        }
    }
}
