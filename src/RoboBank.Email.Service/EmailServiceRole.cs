using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.Practices.Unity;
using RoboBank.Email.Application;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;

namespace RoboBank.Email.Service
{
    public class EmailServiceRole : RoleEntryPoint
    {
        private QueueClient _client;
        private EmailApplicationService _emailApplicationService;
        private TelemetryClient _telemetryClient;
        private readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");

            _client.OnMessageAsync(async receivedMessage =>
                {
                    try
                    {
                        Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());

                        await
                            _emailApplicationService.SendEmailForEventAsync(new AccountEventInfo
                            {
                                AccountId = receivedMessage.Properties["AccountId"].ToString(),
                                CustomerId = receivedMessage.Properties["CustomerId"].ToString(),
                                Type = receivedMessage.Properties["Type"].ToString(),
                                Amount = (decimal) receivedMessage.Properties["Amount"]
                            });
                    }
                    catch (Exception ex)
                    {
                        _telemetryClient.TrackException(ex);
                    }
                });

            _completedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var queueName = ConfigurationManager.AppSettings["QueueName"];

            _client = QueueClient.CreateFromConnectionString(connectionString, queueName);
            _emailApplicationService = UnityConfig.GetConfiguredContainer().Resolve<EmailApplicationService>();
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["ApplicationInsightsKey"];
            _telemetryClient = new TelemetryClient();
            return base.OnStart();
        }

        public override void OnStop()
        {
            _client.Close();
            _completedEvent.Set();
            base.OnStop();
        }
    }
}
