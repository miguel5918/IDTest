using FormUI.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FormUI.Services
{
    public class ApiService
    {

        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> SendMessageAsync(CMessage message)
        {
            // URI
            var apiUrl = "http://localhost:5095/api/message";

            string id = "NO";
            try
            {
                // serialize message to JSON
                var json = JsonSerializer.Serialize(message);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request
                var response = await _httpClient.PostAsync(apiUrl, content);


                try
                {




                    if (response.IsSuccessStatusCode)
                    {
                        //read data from content
                        var responseContent = await response.Content.ReadAsStringAsync();

                        //option to avoid case sentive problems
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        //Deserilize response
                        var responseData = JsonSerializer.Deserialize<TwiloAnswer>(responseContent, options);


                        string TwiloId = responseData?.Twilo ?? "NO Data";
                        id = TwiloId;


                    }
                    else
                    {

                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($" {response.StatusCode} - {errorContent}");




                    }
                }
                catch (Exception ex)
                {

                    string errorMessage = ex.Message.Contains("unverified") == true && ex.Message.Contains("The number") == true ? "Number not  verified by Twilo " : ex.Message;
                    return $"Error de Twilio: {errorMessage}";
                }
                // return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // manage errors
                System.Diagnostics.Debug.WriteLine($"Error sending message: {ex.Message}");
                return "NO";
            }
            return id;
        }
        public async Task<List<CMessage>> GetAllMessages()
        {
            // URI
            var apiUrl = "http://localhost:5095/api/message";

            var LMessages = new List<CMessage>();
            try
            {
                // GET
                var response = await _httpClient.GetAsync(apiUrl);

                // answer validation
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    // Deserialize  JSON to object list
                    var Messages = JsonSerializer.Deserialize<List<CMessage>>(json, options);


                    LMessages = Messages;

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener datos: {ex.Message}");
            }

            return LMessages ?? new List<CMessage>();
        }
    }
}
