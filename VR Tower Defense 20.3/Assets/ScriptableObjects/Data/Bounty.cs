using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player/Bounties/Bounty")]
public class Bounty : ScriptableObject
{
    public enum TrackerType { SimpleAccumulator, PredicatedAccumulator }
    public enum Relations { LessThan, GreaterThan, Equal, LessThanOrEqual, GreaterThanOrEqual, NotEqual }
    public enum BountyTypes { Kill, Wave, Action }

    public Dictionary<BountyTypes, string> bountyTypeName = new Dictionary<BountyTypes, string>()
    {
        { BountyTypes.Kill, "Kill" },
        { BountyTypes.Action, "Action" },
        { BountyTypes.Wave, "Wave" }
    };

    public TrackerType trackerType;

    // filter
    public string bountyActionId;
    
    // Accumulator
    public int accumulator = 0;
    public int completionVal;

    // Predicate
    public Relations relation;
    [FormerlySerializedAs("testVal")] public float lhs;
    [FormerlySerializedAs("passVal")] public float rhs;
    
    [FormerlySerializedAs("type")]
    public BountyTypes bountyType;

    public bool invert;
    public List<int> availableStints;
    public int availableAfterWave;
    
    // Meta
    public string bountyName;
    public int reward;
    public Sprite iconDefault;
    public Sprite iconSelected;
    public Sprite buttonDefault;
    public Sprite buttonSelected;
    public string description;
    public string shortDescription;
    public bool completed;

    public bool Evaluate()
    {
        if (trackerType == TrackerType.SimpleAccumulator) SimpleAccumulator();
        else if (trackerType == TrackerType.PredicatedAccumulator) PredicatedAccumulator();

        return completed;
    }

    private void SimpleAccumulator()
    {
        ++accumulator;
        IsComplete();
    }

    private void PredicatedAccumulator()
    {
        if (relation == Relations.Equal && (int)lhs == (int)rhs) ++accumulator;
        else if (relation == Relations.GreaterThan && lhs > rhs) ++accumulator;
        else if (relation == Relations.LessThan && lhs < rhs) ++accumulator;
        else if (relation == Relations.NotEqual && (int) lhs != (int) rhs) ++accumulator;
        else if (relation == Relations.GreaterThanOrEqual && lhs >= rhs) ++accumulator;
        else if (relation == Relations.LessThanOrEqual && lhs <= rhs) ++accumulator;
        
        IsComplete();
    }

    public bool IsComplete()
    {
        if (accumulator >= completionVal) completed = true;
        return completed;
    }

    public void ResetBounty()
    {
        accumulator = 0;
        completed = false;
    }

    public bool IsAvailableForStint(int stint)
    {
        bool available = availableStints.Contains(stint);
        if (invert) available = !available;

        return available;
    }
}

[CustomEditor(typeof(Bounty))]
public class BountyEditor : Editor
{
    private SerializedProperty m_BountyType;
    private SerializedProperty m_BountyName;
    private SerializedProperty m_BountyReward;
    private SerializedProperty m_BountyID;
    private SerializedProperty m_TrackerType;
    private SerializedProperty m_BountyRelation;
    private SerializedProperty m_LHS;
    private SerializedProperty m_RHS;
    private SerializedProperty m_BountyComplete;
    private SerializedProperty m_Accumulator;
    private SerializedProperty m_BountyCompletionVal;
    private SerializedProperty m_Description;
    private SerializedProperty m_ShortDescription;
    private SerializedProperty m_DefaultIcon;
    private SerializedProperty m_SelectedIcon;
    private SerializedProperty m_InvertStints;
    private SerializedProperty m_AvailableStints;
    private SerializedProperty m_ButtonDefault;
    private SerializedProperty m_ButtonSelected;
    
    protected virtual void OnEnable () {
        m_BountyType = this.serializedObject.FindProperty ("bountyType"); // enum
        m_TrackerType = this.serializedObject.FindProperty ("trackerType"); // enum
        m_BountyName = this.serializedObject.FindProperty ("bountyName"); // string
        m_BountyReward = this.serializedObject.FindProperty ("reward"); // int
        m_BountyID = this.serializedObject.FindProperty ("bountyActionId"); // string
        
        m_BountyRelation = this.serializedObject.FindProperty ("relation"); // enum
        m_LHS = this.serializedObject.FindProperty ("lhs");
        m_RHS = this.serializedObject.FindProperty ("rhs");
        
        m_BountyComplete = this.serializedObject.FindProperty ("completed");
        m_Accumulator = this.serializedObject.FindProperty ("accumulator");
        m_BountyCompletionVal = this.serializedObject.FindProperty ("completionVal");
        
        m_Description = this.serializedObject.FindProperty ("description");
        m_ShortDescription = this.serializedObject.FindProperty ("shortDescription");
        m_DefaultIcon = this.serializedObject.FindProperty ("iconDefault");
        m_SelectedIcon = this.serializedObject.FindProperty ("iconSelected");
    
        m_InvertStints = this.serializedObject.FindProperty ("invert");
        m_AvailableStints = this.serializedObject.FindProperty ("availableStints");
        
        m_ButtonSelected = this.serializedObject.FindProperty ("buttonSelected");
        m_ButtonDefault = this.serializedObject.FindProperty ("buttonDefault");
    }
    
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        
        this.serializedObject.Update ();

        var bounty = target as Bounty;

