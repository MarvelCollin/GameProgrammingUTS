using UnityEngine;
using System;
using System.IO;

[Serializable]
public class CropSaveData
{
    public int carrotCount;
    public int potatoCount;
    public int wheatCount;
    public int pumpkinCount;
    public int cabbageCount;
    public int beetrootCount;

    public CropSaveData()
    {
        carrotCount = 0;
        potatoCount = 0;
        wheatCount = 0;
        pumpkinCount = 0;
        cabbageCount = 0;
        beetrootCount = 0;
    }

    public int GetTotal()
    {
        return carrotCount + potatoCount + wheatCount + pumpkinCount + cabbageCount + beetrootCount;
    }

    public int GetCount(CropType cropType)
    {
        switch (cropType)
        {
            case CropType.Carrot: return carrotCount;
            case CropType.Potato: return potatoCount;
            case CropType.Wheat: return wheatCount;
            case CropType.Pumpkin: return pumpkinCount;
            case CropType.Cabbage: return cabbageCount;
            case CropType.Beetroot: return beetrootCount;
            default: return 0;
        }
    }

    public void AddCrop(CropType cropType)
    {
        switch (cropType)
        {
            case CropType.Carrot: carrotCount++; break;
            case CropType.Potato: potatoCount++; break;
            case CropType.Wheat: wheatCount++; break;
            case CropType.Pumpkin: pumpkinCount++; break;
            case CropType.Cabbage: cabbageCount++; break;
            case CropType.Beetroot: beetrootCount++; break;
        }
    }
}

public class CropDataManager : MonoBehaviour
{
    public static CropDataManager Instance { get; private set; }

    private CropSaveData cropData;
    private string saveFilePath;

    public event Action OnCropDataChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "cropdata.json");
        LoadData();
    }

    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            cropData = JsonUtility.FromJson<CropSaveData>(json);
        }
        else
        {
            cropData = new CropSaveData();
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(cropData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void AddCrop(CropType cropType)
    {
        cropData.AddCrop(cropType);
        SaveData();
        OnCropDataChanged?.Invoke();
    }

    public int GetCropCount(CropType cropType)
    {
        return cropData.GetCount(cropType);
    }

    public int GetTotalCrops()
    {
        return cropData.GetTotal();
    }

    public CropSaveData GetCropData()
    {
        return cropData;
    }

    public void ResetData()
    {
        cropData = new CropSaveData();
        SaveData();
        OnCropDataChanged?.Invoke();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
}
