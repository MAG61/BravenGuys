using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class RandomMapSelect : MonoBehaviour
{

    public SpriteRenderer[] slots;
    public Sprite[] images;
    public string[] scenes;
    public string selectedScene;
    public float turnSpeed;
    private Rigidbody slot;
    private bool controlB = false;

    void Start()
    {
        StartCoroutine(control());
        slot = GameObject.Find("Slot").GetComponent<Rigidbody>();
        slot.AddForce(new Vector3(0, Random.Range(turnSpeed - 2000, turnSpeed + 2000), 0), ForceMode.Impulse);

        foreach (SpriteRenderer slot in slots)
        {
            string slotMap = scenes[Random.Range(0, scenes.Length)];
            slot.transform.parent = this.slot.transform;

            foreach (Sprite sprite in images)
            {
                if (sprite.name == slotMap) slot.sprite = sprite;
            }
        }
    }

    private void Update()
    {
        if (slot.velocity.y > -3f && controlB)
        {
            slot.transform.DetachChildren();
            foreach (SpriteRenderer slot in slots)
            {
                if (slot.transform.localPosition.y < 602 && slot.transform.localPosition.y > -602)
                {
                    selectedScene = slot.sprite.name;
                    StartCoroutine(goMap());
                }
                else
                {
                    Color color = new();
                    color = Color.red;
                    color.a = 155f;
                    slot.color = color;
                }
            }
        }
    }

    IEnumerator goMap()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.SetMap(selectedScene);
    }

    IEnumerator control()
    {
        yield return new WaitForSeconds(1.5f);
        controlB = true;
    }
}
