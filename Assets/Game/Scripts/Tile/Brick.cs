using System.Collections.Generic;
using UnityEngine;

public class Brick : TileObject, IObstacle, IDamageableObstacle
{
    enum Elements
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    [SerializeField] private GameObject[] _elementMasks;
    private bool[] _elements = new bool[4];

    public bool TakeDamage(Quaternion rotation, bool isPowered)
    {
        if (isPowered == false)
        {
            List<Elements> frontElements = GetFrontElements(rotation);
            List<Elements> backElements = GetBackElements(frontElements);

            List<Elements> destroyedElements = GetDestroyedElements(frontElements, backElements);
            DestroyElements(destroyedElements);

            if (IsTileDestroyed() == true)
            {
                Destroy();
            }
        }
        else
        {
            Destroy();
        }

        return true;
    }

    private void Start()
    {
        for (int i = 0; i < _elements.Length; i++)
        {
            _elements[i] = true;
            _elementMasks[i].SetActive(false);
        }
    }

    private List<Elements> GetFrontElements(Quaternion rotation)
    {
        float z = Mathf.Repeat(rotation.eulerAngles.z, 360f);
 
        List<Elements> results = new List<Elements>();
        if (z == 0)
        {
            results.Add(Elements.DownLeft);
            results.Add(Elements.DownRight);
        }
        if (z == 90)
        {
            results.Add(Elements.UpRight);
            results.Add(Elements.DownRight);
        }
        if (z == 180)
        {
            results.Add(Elements.UpLeft);
            results.Add(Elements.UpRight);
        }
        if (z == 270)
        {
            results.Add(Elements.UpLeft);
            results.Add(Elements.DownLeft);
        }

        return results;
    }

    private List<Elements> GetBackElements(List<Elements> frontElements)
    {
        List<Elements> results = new List<Elements>{
            Elements.UpLeft, Elements.UpRight, Elements.DownLeft, Elements.DownRight
        };

        foreach (Elements frontElement in frontElements)
        {
            results.Remove(frontElement);
        }

        return results;
    }

    private List<Elements> GetDestroyedElements(List<Elements> frontElements, List<Elements> backElements)
    {
        if (_elements[(int)frontElements[0]] == true || _elements[(int)frontElements[1]] == true)
        {
            return frontElements;
        }
        else
        {
            return backElements;
        }
    }

    private void DestroyElements(List<Elements> destroyedElements)
    {
        foreach (Elements element in destroyedElements)
        {
            _elements[(int)element] = false;
            _elementMasks[(int)element].SetActive(true);
        }
    }

    private bool IsTileDestroyed()
    {
        bool isTileDestroyed = true;
        foreach (bool element in _elements)
        {
            if (element == true)
            {
                isTileDestroyed = false;
                break;
            }
        }

        return isTileDestroyed;
    }
}
