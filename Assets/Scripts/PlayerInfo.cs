using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private int score = 0;

    [SerializeField] 
    private List<Collectable> inventory = new List<Collectable>();


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewLevelLoaded;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= NewLevelLoaded;
    }

    void NewLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerManagerScript player = GameObject.FindWithTag("Player")?.GetComponent<PlayerManagerScript>();

        if (player != null)
        {
            player.health = health;
            player.score = score;
            foreach(Collectable item in inventory)
            {
                player.inventory.Add(item);
            }
            player.OnHealthChange.AddListener(HealthChange);
            player.OnScoreChange.AddListener(ScoreChange);

            player.OnInventoryAdd.AddListener(InventoryAdd);
            player.OnInventoryRemove.AddListener(InventoryRemove);
        }
    }

    void HealthChange(int newHealth)
    {
        health = newHealth;
    }

    void ScoreChange(int newScore)
    {
        score = newScore;
    }
    void InventoryAdd(Collectable item)
{
    inventory.Add(item);
}

void InventoryRemove(Collectable item)
{
    if (item == null)
    {
        inventory.Clear();
    }
    else
    {
        inventory.Remove(item);
    }
}




}
