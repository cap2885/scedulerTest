using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Ms365Scheduler
{
    public class SendMailDto
    {
        public string Recipients { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }
        public int Type { get; set; }
    }

    internal class Program
    {

        static async Task Main(string[] args)
        {

            Console.WriteLine($"스케줄러 실행됨: {DateTime.Now}");

            string email = "capdntjr@emnet.co.kr";
            await SendReservationMails(email, "테스트 제목", "테스트 본문", 0);

            Console.WriteLine("스케줄러 종료됨");
        }

        static async Task SendReservationMails(string email, string subject, string msg, int type)
        {
            //var requestUrl = "http://ms365.api.emnet.co.kr/api/Mail/SendMailFromDB";

            var requestUrl = "http://ms365.api.emnet.co.kr/api/Mail/SendMail";

            //var formContent = new MultipartFormDataContent();

            //formContent.Add(new StringContent("0"), "Type");
            //formContent.Add(new StringContent(subject), "Subject");
            //formContent.Add(new StringContent(string.Format(msg)), "Body");

            ////수신자 목록 추가
            //formContent.Add(new StringContent(email), "Recipients");


            using (HttpClient client = new HttpClient())
            {
                var mailDto = new SendMailDto
                {
                    Recipients = email,
                    Subject = subject,
                    Body = msg,
                    Type = type
                };


                string json = JsonSerializer.Serialize(mailDto);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {

                    Console.WriteLine("예약 메일 발송 시작");

                    HttpResponseMessage response = await client.PostAsync(requestUrl, content); // POST 방식 호출
                    string responseBody = await response.Content.ReadAsStringAsync(); // 응답 내용 받기

                    Console.WriteLine($"응답 코드: {response.StatusCode}");
                    Console.WriteLine($"응답 내용: {responseBody}");

                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"HTTP 요청 오류 발생: {httpEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"오류 발생: {ex.Message}");
                }

            }


        }
    }
}
