using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;
//using Photon.Realtime;
public class TopMessage : MonoBehaviour //MonoBehaviourPun
{
    public static TopMessage instance = null;
    void Awake()
    {
        instance = this;
    }
    [SerializeField] RectTransform ogItem = null;
    [SerializeField] RectTransform mainUI = null;
    [SerializeField] List<RectTransform> itemList = new List<RectTransform>();
    /// <summary>展示一則訊息</summary>
    public void AddMessage(string say)
    {
        RectTransform temp = Instantiate<RectTransform>(ogItem, mainUI);
        temp.GetComponentInChildren<Text>().text = "  " + say + "  ";
        temp.gameObject.SetActive(true);
        itemList.Add(temp);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddMessage(Time.time.ToString("F3"));
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].anchoredPosition = Vector2.Lerp(itemList[i].anchoredPosition, new Vector2(itemList[i].anchoredPosition.x, -70f * ((itemList.Count - 1) - i + 1f)), Time.deltaTime * 10f);
        }
    }
    public void Remove(RectTransform item)
    {
        itemList.Remove(item);
    }
    /*
    /// <summary>展示一則訊息給所有玩家(包含自己)</summary>
    public void AddMessageToAllPlayer(string say)
    {
        photonView.RPC("RPCAddMessage", RpcTarget.All, say);
    }
    /// <summary>展示一則訊息給我以外的玩家</summary>
    public void AddMessageToOtherPlayer(string say)
    {
        photonView.RPC("RPCAddMessage", RpcTarget.Others, say);
    }
    [PunRPC]
    void RPCAddMessage(string say, PhotonMessageInfo info)
    {
        AddMessage(say);
    }
    */
}

// 2020 by 阿葉
