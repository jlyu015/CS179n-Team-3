using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icicleScript : MonoBehaviour
{
    public LayerMask groundMask;
    public List<Transform> icicles;
    private List<Rigidbody2D> icicleRBs;
    private List<Collider2D> icicleCols;
    private List<SpriteRenderer> icicleRenderers;
    private bool hitGround = false;
    public float fallDelay = 2f;
    public float respawnDelay = 5f;
    private List<Vector2> startPositions;
    private List<Quaternion> startRotations;

    void Start()
    {
        icicleRBs = new List<Rigidbody2D>();
        icicleCols = new List<Collider2D>();
        icicleRenderers = new List<SpriteRenderer>();
        startPositions = new List<Vector2>();
        startRotations = new List<Quaternion>();

        foreach (Transform icicle in icicles)
        {
            Rigidbody2D icicleRB = icicle.GetComponent<Rigidbody2D>();
            Collider2D icicleCol = icicle.GetComponent<Collider2D>();
            SpriteRenderer icicleRenderer = icicle.GetComponent<SpriteRenderer>();
            icicleRBs.Add(icicleRB);
            icicleCols.Add(icicleCol);
            icicleRenderers.Add(icicleRenderer);
            startPositions.Add(icicle.position);
            startRotations.Add(icicle.rotation);
            icicleRB.simulated = false;
        }

        StartCoroutine(HangBeforeFall(fallDelay));
    }

    void Update()
    {
        if (!hitGround && icicleCols.TrueForAll(icicleCol => icicleCol.IsTouchingLayers(groundMask)))
        {
            hitGround = true;
            foreach (Rigidbody2D icicleRB in icicleRBs)
            {
                icicleRB.simulated = false;
                icicleRB.velocity = Vector2.zero;
                icicleRB.angularVelocity = 0f;
            }
            foreach (SpriteRenderer icicleRenderer in icicleRenderers)
            {
                icicleRenderer.enabled = false;
            }
            StartCoroutine(RespawnAfterDelay(respawnDelay));
        }
    }

    IEnumerator HangBeforeFall(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (Rigidbody2D icicleRB in icicleRBs)
        {
            icicleRB.simulated = true;
        }
    }

    IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < icicles.Count; i++)
        {
            icicles[i].position = startPositions[i];
            icicles[i].rotation = startRotations[i];
            Rigidbody2D icicleRB = icicleRBs[i];
            Collider2D icicleCol = icicleCols[i];
            SpriteRenderer icicleRenderer = icicleRenderers[i];
            icicleCol.enabled = true;
            icicleRenderer.enabled = true;
            icicleRB.simulated = false;
        }
        hitGround = false;
        StartCoroutine(HangBeforeFall(fallDelay));
    }
}