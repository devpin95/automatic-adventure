using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Bounties/Bounty List")]
public class BountyList : ScriptableObject
{
    public List<Bounty> bounties;

    public void ResetObject()
    {
        foreach (var bounty in bounties)
        {
            bounty.ResetBounty();
        }
    }
}

[CustomEditor(typeof(BountyList))]
public class BountyListEditor : Editor
{
    private SerializedProperty m_BountyList;
    
    protected virtual void OnEnable () {
        m_BountyList = this.serializedObject.FindProperty ("bounties"); // list
    }
    
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update ();
        
        var bountylist = target as BountyList;
        // var list = serializedObject.FindProperty("bounties");

        // bountylist.bounties = bountylist.bounties.OrderBy(t => t.trackerType).ToList();
        EditorGUILayout.PropertyField(m_BountyList, new GUIContent("Bounties"), true);
        
        bool emptyElement = bountylist.bounties.Any(t => t == null);

        if (emptyElement)
        {
            EditorGUILayout.HelpBox("There is an empty element in the list!", MessageType.Warning);
        }
         
        EditorGUILayout.Space();

        var killbounties = bountylist.bounties.Where((t => t != null && t.bountyType == Bounty.BountyTypes.Kill)).ToList();
        var actionbounties = bountylist.bounties.Where((t => t != null && t.bountyType == Bounty.BountyTypes.Action)).ToList();
        var wavebounties = bountylist.bounties.Where((t => t != null && t.bountyType == Bounty.BountyTypes.Wave)).ToList();
        
        GUI.enabled = false;

        ShowBountyGroup("Kill Bounties", killbounties);

        EditorGUILayout.Space();
        
        ShowBountyGroup("Action Bounties", actionbounties);

        EditorGUILayout.Space();

        ShowBountyGroup("Wave Bounties", wavebounties);

        GUI.enabled = true;
        
        this.serializedObject.ApplyModifiedProperties ();
    }

    private void ShowBountyGroup(string label, List<Bounty> bounties)
    {
        EditorGUILayout.LabelField(label + " (" + bounties.Count + ")", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (var bounty in bounties)
        {
            if (bounty == null) EditorGUILayout.TextField("--", "--");
            else EditorGUILayout.TextField(bounty.name, bounty.description);
        }
        EditorGUI.indentLevel--;
    }
}
