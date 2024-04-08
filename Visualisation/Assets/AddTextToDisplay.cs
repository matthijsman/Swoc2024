using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class AddTextToDisplay : MonoBehaviour
{
    private ScrollView scrollView;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        scrollView = root.Q<ScrollView>("PlayerNameList");
    }

    public void UpdatePlayerScores(List<Player> players)
    {
        scrollView.Clear();

        var playerItemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UIComponents/PlayerListEntry.uxml");
        var header = playerItemTemplate.Instantiate();
        header.Q<Label>("lbl_PlayerName").text = "Player";
        header.Q<Label>("lbl_PlayerSnakes").text = "#Snakes";
        header.Q<Label>("lbl_PlayerScore").text = "Score";
        scrollView.Add(header);
        foreach (var player in players.OrderByDescending(x => x.Score).ThenByDescending(x => x.Snakes))
        {
            var playerItem = playerItemTemplate.Instantiate();
            playerItem.Q<Label>("lbl_PlayerName").text = player.Name;
            playerItem.Q<Label>("lbl_PlayerName").style.color = player.Color;
            playerItem.Q<Label>("lbl_PlayerSnakes").text = player.Snakes.ToString();
            playerItem.Q<Label>("lbl_PlayerScore").text = player.Score.ToString();
            scrollView.Add(playerItem);
        }
    }
}