using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cut_Action : MonoBehaviour
{
    public GameObject cutPrefab;
    public float spacing = 0.1f;
    public GameObject painOverlay;
    public Transform parentTransform;
    public AudioSource pigPain;

    private List<Vector3> cutPath = new List<Vector3>();
    private Vector3 lastPosition;

    public Image healthBar;
    public double healthAmount = 100f;

    



    void Start()
    {        
        //Turn sad face off
        painOverlay.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cutPath.Clear();
            lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastPosition.z = 0;            
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0;

            //Raycast to detect if the mouse is over the pig sprite
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, Vector2.zero);

            //If cursor is inside pig sprite
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                if (painOverlay != null)
                {
                    // Toggle the overlay on each click
                    painOverlay.SetActive(!painOverlay.activeSelf);
                    pigPain.Play();
                }

                //Checks the position of sprites and spacing of sprites (defined in inspector)
                if (Vector3.Distance(currentPosition, lastPosition) >= spacing)
                {
                    cutPath.Add(currentPosition);
                    lastPosition = currentPosition;

                    // Generate a random rotation
                    Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

                    //Instantiate the cut prefab at current position
                    Instantiate(cutPrefab, currentPosition, randomRotation, parentTransform);

                    //Take damage with each 
                    TakeDamage(0.5);
                }
            }
        }
        
    }

    public void TakeDamage(double damage)
    {       
        healthAmount -= damage;

        if (healthAmount <= 0)
        {
            Debug.Log("Pig is dead");
        }



        //Removes green bar from health bar UI
        healthBar.fillAmount = (float)(healthAmount /100f);      
        
        //Debug.LogError(healthAmount.ToString());

    }
}
