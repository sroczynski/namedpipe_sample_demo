using System.IO.Pipes;
using System.Text;

const string pipeName = "ServerPipe";

using var pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut);

Console.WriteLine("[*] Connecting to server...");
pipe.Connect();
Console.WriteLine("[*] Connected to server.");

while (true)
{
    Console.Write($"Client > ");
    var input = Console.ReadLine();

    if (String.IsNullOrEmpty(input)) 
        continue;

    byte[] bytes = Encoding.UTF8.GetBytes(input);
    pipe.Write(bytes, 0, bytes.Length);

    var result = ReadMessage(pipe);

    Console.WriteLine(result);
}


string ReadMessage(PipeStream pipe)
{
    byte[] buffer = new byte[512];
    var bufferLength = pipe.Read(buffer, 0, buffer.Length);
    if (bufferLength == 0)
    {
        Console.WriteLine("Connection finished from server side");
        Environment.Exit(0);
    }

    return Encoding.UTF8.GetString(buffer, 0, bufferLength);
}