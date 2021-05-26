using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeavyWeaponGuiSearcher : MonoBehaviour
{
    [FormerlySerializedAs("parent")] public GameObject guiParent;
    private List<GameObject> elements = new List<GameObject>();

    public List<GameObject> FindGUIElementsByName(string oname)
    {
        elements.Clear();
        SearchChildren(guiParent.transform, oname);
        return elements;
    }

    public GameObject FindSingleGUIElementByName(string oname)
    {
        return SearchForSingleChild(guiParent.transform, oname);
    }

    private void SearchChildren(Transform tparent, string oname)
    {
        for (int i = 0; i < tparent.childCount; ++i)
        {
            Transform tchild = tparent.GetChild(i);
            if (tchild.name == oname)
            {
                elements.Add(tchild.gameObject);
            }

            if (tchild.childCount > 0)
            {
                SearchChildren(tchild.transform, oname);
            }
        }
    }

    private GameObject SearchForSingleChild(Transform tparent, string oname)
    {
        for (int i = 0; i < tparent.childCount; ++i)
        {
            Transform tchild = tparent.GetChild(i);
            if (tchild.name == oname)
            {
                return tchild.gameObject;
            }

            if (tchild.childCount > 0)
            {
                return SearchForSingleChild(tchild.transform, oname);
            }
        }
        return null;
    }
}
