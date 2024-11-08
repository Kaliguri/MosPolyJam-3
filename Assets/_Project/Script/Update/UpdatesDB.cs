using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_DB", menuName = "ParryThis/DB")]
public class DB : ScriptableObject
{
    [Header("Updates")]
    public List<UpdateData> UpdatesList; //Only available Updates!
}
