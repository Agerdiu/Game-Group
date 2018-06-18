using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ViewControll : MonoBehaviour {
    public float speed = 100;
    public float mouseSpeed = 60;
    public TowerGhost ghost;
    public TowerLevel tower;
    // Update is called once per frame
    void Start()
    {
        ghost = Instantiate(ghost);
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouse = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(new Vector3(h * speed, mouse * mouseSpeed, v * speed) * Time.deltaTime, Space.World);
        bool flag = Input.GetMouseButtonDown(0);
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Grid"));
            //ghost.Show();
            if (hit.collider != ghost.GetComponent<Collider>())
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (isCollider)
                {

                    TowerPlacementGrid CurrentGrid = hit.collider.gameObject.transform.parent.parent.GetComponent<TowerPlacementGrid>();
                    Vector2 GridPosition = CurrentGrid.WorldToGrid(hit.point, tower.dimensions);
                    bool validPos = CurrentGrid.Fits(GridPosition, tower.dimensions) == TowerFitStatus.Fits;
                    // ghost.Move(CurrentGrid.GridToWorld(GridPosition, tower.dimensions), Quaternion.identity, validPos);
                    ghost.Move(hit.point, Quaternion.identity, validPos);
                    
                }
                else
                {
                    Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Plane"));
                    if (hit.collider != ghost.GetComponent<Collider>())
                        ghost.Move(hit.point, Quaternion.identity, isCollider);
                }
                if (flag && isCollider)
                {
                    TowerPlacementGrid CurrentGrid = hit.collider.gameObject.transform.parent.parent.GetComponent<TowerPlacementGrid>();
                    Vector2 GridPosition = CurrentGrid.WorldToGrid(hit.point, tower.dimensions);
                    Debug.Log(GridPosition);
                    bool validPos = flag && (CurrentGrid.Fits(GridPosition, tower.dimensions) == TowerFitStatus.Fits);
                    Debug.Log(CurrentGrid.Fits(GridPosition, tower.dimensions));
                    if (CurrentGrid.Fits(GridPosition, tower.dimensions) == TowerFitStatus.Fits)
                    {
                        Vector3 pos = CurrentGrid.GridToWorld(GridPosition, tower.dimensions);
                        CurrentGrid.SetOccupy(GridPosition, tower.dimensions);
                        tower.Create(pos, Quaternion.identity);
                       // ghost.Hide();
                    }
                }
            }           
        }
    }
}
