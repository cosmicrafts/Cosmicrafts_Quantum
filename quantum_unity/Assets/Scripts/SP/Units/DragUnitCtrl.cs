namespace CosmicraftsSP {
using UnityEngine;

/*
 * This script controls and validates the draging cards to deploy them (in-game)
 */

public class DragUnitCtrl : MonoBehaviour
{
    //The number of valid detected areas for deploying
    int areas;
    //The enemy base position
    Vector3 target;

    //The preview 3d model and effects of the card to deploy
    public MeshRenderer MyMesh;
    public MeshFilter MyMeshFilter;
    GameObject currentPreview;

    //The energy cost of the current draging card
    public float TargetCost;

    //The default outline color of the model
    Color DefaultColor;

    //The player data reference
    Player player;

    private void Start()
    {
        //Initialize variables
        areas = 0;
        target = GameMng.GM.GetDefaultTargetPosition(GameMng.P.MyTeam);
        DefaultColor = Color.green;
        player = GameMng.P;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        //Update the position and rotation of the draging card
        transform.position = CMath.GetMouseWorldPos();
        transform.LookAt(CMath.LookToY(transform.position, target));
    }

    //spawnable area detected
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawnarea"))
        {
            areas++;
        }
    }

    //spawnable area lost
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spawnarea"))
        {
            areas--;
        }
    }

    //Return if the player can deploys on the current position
    public bool IsValid()
    {
        return areas > 0;
    }

    //Set the current draging status color
    void SetStatusColor(Color color)
    {
    }

    //Set the current preview from a mesh and material
    public void SetMeshAndTexture(Mesh mesh, Material mat)
    {
        MyMesh.material = mat;
        MyMeshFilter.mesh = mesh;
    }

    //Shows and hides the current preview game object
    public void setMeshActive(bool active)
    {
        MyMesh.gameObject.SetActive(active);
        if (!active && currentPreview != null)
        {
            Destroy(currentPreview);
        }
    }

    //Set the current preview from a game object
    public void CreatePreviewObj(GameObject preview)
    {
        currentPreview = Instantiate(preview, transform);

        UnitAnimLis unitAnimLis = currentPreview.GetComponent<UnitAnimLis>();
        if(unitAnimLis != null) { DestroyImmediate(unitAnimLis, true);}

        SphereCollider sphereCollider = currentPreview.GetComponent<SphereCollider>(); 
        if(sphereCollider != null) { DestroyImmediate(sphereCollider, true);}
    }
}
}