using EXE_BE.Models;
using System.Text;
using System.Text.Json;

namespace EXE_BE.Services
{
    public class RecommendService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyBz7XI7Ilgmgtt_14uGFiEzhhup7y49Bds";
        private readonly string _model = "gemini-2.5-flash";
        private readonly UserActivitiesSerivce _userActivitiesSerivce ;
        public RecommendService(HttpClient httpClient, UserActivitiesSerivce userActivitiesSerivce)
        {
            _httpClient = httpClient;
            _userActivitiesSerivce = userActivitiesSerivce;
        }

        public async Task<string> GetRecommend(int acid)
        {
            var activity = await _userActivitiesSerivce.GetUserActivitiesByIdAsync(acid);

            if (activity == null)
            {
                return "Không tìm thấy hoạt động để đưa ra lời khuyên.";
            }

            // Format the activity
            var activityText = FormatActivity(activity);

            var userPrompt =
                $"Based on this activity summary:\n{activityText}\n" +
                "give me short and concise advice to reduce my carbon footprint.";

            var url = $"https://generativelanguage.googleapis.com/v1/models/{_model}:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
        new
        {
            role = "user",
            parts = new[]
            {
                new { text = userPrompt }
            }
        }
    }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error: {response.StatusCode} - {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);
            var aiReply = doc.RootElement
                             .GetProperty("candidates")[0]
                             .GetProperty("content")
                             .GetProperty("parts")[0]
                             .GetProperty("text")
                             .GetString();

            return aiReply ?? "Xin lỗi, tôi chưa có câu trả lời.";
        }

        private string FormatActivity(UserActivities activity)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"On {activity.Date:yyyy-MM-dd}, user {activity.UserId} generated a total of {activity.TotalCO2Emission} kg of CO2.");

            // 🚗 Traffic
            if (activity.TrafficUsage != null)
            {
                sb.AppendLine(
                    $"Traffic: {activity.TrafficUsage.trafficCategory} for {activity.TrafficUsage.distance} km, " +
                    $"emitting {activity.TrafficUsage.CO2emission} kg CO2."
                );
            }

            // 🛍️ Plastic
            if (activity.PlasticUsage != null)
            {
                sb.AppendLine($"Plastic: emitted {activity.PlasticUsage.CO2emission} kg CO2.");
                if (activity.PlasticUsage.PlasticItems != null && activity.PlasticUsage.PlasticItems.Any())
                {
                    foreach (var item in activity.PlasticUsage.PlasticItems)
                    {
                        sb.AppendLine(
                            $" - Plastic item: {item.PlasticCategory}, weight: {item.Weight} kg."
                        );
                    }
                }
            }

            // 🍔 Food
            if (activity.FoodUsage != null)
            {
                sb.AppendLine($"Food: emitted {activity.FoodUsage.CO2emission} kg CO2 (diet score: {activity.FoodUsage.score}).");
                if (activity.FoodUsage.FoodItems != null && activity.FoodUsage.FoodItems.Any())
                {
                    foreach (var item in activity.FoodUsage.FoodItems)
                    {
                        sb.AppendLine(
                            $" - Food item: {item.FoodCategory}, weight: {item.Weight} kg."
                        );
                    }
                }
            }

            // ⚡ Energy
            if (activity.EnergyUsage != null)
            {
                sb.AppendLine(
                    $"Energy: {activity.EnergyUsage.electricityconsumption} kWh consumed, " +
                    $"emitting {activity.EnergyUsage.CO2emission} kg CO2."
                );
            }

            return sb.ToString();
        }


    }
}
