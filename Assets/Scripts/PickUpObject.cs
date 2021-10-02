using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] coloredMeshes;
    [SerializeField] private Material[] colorMaterials;

    [SerializeField] private ObjectInfoDataClass objectInfo;
    public ObjectInfoDataClass GetObjectInfo
    {
        get
        {
            return objectInfo;
        }
    }

    private void Awake()
    {
        UpdateColor();
        objectInfo.Object = this;
    }

    private void UpdateColor()
    {
        Material mat = FindMaterial();
        foreach (MeshRenderer mr in coloredMeshes)
        {
            mr.material = mat;
        }        
        objectInfo.ID = this.transform.GetSiblingIndex() + "." + objectInfo.ObjectColor.ToString() + "." + objectInfo.ObjectType.ToString();
        this.transform.name = objectInfo.ID;
    }

    private Material FindMaterial()
    {
        Material mat = null;
        foreach (Material m in colorMaterials)
        {
            if (m.name.Contains(objectInfo.ObjectColor.ToString()))
            {
                mat = m;
                break;
            }                
        }

        return mat;
    }

    public string GetObjectString()
    {
        return objectInfo.ObjectColor.ToString() + " " + objectInfo.ObjectType.ToString();
    }
}
