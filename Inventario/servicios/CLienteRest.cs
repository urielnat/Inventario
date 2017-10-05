using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace GetRest
{

	public class CLienteRest
	{
        public async Task<T> Get<T>(string url)
        { 

            try{
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            System.Diagnostics.Debug.WriteLine(response);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.Diagnostics.Debug.WriteLine("SE HIZO LA CONEXION");
                var jsonString = await response.Content.ReadAsStringAsync();
                    jsonString = "{\"data\":" + jsonString + "}";
                    System.Diagnostics.Debug.WriteLine(jsonString);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString); ;

            }
            else
            {
                System.Diagnostics.Debug.WriteLine(response);
            }
            } catch(HttpRequestException e)
{
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
			}


			return default(T);
		}
	}
    }