
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeReviewTest : MonoBehaviour
{
    // 测试 1：未使用字段
    private int _unusedField = 10;

    // 测试 2：可以考虑声明为 readonly
    private int _maxHealth = 100;

    // 测试 3：重复创建集合、潜在 GC 分配
    private void Update()
    {
        List<int> numbers = new List<int>();
        numbers.Add(1);
        numbers.Add(2);

        // 测试 4：可能产生额外开销的查找
        Camera currentCamera = Camera.main;

        // 测试 5：冗余布尔比较
        bool isActive = gameObject.activeSelf;
        if (isActive == true)
        {
            Debug.Log("Object is active");
        }

        // 测试 6：冗余 ToString
        int count = numbers.Count;
        Debug.Log(count.ToString());

        // 测试 7：重复代码
        CalculateDamage(10);
        CalculateDamage(10);
    }

    private int CalculateDamage(int damage)
    {
        int result = damage;
        result = result + 0;
        return result;
    }

    // 测试 8：可疑的 Unity 对象空值检查
    private void CheckCamera()
    {
        Camera camera = Camera.main;

        if (camera != null)
        {
            Debug.Log(camera.name);
        }
    }

    // 测试 9：未使用私有方法
    private void UnusedMethod()
    {
        Debug.Log("This method is never called");
    }

    // 测试 10：冗余条件判断
    private bool IsAlive(int health)
    {
        if (health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 测试 11：协程测试
    private IEnumerator ExampleCoroutine()
    {
        yield return null;
        Debug.Log("Coroutine finished");
    }
}
