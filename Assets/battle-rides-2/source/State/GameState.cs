﻿using Luderia.BattleRides2.Cars;
using Luderia.BattleRides2.Data;
using LuftSchloss;
using LuftSchloss.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luderia.BattleRides2.States {
    public class GameState : AppState {
        protected override IBindingStrategy CreateBinding() {
            return new GameStateBindings();
        }

        [SerializeField]
        private CarDataList _carDataList;
        private ShuffleBag<CarData> _carShuffleBag;

        public override void Initialize() {
            base.Initialize();

            _carShuffleBag = new ShuffleBag<CarData>();
            for (int i = 0; i < _carDataList.CarList.Count; i++) {
                _carShuffleBag.Add(_carDataList.CarList[i]);
            }

            CarSpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<CarSpawnPoint>();
            for (int i = 0; i < spawnPoints.Length; i++) {
                var currentPoint = spawnPoints[i];
                CreateCarAt(currentPoint);

                GameObject.Destroy(currentPoint.gameObject);
            }
        }

        private void CreateCarAt(CarSpawnPoint spawnPoint) {
            var carController = AddChild<CarController>();
            carController.InitializeCar(_carShuffleBag.Next(), spawnPoint.transform.position, spawnPoint.transform.rotation);

            AddChild(carController.View);

            // TODO: remover isso quando tiver a arena
            CameraFollow cam = FindChild<CameraFollow>();
            cam.SetTarget(carController.View.transform);
        }
    }
}