using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkClient : MonoBehaviour
{
    private UdpClient udpClient;
    public string serverIp = "192.168.0.135";
    public int serverPort = 5005;

    void Start()
    {
        udpClient = new UdpClient();
    }

    public void SendSelectionMessage(int cubeNumber)
    {
        string message = $"Selection: {cubeNumber}";
        SendMessageToServer(message);
    }
    
    private void SendMessageToServer(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
            Debug.Log("Message sent via UDP.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"UDP send failed: {ex.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        udpClient?.Close();
    }
}
