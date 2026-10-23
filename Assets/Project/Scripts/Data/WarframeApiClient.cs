using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WarframeApiClient : MonoBehaviour
{
    // ========== Warframestat (世界状态) 基础地址 ==========
    private const string WarframestatBase = "https://api.warframe.com/cdn/worldState.php";
    // 平台参数: pc / ps4 / xb1 / switch
    public enum WfPlatform
    {
        Pc, Ps4, Xb1, Switch
    }

    // ========== Warframe.Market v2 基础地址 ==========
    private const string MarketBase = "https://api.warframe.market/v2";

    #region Warframestat - 世界状态接口
    /// <summary>
    /// 获取指定平台完整世界状态（裂隙、突击、Baro、电波、开放世界循环等）
    /// </summary>
    public void FetchWorldState(WfPlatform platform, Action<string> onSuccess = null, Action<string> onError = null)
    {
        string url = $"{WarframestatBase}/{platform.ToString().ToLower()}";
        Debug.Log($"[Warframestat] 请求地址: {url}");
        StartCoroutine(GetRequestCoroutine(url, json =>
        {
            Debug.Log($"[Warframestat] 成功获取 WorldState 数据:\n{json}");
            onSuccess?.Invoke(json);
        }, err =>
        {
            Debug.LogError($"[Warframestat] 请求失败: {err}");
            onError?.Invoke(err);
        }));
    }

    /// <summary>
    /// 单独拿虚空商人 Baro
    /// </summary>
    public void FetchBaro(WfPlatform platform, Action<string> onSuccess = null, Action<string> onError = null)
    {
        string url = $"{WarframestatBase}/{platform.ToString().ToLower()}/baro";
        Debug.Log($"[Warframestat] 请求地址: {url}");
        StartCoroutine(GetRequestCoroutine(url, json =>
        {
            Debug.Log($"[Warframestat] Baro 数据:\n{json}");
            onSuccess?.Invoke(json);
        }, err =>
        {
            Debug.LogError($"[Warframestat Baro] Error: {err}");
            onError?.Invoke(err);
        }));
    }

    /// <summary>
    /// 单独拿虚空裂隙 Fissures
    /// </summary>
    public void FetchFissures(WfPlatform platform, Action<string> onSuccess = null, Action<string> onError = null)
    {
        string url = $"{WarframestatBase}/{platform.ToString().ToLower()}/fissures";
        Debug.Log($"[Warframestat] 请求地址: {url}");
        StartCoroutine(GetRequestCoroutine(url, json =>
        {
            Debug.Log($"[Warframestat] Fissures 裂隙数据:\n{json}");
            onSuccess?.Invoke(json);
        }, err =>
        {
            Debug.LogError($"[Warframestat Fissures] Error: {err}");
            onError?.Invoke(err);
        }));
    }
    #endregion

    #region Warframe.Market v2 接口
    /// <summary>
    /// 获取物品订单 (买卖单)
    /// itemUrlName: 比如 "forma"、"soma-prime"，来自 /items 接口
    /// </summary>
    public void FetchItemOrders(string itemUrlName, Action<string> onSuccess = null, Action<string> onError = null)
    {
        string url = $"{MarketBase}/items/{itemUrlName}/orders";
        Debug.Log($"[WarframeMarket] 请求订单: {url}");
        StartCoroutine(GetRequestCoroutine(url, json =>
        {
            Debug.Log($"[WarframeMarket] {itemUrlName} 订单数据:\n{json}");
            onSuccess?.Invoke(json);
        }, err =>
        {
            Debug.LogError($"[WarframeMarket Orders] Error: {err}");
            onError?.Invoke(err);
        }));
    }

    /// <summary>
    /// 紫卡拍卖搜索接口
    /// 简单示例：搜索 Rifle 紫卡，你后续可以加参数（倾向性、词条、价格过滤）
    /// </summary>
    public void SearchRivenAuctions(string weaponUrlName = "", Action<string> onSuccess = null, Action<string> onError = null)
    {
        string url = $"{MarketBase}/auctions/search?type=riven";
        if (!string.IsNullOrEmpty(weaponUrlName))
        {
            url += $"&weapon_url_name={weaponUrlName}";
        }
        Debug.Log($"[WarframeMarket] 紫卡拍卖搜索: {url}");
        StartCoroutine(GetRequestCoroutine(url, json =>
        {
            Debug.Log($"[WarframeMarket] Riven Auction Result:\n{json}");
            onSuccess?.Invoke(json);
        }, err =>
        {
            Debug.LogError($"[WarframeMarket Riven Auction] Error: {err}");
            onError?.Invoke(err);
        }));
    }

    /// <summary>
    /// 玄骸/姐妹拍卖
    /// type=lich 赤毒玄骸 | type=sister 帕尔沃斯姐妹
    /// </summary>
    public void SearchLichSisterAuctions(bool isLich, Action<string> onSuccess = null, Action<string> onError = null)
    {
        string type = isLich ? "lich" : "sister";
        string url = $"{MarketBase}/auctions/search?type={type}";
        Debug.Log($"[WarframeMarket] {type} auction search: {url}");
        StartCoroutine(GetRequestCoroutine(url, json =>
        {
            Debug.Log($"[WarframeMarket] {type} auction data:\n{json}");
            onSuccess?.Invoke(json);
        }, err =>
        {
            Debug.LogError($"[WarframeMarket {type} Auction] Error: {err}");
            onError?.Invoke(err);
        }));
    }
    #endregion

    #region 底层通用 GET 协程
    private IEnumerator GetRequestCoroutine(string url, Action<string> success, Action<string> fail)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            // 超时 10s
            req.timeout = 10;
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                success?.Invoke(req.downloadHandler.text);
            }
            else
            {
                string msg = $"Code:{req.responseCode} | Error:{req.error}";
                fail?.Invoke(msg);
            }
        }
    }
    #endregion
}
