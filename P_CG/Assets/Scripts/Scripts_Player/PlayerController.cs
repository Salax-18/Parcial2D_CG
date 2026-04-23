using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;

    public List<Vector3> positionHistory = new List<Vector3>();
    public float recordTime = 0.02f;
    private float timer;

    public PlayerController leader;
    public int delayIndex = 0;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;

   
    private Vector3 originalScale;

    private void Awake() {
        playerControls = new PlayerControls(); 
        rb = GetComponent<Rigidbody2D>();

        
        originalScale = transform.localScale;

        if (gameObject.name == "Player_2")
            leader = GameObject.Find("Player_1").GetComponent<PlayerController>();

        if (gameObject.name == "Player_3")
            leader = GameObject.Find("Player_2").GetComponent<PlayerController>();

        if (gameObject.name == "Player_4")
            leader = GameObject.Find("Player_3").GetComponent<PlayerController>();

    }

    private void OnEnable(){
         playerControls.Enable(); 
    }
    
    private void Update (){
        PlayerInput();

        timer += Time.deltaTime;

        if (timer >= recordTime)
        {
            timer = 0;
            positionHistory.Insert(0, transform.position);
        }
    }

    private void FixedUpdate(){
      
       if (gameObject.name == "Player_1")
        {
            Move();
        }
        else
        {
            FollowLeader();
        }
    }

    private void PlayerInput(){
         movement = playerControls.Movement.Move.ReadValue<Vector2>(); 
    }

    private void Move(){
        rb.MovePosition(rb.position + movement *(moveSpeed * Time.fixedDeltaTime));
       if (movement.x != 0)
       {
          transform.localScale = new Vector3(movement.x > 0 ? -originalScale.x : originalScale.x, originalScale.y, originalScale.z);
       }
    
    }

    private void FollowLeader()
  {
    if (leader != null && leader.positionHistory.Count > delayIndex)
    {
      
        if (leader.positionHistory[0] == leader.positionHistory[1])
            return;

        Vector3 targetPos = leader.positionHistory[delayIndex];

        
        if (targetPos.x > transform.position.x)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else if (targetPos.x < transform.position.x)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);

        
        transform.position = targetPos;
    }
  }


    void Start()
    {
      if (gameObject.name == "Player_2") delayIndex = 20;
      if (gameObject.name == "Player_3") delayIndex = 10;
      if (gameObject.name == "Player_4") delayIndex = 10; 
    }
}