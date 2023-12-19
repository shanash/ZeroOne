using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Server;
#nullable enable

namespace ProtocolShared.MQTT
{
    public class MosquittoService
    {
        MqttFactory _mqttFactory = new MqttFactory();
        IMqttClient _mqttClient;

        MqttClientOptions? _options = null;
        bool isDisconnect = false;
        Func<string, string, Task>? _onMessage = null;
        public MosquittoService(){
            _mqttClient = _mqttFactory.CreateMqttClient();


        }
        public async Task<bool> Connect(string host, ushort port, Func<string, string, Task>? onMessage = null, Func<MqttClientDisconnectedEventArgs, Task>? onDisconnected = null, Func<MqttClientConnectedEventArgs, Task>? onConnected = null)
        {
            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(host, port) // MQTT 브로커 주소 및 포트
                .WithCleanSession()
                .Build();

            _onMessage = onMessage;

            _mqttClient.ApplicationMessageReceivedAsync += OnMessage;

            if(onConnected == null)
            {
                _mqttClient.ConnectedAsync += OnConnected;
            }
            else
            {
                _mqttClient.ConnectedAsync += onConnected;
            }

            if (onDisconnected == null)
            {
                _mqttClient.DisconnectedAsync += OnDisconnected;
            }
            else
            {
                _mqttClient.DisconnectedAsync += onDisconnected;
            }

            MqttClientConnectResult result = await _mqttClient.ConnectAsync(_options);

            if(result.ResultCode != MqttClientConnectResultCode.Success)
            {
                return false; 
            }

            return true;
        }

        public async Task Disconnect()
        {
            isDisconnect = true;

            await _mqttClient.DisconnectAsync();
        }

        private Task OnConnected(MqttClientConnectedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private async Task OnDisconnected(MqttClientDisconnectedEventArgs arg)
        {
            if(isDisconnect == false)
            {
                // 연결이 끊기면 재접속 한다.
                MqttClientConnectResult result = await _mqttClient.ConnectAsync(_options);

                if (result.ResultCode != MqttClientConnectResultCode.Success)
                {
                    
                }
            }
        }

        public Task OnMessage(MqttApplicationMessageReceivedEventArgs arg)
        {
            var payloadSegment = arg.ApplicationMessage.PayloadSegment;

            if(payloadSegment.Array != null)
            {
                byte[] payload = new byte[payloadSegment.Count];
                Array.Copy(payloadSegment.Array, payloadSegment.Offset, payload, 0, payloadSegment.Count);

                var messageString = Encoding.UTF8.GetString(payload);

                if (_onMessage != null)
                    _onMessage(arg.ApplicationMessage.Topic.ToString(), messageString);
            }

            return Task.CompletedTask;
        }

        public async Task Publish(string topic, string message)
        {
            var builder = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

            await _mqttClient.PublishAsync(builder);

        }
        public async Task Subscribe(string topic)
        {
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
        }

    }
}
