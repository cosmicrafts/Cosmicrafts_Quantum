namespace CosmicraftsSP {
    using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/*
 * This is a API request controller
 * Used to communicate with WEB APIS
 */

//Estado de respuesta
public enum EStatus
{
    success,
    refuse,
    exception
}
//Metodos
public enum EMethod
{
    GET,
    POST,
    PUT,
    PATCH,
    DELETE
}
//Estructura datos de error
public class ErrorData
{
    public string message { get; set; }
}
//Estructura datos de respuesta
public struct NetResult
{
    public EStatus Status;
    public string Response;

    public NetResult(EStatus succes, string response)
    {
        Status = succes;
        Response = response;
    }
}
//Patch Format
public class PatchData
{
    string op { get; set; }
    public string path { get; set; }
    public string value { get; set; }

    public PatchData(string _key, object _value)
    {
        op = "replace";
        path = "/" + _key;
        value = _value.ToString();
    }
}
//Client Controller
public static class NetClient
{
    public static readonly string BaseUri = "http://18.144.99.91:8080"; //IP Base

    public static bool IsBusy = false;

    static double TimeOut = 8;

    public static void InitNetClient()
    {
        if (GlobalManager.GMD.DebugMode)
            TimeOut = 99;
        else
            TimeOut = 8;
    }

    public static string GetApiUrl()
    {
        if (GlobalManager.GMD.DebugMode)
            return "http://localhost:5000/api/";

        return BaseUri + "/api/";
    }

    static async Task<NetResult> Base(string uri, EMethod eMethod, object content = null)
    {
        IsBusy = true;
        //Refresh Screen
        await Task.Delay(120);
        //Prepare Result
        NetResult result = new NetResult(EStatus.refuse, "404");

        //Prepare Data
        Uri baseUrl = new Uri(GetApiUrl() + uri);

        StringContent _content = content == null ? null : new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        //Create Client and Content
        HttpClient _client = new HttpClient();
        _client.Timeout = TimeSpan.FromSeconds(TimeOut);
        //Auth
        if (GlobalManager.GMD.UserIsInit())
        {
            User user = GlobalManager.GMD.GetUserData();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        } else
        {
            return new NetResult { Response = "No user init", Status = EStatus.refuse };
        }

        //Server Request
        try
        {
            HttpResponseMessage _response = null;

            switch (eMethod)
            {
                case EMethod.PUT:
                    { _response = await _client.PutAsync(baseUrl, _content); }
                    break;
                case EMethod.POST:
                    { _response = await _client.PostAsync(baseUrl, _content); }
                    break;
                case EMethod.PATCH:
                    {
                        var method = new HttpMethod("PATCH");

                        var request = new HttpRequestMessage(method, baseUrl)
                        {
                            Content = _content
                        };
                        _response = await _client.SendAsync(request); 
                    }
                    break;
                case EMethod.DELETE:
                    { _response = await _client.DeleteAsync(baseUrl); }
                    break;
                default:
                    { _response = await _client.GetAsync(baseUrl); }
                    break;
            }

            result.Status = (_response.StatusCode == HttpStatusCode.OK 
                            || _response.StatusCode == HttpStatusCode.NoContent) ? EStatus.success : EStatus.refuse;
            
            result.Response = _response.StatusCode != HttpStatusCode.NoContent ? await _response.Content.ReadAsStringAsync() : string.Empty;
        }
        catch (Exception ex)
        {
            //Error
            result.Status = EStatus.exception;
            result.Response = ex.Message;
        }

        IsBusy = false;

        return result;
    }

    public static async Task<NetResult> GET(string uri)
    {
        return await Base(uri, EMethod.GET);
    }

    public static async Task<NetResult> POST(object content, string uri)
    {
        return await Base(uri, EMethod.POST, content);
    }

    public static async Task<NetResult> PUT(object content, string uri)
    {
        return await Base(uri, EMethod.PUT, content);
    }

    public static async Task<NetResult> PATCH(List<PatchData> patchdata, string uri)
    {
        return await Base(uri, EMethod.PATCH, patchdata);
    }

    public static async Task<NetResult> DELETE(string uri)
    {
        return await Base(uri, EMethod.DELETE);
    }
}
}