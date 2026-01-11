using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace SaintPolycarp.BanhChung.Google.SharedComponents
{
    public static class Constants
    {
        //public static string CREDENTIAL_FILE = "GoogleCredentials_Tetpolycarp.json";
        public static string CREDENTIAL_FILE = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), @"GoogleShareComponents\GoogleCredentials_Tetpolycarp.json");

    }

    public static class Methods
    {
        public static SheetsService GetCredentialService()
        {

            //First time while running this code, we have to manually allow access throught the browser
            //Then this file create in the cache C:\Users\chau\Documents\DocumentsGoogle.Apis.Auth.OAuth2.Responses.TokenResponse-user
            string[] Scopes = { SheetsService.Scope.Spreadsheets }; // static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            string ApplicationName = "Update Inventory";

            UserCredential credential;

            using (var stream = new FileStream(Constants.CREDENTIAL_FILE, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);


                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }




            // Create Google Sheets API service.
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public static IList<IList<Object>> GetAllValuesInSheet(string spreadsheetId, string spreadsheetName)
        {
            try
            {
                SheetsService service = GetCredentialService();
                String range = spreadsheetName;
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;

                if (values != null)
                    return values;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        //cellRange = "A1:B3"
        public static IList<IList<Object>> GetRangeValuesInSheet(string spreadsheetId, string spreadsheetName, string cellRange)
        {
            try
            {
                SheetsService service = GetCredentialService();
                String range = string.Format("{0}!{1}", spreadsheetName, cellRange);
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;

                if (values != null)
                    return values;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public static string GetCellValueInSheet(string spreadsheetId, string spreadsheetName, string cell)
        {
            try
            {
                SheetsService service = GetCredentialService();
                String range = string.Format("{0}!{1}", spreadsheetName, cell);
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;

                if (values != null)
                    return (string)values[0][0];
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public static void UpdateCellValueInSheet(string spreadsheetId, string spreadsheetName, string cell, string newValue)
        {
            try
            {
                SheetsService service = GetCredentialService();
                String range = string.Format("{0}!{1}", spreadsheetName, cell);

                List<IList<Object>> newValueObj = new List<IList<Object>>(); //double list, which is the matrix
                IList<Object> valueObj = new List<Object>(); //this is the row, which is the first list
                valueObj.Add(newValue);
                newValueObj.Add(valueObj); //add each row into the matrix

                SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(new ValueRange() { Values = newValueObj }, spreadsheetId, range);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                UpdateValuesResponse result2 = update.Execute();



            }
            catch (Exception ex)
            {

            }

        }
        public static void UpdateRangeValueInSheet(string spreadsheetId, string spreadsheetName, string updatedRange, List<IList<Object>> newValueObj)
        {
            try
            {
                SheetsService service = GetCredentialService();
                String range = string.Format("{0}!{1}", spreadsheetName, updatedRange);


                SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(new ValueRange() { Values = newValueObj }, spreadsheetId, range);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                UpdateValuesResponse result2 = update.Execute();



            }
            catch (Exception ex)
            {

            }

        }
    }
}
