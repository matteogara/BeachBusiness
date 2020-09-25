using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
//using System.Collections;
//using System.Collections.Generic;
using System.IO;
//using System.Linq;
//using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using UnityEngine;


public class CalendarReader : MonoBehaviour
{

    static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
    static string ApplicationName = "Google Calendar API .NET Quickstart";

    CalendarService service;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("New calendar request");


        //Create credentials in buid folder
        string credText = Resources.Load("credentials").ToString();
        File.WriteAllText(Application.persistentDataPath + "/credentials.json", credText);


        UserCredential credential;

        using (var stream =
                new FileStream(Application.persistentDataPath + "/credentials.json", FileMode.Open, FileAccess.Read))
        {
            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.

            string credPath = Application.persistentDataPath + "/token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            Debug.Log("Credential file saved to: " + credPath);
        }

        // Create Google Calendar API service.
        service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || OVRInput.GetDown(OVRInput.Button.One)) {
            Debug.Log("Issuing new request");
            NewRequest();
        }
    }


    void NewRequest() {
        // Define parameters of request.
        EventsResource.ListRequest request = service.Events.List("primary");
        request.TimeMin = DateTime.Now;
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.MaxResults = 10;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        // List events.
        Events events = request.Execute();
        Debug.Log("Upcoming events:");
        if (events.Items != null && events.Items.Count > 0)
        {
            foreach (var eventItem in events.Items)
            {
                DebugEvent(eventItem);
            }
        }
        else
        {
            Debug.Log("No upcoming events found.");
        }
        //Console.Read();
    }


    void DebugEvent(Google.Apis.Calendar.v3.Data.Event _e) {
        string _when = _e.Start.DateTime.ToString();
        if (String.IsNullOrEmpty(_when))
        {
            _when = _e.Start.Date;
        }

        Debug.Log(_e.Summary);
        Debug.Log(_when);
        if (_e.Location != null) {
            Debug.Log(_e.Location);
        }
    }
}
