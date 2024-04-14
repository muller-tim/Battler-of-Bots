using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject lowerBody;
    [SerializeField] private GameObject upperBody;
    [SerializeField] private float controllerDeadZone = 0.1f;
    [SerializeField] private float rotateSmoothing = 1000f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    public Vector2 MovementValue { get; private set; }
    public Vector2 AimValue { get; private set; }
    public bool Move { get; private set; }
    public bool Aim { get; private set; }
    public bool Dash { get; private set; }
    public bool StandardAttack { get; private set; }
    public bool HeavyAttack { get; private set; }
    public bool AOEAttack { get; private set; }
    public bool ShortDistanceAttack { get; private set; }
    public bool Menu { get; private set; }
    private bool isGamepad;

    private void FixedUpdate() {

        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        MovePlayer();
        RotatePlayer();
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Menu pressed");
        if (context.performed)
        {
            Menu = true;
        }
        else if (context.canceled)
        {
            Menu = false;
        }
        if (Menu = true)
        {
            MenuManager.ShowMenu("Options");
        }
        else
        {
            MenuManager.ShowMenu("Game HUD");
        }
    }

    public void OnStandardAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StandardAttack = true;
        }
        else if (context.canceled)
        {
            StandardAttack = false;
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HeavyAttack = true;
        }
        else if (context.canceled)
        {
            HeavyAttack = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Dash = true;
        }
        else if (context.canceled)
        {
            Dash = false;
        }
    }

    public void OnAOEAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AOEAttack = true;
        }
        else if (context.canceled)
        {
            AOEAttack = false;
        }
    }

    public void OnShortDistanceAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShortDistanceAttack = true;
        }
        else if (context.canceled)
        {
            ShortDistanceAttack = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
        if (context.performed)
        {
            Move = true;
        }
        else if (context.canceled)
        {
            Move = false;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimValue = context.ReadValue<Vector2>();
        if (context.performed)
        {
            Aim = true;
        }
        else if (context.canceled)
        {
            Aim = false;
        }
    }
    
    private void MovePlayer(){
        Vector3 movement =  new Vector3(MovementValue.x, 0, MovementValue.y);
        
        characterController.Move(movement * speed * Time.fixedDeltaTime);

        playerVelocity.y += gravityValue * Time.fixedDeltaTime;
        characterController.Move(playerVelocity * Time.fixedDeltaTime);

        // Richtung der Bewegung überprüfen und lowerBody drehen
        if (movement != Vector3.zero) {
            animator.SetBool("Move", true);
            Vector3 direction = Vector3.right * MovementValue.x + Vector3.forward * MovementValue.y;
            if(direction.sqrMagnitude > 0.0f){
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                rotation *= Quaternion.Euler(0, -90, 0);
                lowerBody.transform.rotation = Quaternion.RotateTowards(lowerBody.transform.rotation, rotation, rotateSmoothing * Time.fixedDeltaTime);
            }
        }
        else{
            animator.SetBool("Move", false);
        }
    }

    private void RotatePlayer(){
        if(isGamepad){
            if(Mathf.Abs(AimValue.x) > controllerDeadZone || Mathf.Abs(AimValue.y) > controllerDeadZone){
                Vector3 direction = Vector3.right * AimValue.x + Vector3.forward * AimValue.y;
                if(direction.sqrMagnitude > 0.0f){
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    rotation *= Quaternion.Euler(0, -90, 0);
                    upperBody.transform.rotation = Quaternion.RotateTowards(upperBody.transform.rotation, rotation, rotateSmoothing * Time.fixedDeltaTime);
                }
            }
        }
        else{
            Ray ray = Camera.main.ScreenPointToRay(AimValue);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if(groundPlane.Raycast(ray, out rayDistance)){
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
    }

    private void LookAt(Vector3 point)
    {
        Vector3 heightCorrectedPoint = new Vector3(point.x, upperBody.transform.position.y, point.z);
        upperBody.transform.LookAt(heightCorrectedPoint);
        upperBody.transform.rotation *= Quaternion.Euler(0, -90, 0);
    }

    public void OnDeviceChange(PlayerInput input){
        isGamepad = input.currentControlScheme.Equals("Gamepad") ? true : false;
    }
}
