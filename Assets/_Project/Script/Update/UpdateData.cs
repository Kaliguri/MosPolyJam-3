using UnityEngine;

[CreateAssetMenu(fileName = "UpdateData", menuName = "ParryThis/UpdateData")]
public class UpdateData : ScriptableObject
{
    [Header("Text")]
    public string Name;
    [TextArea] public string Description;

    [Header("Visual")]
    public GameObject Art;

    [SerializeReference]
    [Header("Script")]
    public UpdateScript UpdateScript = new UpdateScript();

}
