using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSkill : BaseSkill
{
    public float moveDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleKey();
    }
    
    void HandleKey()
    {
        if (Input.GetKeyDown(key)) {
            activate = true;
            Flash();
            SoundManager.instance.PlaySound(SoundClip.Bullet14);
            activate = false;
        }
    }

    void Flash()
    {
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.RightArrow)) { moveDirection += new Vector3(0, 0, 1); }
        if (Input.GetKey(KeyCode.LeftArrow)) { moveDirection += new Vector3(0, 0, -1); }
        if (Input.GetKey(KeyCode.UpArrow)) { moveDirection += new Vector3(0, 1, 0); }
        if (Input.GetKey(KeyCode.DownArrow)) { moveDirection += new Vector3(0, -1, 0); }

        moveDirection.Normalize();
        if (moveDirection != Vector3.zero) {
            Vector3 target = DecideFlashDestination(moveDirection);
            transform.position = target;
        }
    }

    private Vector3 DecideFlashDestination(Vector3 moveDirection)
    {
        float blockLength = 3;
        float bonusFactor = 0.7f;
        Vector3 right = new Vector3(0, 0, 1);
        Vector3 left = new Vector3(0, 0, -1);
        Vector3 targetPos = transform.position + moveDirection * moveDistance;
        Vector3 finalPos = transform.position;
        Vector3 topPos = targetPos + new Vector3(0, blockLength * bonusFactor, 0);
        Vector3 rightPos = targetPos + new Vector3(0, 0, blockLength * bonusFactor);
        Vector3 leftPos = targetPos + new Vector3(0, 0, -blockLength * bonusFactor);

        if (!isPositionHasObject(targetPos)) {
            finalPos = targetPos;
        } else if (!isPositionHasObject(topPos)) {
            finalPos = topPos;
            Ray downRay = new Ray(finalPos, Vector3.down);
            RaycastHit downRayHit;
            if (Physics.Raycast(downRay, out downRayHit, moveDistance)) {
                if (downRayHit.distance > 0.1f) {
                    finalPos += Vector3.down * (downRayHit.distance - 0.1f);
                }
            }
        } else if (!isPositionHasObject(rightPos) && Vector3.Dot(moveDirection.normalized, right) > 0) {
            finalPos = rightPos;
        } else if (!isPositionHasObject(leftPos) && Vector3.Dot(moveDirection.normalized, left) > 0) {
            finalPos = leftPos;
        } else {
            // The raycast from player and move to the flash target
            Ray directRay = new Ray(transform.position, moveDirection);
            RaycastHit rayHit;
            if (Physics.Raycast(directRay, out rayHit, moveDistance)) {
                finalPos = transform.position + moveDirection * (rayHit.distance - blockLength / 2);
            }
        }

        return finalPos;
    }

    private bool isPositionHasObject(Vector3 position)
    {
        Vector3 rayStartPos = position + new Vector3(10, 0, 0);
        Ray cubeRay = new Ray(rayStartPos, position - rayStartPos);
        RaycastHit rayHit;
        if (Physics.Raycast(cubeRay, out rayHit)) {
            Debug.Log(rayHit.collider.gameObject.name);
            if (rayHit.collider.gameObject.name.IndexOf("BackGround") == -1) {
                return true;
            }
        }
        return false;
    }
}
