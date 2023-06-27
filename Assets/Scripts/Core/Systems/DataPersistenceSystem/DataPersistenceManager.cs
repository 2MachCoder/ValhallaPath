using System.Collections.Generic;
using Modules.Game.Scripts;
using UnityEngine;
using Zenject;

namespace Core.Systems.DataPersistenceSystem
{

    public class DataPersistenceManager : MonoBehaviour
    {
        [Inject] private AudioSystem _audioSystem;
        [Inject] private PlayModeManager _playModeManager;
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption;
        private readonly List<IDataPersistence> _dataPersistenceObjects = new();
        private GameData _gameData;
        private FileDataHandler _dataHandler;

        private void Awake()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath,fileName,useEncryption);
            SetDataPersistenceObjects();
            LoadData();
        }

        private void NewData() => _gameData = new GameData();

        private void LoadData()
        {
            _gameData = _dataHandler.Load();
            if(_gameData == null)
                NewData();
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
                dataPersistenceObject.LoadData(_gameData);
        }
        
        private void SaveData()
        {
            foreach (var dataPersistenceObject in _dataPersistenceObjects)
                dataPersistenceObject.SaveData(ref _gameData);
            _dataHandler.Save(_gameData);
        }
        
        private void SetDataPersistenceObjects()
        {
            _dataPersistenceObjects.Add(_audioSystem);
            _dataPersistenceObjects.Add(_playModeManager);
        }
        private void OnApplicationPause(bool pauseStatus) { if (pauseStatus) SaveData(); }
        private void OnApplicationQuit() => SaveData();
    }
}