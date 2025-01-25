using System.Threading.Tasks;
using LLMUnity;
using UnityEngine;

public class AITestScript : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        for (int i = 0; i < 10; ++i)
        {
            var response1 = await llmCharacter.Chat("IEX +0.4%", addToHistory: false);
            var response2 = await llmCharacter.Chat("GMX -0.1%", addToHistory: false);
            var response3 = await llmCharacter.Chat("LKB -5.4%", addToHistory: false);
            var response4 = await llmCharacter.Chat("POI -60%", addToHistory: false);
            Debug.Log(response1);
            Debug.Log(response2);
            Debug.Log(response3);
            Debug.Log(response4);
            
            await Task.Delay(1000);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
