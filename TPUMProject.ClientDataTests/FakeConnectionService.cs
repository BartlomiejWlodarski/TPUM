using ClientData.Abstract;
using ClinetAPI;
using System.Text.Json.Nodes;

namespace TPUMProject.ClientDataTests
{
    internal class FakeConnectionService : IConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        public Task Connect(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            return true;
        }

        public async Task SendAsync(string message)
        {
            if (serializer.GetCommandHeader(message) == GetBooksCommand.StaticHeader)
            {
                AllBooksUpdateResponse response = new AllBooksUpdateResponse();
                response.Books = [new BookDTO(nextId, "TestTitle", "TestAuthor", 20, false, 1)];
                nextId++;
                OnMessage?.Invoke(serializer.Serialize(response));
            }
            else if (serializer.GetCommandHeader(message) == SellBookCommand.StaticHeader)
            {
                SellBookCommand sellItemCommand = serializer.Deserialize<SellBookCommand>(message);
                lastBoughtId = sellItemCommand.BookID;

                TransactionResultResponse response = new TransactionResultResponse(lastBoughtId, "TestUser", 0);
                OnMessage?.Invoke(serializer.Serialize(response));
            }

            await Task.Delay(0);
        }

        private Serializer serializer = Serializer.Create();
        public int lastBoughtId;
        public int nextId = 5;

        public void MockUpdateAll(BookDTO[] books)
        {
            AllBooksUpdateResponse response = new AllBooksUpdateResponse();
            response.Books = books;
            OnMessage?.Invoke(serializer.Serialize(response));
        }
    }

    public abstract class Serializer
    {
        public abstract string Serialize<T>(T toSerialize);
        public abstract T Deserialize<T>(string json);

        public abstract string? GetCommandHeader(string command);

        public static Serializer Create()
        {
            return new JsonSerializer();
        }

    }

    internal class JsonSerializer : Serializer
    {
        public override T Deserialize<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }

        public override string? GetCommandHeader(string command)
        {
            JsonObject jsonObj = JsonObject.Parse(command).AsObject();
            if (jsonObj.ContainsKey("Header"))
            {
                return (string)jsonObj["Header"];
            }
            return null;
        }

        public override string Serialize<T>(T toSerialize)
        {
            return System.Text.Json.JsonSerializer.Serialize<T>(toSerialize);
        }
    }
}
