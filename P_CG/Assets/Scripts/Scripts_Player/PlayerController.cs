using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;

    private void Awake() {
        playerControls = new PlayerControls(); 
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable(){
         playerControls.Enable(); 
    }
    
    private void Update (){
        PlayerInput();
    }

    private void FixedUpdate(){
        Move();
    }

    private void PlayerInput(){
         movement = playerControls.Movement.Move.ReadValue<Vector2>(); 
    }

    private void Move(){
        rb.MovePosition(rb.position + movement *(moveSpeed * Time.fixedDeltaTime));
       if (movement.x != 0)
       {
          transform.localScale = new Vector3(movement.x > 0 ? -1 : 1, 1, 1);
       }
    
    }

    void Start()
    {
      
    }
}