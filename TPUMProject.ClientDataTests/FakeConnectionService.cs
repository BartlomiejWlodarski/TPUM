using ClientData;
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

        public void MockUpdateAllBooks(BookDTO[] books)
        {
            AllBooksUpdateResponse response = new AllBooksUpdateResponse();
            response.Books = books;
            OnMessage?.Invoke(serializer.Serialize(response));
        }

        public void MockUpdateUser(UserDTO user)
        {
            UserChangedResponse response = new UserChangedResponse();
            response.User = user;
            OnMessage?.Invoke(serializer.Serialize(response));
        }
    }
}
