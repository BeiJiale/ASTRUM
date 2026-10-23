using UnityEngine;

public class RequestData : MonoBehaviour
{
    public WarframeApiClient apiClient;

    void Start()
    {
        if (apiClient == null)
        {
            Debug.LogError("请把 WarframeApiClient 拖到引用");
            return;
        }

        // 测试1：PC平台完整世界状态
        apiClient.FetchWorldState(WarframeApiClient.WfPlatform.Pc);

        // 测试2：Baro虚空商人
        //apiClient.FetchBaro(WarframeApiClient.WfPlatform.Pc);

        // 测试3：裂隙
        //apiClient.FetchFissures(WarframeApiClient.WfPlatform.Pc);

        // 测试4：紫卡拍卖搜索（步枪紫卡）
        //apiClient.SearchRivenAuctions("kohm");

        // 测试5：玄骸拍卖
        //apiClient.SearchLichSisterAuctions(true);
    }
}
