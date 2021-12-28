using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class GetRChannel : MonoBehaviour
{
    public Texture2D texture;

    [SerializeField] private TMP_Text resultInputField;


    private void Start()
    {
        CalculateR();
    }

    public void CalculateR()
    {
        NativeArray<Color32> pixels = new NativeArray<Color32>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);
        NativeArray<float> result = new NativeArray<float>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);

        pixels = texture.GetRawTextureData<Color32>();


        RChannelSingleParallelJob jobCalculator = new RChannelSingleParallelJob
        {
            pixels = pixels,
            result = result
        };

        JobHandle jobHandle = jobCalculator.Schedule(pixels.Length, 100);

        jobHandle.Complete();

        if (resultInputField != null)
        {
            resultInputField.text = result[result.Length - 1].ToString();
        }
    }

    public void CalculateChannelR()
    {
        for (int i = 0; i < 4; i++)
        {
            int n = texture.GetRawTextureData<Color32>().Length / 4;
            int startAt = (i * n) / 4;              // [0n/4], [1n/4], [2n/4], [3n/4] (✔)
            int endAt = ((i + 1) * n) / 4;          // [1n/4], [2n/4], [3n/4], [4n/4] (✔)  (1n/4) (2n/4) (3n/4) (4n/4)

            Debug.Log($"count Value: {n}, Start At: {startAt}, End At: {endAt}");

            NativeArray<Color32> pixels = new NativeArray<Color32>(endAt, Allocator.TempJob);
            NativeArray<float> result = new NativeArray<float>(endAt, Allocator.TempJob);

            pixels = texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt);

            RChannelParallelJob jobCalculator = new RChannelParallelJob
            {
                pixels = pixels,
                result = result
            };

            JobHandle jobHandle = jobCalculator.Schedule(endAt, 100);

            jobHandle.Complete();

            Debug.Log(result[endAt - 1]);

            pixels.Dispose();
            result.Dispose();
        }
    }

    public void CalculateChannelR2()
    {
        NativeArray<Color32> regionOne = new NativeArray<Color32>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);
        NativeArray<Color32> regionTwo = new NativeArray<Color32>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);
        NativeArray<Color32> regionThree = new NativeArray<Color32>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);
        NativeArray<Color32> regionFour = new NativeArray<Color32>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);
        NativeArray<float> result = new NativeArray<float>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);


        for (int i = 0; i < 4; i++)
        {
            int n = texture.GetRawTextureData<Color32>().Length;
            int startAt = (i * n) / 4;              // [0n/4], [1n/4], [2n/4], [3n/4] (✔)
            int endAt = ((i + 1) * n) / 4;          // [1n/4], [2n/4], [3n/4], [4n/4] (✔)

            Debug.Log($"count Value: {n}, Start At: {startAt}, End At: {endAt}, Count Region: {endAt - startAt}");
            switch (i)
            {
                case 0:
                    regionOne = new NativeArray<Color32>(endAt - startAt, Allocator.TempJob);
                    regionOne = texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt - startAt);
                    Debug.Log($"count Region One: {regionOne.Length} & real value: { texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt - startAt).Length}");
                    break;
                case 1:
                    /* regionTwo = new NativeArray<Color32>(endAt - startAt, Allocator.TempJob);
                     regionTwo = texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt - startAt);
                     Debug.Log($"count Region Two: {regionTwo.Length} & real value: { texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt - startAt).Length}");*/
                    break;
                case 2:
                    /*regionThree = new NativeArray<Color32>(endAt - startAt, Allocator.TempJob);
                    regionThree = texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt - startAt);
                    Debug.Log($"count Region Three: {regionThree.Length} & real value: { texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt - startAt).Length}");*/
                    break;
                case 3:
                    regionFour = new NativeArray<Color32>(endAt - startAt, Allocator.TempJob);
                    regionFour = texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt - startAt);
                    Debug.Log($"count Region Four: {regionFour.Length} & real value: { texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt - startAt).Length}");
                    break;
                default:
                    regionOne = new NativeArray<Color32>(endAt - startAt, Allocator.TempJob);
                    regionOne = texture.GetRawTextureData<Color32>().GetSubArray(startAt, endAt);
                    Debug.Log("yepa");
                    break;
            }
        }

        SumRegionsParallelJob jobCalculator = new SumRegionsParallelJob
        {
            regionOne = regionOne,
            regionTwo = regionTwo,
            regionThree = regionThree,
            regionFour = regionFour,
            result = result
        }
        ;
        JobHandle jobHandle = jobCalculator.Schedule(texture.GetRawTextureData<Color32>().Length, 100);

        jobHandle.Complete();

        Debug.Log(result[texture.GetRawTextureData<Color32>().Length / 4 - 1]);

        // regionOne.Dispose();
        result.Dispose();

    }

    public void CalculateChannelR3()
    {
        NativeArray<float> result = new NativeArray<float>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);
        NativeArray<Color32> pixels = new NativeArray<Color32>(texture.GetRawTextureData<Color32>().Length, Allocator.TempJob);

        pixels = texture.GetRawTextureData<Color32>();

        RChannelParallelJob jobCalculator = new RChannelParallelJob
        {
            pixels = pixels,
            result = result,
        };

        JobHandle jobHandle = jobCalculator.Schedule(pixels.Length, 100);

        jobHandle.Complete();

        Debug.Log(result[pixels.Length - 1]);

        // pixels.Dispose();
        result.Dispose();
    }
}

[BurstCompile]
public struct RChannelSingleParallelJob : IJobParallelFor
{
    public NativeArray<Color32> pixels;
    public NativeArray<float> result;
    public float currentSum;

    public void Execute(int index)
    {
        currentSum += pixels[index].r;
        result[index] = currentSum;
    }
}

[BurstCompile]
public struct RChannelParallelJob : IJobParallelFor
{
    public NativeArray<Color32> pixels;
    public NativeArray<float> result;

    public void Execute(int index)
    {
        result[index] = pixels[index].r;
    }
}

[BurstCompile]
public struct RChannelParallelJob2 : IJobParallelFor
{
    public NativeArray<Color32> pixels;
    [ReadOnly] public float result;

    public void Execute(int index)
    {
        result += pixels[index].r;
    }
}

[BurstCompile]
public struct SumRegionsParallelJob : IJobParallelFor
{
    public NativeArray<Color32> regionOne;
    public NativeArray<Color32> regionTwo;
    public NativeArray<Color32> regionThree;
    public NativeArray<Color32> regionFour;
    public NativeArray<float> result;

    public void Execute(int index)
    {
        result[index] = regionOne[index].r + regionTwo[index].r + regionThree[index].r + regionFour[index].r;
    }
}