        // Meta Data ---------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        EditorGUILayout.LabelField("Meta Data", EditorStyles.boldLabel);
        // bounty.bountyType = (Bounty.BountyTypes)EditorGUILayout.EnumPopup("Bounty Type", bounty.bountyType);
        EditorGUILayout.PropertyField(m_BountyType);
        
        // bounty.bountyName = EditorGUILayout.TextField("Bounty Name", bounty.bountyName);
        // bounty.reward = EditorGUILayout.IntField("Reward", bounty.reward);
        // bounty.bountyActionId = EditorGUILayout.TextField("Bounty Action ID", bounty.bountyActionId);
        
        EditorGUILayout.PropertyField(m_BountyName);
        EditorGUILayout.PropertyField(m_BountyReward);
        EditorGUILayout.PropertyField(m_BountyID);
        
        EditorGUILayout.Space();
        
        // Tracker -----------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        EditorGUILayout.LabelField("Tracker", EditorStyles.boldLabel);
        // bounty.trackerType = (Bounty.TrackerType)EditorGUILayout.EnumPopup("Tracker Type", bounty.trackerType);
        EditorGUILayout.PropertyField(m_TrackerType);

        if (bounty.trackerType == Bounty.TrackerType.PredicatedAccumulator)
        {
            EditorGUI.indentLevel++;
            // bounty.relation = (Bounty.Relations)EditorGUILayout.EnumPopup("Predicate Relation", bounty.relation);
            EditorGUILayout.PropertyField(m_BountyRelation);

            // bounty.lhs = EditorGUILayout.FloatField("Predicate LHS", bounty.lhs);
            // bounty.rhs = EditorGUILayout.FloatField("Predicate RHS", bounty.rhs);
            EditorGUILayout.PropertyField(m_LHS);
            EditorGUILayout.PropertyField(m_RHS);
            
            GUI.enabled = false;
            
            string relationSymbol = "--";
            switch (bounty.relation)
            {
                case Bounty.Relations.Equal: relationSymbol = "(int)lhs == (int)rhs"; break;
                case Bounty.Relations.GreaterThan: relationSymbol = "lhs > rhs"; break;
                case Bounty.Relations.LessThan: relationSymbol = "lhs < rhs"; break;
                case Bounty.Relations.NotEqual: relationSymbol = "(int)lhs != (int)rhs"; break;
                case Bounty.Relations.GreaterThanOrEqual: relationSymbol = "lhs >= rhs"; break;
                case Bounty.Relations.LessThanOrEqual: relationSymbol = "lhs <= rhs"; break;
            }
            
            EditorGUILayout.TextField(" ", relationSymbol);
            
            GUI.enabled = true;
            
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.Space();

        // bounty.accumulator = EditorGUILayout.IntField("Accumulator", bounty.accumulator);
        // bounty.completionVal = EditorGUILayout.IntField("Completion Value", bounty.completionVal);
        // bounty.completed = EditorGUILayout.Toggle("Bounty Complete", bounty.completed);
        EditorGUILayout.PropertyField(m_Accumulator);
        EditorGUILayout.PropertyField(m_BountyCompletionVal);
        EditorGUILayout.PropertyField(m_BountyComplete);

        EditorGUILayout.Space();
        
        // Availability ------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        EditorGUILayout.LabelField("Availability", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_InvertStints);
        EditorGUILayout.PropertyField(m_AvailableStints);
        
        if (bounty.availableStints.Count == 0)
        {
            string message;
            MessageType mtype;
            if (m_InvertStints.boolValue)
            {
                message = "This bounty is never available";
                mtype = MessageType.Warning;
            }
            else
            {
                message = "This bounty is always available.";
                mtype = MessageType.Info;
            }
            
            EditorGUILayout.HelpBox(message, mtype);
        }

        EditorGUILayout.Space();
        
        // GUI ---------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------
        EditorGUILayout.LabelField("GUI", EditorStyles.boldLabel);
        // bounty.description = EditorGUILayout.TextField("Bounty Description", bounty.description);
        // bounty.shortDescription = EditorGUILayout.TextField("Bounty Description", bounty.shortDescription);
        EditorGUILayout.PropertyField(m_Description);
        EditorGUILayout.PropertyField(m_ShortDescription);

        EditorGUILayout.PropertyField(m_DefaultIcon);
        GUI.enabled = false;
        EditorGUI.indentLevel++;
        var gar = (Sprite)EditorGUILayout.ObjectField("Preview:", bounty.iconDefault, typeof(Sprite));
        EditorGUI.indentLevel--;
        GUI.enabled = true;
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(m_SelectedIcon);
        GUI.enabled = false;
        EditorGUI.indentLevel++;
        var bage = (Sprite)EditorGUILayout.ObjectField("Preview:", bounty.iconSelected, typeof(Sprite));
        EditorGUI.indentLevel--;
        GUI.enabled = true;
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(m_ButtonDefault);
        GUI.enabled = false;
        EditorGUI.indentLevel++;
        gar = (Sprite)EditorGUILayout.ObjectField("Preview:", bounty.buttonDefault, typeof(Sprite));
        EditorGUI.indentLevel--;
        GUI.enabled = true;
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(m_ButtonSelected);
        GUI.enabled = false;
        EditorGUI.indentLevel++;
        gar = (Sprite)EditorGUILayout.ObjectField("Preview:", bounty.buttonSelected, typeof(Sprite));
        EditorGUI.indentLevel--;
        GUI.enabled = true;

        this.serializedObject.ApplyModifiedProperties ();
    }
}
