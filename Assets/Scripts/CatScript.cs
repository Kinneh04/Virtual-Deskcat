using CodeMonkey.Utils;
using HuggingFace.API;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatScript : MonoBehaviour
{

    public string Name;
    public bool isDragging = false;
    private Rigidbody2D rb;
    public Vector3 Offset;
    private Button_Sprite CMBS;
    public RectTransform rectTransform;
    public Animator CatAnimator;

    public AnimationClip CatStandIdleAnimClip;
    public AnimationClip[] CatIdleAnimClips;

    float timer = 0;
    public float minIdleAnim, maxIdleAnim;

    float sleepTimer = 0;
    public bool isSleeping = false;
    public float sleepChance = 0.05f;
    public float idleTimer = 0.0f;

    [Header("Popup")]
    public float lastsForTime;
    public GameObject PopupBox;
    public Speechbubble SB;
    public float speechtimer;
    public float minText, maxText;
    [Header("Texts")]
    public List<string> RandomTexts = new List<string>();
    public List<string> RandomTextsSleeping = new List<string>();
    public List<string> RandomJokes = new List<string>();
    public List<string> RandomTextsHello = new List<string>();
    [Header("Keywords")]
    public string[] HelloKeywords;
    public string[] JokeKeywords;
    bool CheckKeywords(string inputString, string[] keywords)
    {
        return keywords.Any(keyword => inputString.Contains(keyword.ToLower()));
    }

    public string chooseRandomFrom(List<string> choices)
    {
        return (choices[Random.Range(0, choices.Count)]);
    }

    public void CatProcessSpeech(string spoken)
    {
        //spoken = spoken.ToLower();
        //if(spoken.Contains(Name))
        //{
        //    if(CheckKeywords(spoken, HelloKeywords))
        //    {
        //        PopUpSpeech(chooseRandomFrom(RandomTextsHello));
        //    }
        //}
        HuggingFace.API.Conversation conversation = new HuggingFace.API.Conversation();
        conversation.AddUserInput("You are a white domestic shorthair cat named muffin. Roleplay as the cat, and I will speak to you as the owner. You will end each response with Meow");
        conversation.AddGeneratedResponse("Ok Meow. My name is muffin, and I am a cat, and you are my new owner");
        HuggingFaceAPI.Conversation(spoken, response =>
        {
            PopUpSpeech(response);
        }, error =>
        {
            PopUpSpeech("Uh, what?");
        }, context: conversation);
    }




    public void PopUpSpeech(string speech, float duration = 4.0f)
    {
        PopupBox.SetActive(false);
        PopupBox.SetActive(true);
        lastsForTime = duration;
        SB.InputNewString(speech);
    }

    private void Start()
    {
        timer = maxIdleAnim;
        rb = GetComponent<Rigidbody2D>();
        CMBS = GetComponent<Button_Sprite>();
        CMBS.MouseDownOnceFunc = () =>
        {
            StartDrag();
        };
        CMBS.MouseUpOnceFunc = () =>
        {
            EndDrag();
        };

        PopUpSpeech("Meow");
        speechtimer = maxText;
    }
    private void StartDrag()
    {
        isDragging = true;
        rb.gravityScale = 0f; // Disable gravity while dragging
    }

    private void EndDrag()
    {
        isDragging = false;
        rb.gravityScale = 1f; // Enable gravity after releasing the sprite
    }

    private void Update()
    {

        if (speechtimer > 0 && !PopupBox.activeSelf)
        {
            speechtimer -= Time.deltaTime;
            if (speechtimer <= 0)
            {
                speechtimer = Random.Range(minText, maxText);
                if (isSleeping)
                {
                    PopUpSpeech(RandomTextsSleeping[Random.Range(0, RandomTexts.Count)]);
                }
                else
                    PopUpSpeech(RandomTexts[Random.Range(0, RandomTexts.Count)]);
            }
        }

        if (PopupBox.activeSelf)
        {
            lastsForTime -= Time.deltaTime;
            if (lastsForTime <= 0) PopupBox.SetActive(false);
        }

        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
  Input.mousePosition.y, 10.0f));
            //Debug.Log(mousePos);
            transform.position = new Vector2(mousePos.x, mousePos.y);

        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + Offset, Vector2.down, 0.1f);

        if (hit.collider == null)
        {
            //  Debug.Log("Sprite is in the air!");
            CatAnimator.SetBool("isFalling", true);
        }
        else
        {
            //Debug.Log(hit.collider.name);
            CatAnimator.SetBool("isFalling", false);

            if (!isDragging && !isSleeping) idleTimer += Time.deltaTime;
            else idleTimer = 0;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && !isDragging && !isSleeping)
            {
                CatAnimator.Play(CatIdleAnimClips[Random.Range(0, CatIdleAnimClips.Length)].name);
                timer = Random.Range(minIdleAnim, maxIdleAnim);
            }
        }

        if (idleTimer > 5f && !isSleeping)
        {
            isSleeping = true;
            CatAnimator.SetBool("isSleeping", true);
            sleepTimer = 10.0f;
            idleTimer = 0.0f;
        }
        else if (isSleeping)
        {
            sleepTimer -= Time.deltaTime;
            if (sleepTimer <= 0)
            {
                CatAnimator.SetBool("isSleeping", false);
                isSleeping = false;
            }
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    // Create a ray from the mouse position
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit2;

        //    // Perform the raycast
        //    if (Physics.Raycast(ray, out hit2))
        //    {
        //        // Check if the object hit is tagged as "cat"
        //        if (hit2.collider.CompareTag("cat") && !isDragging)
        //        {
        //            StartDrag();
        //        }
        //    }
        //}

        //if (Input.GetMouseButtonUp(1))
        //{
        //    if (isDragging)
        //    {
        //        EndDrag();
        //    }
        //}

        // else Debug.Log(hit.collider.name);
    }
}
