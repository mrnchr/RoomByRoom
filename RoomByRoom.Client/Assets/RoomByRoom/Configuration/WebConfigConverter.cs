using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Configuration
{
  public class WebConfigConverter
  {
    private const string WebConfigFile = "WebConfig.json";

    public WebConfig Convert()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
      var uri = new Uri(Application.dataPath);
      int index = uri.ToString().LastIndexOf(uri.PathAndQuery, StringComparison.Ordinal);
      string hostName = uri.ToString()[..index] + '/';
      return new WebConfig { ServerName = hostName };
#else
      return JsonConvert.DeserializeObject<WebConfig>(
        File.ReadAllText(Path.Combine(Application.streamingAssetsPath, WebConfigFile)));
#endif
    }
  }
}