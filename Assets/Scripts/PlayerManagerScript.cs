using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class PlayerManagerScript : MonoBehaviour
{
    // Player Values
    public int health = 100;
    public int score = 0;
 
    // Bool Values
    private bool isPaused = false;
 
    // Events
    public UnityEvent OnPauseToggle;
    public UnityEvent OnLoseGame;
    public UnityEvent_Int OnHealthChange;
    public UnityEvent_Int OnScoreChange;
 
    // Keycodes
    public KeyCode useKey = KeyCode.E;
    public KeyCode swapKey = KeyCode.I;
 
    // New variables
    public List<Collectable> inventory = new List<Collectable>();

        public UnityEvent_Collectable OnInventoryAdd;
    public UnityEvent_Collectable OnInventoryChange;
    public UnityEvent_Collectable OnInventoryRemove;

    [SerializeField]
    private int currentSelection = 0;
    private int idCounter = 1;
    void Awake()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
    }
 
    // Start is called before the first frame update
    void Start()
    {
        OnHealthChange?.Invoke(health);
        OnScoreChange?.Invoke(score);
        if(inventory.Count > 0){
            OnInventoryChange?.Invoke(inventory[currentSelection]);
        }
    }
 
    private void OnEnable()
    {
        OnPauseToggle.AddListener(TogglePauseGame);
    }
 
    private void OnDisable()
    {
        OnPauseToggle.RemoveListener(TogglePauseGame);
    }
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPauseToggle?.Invoke();
        }
        if(Input.GetKeyDown(useKey) && inventory.Count > 0){
            Collectable currentItem = inventory[currentSelection];
            InventoryRemove();
            currentItem.Use();
        }
        if(Input.GetKeyDown(swapKey) && inventory.Count > 0) {
            OnInventoryChange?.Invoke(inventory[currentSelection]);
            currentSelection = (currentSelection + 1) % inventory.Count;
        }
    }
 
    public void SetPlayerInfo(int newHealth, int newScore)
    {
        health = newHealth;
        score = newScore;
        OnHealthChange?.Invoke(newHealth);
        OnHealthChange?.Invoke(newScore);
    }
 
    public void ChangeHealth(int value)
    {
        health += value;
        OnHealthChange?.Invoke(health);
        if (health <= 0)
        {
            OnLoseGame?.Invoke();
        }
    }
 
    public void ChangeScore(int value)
    {
        score += value;
        OnScoreChange?.Invoke(score);
    }
 
    private void TogglePauseGame()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0.0f;
        }
    }
 
    private void OnDestroy()
    {
        OnPauseToggle.RemoveAllListeners();
        OnLoseGame.RemoveAllListeners();
        OnHealthChange.RemoveAllListeners();
        OnScoreChange.RemoveAllListeners();
    }
 
    // Adds the object to the object to your inventory and executes its function
    // For coin, it will increase the score
    private void InventoryAdd(Collectable item){
        // ($"") - f-string
        // Difference between f-string and normal strings is
        // the f-string can have variables in it while the normal string cannot
        // i.e idCounter = 10, String s = $"{idCounter}"
        // What would s print out?
                // 10 or {idCounter}
        OnInventoryChange?.Invoke(inventory[currentSelection]);
        item.collectableName += $"{idCounter++}";
        item.player = this.gameObject;
        item.transform.parent = null;
        currentSelection = inventory.Count - 1;
        item.gameObject.SetActive(false);
        DontDestroyOnLoad(item.gameObject);
    }
 
    private void InventoryRemove()
    {
        OnInventoryChange?.Invoke(inventory[currentSelection]);
        inventory.RemoveAt(currentSelection);
        if(inventory.Count == 0){
            currentSelection = 0;
            OnInventoryChange?.Invoke(null);
        }
        else {
            currentSelection = (currentSelection+1) % inventory.Count;
            OnInventoryChange?.Invoke(inventory[currentSelection]);
        }
    }
 
    private void OnTriggerEnter2D(Collider2D collision) {
        Collectable item = collision.GetComponent<Collectable>();
        if(item!=null){
            Debug.Log(item);
            inventory.Add(item);
            InventoryAdd(item);
        }
    }
 
}
 
[System.Serializable]
public class UnityEvent_Int : UnityEvent<int>
{
}


[System.Serializable]
  public class UnityEvent_Collectable: UnityEvent<Collectable>
  {}