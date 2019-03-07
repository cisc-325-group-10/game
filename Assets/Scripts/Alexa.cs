using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.DynamoDBv2.Model;
using AmazonsAlexa.Unity.AlexaCommunicationModule;
using UnityEngine.SceneManagement;
public class Alexa : MonoBehaviour
{

    //TODO: explore using seperate lower privledge keys on unity end
    private const string publishKey = "pub-c-6592a63e-134f-4327-9ed7-b2f36a38b8b2";
    private const string subscribeKey = "sub-c-6ba13e32-38a0-11e9-b5cf-1e59042875b2";
    private const string tableName = "AlexaPlusUnityTest";
    //TODO: reduce the scope of db write permissions
    private const string identityPoolId = "us-east-1:36470eab-6335-4f67-a00e-503f04865b2e";
    private string AWSRegion = RegionEndpoint.USEast1.SystemName;
    private const bool debug = false;
    // TODO: use the value provided from alexa to support multiple games
    private const string channel = "XXXXX";
    private Dictionary<string, AttributeValue> attributes;
    private AmazonAlexaManager alexaManager;

    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        alexaManager = new AmazonAlexaManager(publishKey, subscribeKey, channel, tableName, identityPoolId, AWSRegion, null, OnAlexaMessage, null, debug);
    }

    private void ConfirmSetup(GetSessionAttributesEventData eventData)
    {
        //Notify the skill that setup has completed by updating the skills persistant attributes (in DynamoDB)
        attributes = eventData.Values;
        attributes["SETUP_STATE"] = new AttributeValue { S = "COMPLETED" }; //Set SETUP_STATE attribute to a string, COMPLETED
        alexaManager.SetSessionAttributes(attributes, SetAttributesCallback);
    }


    private void OnAlexaMessage(HandleMessageEventData eventData)
    {
        //Listen for new messages from the Alexa skill
        Debug.Log("OnAlexaMessage");

        Dictionary<string, object> message = eventData.Message;

        //Get Session Attributes with in-line defined callback
        switch (message["type"] as string)
        {
            case "AlexaUserId":
                Debug.Log("AlexaUserId: " + message["message"]);
                alexaManager.alexaUserDynamoKey = message["message"] as string;
                break;
        }

        alexaManager.GetSessionAttributes((result) =>
        {
            if (result.IsError)
                Debug.LogError(eventData.Exception.Message);

            switch (message["type"] as string)
            {
                case "AlexaUserId":
                    ConfirmSetup(result);
                    break;
                case "StartRequest":
                    string response = StartGame();
                    alexaManager.SendToAlexaSkill(response, null);
                    break;
                case "MathGameAnswer":
                    string answer = message["answer"] as string;
                    alexaManager.SendToAlexaSkill(onMathAnswer(answer), null);
                    break;
                default:
                    break;
            }
        });
    }

    private string onMathAnswer(string answer)
    {
        DisplayScriptFastMath[] mathComp = FindObjectsOfType<DisplayScriptFastMath>();
        if (mathComp.Length > 0)
        {
            return mathComp[0].onAnswer(answer);
        }
        return "Math answers not accepted at this time.";
    }

    private string StartGame()
    {
        DisplayScriptFastMath[] mathComp = FindObjectsOfType<DisplayScriptFastMath>();
        if (mathComp.Length > 0)
        {
            return mathComp[0].startGame();
        }
        DisplayScriptColourMemory[] colorComp = FindObjectsOfType<DisplayScriptColourMemory>();
        if (colorComp.Length > 0)
        {
            return colorComp[0].startGame();
        }
        return "Game can't be started at this time.";
    }
    private void SetAttributesCallback(SetSessionAttributesEventData eventData)
    {
        //Callback for when session attributes have been updated
        Debug.Log("OnSetAttributes");
        if (eventData.IsError)
            Debug.LogError(eventData.Exception.Message);
    }

}
