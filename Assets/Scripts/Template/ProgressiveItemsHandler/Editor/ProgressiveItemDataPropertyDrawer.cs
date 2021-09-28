using System.Collections;
using System.Collections.Generic;
using Codice.Client.Common;
using Template.ProgressiveItemsHandler;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ProgressiveItemData))]
public class ProgressiveItemDataPropertyDrawer : PropertyDrawer
{
    private SerializedProperty _itemName;
    private SerializedProperty _icon;
    private SerializedProperty _unlockedToShopByDefault;
    private SerializedProperty _unlockedToUseByDefault;
    private SerializedProperty _progressToUnlock;
    private SerializedProperty _unlockCompletelyOnProgressPassed;
    private SerializedProperty _priceType;
    private SerializedProperty _price;

    private Rect itemNameRect;
    private Rect iconRect;
    private Rect unlockedToShopByDefaultRect;
    private Rect unlockedToUseByDefaultRect;
    private Rect progressToUnlockRect;
    private Rect unlockCompletelyOnProgressPassedRect;
    private Rect priceTypeRect;
    private Rect priceRect;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
    { 
        int propertiesCount = 8; 
        return EditorGUIUtility.singleLineHeight * propertiesCount + 18;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CacheProperties(property);

        EditorGUI.BeginProperty(position, label, property);

        var labelRect = new Rect(position.x, position.y, position.width, 16);
        EditorGUI.LabelField(labelRect, label);

        CacheRects(position);

        EditorGUI.indentLevel++;

        DrawProperties();

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    private void DrawProperties()
    {
        EditorGUI.PropertyField(itemNameRect, _itemName);
        EditorGUI.PropertyField(iconRect, _icon);

        if (!_unlockCompletelyOnProgressPassed.boolValue)
        {
            EditorGUI.PropertyField(unlockedToShopByDefaultRect, _unlockedToShopByDefault);
            EditorGUI.PropertyField(unlockedToUseByDefaultRect, _unlockedToUseByDefault);
        }

        if (!_unlockedToShopByDefault.boolValue && !_unlockedToUseByDefault.boolValue)
        {
            EditorGUI.PropertyField(progressToUnlockRect, _progressToUnlock);
            EditorGUI.PropertyField(unlockCompletelyOnProgressPassedRect, _unlockCompletelyOnProgressPassed);
        }

        if (_unlockedToUseByDefault.boolValue)
        {
            _unlockedToShopByDefault.boolValue = true;
        }

        if (!_unlockedToUseByDefault.boolValue && !_unlockCompletelyOnProgressPassed.boolValue)
        {
            EditorGUI.PropertyField(priceTypeRect, _priceType);
        }

        if (_priceType.enumValueIndex == 0 && !_unlockCompletelyOnProgressPassed.boolValue && !_unlockedToUseByDefault.boolValue)
        {
            EditorGUI.PropertyField(priceRect, _price);
        }
    }

    private void CacheProperties(SerializedProperty property)
    {
        _itemName = property.FindPropertyRelative("itemName");
        _icon = property.FindPropertyRelative("icon");
        _unlockedToShopByDefault = property.FindPropertyRelative("unlockedToShopByDefault");
        _unlockedToUseByDefault = property.FindPropertyRelative("unlockedToUseByDefault");
        _progressToUnlock = property.FindPropertyRelative("progressToUnlock");
        _unlockCompletelyOnProgressPassed = property.FindPropertyRelative("unlockCompletelyOnProgressPassed");
        _priceType = property.FindPropertyRelative("priceType");
        _price = property.FindPropertyRelative("price");
    }
    
    private void CacheRects(Rect position)
    {
        itemNameRect = new Rect(position.x, position.y + 18, position.width, 16);
        iconRect = new Rect(position.x, position.y + 18 * 2, position.width, 16);
        unlockedToShopByDefaultRect = new Rect(position.x, position.y + 18 * 3, position.width, 16);
        unlockedToUseByDefaultRect = new Rect(position.x, position.y + 18 * 4, position.width, 16);
        progressToUnlockRect = new Rect(position.x, position.y + 18 * 5, position.width, 16);
        unlockCompletelyOnProgressPassedRect = new Rect(position.x, position.y + 18 * 6, position.width, 16);
        priceTypeRect = new Rect(position.x, position.y + 18 * 7, position.width, 16);
        priceRect = new Rect(position.x, position.y + 18 * 8, position.width, 16);
    }
}