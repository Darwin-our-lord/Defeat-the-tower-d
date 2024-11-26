using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomGen : MonoBehaviour
{

    List<GameObject> roomList = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public RoomSpawn roomSpawnScript;


    public GameObject Room1;
    public GameObject Room2;
    public GameObject Room3;
    public GameObject ItemRoom;
    public GameObject OldRoom;

    GameObject[] everyRoomSpawn;
    public GameObject player;
    public GameObject playerSpawn;


    public int ItemChance;

    public TMP_Text chanceText;

    // Start is called before the first frame update
    void Start()
    {
        enemies.Add(this.gameObject);


        roomList.Add(ItemRoom);
        roomList.Add(Room1);
        roomList.Add(Room2);
        roomList.Add(Room3);

        NextRoom();

    }

    void Update()
    {
        if (enemies.Count == 1)
        {
            GameObject roomspawn = GameObject.FindWithTag("RoomSpawn");
            roomSpawnScript = roomspawn.GetComponent<RoomSpawn>();
            roomSpawnScript.AllowNextRoom();
        }

    }

    public void NextRoom()
    {
        if (ItemChance >= Random.Range(1, 101))
        {
            if (OldRoom != null) Destroy(OldRoom);
            OldRoom = Instantiate(roomList[0]);
            ItemChance = 1;

            player = GameObject.FindWithTag("Player");
            playerSpawn = GameObject.FindWithTag("PlayerSpawn");
            player.transform.position = playerSpawn.transform.position;

            chanceText.text = ItemChance + "%";
        }
        else
        {

            if (OldRoom != null) Destroy(OldRoom);
            int prefabIndex = UnityEngine.Random.Range(1, 3);
            OldRoom = Instantiate(roomList[prefabIndex]);
            ItemChance += Random.Range(1, 16);

            player = GameObject.FindWithTag("Player");
            playerSpawn = GameObject.FindWithTag("PlayerSpawn");
            player.transform.position = playerSpawn.transform.position;

            chanceText.text = ItemChance + "%";

        }
    }
}
