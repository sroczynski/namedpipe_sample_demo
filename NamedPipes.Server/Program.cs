using System.IO.Pipes;
using System.Text;

const string pipe = "ServerPipe";

using var server = new NamedPipeServerStream(pipe, PipeDirection.InOut, 1);

Console.WriteLine("[*] Waiting for client connection.");
server.WaitForConnection();
Console.WriteLine("[*] Client connected.");

while (true)
{
    var buffer = new byte[512];

    var bufferLength = server.Read(buffer, 0, buffer.Length);
    if (bufferLength == 0) 
        break;

    var message = Encoding.UTF8.GetString(buffer, 0, bufferLength);

    Console.WriteLine($"Received: {message}");

    var response = Encoding.UTF8.GetBytes(message.ToUpper());
    server.Write(response, 0, response.Length);
}