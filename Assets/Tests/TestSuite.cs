using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestSuite
    {
        [UnityTest]
        public IEnumerator Enemy_TakeDamage_CurrentHealthBecameLess()
        {
            var enemy = new Enemy(); // Вызывает Warning из-за того, что половина методов перестает работать как надо
            enemy.currentHealth = 100;
            
            enemy.TakeDamage_(20);
            
            Assert.True(enemy.currentHealth == 80);
            
            yield return null;
            GameObject.Destroy(enemy);
        }
        
        [UnityTest]
        public IEnumerator SaveSystem_SavePlayer_DataSavedSuccessfully()
        {
            var player = new Player(); // Вызывает Warning из-за того, что половина методов перестает работать как надо
            player.activeSaveIndex = 3;
            player.nickname = "MASTERPIECE";
            player.currentHealth = 42;

            bool success;
            try
            {
                SaveSystem.SavePlayer(player);
                success = true;
            }
            catch
            {
                success = false;
            }
            
            Assert.True(success);
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator SaveSystem_LoadPlayer_CorrectNicknameLoaded()
        {
            Slot.activeIndex = 3;
            var loadedData = SaveSystem.LoadPlayer();

            Assert.True(loadedData.nickname == "MASTERPIECE");
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator SaveSystem_LoadPlayer_CorrectHealthDataLoaded()
        {
            Slot.activeIndex = 3;
            var loadedData = SaveSystem.LoadPlayer();

            Assert.True(loadedData.playerHealth == 42);
            yield return null;
        }
    }
}
