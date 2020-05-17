using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public int price;
    public bool open;
    public Text purchaseText;

    private Animator leftDoor;
    private Animator rightDoor;
    private bool canBuy;

    private InventoryController inventoryController;

    // Start is called before the first frame update
    void Start()
    {
        leftDoor = transform.GetChild(0).gameObject.GetComponent<Animator>();
        rightDoor = transform.GetChild(1).gameObject.GetComponent<Animator>();
        inventoryController = GameObject.Find("Player").GetComponent<InventoryController>();
        canBuy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && canBuy)
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            purchaseText.text = "Press 'E' to purchase door\n" + price + " Points";
            purchaseText.gameObject.SetActive(true);
            setCanBuy();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            purchaseText.gameObject.SetActive(false);
            canBuy = false;
        }
    }

    private void OpenDoor()
    {
        inventoryController.SpendPoints(price);
        purchaseText.gameObject.SetActive(false);
        canBuy = false;
        GetComponent<BoxCollider>().enabled = false;
        leftDoor.SetTrigger("Open");
        rightDoor.SetTrigger("Open");
    }

    private void setCanBuy()
    {
        if (inventoryController.points >= price) canBuy = true;
        else canBuy = false;
    }
}
