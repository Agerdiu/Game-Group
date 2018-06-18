using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGhost : MonoBehaviour {
    bool validPos = false;
    Vector3 targetPos;
    public Material material;
    public Material invalidPositionMaterial;
    protected MeshRenderer[] meshRenderers;
    public float dampSpeed = 0.075f;
    protected Vector3 moveVel=Vector3.zero;
    public Collider ghostCollider { get; private set; }
    public float radius;
    public GameObject range;
    // Use this for initialization
    public void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
    {
        targetPos = worldPosition;

        if (!validPos)
        {
            // Immediately move to the given position
            validPos = true;
            transform.position = targetPos;
            range.transform.position = targetPos;
        }

        transform.rotation = rotation;
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.sharedMaterial = validLocation ? material : invalidPositionMaterial;
        }
    }
    public virtual void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            range.SetActive(true);
            validPos = false;
        }

    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
        range.SetActive(false);
    }
    void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        ghostCollider = GetComponent<Collider>();             
        range.transform.localScale = new Vector3(radius,Mathf.Pow(10,-6),radius);
        range = Instantiate(range);
    }
    // Update is called once per frame
    void Update () {
        Vector3 currentPos = transform.position;

        if (Vector3.SqrMagnitude(currentPos - targetPos) > 0.01f)
        {
            currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref moveVel, dampSpeed);
            range.transform.position = currentPos;
            transform.position = currentPos;
        }
        else
        {
            moveVel = Vector3.zero;
        }
    }
    /*
    public bool IsGhostAtValidPosition(TowerPlacementGrid grid )
    {
        if (grid == null)
        {
            return false;
        }
        Vector2 pos2D = grid.WorldToGrid(targetPos, ghostCollider.dimensions);
       TowerFitStatus fits = grid.Fits(targetPos, );
        return fits == TowerFitStatus.Fits;
    }*/
}
