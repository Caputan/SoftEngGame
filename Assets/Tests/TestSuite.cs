using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
    {
        public class TestSuite
        {
            // // A Test behaves as an ordinary method
            // [Test]
            // public void TestSuiteSimplePasses()
            // {
            //     // Use the Assert class to test conditions
            // }
            //
            // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
            // // `yield return null;` to skip a frame.
            // [UnityTest]
            // public IEnumerator TestSuiteWithEnumeratorPasses()
            // {
            //     // Use the Assert class to test conditions.
            //     // Use yield to skip a frame.
            //     yield return null;
            // }
            
            // private Game game;

            private GameObject _gameObject;
            private Player _player;
            
            [UnityTest]
            public IEnumerator PlayerTakesDamage()
            {
                _gameObject = GameObject.Instantiate(new GameObject());
                var player = _gameObject.AddComponent<Player>();
                Debug.Log(player);
                
                // GameObject farmObject = MonoBehaviour.Instantiate(prefab);
                // var player = farmObject.GetComponent<Player>();

                player.currentHealth = 100;
                var hpStart = player.currentHealth;
                player.TakeDamage(20);
                var hpEnd = player.currentHealth;
                Assert.True(hpStart - hpEnd == 20);
                yield return null;
            }
        }
    }

